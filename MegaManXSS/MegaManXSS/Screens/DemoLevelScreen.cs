using FlatRedBall;

namespace MegaManXSS.Screens
{
	public partial class DemoLevelScreen
	{

		void CustomInitialize()
        {
            HandleDebugInitialization();
            HandleCameraInitialization();
		    Player1Object.LevelCollision = LevelCollision;
        }

		void CustomActivity(bool firstTimeCalled)
        {
            if (ShowDebugMenu)
            {
                DebugWindow.UpdateDebugData(Player1Object);
            }

            if (ShowFPS)
            {
                FPSWindow.UpdateDebugData(TimeManager.SecondDifference);
            }
        }

		void CustomDestroy()
		{

		}

        static void CustomLoadStaticContent(string contentManagerName)
        {

        }


        /// <summary>
        /// Handles debug mode. Debug mode shows collision boxes around Entities and other statistical information
        /// about the game's state.
        /// </summary>
        private void HandleDebugInitialization()
        {
            // Show or hide collision boxes depending on the setting.
            if (ShowCollisionBoxes)
            {
                Player1Object.CollisionBox.Visible = true;
            }
            else
            {
                Player1Object.CollisionBox.Visible = false;
            }

            // Attach Debug and FPS windows to the player ( which the camera is attached to )
            DebugWindow.AttachTo(Player1Object, true);
            FPSWindow.AttachTo(Player1Object, true);
        }
        
        /// <summary>
        /// Handles initialization of the camera.
        /// </summary>
        private void HandleCameraInitialization()
        {
            SpriteManager.Camera.AttachTo(Player1Object, true);
            SpriteManager.Camera.OrthogonalHeight = 200.0f;
            SpriteManager.Camera.FixAspectRatioYConstant();
        }

	}
}
