using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;

using FlatRedBall.Graphics.Model;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Math.Splines;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
using FlatRedBall.Localization;
using MegaManXSS.Entities;

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles; // If using FlatRedBall XNA 4.0 (namespace changed in XNA framework)
#endif

namespace MegaManXSS.Screens
{
	public partial class DemoLevelScreen
	{

		void CustomInitialize()
        {
            this.HandleDebugInitialization();
            this.HandleCameraInitialization();
		}

		void CustomActivity(bool firstTimeCalled)
        {
            Player1Object.Update(LevelCollision);

            #region Debugging section

            if (ShowDebugMenu)
            {
                DebugWindow.UpdateDebugData(Player1Object);
            }

            if (ShowFPS)
            {
                FPSWindow.UpdateDebugData(TimeManager.SecondDifference);
            }

            #endregion
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
