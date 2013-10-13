using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Math.Geometry;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Input;
using FlatRedBall.IO;
using FlatRedBall.Instructions;
using FlatRedBall.Math.Splines;
using FlatRedBall.Utilities;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;

using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;

#if XNA4 || WINDOWS_8
using Color = Microsoft.Xna.Framework.Color;
#elif FRB_MDX
using Color = System.Drawing.Color;
#else
using Color = Microsoft.Xna.Framework.Graphics.Color;
#endif

#if FRB_XNA || SILVERLIGHT
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Microsoft.Xna.Framework.Media;
#endif

// Generated Usings
using MegaManXSS.Entities.Debugging;
using MegaManXSS.Entities.GameObjectInstances.PlayerEntities;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using MegaManXSS.Entities.GameObjects;
using MegaManXSS.Entities.HUD;
using MegaManXSS.Entities.PowerUps;
using MegaManXSS.Entities.Testing;
using MegaManXSS.Factories;
using FlatRedBall;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Math;

namespace MegaManXSS.Screens
{
	public partial class DemoLevelScreen : Screen
	{
		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		public enum VariableState
		{
			Uninitialized, //This exists so that the first set call actually does something
			CurrentLevelState
		}
		VariableState mCurrentState = VariableState.Uninitialized;
		public VariableState CurrentState
		{
			get
			{
				return mCurrentState;
			}
			set
			{
				mCurrentState = value;
				switch(mCurrentState)
				{
					case  VariableState.CurrentLevelState:
						break;
				}
			}
		}
		private FlatRedBall.Math.Geometry.ShapeCollection LevelCollisionShapes;
		private FlatRedBall.Scene Background;
		
		private MegaManXSS.Entities.GameObjectInstances.PlayerEntities.MegaManEntity Player1Object;
		private MegaManXSS.Entities.GameObjectInstances.PlayerEntities.ZeroEntity Player2Object;
		private FlatRedBall.Scene LevelBackground;
		private FlatRedBall.Math.Geometry.ShapeCollection LevelCollision;
		private MegaManXSS.Entities.Debugging.DebugWindowEntity DebugWindow;
		private PositionedObjectList<EnemyEntity> Enemies;
		private PositionedObjectList<BusterProjectile> PlayerBullets;
		private MegaManXSS.Entities.Debugging.FPSEntity FPSWindow;
		public virtual bool ShowCollisionBoxes { get; set; }
		public virtual bool ShowDebugMenu { get; set; }
		public virtual bool ShowFPS { get; set; }

		public DemoLevelScreen()
			: base("DemoLevelScreen")
		{
		}

        public override void Initialize(bool addToManagers)
        {
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			if (!FlatRedBallServices.IsLoaded<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/screens/gamescreen/levelcollisionshapes.shcx", ContentManagerName))
			{
			}
			LevelCollisionShapes = FlatRedBallServices.Load<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/screens/gamescreen/levelcollisionshapes.shcx", ContentManagerName);
			if (!FlatRedBallServices.IsLoaded<FlatRedBall.Scene>(@"content/screens/gamescreen/background.scnx", ContentManagerName))
			{
			}
			Background = FlatRedBallServices.Load<FlatRedBall.Scene>(@"content/screens/gamescreen/background.scnx", ContentManagerName);
			LevelBackground = Background;
			LevelCollision = LevelCollisionShapes;
			Player1Object = new MegaManXSS.Entities.GameObjectInstances.PlayerEntities.MegaManEntity(ContentManagerName, false);
			Player1Object.Name = "Player1Object";
			Player2Object = new MegaManXSS.Entities.GameObjectInstances.PlayerEntities.ZeroEntity(ContentManagerName, false);
			Player2Object.Name = "Player2Object";
			DebugWindow = new MegaManXSS.Entities.Debugging.DebugWindowEntity(ContentManagerName, false);
			DebugWindow.Name = "DebugWindow";
			Enemies = new PositionedObjectList<EnemyEntity>();
			PlayerBullets = new PositionedObjectList<BusterProjectile>();
			FPSWindow = new MegaManXSS.Entities.Debugging.FPSEntity(ContentManagerName, false);
			FPSWindow.Name = "FPSWindow";
			
			
			PostInitialize();
			base.Initialize(addToManagers);
			if (addToManagers)
			{
				AddToManagers();
			}

        }
        
// Generated AddToManagers
		public override void AddToManagers ()
		{
			BusterProjectileFactory.Initialize(PlayerBullets, ContentManagerName);
			base.AddToManagers();
			AddToManagersBottomUp();
			CustomInitialize();
		}


		public override void Activity(bool firstTimeCalled)
		{
			// Generated Activity
			if (!IsPaused)
			{
				
				Player1Object.Activity();
				Player2Object.Activity();
				DebugWindow.Activity();
				for (int i = Enemies.Count - 1; i > -1; i--)
				{
					if (i < Enemies.Count)
					{
						// We do the extra if-check because activity could destroy any number of entities
						Enemies[i].Activity();
					}
				}
				for (int i = PlayerBullets.Count - 1; i > -1; i--)
				{
					if (i < PlayerBullets.Count)
					{
						// We do the extra if-check because activity could destroy any number of entities
						PlayerBullets[i].Activity();
					}
				}
				FPSWindow.Activity();
			}
			else
			{
			}
			base.Activity(firstTimeCalled);
			if (!IsActivityFinished)
			{
				CustomActivity(firstTimeCalled);
			}
			Background.ManageAll();


				// After Custom Activity
				
            
		}

		public override void Destroy()
		{
			// Generated Destroy
			BusterProjectileFactory.Destroy();
			if (this.UnloadsContentManagerWhenDestroyed)
			{
				LevelCollisionShapes.RemoveFromManagers(ContentManagerName != "Global");
			}
			else
			{
				LevelCollisionShapes.RemoveFromManagers(false);
			}
			if (this.UnloadsContentManagerWhenDestroyed)
			{
				Background.RemoveFromManagers(ContentManagerName != "Global");
			}
			else
			{
				Background.RemoveFromManagers(false);
			}
			
			if (Player1Object != null)
			{
				Player1Object.Destroy();
				Player1Object.Detach();
			}
			if (Player2Object != null)
			{
				Player2Object.Destroy();
				Player2Object.Detach();
			}
			if (LevelBackground != null)
			{
				LevelBackground.RemoveFromManagers(ContentManagerName != "Global");
			}
			if (LevelCollision != null)
			{
				LevelCollision.RemoveFromManagers(ContentManagerName != "Global");
			}
			if (DebugWindow != null)
			{
				DebugWindow.Destroy();
				DebugWindow.Detach();
			}
			for (int i = Enemies.Count - 1; i > -1; i--)
			{
				Enemies[i].Destroy();
			}
			for (int i = PlayerBullets.Count - 1; i > -1; i--)
			{
				PlayerBullets[i].Destroy();
			}
			if (FPSWindow != null)
			{
				FPSWindow.Destroy();
				FPSWindow.Detach();
			}

			base.Destroy();

			CustomDestroy();

		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			ShowCollisionBoxes = false;
			ShowDebugMenu = true;
			ShowFPS = true;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp ()
		{
			LevelCollisionShapes.AddToManagers(mLayer);
			Background.AddToManagers(mLayer);
			Player1Object.AddToManagers(mLayer);
			Player2Object.AddToManagers(mLayer);
			DebugWindow.AddToManagers(mLayer);
			FPSWindow.AddToManagers(mLayer);
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			Background.ConvertToManuallyUpdated();
			Player1Object.ConvertToManuallyUpdated();
			Player2Object.ConvertToManuallyUpdated();
			LevelBackground.ConvertToManuallyUpdated();
			DebugWindow.ConvertToManuallyUpdated();
			for (int i = 0; i < Enemies.Count; i++)
			{
				Enemies[i].ConvertToManuallyUpdated();
			}
			for (int i = 0; i < PlayerBullets.Count; i++)
			{
				PlayerBullets[i].ConvertToManuallyUpdated();
			}
			FPSWindow.ConvertToManuallyUpdated();
		}
		public static void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new ArgumentException("contentManagerName cannot be empty or null");
			}
			#if DEBUG
			if (contentManagerName == FlatRedBallServices.GlobalContentManager)
			{
				HasBeenLoadedWithGlobalContentManager = true;
			}
			else if (HasBeenLoadedWithGlobalContentManager)
			{
				throw new Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
			}
			#endif
			MegaManXSS.Entities.GameObjectInstances.PlayerEntities.MegaManEntity.LoadStaticContent(contentManagerName);
			MegaManXSS.Entities.GameObjectInstances.PlayerEntities.ZeroEntity.LoadStaticContent(contentManagerName);
			MegaManXSS.Entities.Debugging.DebugWindowEntity.LoadStaticContent(contentManagerName);
			MegaManXSS.Entities.Debugging.FPSEntity.LoadStaticContent(contentManagerName);
			CustomLoadStaticContent(contentManagerName);
		}
		static VariableState mLoadingState = VariableState.Uninitialized;
		public static VariableState LoadingState
		{
			get
			{
				return mLoadingState;
			}
			set
			{
				mLoadingState = value;
			}
		}
		public Instruction InterpolateToState (VariableState stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  VariableState.CurrentLevelState:
					break;
			}
			var instruction = new DelegateInstruction<VariableState>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
			InstructionManager.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (VariableState stateToStop)
		{
			switch(stateToStop)
			{
				case  VariableState.CurrentLevelState:
					break;
			}
			CurrentState = stateToStop;
		}
		public void InterpolateBetween (VariableState firstState, VariableState secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  VariableState.CurrentLevelState:
					break;
			}
			switch(secondState)
			{
				case  VariableState.CurrentLevelState:
					break;
			}
		}
		public override void MoveToState (int state)
		{
			this.CurrentState = (VariableState)state;
		}
		
		/// <summary>Sets the current state, and pushes that state onto the back stack.</summary>
		public void PushState (VariableState state)
		{
			this.CurrentState = state;
			
			ScreenManager.PushStateToStack((int)this.CurrentState);
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			return null;
		}
		public static object GetFile (string memberName)
		{
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "LevelCollisionShapes":
					return LevelCollisionShapes;
				case  "Background":
					return Background;
			}
			return null;
		}


	}
}
