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

namespace MegaManXSS
{
    internal static class ControllerConfiguration
    {
        #region Members
        private static Keys _moveLeftButton = Keys.A;
        private static Keys _moveRightButton = Keys.D;
        private static Keys _moveUpButton = Keys.W;
        private static Keys _moveDownButton = Keys.S;
        private static Keys _shootButton = Keys.J;
        private static Keys _dashButton = Keys.K;
        private static Keys _jumpButton = Keys.L;
        private static Keys _startButton = Keys.Space;
        private static Keys _selectButton = Keys.V;
        private static Keys _cycleWeaponRight = Keys.U;
        private static Keys _cycleWeaponLeft = Keys.E;



        #endregion


        #region Properties

        /// <summary>
        /// Gets/Sets the keyboard key used for moving left
        /// </summary>
        public static Keys MoveLeftButton
        {
            get { return ControllerConfiguration._moveLeftButton; }
            set { ControllerConfiguration._moveLeftButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for moving right
        /// </summary>
        public static Keys MoveRightButton
        {
            get { return ControllerConfiguration._moveRightButton; }
            set { ControllerConfiguration._moveRightButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for moving up
        /// </summary>
        public static Keys MoveUpButton
        {
            get { return ControllerConfiguration._moveUpButton; }
            set { ControllerConfiguration._moveUpButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for moving down / ducking
        /// </summary>
        public static Keys MoveDownButton
        {
            get { return ControllerConfiguration._moveDownButton; }
            set { ControllerConfiguration._moveDownButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for shooting
        /// </summary>
        public static Keys ShootButton
        {
            get { return ControllerConfiguration._shootButton; }
            set { ControllerConfiguration._shootButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for dashing
        /// </summary>
        public static Keys DashButton
        {
            get { return ControllerConfiguration._dashButton; }
            set { ControllerConfiguration._dashButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for jumping
        /// </summary>
        public static Keys JumpButton
        {
            get { return ControllerConfiguration._jumpButton; }
            set { ControllerConfiguration._jumpButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for entering the inventory screen
        /// </summary>
        public static Keys StartButton
        {
            get { return ControllerConfiguration._startButton; }
            set { ControllerConfiguration._startButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for the select button
        /// </summary>
        public static Keys SelectButton
        {
            get { return ControllerConfiguration._selectButton; }
            set { ControllerConfiguration._selectButton = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for cycling current weapon to the right
        /// </summary>
        public static Keys CycleWeaponRight
        {
            get { return ControllerConfiguration._cycleWeaponRight; }
            set { ControllerConfiguration._cycleWeaponRight = value; }
        }

        /// <summary>
        /// Gets/Sets the keyboard key used for cycling current weapon to the left
        /// </summary>
        public static Keys CycleWeaponLeft
        {
            get { return ControllerConfiguration._cycleWeaponLeft; }
            set { ControllerConfiguration._cycleWeaponLeft = value; }
        }

        #endregion

    }
}
