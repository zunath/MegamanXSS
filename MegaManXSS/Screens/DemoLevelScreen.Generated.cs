#if ANDROID
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif


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
using MegaManXSS.Entities.GameObjects;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using MegaManXSS.Entities.HUD;
using MegaManXSS.Entities.PowerUps;
using MegaManXSS.Entities.Testing;
using MegaManXSS.Factories;
using FlatRedBall;
using FlatRedBall.Screens;
using System;
using System.Collections.Generic;
using System.Text;
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
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			CurrentLevelState = 2
		}
		protected int mCurrentState = 0;
		public Screens.DemoLevelScreen.VariableState CurrentState
		{
			get
			{
				if (Enum.IsDefined(typeof(VariableState), mCurrentState))
				{
					return (VariableState)mCurrentState;
				}
				else
				{
					return VariableState.Unknown;
				}
			}
			set
			{
				mCurrentState = (int)value;
				switch(CurrentState)
				{
					case  VariableState.Uninitialized:
						break;
					case  VariableState.Unknown:
						break;
					case  VariableState.CurrentLevelState:
						break;
				}
			}
		}
		protected FlatRedBall.Math.Geometry.ShapeCollection LevelCollisionShapes;
		protected FlatRedBall.Scene Background;
		
		private MegaManXSS.Entities.GameObjects.MegaManEntity Player1Object;
		private FlatRedBall.Scene LevelBackground;
		private FlatRedBall.Math.Geometry.ShapeCollection LevelCollision;
		private MegaManXSS.Entities.Debugging.DebugWindowEntity DebugWindow;
		private PositionedObjectList<MegaManXSS.Entities.GameObjects.EnemyEntity> Enemies;
		private PositionedObjectList<MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles.BusterProjectile> PlayerBullets;
		private MegaManXSS.Entities.Debugging.FPSEntity FPSWindow;
		bool mShowCollisionBoxes = false;
		public bool ShowCollisionBoxes
		{
			set
			{
				mShowCollisionBoxes = value;
			}
			get
			{
				return mShowCollisionBoxes;
			}
		}
		bool mShowDebugMenu = true;
		public bool ShowDebugMenu
		{
			set
			{
				mShowDebugMenu = value;
			}
			get
			{
				return mShowDebugMenu;
			}
		}
		bool mShowFPS = true;
		public bool ShowFPS
		{
			set
			{
				mShowFPS = value;
			}
			get
			{
				return mShowFPS;
			}
		}

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
			Player1Object = new MegaManXSS.Entities.GameObjects.MegaManEntity(ContentManagerName, false);
			Player1Object.Name = "Player1Object";
			DebugWindow = new MegaManXSS.Entities.Debugging.DebugWindowEntity(ContentManagerName, false);
			DebugWindow.Name = "DebugWindow";
			Enemies = new PositionedObjectList<MegaManXSS.Entities.GameObjects.EnemyEntity>();
			Enemies.Name = "Enemies";
			PlayerBullets = new PositionedObjectList<MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles.BusterProjectile>();
			PlayerBullets.Name = "PlayerBullets";
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
			LevelCollisionShapes.AddToManagers(mLayer);
			Background.AddToManagers(mLayer);
			BusterProjectileFactory.Initialize(PlayerBullets, ContentManagerName);
			Player1Object.AddToManagers(mLayer);
			DebugWindow.AddToManagers(mLayer);
			FPSWindow.AddToManagers(mLayer);
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
			if (this.UnloadsContentManagerWhenDestroyed && ContentManagerName != "Global")
			{
				LevelCollisionShapes.RemoveFromManagers(ContentManagerName != "Global");
			}
			else
			{
				LevelCollisionShapes.RemoveFromManagers(false);
			}
			if (this.UnloadsContentManagerWhenDestroyed && ContentManagerName != "Global")
			{
				Background.RemoveFromManagers(ContentManagerName != "Global");
			}
			else
			{
				Background.RemoveFromManagers(false);
			}
			
			Enemies.MakeOneWay();
			PlayerBullets.MakeOneWay();
			if (Player1Object != null)
			{
				Player1Object.Destroy();
				Player1Object.Detach();
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
			Enemies.MakeTwoWay();
			PlayerBullets.MakeTwoWay();

			base.Destroy();

			CustomDestroy();

		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp ()
		{
			CameraSetup.ResetCamera(SpriteManager.Camera);
			AssignCustomVariables(false);
		}
		public virtual void RemoveFromManagers ()
		{
			Player1Object.RemoveFromManagers();
			if (LevelBackground != null)
			{
				LevelBackground.RemoveFromManagers(false);
			}
			if (LevelCollision != null)
			{
				LevelCollision.RemoveFromManagers(false);
			}
			DebugWindow.RemoveFromManagers();
			for (int i = Enemies.Count - 1; i > -1; i--)
			{
				Enemies[i].Destroy();
			}
			for (int i = PlayerBullets.Count - 1; i > -1; i--)
			{
				PlayerBullets[i].Destroy();
			}
			FPSWindow.RemoveFromManagers();
		}
		public virtual void AssignCustomVariables (bool callOnContainedElements)
		{
			if (callOnContainedElements)
			{
				Player1Object.AssignCustomVariables(true);
				DebugWindow.AssignCustomVariables(true);
				FPSWindow.AssignCustomVariables(true);
			}
			ShowCollisionBoxes = false;
			ShowDebugMenu = true;
			ShowFPS = true;
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			Background.ConvertToManuallyUpdated();
			Player1Object.ConvertToManuallyUpdated();
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
			MegaManXSS.Entities.GameObjects.MegaManEntity.LoadStaticContent(contentManagerName);
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
		public FlatRedBall.Instructions.Instruction InterpolateToState (VariableState stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  VariableState.CurrentLevelState:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<VariableState>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			FlatRedBall.Instructions.InstructionManager.Add(instruction);
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
			if (interpolationValue < 1)
			{
				mCurrentState = (int)firstState;
			}
			else
			{
				mCurrentState = (int)secondState;
			}
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
