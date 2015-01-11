using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Math.Geometry;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using FlatRedBall.Math;

namespace MegaManXSS.Entities.GameObjects
        private PositionedObjectList<BusterProjectile> CharacterBullets { get; set; }
        private const float _shootTimerLength = 0.2f;
        private const float _dashTimerLength = 0.7f;
        private const float _landingTimerLength = 0.18f;
        
        private void CustomInitialize()
		{
            CharacterBullets = new PositionedObjectList<BusterProjectile>();
		}

		private void CustomActivity()
		{
            // This method is fired automatically while this entity is active.
            // We are manually calling activity methods from the scene because of some issues with default functionality of FRB.
            // (Collision detection was firing AFTER everything else - we need it BEFORE)
		}

		private void CustomDestroy()
		{

		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {

        }



        #region Update methods

        internal void Update(ShapeCollection levelCollision)
        {
            HandleGravity();
            HandleLevelCollision(levelCollision);
            HandleInput(levelCollision);
            HandleTimers();
            HandleFalling();
        }

        #endregion

        #region Helper Methods

        private void ChangeDirectionalAnimation(Animation leftAnimation, Animation rightAnimation, int frame = -1)
        {
            CurrentAnimationState = IsFacingRight ? rightAnimation : leftAnimation;

            // Change to specified frame on the new animation.
            if (frame > -1)
            {
                SpriteObject.CurrentFrameIndex = frame;
            }
        }

	    #endregion

        #region Event Handlers

	    private void HandleGravity()
        {
            YAcceleration = -Gravity;
            Drag = CollisionBoxDrag;
        }

        private void HandleFalling()
        {
            // If Y velocity is less than zero, character is no longer jumping
            if (YVelocity < 0.0f)
            {
                IsJumping = false;
                YVelocity = -MaxFallSpeed;
            }
            
            // Hard cap on velocity. Prevents character from falling through the world.
            if (YVelocity < -MaxFallSpeed)
            {
                YVelocity = -MaxFallSpeed;
            }

            SpriteObject.Animate = true;
            
            if (IsJumping || !IsInAir) return;
            if (SpriteObject.CurrentChainName == "FallingRight" || SpriteObject.CurrentChainName == "FallingLeft")
            {
                // Animation pauses on the second frame. (FYI: Animations are zero-based)
                if (SpriteObject.CurrentFrameIndex == 1)
                {
                    SpriteObject.Animate = false;
                }
            }
            // Player hasn't changed to the falling animation yet. Switch to it now.
            else
            {
                CurrentAnimationState = IsFacingRight ? Animation.FallingRightAnimation : Animation.FallingLeftAnimation;
            }
        }

        private void HandleTimers()
        {
            // Shooting animation timer
            if (IsShooting && !IsDashing)
            {
                ShootAnimationTimer += TimeManager.SecondDifference;
                if (ShootAnimationTimer > _shootTimerLength)
                {
                    IsShooting = false;
                    ShootAnimationTimer = 0.0f;
                }
            }

            // Dashing animation timer
            if (IsDashing)
            {
                DashingTimer += TimeManager.SecondDifference;

                // Continuing a dash.
                if (InputManager.Keyboard.KeyDown(ControllerConfiguration.DashButton) && DashingTimer <= _dashTimerLength)
                {
                    if (IsFacingRight && IsOnGround)
                    {
                        XVelocity = DashingVelocity;
                    }
                    else if(!IsFacingRight && IsOnGround)
                    {
                        XVelocity = -DashingVelocity;
                    }
                }
                // Stop a dash.
                else
                {
                    IsDashing = false;
                    DashingTimer = 0.0f;
                }
            }

            // Landing animation timer
            if (IsLanding)
            {
                LandingTimer += TimeManager.SecondDifference;

                if (LandingTimer > _landingTimerLength)
                {
                    IsLanding = false;
                    LandingTimer = 0.0f;
                }
            }

        }

        private void HandleInput(ShapeCollection levelCollision)
        {
            CurrentDirectionState = IsFacingRight ? Direction.Right : Direction.Left;

            // Just fired a single shot
            if (InputManager.Keyboard.KeyPushed(ControllerConfiguration.ShootButton))
            {
                DoFireShot();
            }

            // Shoot button was released. Fire a charged shot if character was charging
            else if (InputManager.Keyboard.KeyReleased(ControllerConfiguration.ShootButton))
            {
            }

            // Start charging after the shoot timer has completed
            if (InputManager.Keyboard.KeyPushed(ControllerConfiguration.ShootButton) && !IsShooting)
            {

            }

            // Started a new dash
            if (InputManager.Keyboard.KeyPushed(ControllerConfiguration.DashButton) && !IsInAir)
            {
                ChangeDirectionalAnimation(Animation.DashingLeftAnimation, Animation.DashingRightAnimation);
            }

            // Started a new jump
            if (InputManager.Keyboard.KeyPushed(ControllerConfiguration.JumpButton) && !IsJumping && !IsInAir)
            {
                DoJump();
            }
            else if(InputManager.Keyboard.KeyReleased(ControllerConfiguration.JumpButton) && IsJumping && IsInAir)
            {
                IsJumping = false;
                if (YVelocity > 0)
                {
                    YVelocity = 0;
                }
            }

            // Moving to the right
            if (InputManager.Keyboard.KeyDown(ControllerConfiguration.MoveRightButton) && !IsDashing)
            {
                DoMovement(true);
            }

            // Moving to the left
            else if (InputManager.Keyboard.KeyDown(ControllerConfiguration.MoveLeftButton) && !IsDashing)
            {
                DoMovement(false);
            }

            // Stopped moving
            else if (InputManager.Keyboard.KeyReleased(ControllerConfiguration.MoveRightButton) || InputManager.Keyboard.KeyReleased(ControllerConfiguration.MoveLeftButton))
            {
                IsMoving = false;
            }
            
            // Idle
            if(!IsDashing && !IsMoving)
            {
                DoIdle();
            }


        }


        #endregion

        #region Actions

        /// <summary>
        /// Causes character to perform landing on ground.
        /// </summary>
        private void DoLandOnGround()
        {
            YAcceleration = 0.0f;

            // Character was jumping, but now he has landed on the ground.
            // Play the landing animation for a short time.
            if (IsInAir && !IsLanding && !IsJumping)
            {
                ChangeDirectionalAnimation(Animation.LandingLeftAnimation, Animation.LandingRightAnimation);
                CurrentJumpTypeState = JumpType.NoJump;
                YVelocity = -10.0f;
            }
        }

        /// <summary>
        /// Causes character to perform a wall jump
        /// </summary>
        private void DoWallClimb()
        {
            IsOnGround = true;
            IsJumping = false;
            IsInAir = false;
        }

        /// <summary>
        /// Causes character to perform a new jump.
        /// </summary>
        private void DoJump()
        {
            ChangeDirectionalAnimation(Animation.JumpLeftStartAnimation, Animation.JumpRightStartAnimation);
            YVelocity = JumpingVelocity;
            XVelocity = 0.0f;

            if (IsDashing)
            {
                CurrentJumpTypeState = JumpType.DashJump;
            }
            else if (IsMoving)
            {
                CurrentJumpTypeState = JumpType.RunJump;
            }
            else
            {
                CurrentJumpTypeState = JumpType.IdleJump;
            }

            IsDashing = false;
        }

        /// <summary>
        /// Handles movement animation changes and velocity.
        /// If moveRight is TRUE, player will move towards the right of the screen.
        /// If moveRight is FALSE, player will move towards the left of the screen.
        /// </summary>
        /// <param name="moveRight"></param>
        private void DoMovement(bool moveRight)
        {
            IsMoving = true;

            // Able to move in ground or in air, as long as player is not performing a dash jump.
            if (IsOnGround || (IsInAir && CurrentJumpTypeState != JumpType.DashJump))
            {
                if (moveRight)
                {
                    XVelocity = MovingVelocity;
                }
                else
                {
                    XVelocity = -MovingVelocity;
                }
            }

            // Different velocities based on the type of jump being performed.
            // I.E: An idle jump has a smaller velocity than a dash jump.
            else if (IsJumping || IsInAir)
            {
                // Normal jumps: Regular moving velocity
                if (CurrentJumpTypeState == JumpType.IdleJump || CurrentJumpTypeState == JumpType.RunJump)
                {
                    if (moveRight)
                    {
                        XVelocity = MovingVelocity;
                    }
                    else
                    {
                        XVelocity = -MovingVelocity;
                    }
                }
                // Dash jumps: Dash moving velocity
                else if (CurrentJumpTypeState == JumpType.DashJump)
                {
                    if (moveRight)
                    {
                        XVelocity = DashingVelocity;
                    }
                    else
                    {
                        XVelocity = -DashingVelocity;
                    }
                }
            }


            CurrentDirectionState = moveRight ? Direction.Right : Direction.Left;

            if (IsShooting)
            {
                if (IsInAir)
                {
                }
                else
                {
                    ChangeDirectionalAnimation(Animation.MovingAndShootingLeftAnimation, Animation.MovingAndShootingRightAnimation, SpriteObject.CurrentFrameIndex);
                }
            }
            else
            {
                if (IsInAir)
                {
                }
                else
                {
                    ChangeDirectionalAnimation(Animation.MovingLeftAnimation, Animation.MovingRightAnimation, SpriteObject.CurrentFrameIndex);
                }
            }
        }

        /// <summary>
        /// Causes character to be idle.
        /// </summary>
        private void DoIdle()
        {
            XVelocity = 0.0f;

            if (!IsJumping && !IsInAir)
            {
                if (IsShooting)
                {
                    ChangeDirectionalAnimation(Animation.ShootingLeftAnimation, Animation.ShootingRightAnimation);
                }
                else
                {
                    if (!IsLanding)
                    {
                        CurrentAnimationState = Animation.IdleAnimation;
                    }
                }
            }
        }

        /// <summary>
        /// Performs a single shot of the X Buster. 
        /// </summary>
        private void DoFireShot()
        {
            ShootAnimationTimer = 0.0f;
            IsShooting = true;
            BusterProjectile newBullet = Factories.BusterProjectileFactory.CreateNew();
            newBullet.X = X;
            newBullet.Y = Y + 2;

            if (IsFacingRight)
            {
                newBullet.X += 20;
                newBullet.CurrentDirectionState = BusterProjectile.Direction.Right;
            }
            else
            {
                newBullet.X -= 20;
                newBullet.CurrentDirectionState = BusterProjectile.Direction.Left;
            }

        }

        #endregion

        #region Collision Detection and Handling

        /// <summary>
        /// Event handling for collision with a level's collision box.
        /// </summary>
        /// <param name="levelCollision"></param>
        private void HandleLevelCollision(ShapeCollection levelCollision)
        {
            if (levelCollision.CollideAgainstMove(CollisionBox, 1, 0))
            {
                // Player is on the ground
                if (CollisionBox.LastMoveCollisionReposition.Y > 0)
                {
                    DoLandOnGround();
                }
                // Player's head bumped into the ground
                else if (CollisionBox.LastMoveCollisionReposition.Y < 0)
                {
                    YVelocity = 0.0f;
                    IsJumping = false;
                }
                // Player collided with a wall. 
                else if (CollisionBox.LastMoveCollisionReposition.X > 0 || CollisionBox.LastMoveCollisionReposition.X < 0)
                {
                    // Player ran into a wall.
                    if (!IsInAir)
                    {
                    }

                    DoWallClimb();
                }
            }
            else
            {
                IsOnGround = false;
                IsInAir = true;
            }
        }

        #endregion
    }
}
