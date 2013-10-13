using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Model;

using FlatRedBall.Input;
using FlatRedBall.Utilities;

using FlatRedBall.Instructions;
using FlatRedBall.Math.Splines;
using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
// Generated Usings
using MegaManXSS.Screens;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
using MegaManXSS.Performance;
using MegaManXSS.Entities.Debugging;
using MegaManXSS.Entities.GameObjectInstances.PlayerEntities;
using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using MegaManXSS.Entities.GameObjects;
using MegaManXSS.Entities.HUD;
using MegaManXSS.Entities.PowerUps;
using MegaManXSS.Entities.Testing;
using MegaManXSS.Factories;
using FlatRedBall;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Math.Geometry;

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
#endif

#if FRB_XNA && !MONODROID
using Model = Microsoft.Xna.Framework.Graphics.Model;
#endif

namespace MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles
{
	public partial class BusterProjectile : PositionedObject, IDestroyable, IPoolable
	{
        // This is made global so that static lazy-loaded content can access it.
        public static string ContentManagerName
        {
            get;
            set;
        }

		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		public enum Direction
		{
			Uninitialized, //This exists so that the first set call actually does something
			Left, 
			Right
		}
		Direction mCurrentDirectionState = Direction.Uninitialized;
		public Direction CurrentDirectionState
		{
			get
			{
				return mCurrentDirectionState;
			}
			set
			{
				mCurrentDirectionState = value;
				switch(mCurrentDirectionState)
				{
					case  Direction.Left:
						break;
					case  Direction.Right:
						break;
				}
			}
		}
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		private static FlatRedBall.Graphics.Animation.AnimationChainList BusterAnimations;
		private static FlatRedBall.Math.Geometry.ShapeCollection CollisionBoxShapeCollection;
		private static FlatRedBall.Scene BusterProjectileScene;
		
		private FlatRedBall.Graphics.Animation.AnimationChain Sprite;
		private FlatRedBall.Math.Geometry.ShapeCollection CollisionBox;
		private FlatRedBall.Scene EntireScene;
		public virtual float BulletVelocity { get; set; }
		public int Index { get; set; }
		public bool Used { get; set; }
		protected Layer LayerProvidedByContainer = null;

        public BusterProjectile(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public BusterProjectile(string contentManagerName, bool addToManagers) :
			base()
		{
			// Don't delete this:
            ContentManagerName = contentManagerName;
            InitializeEntity(addToManagers);

		}

		protected virtual void InitializeEntity(bool addToManagers)
		{
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			CollisionBox = CollisionBoxShapeCollection.Clone();
			EntireScene = BusterProjectileScene.Clone();
			for (int i = 0; i < EntireScene.Texts.Count; i++)
			{
				EntireScene.Texts[i].AdjustPositionForPixelPerfectDrawing = true;
			}
			Sprite = BusterAnimations["NormalBuster"];
			
			PostInitialize();
			if (addToManagers)
			{
				AddToManagers(null);
			}


		}

// Generated AddToManagers
		public virtual void AddToManagers (Layer layerToAddTo)
		{
			PostInitialize();
			LayerProvidedByContainer = layerToAddTo;
			SpriteManager.AddPositionedObject(this);
			AddToManagersBottomUp(layerToAddTo);
			CustomInitialize();
		}

		public virtual void Activity()
		{
			// Generated Activity
			
			CustomActivity();
			EntireScene.ManageAll();
			
			// After Custom Activity
		}

		public virtual void Destroy()
		{
			// Generated Destroy
			SpriteManager.RemovePositionedObject(this);
			if (Used)
			{
				BusterProjectileFactory.MakeUnused(this, false);
			}
			
			if (CollisionBox != null)
			{
				CollisionBox.RemoveFromManagers(false);
			}
			if (EntireScene != null)
			{
				EntireScene.RemoveFromManagers(false);
			}


			CustomDestroy();
		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			CollisionBox.CopyAbsoluteToRelative();
			CollisionBox.AttachAllDetachedTo(this, false);
			EntireScene.CopyAbsoluteToRelative();
			EntireScene.AttachAllDetachedTo(this, false);
			BulletVelocity = 300f;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp (Layer layerToAddTo)
		{
			// We move this back to the origin and unrotate it so that anything attached to it can just use its absolute position
			float oldRotationX = RotationX;
			float oldRotationY = RotationY;
			float oldRotationZ = RotationZ;
			
			float oldX = X;
			float oldY = Y;
			float oldZ = Z;
			
			X = 0;
			Y = 0;
			Z = 0;
			RotationX = 0;
			RotationY = 0;
			RotationZ = 0;
			CollisionBox.AddToManagers(layerToAddTo);
			EntireScene.AddToManagers(layerToAddTo);
			X = oldX;
			Y = oldY;
			Z = oldZ;
			RotationX = oldRotationX;
			RotationY = oldRotationY;
			RotationZ = oldRotationZ;
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			this.ForceUpdateDependenciesDeep();
			SpriteManager.ConvertToManuallyUpdated(this);
			EntireScene.ConvertToManuallyUpdated();
		}
		public static void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new ArgumentException("contentManagerName cannot be empty or null");
			}
			ContentManagerName = contentManagerName;
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
			bool registerUnload = false;
			if (LoadedContentManagers.Contains(contentManagerName) == false)
			{
				LoadedContentManagers.Add(contentManagerName);
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("BusterProjectileStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/busteranimations.achx", ContentManagerName))
				{
					registerUnload = true;
				}
				BusterAnimations = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/busteranimations.achx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/collisionboxshapecollection.shcx", ContentManagerName))
				{
					registerUnload = true;
				}
				CollisionBoxShapeCollection = FlatRedBallServices.Load<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/collisionboxshapecollection.shcx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Scene>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/busterprojectilescene.scnx", ContentManagerName))
				{
					registerUnload = true;
				}
				BusterProjectileScene = FlatRedBallServices.Load<FlatRedBall.Scene>(@"content/entities/gameobjectinstances/projectileentities/playerprojectiles/busterprojectile/busterprojectilescene.scnx", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("BusterProjectileStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
			}
			CustomLoadStaticContent(contentManagerName);
		}
		public static void UnloadStaticContent ()
		{
			if (LoadedContentManagers.Count != 0)
			{
				LoadedContentManagers.RemoveAt(0);
				mRegisteredUnloads.RemoveAt(0);
			}
			if (LoadedContentManagers.Count == 0)
			{
				if (BusterAnimations != null)
				{
					BusterAnimations= null;
				}
				if (CollisionBoxShapeCollection != null)
				{
					CollisionBoxShapeCollection.RemoveFromManagers(ContentManagerName != "Global");
					CollisionBoxShapeCollection= null;
				}
				if (BusterProjectileScene != null)
				{
					BusterProjectileScene.RemoveFromManagers(ContentManagerName != "Global");
					BusterProjectileScene= null;
				}
			}
		}
		public Instruction InterpolateToState (Direction stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
			var instruction = new DelegateInstruction<Direction>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (Direction stateToStop)
		{
			switch(stateToStop)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
			CurrentDirectionState = stateToStop;
		}
		public void InterpolateBetween (Direction firstState, Direction secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
			switch(secondState)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
		}
		public static void PreloadStateContent (Direction state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			object throwaway;
			switch(state)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "BusterAnimations":
					return BusterAnimations;
				case  "CollisionBoxShapeCollection":
					return CollisionBoxShapeCollection;
				case  "BusterProjectileScene":
					return BusterProjectileScene;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "BusterAnimations":
					return BusterAnimations;
				case  "CollisionBoxShapeCollection":
					return CollisionBoxShapeCollection;
				case  "BusterProjectileScene":
					return BusterProjectileScene;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "BusterAnimations":
					return BusterAnimations;
				case  "CollisionBoxShapeCollection":
					return CollisionBoxShapeCollection;
				case  "BusterProjectileScene":
					return BusterProjectileScene;
			}
			return null;
		}
		protected bool mIsPaused;
		public override void Pause (InstructionList instructions)
		{
			base.Pause(instructions);
			mIsPaused = true;
		}
		public virtual void SetToIgnorePausing ()
		{
			InstructionManager.IgnorePausingFor(this);
			InstructionManager.IgnorePausingFor(CollisionBox);
			InstructionManager.IgnorePausingFor(EntireScene);
		}

    }
	
	
	// Extra classes
	public static class BusterProjectileExtensionMethods
	{
	}
	
}
