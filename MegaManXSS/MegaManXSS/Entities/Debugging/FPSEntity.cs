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


#endif

namespace MegaManXSS.Entities.Debugging
{
	public partial class FPSEntity
    {
        #region Constants
        #endregion

        #region Members
        #endregion

        #region Properties
        #endregion

        #region Methods

        internal void UpdateDebugData(float secondDifference)
        {
            FPSMessage = "FPS: " + Math.Ceiling(1 / secondDifference);
            this.FPSObject.DisplayText = FPSMessage;
        }

        #endregion


        #region Flat Red Ball Methods

        private void CustomInitialize()
        {
            this.FPSObject.SetColor(255, 255, 0);
        }

        private void CustomActivity()
        {

        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        #endregion

	}
}