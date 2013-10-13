using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using FlatRedBall.Math;

#endif

namespace MegaManXSS.Entities.GameObjectInstances.PlayerEntities
{
	public partial class MegaManEntity
    {
        
        #region Constants
        private const float _shootTimerLength = 0.2f;
        private const float _dashTimerLength = 0.7f;
        private const float _landingTimerLength = 0.18f;

        #endregion
        
        #region Members

        private PositionedObjectList<BusterProjectile> _characterBullets;

        #endregion

        #region Properties

        private float ShootTimerLength
        {
            get { return _shootTimerLength; }
        }

        private float DashTimerLength
        {
            get { return _dashTimerLength; }
        }

        private float LandingTimerLength
        {
            get { return _landingTimerLength; }
        }

        internal PositionedObjectList<BusterProjectile> CharacterBullets
        {
            get { return _characterBullets; }
            set { _characterBullets = value; }
        }

        #endregion

        #region Flat Red Ball Methods

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

        #endregion


        #region Update methods

        internal void Update(ShapeCollection levelCollision)
        {
            this.HandleGravity();
            this.HandleLevelCollision(levelCollision);
            this.HandleInput(levelCollision);
            this.HandleTimers();
            this.HandleFalling();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method used for swapping out animation states that are dependent on direction.
        /// Cuts down on the amount of unnecessary code.
        /// </summary>
        /// <param name="rightAnimation"></param>
        /// <param name="leftAnimation"></param>
        private void ChangeDirectionalAnimation(Animation leftAnimation, Animation rightAnimation, int frame = -1)
        {
            if (IsFacingRight)
            {
                CurrentAnimationState = rightAnimation;
            }
            else
            {
                CurrentAnimationState = leftAnimation;
            }

            // Change to specified frame on the new animation.
            if (frame > -1)
            {
                SpriteObject.CurrentFrameIndex = frame;
            }

        }

        #endregion

        #region Event Handlers

        internal void HandleGravity()
        {
            this.YAcceleration = -Gravity;
            this.Drag = CollisionBoxDrag;
        }

        private void HandleFalling()
        {
            // If Y velocity is less than zero, character is no longer jumping
            if (this.YVelocity < 0.0f)
            {
                IsJumping = false;
                YVelocity = -MaxFallSpeed;
            }
            
            // Hard cap on velocity. Prevents character from falling through the world.
            if (this.YVelocity < -MaxFallSpeed)
            {
                this.YVelocity = -MaxFallSpeed;
            }

            SpriteObject.Animate = true;
            
            // Don't modify the animation if the player is jumping.
            if (!IsJumping)
            {
                // Obviously we need to be in the air to fall
                if (IsInAir)
                {
                    // Player is already falling
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
                        if (IsFacingRight)
                        {
                            CurrentAnimationState = Animation.FallingRightAnimation;
                        }
                        else
                        {
                            CurrentAnimationState = Animation.FallingLeftAnimation;
                        }
                    }
                }
            }
        }

        internal void HandleTimers()
        {
            // Shooting animation timer
            if (IsShooting && !IsDashing)
            {
                ShootAnimationTimer += TimeManager.SecondDifference;
                if (ShootAnimationTimer > ShootTimerLength)
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
                if (InputManager.Keyboard.KeyDown(ControllerConfiguration.DashButton) && DashingTimer <= DashTimerLength)
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

                if (LandingTimer > LandingTimerLength)
                {
                    IsLanding = false;
                    LandingTimer = 0.0f;
                }
            }

        }

        internal void HandleInput(ShapeCollection levelCollision)
        {
            if (IsFacingRight)
            {
                CurrentDirectionState = Direction.Right;
            }
            else
            {
                CurrentDirectionState = Direction.Left;
            }

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
        internal void DoLandOnGround()
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
        internal void DoWallClimb()
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


            if (moveRight)
            {
                CurrentDirectionState = Direction.Right;
            }
            else
            {
                CurrentDirectionState = Direction.Left;
            }

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
            newBullet.X = this.X;
            newBullet.Y = this.Y + 2;

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
        internal void HandleLevelCollision(ShapeCollection levelCollision)
        {
            if (levelCollision.CollideAgainstMove(this.CollisionBox, 1, 0))
            {
                // Player is on the ground
                if (this.CollisionBox.LastMoveCollisionReposition.Y > 0)
                {
                    this.DoLandOnGround();
                }
                // Player's head bumped into the ground
                else if (this.CollisionBox.LastMoveCollisionReposition.Y < 0)
                {
                    this.YVelocity = 0.0f;
                    this.IsJumping = false;
                }
                // Player collided with a wall. 
                else if (this.CollisionBox.LastMoveCollisionReposition.X > 0 || this.CollisionBox.LastMoveCollisionReposition.X < 0)
                {
                    // Player ran into a wall.
                    if (!IsInAir)
                    {
                    }

                    this.DoWallClimb();
                }
            }
            else
            {
                this.IsOnGround = false;
                this.IsInAir = true;
            }
        }

        #endregion
    }
}
