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
using MegaManXSS.DataTypes;
#elif FRB_MDX
using Keys = Microsoft.DirectX.DirectInput.Key;


#endif



namespace MegaManXSS.Entities.Testing
{
	public partial class PlatformerCharacterBase
    {
        #region Fields

        bool mIsOnGround = false;
        bool mHitHead = false;

        bool mHasDoubleJumped = false;

        double mTimeJumpPushed = double.NegativeInfinity;
        MovementValues mValuesJumpedWith;

        MovementValues mCurrentMovement;

        #endregion

        #region Properties

        public Xbox360GamePad GamePad
        {
            get;
            set;
        }

        double CurrentTime
        {
            get
            {
                if (Screens.ScreenManager.CurrentScreen != null)
                {
                    return Screens.ScreenManager.CurrentScreen.PauseAdjustedCurrentTime;
                }
                else
                {
                    return TimeManager.CurrentTime;
                }
            }
        }

        MovementValues CurrentMovement
        {
            get
            {
                return mCurrentMovement;
            }
        }

        protected virtual bool JumpDown
        {
            get
            {
                if (GamePad != null && GamePad.IsConnected)
                {
                    return GamePad.ButtonDown(Xbox360GamePad.Button.A);
                }
                else
                {
                    return InputManager.Keyboard.KeyDown(Keys.Space);
                }
            }
        }

        protected virtual bool JumpPushed
        {
            get
            {
                if (GamePad != null && GamePad.IsConnected)
                {
                    return GamePad.ButtonPushed(Xbox360GamePad.Button.A);
                }
                else
                {
                    return InputManager.Keyboard.KeyPushed(Keys.Space);
                }
            }
        }

        protected virtual float HorizontalRatio
        {
            get
            {
                if (GamePad != null && GamePad.IsConnected)
                {
                    return GamePad.LeftStick.Position.X;
                }
                else
                {
                    if (InputManager.Keyboard.KeyDown(Keys.Left))
                    {
                        return -1;
                    }
                    else if (InputManager.Keyboard.KeyDown(Keys.Right))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

        }

        #endregion

        private void CustomInitialize()
		{
            mCurrentMovement = GroundMovement;

            YAcceleration = this.CurrentMovement.Gravity;

		}

		private void CustomActivity()
		{
            InputActivity();

		}

        private void InputActivity()
        {
            HorizontalMovementActivity();

            JumpVelocity();
        }

        private void JumpVelocity()
        {
            if (JumpPushed && 
                CurrentMovement.JumpVelocity > 0 &&
                (mIsOnGround || AfterDoubleJump == null || (AfterDoubleJump != null && mHasDoubleJumped == false))
                
                )
            {

                mTimeJumpPushed = CurrentTime;
                this.YVelocity = CurrentMovement.JumpVelocity;
                mValuesJumpedWith = CurrentMovement;

                if (mCurrentMovement == AirMovement)
                {
                    mHasDoubleJumped = true ;
                }
            }

            double secondsSincePush = CurrentTime - mTimeJumpPushed;

            if (mValuesJumpedWith != null && 
                secondsSincePush < mValuesJumpedWith.JumpApplyLength &&
                (mValuesJumpedWith.JumpApplyByButtonHold == false || JumpDown)
                )
            {
                this.YVelocity = mValuesJumpedWith.JumpVelocity;

            }

            if (mValuesJumpedWith != null && mValuesJumpedWith.JumpApplyByButtonHold &&
                (!JumpDown || mHitHead)
                )
            {
                mValuesJumpedWith = null;
            }
        }


        private void HorizontalMovementActivity()
        {
            if (this.CurrentMovement.AccelerationTimeX <= 0)
            {
                this.XVelocity = HorizontalRatio * CurrentMovement.MaxSpeedX;
            }
            else
            {
                float acceleration = CurrentMovement.MaxSpeedX / CurrentMovement.AccelerationTimeX;

                float sign = Math.Sign(HorizontalRatio);
                float magnitude = Math.Abs(HorizontalRatio);

                if(sign == 0)
                {
                    sign = -Math.Sign(XVelocity);
                    magnitude = 1;
                }

                if (XVelocity == 0 || sign == Math.Sign(XVelocity))
                {
                    this.XAcceleration = sign * magnitude * CurrentMovement.MaxSpeedX / CurrentMovement.AccelerationTimeX;
                }
                else
                {
                    float accelerationValue = magnitude * CurrentMovement.MaxSpeedX / CurrentMovement.DecelerationTimeX;


                    if (Math.Abs(XVelocity) < accelerationValue * TimeManager.SecondDifference)
                    {
                        this.XAcceleration = 0;
                        this.XVelocity = 0;
                    }
                    else
                    {

                        // slowing down
                        this.XAcceleration = sign * accelerationValue;
                    }

                }

                XVelocity = Math.Min(XVelocity, CurrentMovement.MaxSpeedX);
                XVelocity = Math.Max(XVelocity, -CurrentMovement.MaxSpeedX);
            }
        }

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        public void CollideAgainst(ShapeCollection shapeCollection)
        {
            float lastY = this.Y;
            mIsOnGround = false;
            mHitHead = false;
            if(shapeCollection.CollideAgainstBounceWithoutSnag(this.Collision, 0))
            {
                if (this.Y > lastY)
                {
                    mIsOnGround = true;
                }
                if (this.Y < lastY)
                {
                    mHitHead = true;
                }
            }



        }


        public void DetermineMovementValues()
        {
            if (mIsOnGround)
            {
                mHasDoubleJumped = false;
                if (CurrentMovement == AirMovement ||
                    CurrentMovement == AfterDoubleJump)
                {
                    mCurrentMovement = GroundMovement;
                }
            }
            else
            {
                if (CurrentMovement == GroundMovement)
                {
                    mCurrentMovement = AirMovement;
                }

            }

            if (mCurrentMovement == AirMovement && mHasDoubleJumped)
            {
                mCurrentMovement = AfterDoubleJump;
            }
        


        }



	}
}
