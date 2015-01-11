#if ANDROID
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif

using BitmapFont = FlatRedBall.Graphics.BitmapFont;
using Cursor = FlatRedBall.Gui.Cursor;
using GuiManager = FlatRedBall.Gui.GuiManager;
// Generated Usings
using MegaManXSS.Screens;
using FlatRedBall.Graphics;
using FlatRedBall.Math;
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
using FlatRedBall.Graphics.Animation;

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

namespace MegaManXSS.Entities.GameObjects
{
	public partial class MegaManEntity : MegaManXSS.Entities.GameObjects.PlayerEntity, IDestroyable
	{
        // This is made global so that static lazy-loaded content can access it.
        public static new string ContentManagerName
        {
            get{ return Entities.GameObjects.PlayerEntity.ContentManagerName;}
            set{ Entities.GameObjects.PlayerEntity.ContentManagerName = value;}
        }

		// Generated Fields
		#if DEBUG
		static bool HasBeenLoadedWithGlobalContentManager = false;
		#endif
		public new enum VariableState
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			Idle = 2, 
			Moving = 3, 
			Jumping = 4, 
			Shooting = 5, 
			NotCharging = 6, 
			Charging = 7
		}
		public new Entities.GameObjects.MegaManEntity.VariableState CurrentState
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
					case  VariableState.Idle:
						break;
					case  VariableState.Moving:
						break;
					case  VariableState.Jumping:
						break;
					case  VariableState.Shooting:
						break;
					case  VariableState.NotCharging:
						break;
					case  VariableState.Charging:
						break;
					default:
						base.CurrentState = base.CurrentState;
						break;
				}
			}
		}
		public enum Animation
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			IdleAnimation = 2, 
			MovingRightAnimation = 3, 
			MovingLeftAnimation = 4, 
			FallingRightAnimation = 5, 
			FallingLeftAnimation = 6, 
			StartMovingRightAnimation = 7, 
			StartMovingLeftAnimation = 8, 
			ClimbingAnimation = 9, 
			ShootingRightAnimation = 10, 
			DamagedRightAnimation = 11, 
			DamagedLeftAnimation = 12, 
			DashingRightAnimation = 13, 
			DashingLeftAnimation = 14, 
			IdleLowLifeAnimation = 15, 
			ClimbingAndShootingRightAnimation = 16, 
			ClimbingAndShootingLeftAnimation = 17, 
			MovingAndShootingRightAnimation = 18, 
			MovingAndShootingLeftAnimation = 19, 
			DashingAndShootingRightAnimation = 20, 
			JumpRightStartAnimation = 21, 
			JumpLeftStartAnimation = 22, 
			LandingRightAnimation = 23, 
			LandingLeftAnimation = 24, 
			ShootingLeftAnimation = 25, 
			DashingAndShootingLeftAnimation = 26
		}
		protected int mCurrentAnimationState = 0;
		public Entities.GameObjects.MegaManEntity.Animation CurrentAnimationState
		{
			get
			{
				if (Enum.IsDefined(typeof(Animation), mCurrentAnimationState))
				{
					return (Animation)mCurrentAnimationState;
				}
				else
				{
					return Animation.Unknown;
				}
			}
			set
			{
				mCurrentAnimationState = (int)value;
				switch(CurrentAnimationState)
				{
					case  Animation.Uninitialized:
						break;
					case  Animation.Unknown:
						break;
					case  Animation.IdleAnimation:
						SpriteObjectCurrentChainName = "Idle";
						break;
					case  Animation.MovingRightAnimation:
						SpriteObjectCurrentChainName = "MovingRight";
						break;
					case  Animation.MovingLeftAnimation:
						SpriteObjectCurrentChainName = "MovingLeft";
						break;
					case  Animation.FallingRightAnimation:
						SpriteObjectCurrentChainName = "FallingRight";
						IsOnGround = false;
						IsJumping = false;
						IsInAir = true;
						break;
					case  Animation.FallingLeftAnimation:
						SpriteObjectCurrentChainName = "FallingLeft";
						IsOnGround = false;
						IsJumping = false;
						IsInAir = true;
						break;
					case  Animation.StartMovingRightAnimation:
						SpriteObjectCurrentChainName = "StartMovingRight";
						break;
					case  Animation.StartMovingLeftAnimation:
						SpriteObjectCurrentChainName = "StartMovingLeft";
						break;
					case  Animation.ClimbingAnimation:
						SpriteObjectCurrentChainName = "Climbing";
						break;
					case  Animation.ShootingRightAnimation:
						SpriteObjectCurrentChainName = "ShootingRight";
						IsShooting = true;
						break;
					case  Animation.DamagedRightAnimation:
						SpriteObjectCurrentChainName = "DamagedRight";
						break;
					case  Animation.DamagedLeftAnimation:
						SpriteObjectCurrentChainName = "DamagedLeft";
						break;
					case  Animation.DashingRightAnimation:
						SpriteObjectCurrentChainName = "DashingRight";
						DashingTimer = 0f;
						IsDashing = true;
						break;
					case  Animation.DashingLeftAnimation:
						SpriteObjectCurrentChainName = "DashingLeft";
						DashingTimer = 0f;
						IsDashing = true;
						break;
					case  Animation.IdleLowLifeAnimation:
						SpriteObjectCurrentChainName = "IdleLowLife";
						break;
					case  Animation.ClimbingAndShootingRightAnimation:
						SpriteObjectCurrentChainName = "ClimbingAndShootingRight";
						break;
					case  Animation.ClimbingAndShootingLeftAnimation:
						SpriteObjectCurrentChainName = "ClimbingAndShootingLeft";
						break;
					case  Animation.MovingAndShootingRightAnimation:
						SpriteObjectCurrentChainName = "MovingAndShootingRight";
						break;
					case  Animation.MovingAndShootingLeftAnimation:
						SpriteObjectCurrentChainName = "MovingAndShootingLeft";
						break;
					case  Animation.DashingAndShootingRightAnimation:
						SpriteObjectCurrentChainName = "DashingAndShootingRight";
						break;
					case  Animation.JumpRightStartAnimation:
						SpriteObjectCurrentChainName = "JumpRightStart";
						IsJumping = true;
						IsInAir = true;
						break;
					case  Animation.JumpLeftStartAnimation:
						SpriteObjectCurrentChainName = "JumpLeftStart";
						IsJumping = true;
						IsInAir = true;
						break;
					case  Animation.LandingRightAnimation:
						SpriteObjectCurrentChainName = "LandingRight";
						IsOnGround = true;
						IsJumping = false;
						IsInAir = false;
						LandingTimer = 0f;
						IsLanding = true;
						break;
					case  Animation.LandingLeftAnimation:
						SpriteObjectCurrentChainName = "LandingLeft";
						IsOnGround = true;
						IsJumping = false;
						IsInAir = false;
						LandingTimer = 0f;
						IsLanding = true;
						break;
					case  Animation.ShootingLeftAnimation:
						SpriteObjectCurrentChainName = "ShootingLeft";
						IsShooting = true;
						break;
					case  Animation.DashingAndShootingLeftAnimation:
						SpriteObjectCurrentChainName = "DashingAndShootingLeft";
						break;
				}
			}
		}
		public enum Direction
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			Left = 2, 
			Right = 3
		}
		protected int mCurrentDirectionState = 0;
		public Entities.GameObjects.MegaManEntity.Direction CurrentDirectionState
		{
			get
			{
				if (Enum.IsDefined(typeof(Direction), mCurrentDirectionState))
				{
					return (Direction)mCurrentDirectionState;
				}
				else
				{
					return Direction.Unknown;
				}
			}
			set
			{
				mCurrentDirectionState = (int)value;
				switch(CurrentDirectionState)
				{
					case  Direction.Uninitialized:
						break;
					case  Direction.Unknown:
						break;
					case  Direction.Left:
						SpriteObjectFlipHorizontal = true;
						IsFacingRight = false;
						break;
					case  Direction.Right:
						SpriteObjectFlipHorizontal = false;
						IsFacingRight = true;
						break;
				}
			}
		}
		public enum JumpType
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			NoJump = 2, 
			IdleJump = 3, 
			DashJump = 4, 
			RunJump = 5
		}
		protected int mCurrentJumpTypeState = 0;
		public Entities.GameObjects.MegaManEntity.JumpType CurrentJumpTypeState
		{
			get
			{
				if (Enum.IsDefined(typeof(JumpType), mCurrentJumpTypeState))
				{
					return (JumpType)mCurrentJumpTypeState;
				}
				else
				{
					return JumpType.Unknown;
				}
			}
			set
			{
				mCurrentJumpTypeState = (int)value;
				switch(CurrentJumpTypeState)
				{
					case  JumpType.Uninitialized:
						break;
					case  JumpType.Unknown:
						break;
					case  JumpType.NoJump:
						break;
					case  JumpType.IdleJump:
						break;
					case  JumpType.DashJump:
						break;
					case  JumpType.RunJump:
						break;
				}
			}
		}
		public enum BodyArmor
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			X1Body = 2, 
			X2Body = 3, 
			X3Body = 4, 
			None = 5
		}
		protected int mCurrentBodyArmorState = 0;
		public Entities.GameObjects.MegaManEntity.BodyArmor CurrentBodyArmorState
		{
			get
			{
				if (Enum.IsDefined(typeof(BodyArmor), mCurrentBodyArmorState))
				{
					return (BodyArmor)mCurrentBodyArmorState;
				}
				else
				{
					return BodyArmor.Unknown;
				}
			}
			set
			{
				mCurrentBodyArmorState = (int)value;
				switch(CurrentBodyArmorState)
				{
					case  BodyArmor.Uninitialized:
						break;
					case  BodyArmor.Unknown:
						break;
					case  BodyArmor.X1Body:
						break;
					case  BodyArmor.X2Body:
						break;
					case  BodyArmor.X3Body:
						break;
					case  BodyArmor.None:
						break;
				}
			}
		}
		public enum LegArmor
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			X1Leg = 2, 
			X2Leg = 3, 
			X3Leg = 4, 
			None = 5
		}
		protected int mCurrentLegArmorState = 0;
		public Entities.GameObjects.MegaManEntity.LegArmor CurrentLegArmorState
		{
			get
			{
				if (Enum.IsDefined(typeof(LegArmor), mCurrentLegArmorState))
				{
					return (LegArmor)mCurrentLegArmorState;
				}
				else
				{
					return LegArmor.Unknown;
				}
			}
			set
			{
				mCurrentLegArmorState = (int)value;
				switch(CurrentLegArmorState)
				{
					case  LegArmor.Uninitialized:
						break;
					case  LegArmor.Unknown:
						break;
					case  LegArmor.X1Leg:
						break;
					case  LegArmor.X2Leg:
						break;
					case  LegArmor.X3Leg:
						break;
					case  LegArmor.None:
						break;
				}
			}
		}
		public enum HelmetArmor
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			X1Helmet = 2, 
			X2Helmet = 3, 
			X3Helmet = 4, 
			None = 5
		}
		protected int mCurrentHelmetArmorState = 0;
		public Entities.GameObjects.MegaManEntity.HelmetArmor CurrentHelmetArmorState
		{
			get
			{
				if (Enum.IsDefined(typeof(HelmetArmor), mCurrentHelmetArmorState))
				{
					return (HelmetArmor)mCurrentHelmetArmorState;
				}
				else
				{
					return HelmetArmor.Unknown;
				}
			}
			set
			{
				mCurrentHelmetArmorState = (int)value;
				switch(CurrentHelmetArmorState)
				{
					case  HelmetArmor.Uninitialized:
						break;
					case  HelmetArmor.Unknown:
						break;
					case  HelmetArmor.X1Helmet:
						break;
					case  HelmetArmor.X2Helmet:
						break;
					case  HelmetArmor.X3Helmet:
						break;
					case  HelmetArmor.None:
						break;
				}
			}
		}
		public enum ArmArmor
		{
			Uninitialized = 0, //This exists so that the first set call actually does something
			Unknown = 1, //This exists so that if the entity is actually a child entity and has set a child state, you will get this
			X1Arm = 2, 
			X2Arm = 3, 
			X3Arm = 4, 
			None = 5
		}
		protected int mCurrentArmArmorState = 0;
		public Entities.GameObjects.MegaManEntity.ArmArmor CurrentArmArmorState
		{
			get
			{
				if (Enum.IsDefined(typeof(ArmArmor), mCurrentArmArmorState))
				{
					return (ArmArmor)mCurrentArmArmorState;
				}
				else
				{
					return ArmArmor.Unknown;
				}
			}
			set
			{
				mCurrentArmArmorState = (int)value;
				switch(CurrentArmArmorState)
				{
					case  ArmArmor.Uninitialized:
						break;
					case  ArmArmor.Unknown:
						break;
					case  ArmArmor.X1Arm:
						break;
					case  ArmArmor.X2Arm:
						break;
					case  ArmArmor.X3Arm:
						break;
					case  ArmArmor.None:
						break;
				}
			}
		}
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		protected static FlatRedBall.Graphics.Animation.AnimationChainList MegaManAnimationChainList;
		protected static FlatRedBall.Math.Geometry.ShapeCollection MegaManCollisionBox;
		protected static FlatRedBall.Scene MegaManScene;
		
		private FlatRedBall.Sprite mSpriteObject;
		public FlatRedBall.Sprite SpriteObject
		{
			get
			{
				return mSpriteObject;
			}
			private set
			{
				mSpriteObject = value;
			}
		}
		private FlatRedBall.Math.Geometry.AxisAlignedRectangle mCollisionBox;
		public FlatRedBall.Math.Geometry.AxisAlignedRectangle CollisionBox
		{
			get
			{
				return mCollisionBox;
			}
			private set
			{
				mCollisionBox = value;
			}
		}
		public string SpriteObjectCurrentChainName
		{
			get
			{
				return SpriteObject.CurrentChainName;
			}
			set
			{
				SpriteObject.CurrentChainName = value;
			}
		}
		public bool SpriteObjectFlipHorizontal
		{
			get
			{
				return SpriteObject.FlipHorizontal;
			}
			set
			{
				SpriteObject.FlipHorizontal = value;
			}
		}
		public bool IsFacingRight = true;
		public bool IsShooting;
		public float ShootAnimationTimer;
		public bool IsOnGround;
		public float DashingTimer;
		public bool IsDashing;
		public float DashingVelocity = 175f;
		public bool SpriteObjectIgnoreAnimationChainTextureFlip
		{
			get
			{
				return SpriteObject.IgnoreAnimationChainTextureFlip;
			}
			set
			{
				SpriteObject.IgnoreAnimationChainTextureFlip = value;
			}
		}
		public bool IsJumping;
		public float JumpingVelocity = 200f;
		public bool IsInAir = true;
		public float LandingTimer;
		public bool IsLanding;
		bool mIsMoving = false;
		public bool IsMoving
		{
			set
			{
				mIsMoving = value;
			}
			get
			{
				return mIsMoving;
			}
		}
		float mMaxFallSpeed = 130f;
		public float MaxFallSpeed
		{
			set
			{
				mMaxFallSpeed = value;
			}
			get
			{
				return mMaxFallSpeed;
			}
		}
		float mGravity = 350f;
		public float Gravity
		{
			set
			{
				mGravity = value;
			}
			get
			{
				return mGravity;
			}
		}
		public float CollisionBoxDrag
		{
			get
			{
				return CollisionBox.Drag;
			}
			set
			{
				CollisionBox.Drag = value;
			}
		}
		int mMaxNumberOfBullets = 3;
		public int MaxNumberOfBullets
		{
			set
			{
				mMaxNumberOfBullets = value;
			}
			get
			{
				return mMaxNumberOfBullets;
			}
		}

        public MegaManEntity()
            : this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {

        }

        public MegaManEntity(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public MegaManEntity(string contentManagerName, bool addToManagers) :
			base(contentManagerName, addToManagers)
		{
			// Don't delete this:
            ContentManagerName = contentManagerName;
           

		}

		protected override void InitializeEntity(bool addToManagers)
		{
			// Generated Initialize
			LoadStaticContent(ContentManagerName);
			mSpriteObject = MegaManScene.Sprites.FindByName("MegamanX").Clone();
			mCollisionBox = MegaManCollisionBox.AxisAlignedRectangles.FindByName("AxisAlignedRectangle1").Clone();
			
			base.InitializeEntity(addToManagers);


		}

// Generated AddToManagers
		public override void ReAddToManagers (Layer layerToAddTo)
		{
			base.ReAddToManagers(layerToAddTo);
			SpriteManager.AddToLayer(mSpriteObject, LayerProvidedByContainer);
			ShapeManager.AddToLayer(mCollisionBox, LayerProvidedByContainer);
		}
		public override void AddToManagers (Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			SpriteManager.AddToLayer(mSpriteObject, LayerProvidedByContainer);
			ShapeManager.AddToLayer(mCollisionBox, LayerProvidedByContainer);
			base.AddToManagers(layerToAddTo);
			CustomInitialize();
		}

		public override void Activity()
		{
			// Generated Activity
			base.Activity();
			
			CustomActivity();
			
			// After Custom Activity
		}

		public override void Destroy()
		{
			// Generated Destroy
			base.Destroy();
			
			if (SpriteObject != null)
			{
				SpriteManager.RemoveSprite(SpriteObject);
			}
			if (CollisionBox != null)
			{
				ShapeManager.Remove(CollisionBox);
			}


			CustomDestroy();
		}

		// Generated Methods
		public override void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			base.PostInitialize();
			if (mSpriteObject.Parent == null)
			{
				mSpriteObject.CopyAbsoluteToRelative();
				mSpriteObject.AttachTo(this, false);
			}
			if (mCollisionBox.Parent == null)
			{
				mCollisionBox.CopyAbsoluteToRelative();
				mCollisionBox.AttachTo(this, false);
			}
			CollisionBox.Visible = true;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public override void AddToManagersBottomUp (Layer layerToAddTo)
		{
			base.AddToManagersBottomUp(layerToAddTo);
		}
		public override void RemoveFromManagers ()
		{
			base.RemoveFromManagers();
			base.RemoveFromManagers();
			if (SpriteObject != null)
			{
				SpriteManager.RemoveSpriteOneWay(SpriteObject);
			}
			if (CollisionBox != null)
			{
				ShapeManager.RemoveOneWay(CollisionBox);
			}
		}
		public override void AssignCustomVariables (bool callOnContainedElements)
		{
			base.AssignCustomVariables(callOnContainedElements);
			if (callOnContainedElements)
			{
			}
			mCollisionBox.Visible = true;
			MovingVelocity = 75f;
			SpriteObjectCurrentChainName = "Idle";
			SpriteObjectFlipHorizontal = false;
			IsFacingRight = true;
			DashingVelocity = 175f;
			SpriteObjectIgnoreAnimationChainTextureFlip = true;
			JumpingVelocity = 200f;
			IsInAir = true;
			IsMoving = false;
			MaxFallSpeed = 130f;
			Gravity = 350f;
			CollisionBoxDrag = 0.1f;
			MaxNumberOfBullets = 3;
		}
		public override void ConvertToManuallyUpdated ()
		{
			base.ConvertToManuallyUpdated();
			this.ForceUpdateDependenciesDeep();
			SpriteManager.ConvertToManuallyUpdated(this);
			SpriteManager.ConvertToManuallyUpdated(SpriteObject);
		}
		public static new void LoadStaticContent (string contentManagerName)
		{
			if (string.IsNullOrEmpty(contentManagerName))
			{
				throw new ArgumentException("contentManagerName cannot be empty or null");
			}
			ContentManagerName = contentManagerName;
			PlayerEntity.LoadStaticContent(contentManagerName);
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
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("MegaManEntityStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamananimationchainlist.achx", ContentManagerName))
				{
					registerUnload = true;
				}
				MegaManAnimationChainList = FlatRedBallServices.Load<FlatRedBall.Graphics.Animation.AnimationChainList>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamananimationchainlist.achx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamancollisionbox.shcx", ContentManagerName))
				{
					registerUnload = true;
				}
				MegaManCollisionBox = FlatRedBallServices.Load<FlatRedBall.Math.Geometry.ShapeCollection>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamancollisionbox.shcx", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Scene>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamanscene.scnx", ContentManagerName))
				{
					registerUnload = true;
				}
				MegaManScene = FlatRedBallServices.Load<FlatRedBall.Scene>(@"content/entities/gameobjectinstances/playerentities/megamanentity/megamanscene.scnx", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("MegaManEntityStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
			}
			CustomLoadStaticContent(contentManagerName);
		}
		public static new void UnloadStaticContent ()
		{
			if (LoadedContentManagers.Count != 0)
			{
				LoadedContentManagers.RemoveAt(0);
				mRegisteredUnloads.RemoveAt(0);
			}
			if (LoadedContentManagers.Count == 0)
			{
				if (MegaManAnimationChainList != null)
				{
					MegaManAnimationChainList= null;
				}
				if (MegaManCollisionBox != null)
				{
					MegaManCollisionBox.RemoveFromManagers(ContentManagerName != "Global");
					MegaManCollisionBox= null;
				}
				if (MegaManScene != null)
				{
					MegaManScene.RemoveFromManagers(ContentManagerName != "Global");
					MegaManScene= null;
				}
			}
		}
		static VariableState mLoadingState = VariableState.Uninitialized;
		public static new VariableState LoadingState
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
				case  VariableState.Idle:
					break;
				case  VariableState.Moving:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.Shooting:
					break;
				case  VariableState.NotCharging:
					break;
				case  VariableState.Charging:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<VariableState>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (VariableState stateToStop)
		{
			switch(stateToStop)
			{
				case  VariableState.Idle:
					break;
				case  VariableState.Moving:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.Shooting:
					break;
				case  VariableState.NotCharging:
					break;
				case  VariableState.Charging:
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
				case  VariableState.Idle:
					break;
				case  VariableState.Moving:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.Shooting:
					break;
				case  VariableState.NotCharging:
					break;
				case  VariableState.Charging:
					break;
			}
			switch(secondState)
			{
				case  VariableState.Idle:
					break;
				case  VariableState.Moving:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.Shooting:
					break;
				case  VariableState.NotCharging:
					break;
				case  VariableState.Charging:
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
		public FlatRedBall.Instructions.Instruction InterpolateToState (Animation stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  Animation.IdleAnimation:
					break;
				case  Animation.MovingRightAnimation:
					break;
				case  Animation.MovingLeftAnimation:
					break;
				case  Animation.FallingRightAnimation:
					break;
				case  Animation.FallingLeftAnimation:
					break;
				case  Animation.StartMovingRightAnimation:
					break;
				case  Animation.StartMovingLeftAnimation:
					break;
				case  Animation.ClimbingAnimation:
					break;
				case  Animation.ShootingRightAnimation:
					break;
				case  Animation.DamagedRightAnimation:
					break;
				case  Animation.DamagedLeftAnimation:
					break;
				case  Animation.DashingRightAnimation:
					break;
				case  Animation.DashingLeftAnimation:
					break;
				case  Animation.IdleLowLifeAnimation:
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					break;
				case  Animation.MovingAndShootingRightAnimation:
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					break;
				case  Animation.DashingAndShootingRightAnimation:
					break;
				case  Animation.JumpRightStartAnimation:
					break;
				case  Animation.JumpLeftStartAnimation:
					break;
				case  Animation.LandingRightAnimation:
					break;
				case  Animation.LandingLeftAnimation:
					break;
				case  Animation.ShootingLeftAnimation:
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<Animation>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (Animation stateToStop)
		{
			switch(stateToStop)
			{
				case  Animation.IdleAnimation:
					break;
				case  Animation.MovingRightAnimation:
					break;
				case  Animation.MovingLeftAnimation:
					break;
				case  Animation.FallingRightAnimation:
					break;
				case  Animation.FallingLeftAnimation:
					break;
				case  Animation.StartMovingRightAnimation:
					break;
				case  Animation.StartMovingLeftAnimation:
					break;
				case  Animation.ClimbingAnimation:
					break;
				case  Animation.ShootingRightAnimation:
					break;
				case  Animation.DamagedRightAnimation:
					break;
				case  Animation.DamagedLeftAnimation:
					break;
				case  Animation.DashingRightAnimation:
					break;
				case  Animation.DashingLeftAnimation:
					break;
				case  Animation.IdleLowLifeAnimation:
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					break;
				case  Animation.MovingAndShootingRightAnimation:
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					break;
				case  Animation.DashingAndShootingRightAnimation:
					break;
				case  Animation.JumpRightStartAnimation:
					break;
				case  Animation.JumpLeftStartAnimation:
					break;
				case  Animation.LandingRightAnimation:
					break;
				case  Animation.LandingLeftAnimation:
					break;
				case  Animation.ShootingLeftAnimation:
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					break;
			}
			CurrentAnimationState = stateToStop;
		}
		public void InterpolateBetween (Animation firstState, Animation secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			bool setDashingTimer = true;
			float DashingTimerFirstValue= 0;
			float DashingTimerSecondValue= 0;
			bool setLandingTimer = true;
			float LandingTimerFirstValue= 0;
			float LandingTimerSecondValue= 0;
			switch(firstState)
			{
				case  Animation.IdleAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "Idle";
					}
					break;
				case  Animation.MovingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "MovingRight";
					}
					break;
				case  Animation.MovingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "MovingLeft";
					}
					break;
				case  Animation.FallingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "FallingRight";
					}
					if (interpolationValue < 1)
					{
						this.IsOnGround = false;
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.FallingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "FallingLeft";
					}
					if (interpolationValue < 1)
					{
						this.IsOnGround = false;
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.StartMovingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "StartMovingRight";
					}
					break;
				case  Animation.StartMovingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "StartMovingLeft";
					}
					break;
				case  Animation.ClimbingAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "Climbing";
					}
					break;
				case  Animation.ShootingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "ShootingRight";
					}
					if (interpolationValue < 1)
					{
						this.IsShooting = true;
					}
					break;
				case  Animation.DamagedRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DamagedRight";
					}
					break;
				case  Animation.DamagedLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DamagedLeft";
					}
					break;
				case  Animation.DashingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DashingRight";
					}
					DashingTimerFirstValue = 0f;
					if (interpolationValue < 1)
					{
						this.IsDashing = true;
					}
					break;
				case  Animation.DashingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DashingLeft";
					}
					DashingTimerFirstValue = 0f;
					if (interpolationValue < 1)
					{
						this.IsDashing = true;
					}
					break;
				case  Animation.IdleLowLifeAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "IdleLowLife";
					}
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "ClimbingAndShootingRight";
					}
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "ClimbingAndShootingLeft";
					}
					break;
				case  Animation.MovingAndShootingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "MovingAndShootingRight";
					}
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "MovingAndShootingLeft";
					}
					break;
				case  Animation.DashingAndShootingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DashingAndShootingRight";
					}
					break;
				case  Animation.JumpRightStartAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "JumpRightStart";
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = true;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.JumpLeftStartAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "JumpLeftStart";
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = true;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.LandingRightAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "LandingRight";
					}
					if (interpolationValue < 1)
					{
						this.IsOnGround = true;
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = false;
					}
					LandingTimerFirstValue = 0f;
					if (interpolationValue < 1)
					{
						this.IsLanding = true;
					}
					break;
				case  Animation.LandingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "LandingLeft";
					}
					if (interpolationValue < 1)
					{
						this.IsOnGround = true;
					}
					if (interpolationValue < 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue < 1)
					{
						this.IsInAir = false;
					}
					LandingTimerFirstValue = 0f;
					if (interpolationValue < 1)
					{
						this.IsLanding = true;
					}
					break;
				case  Animation.ShootingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "ShootingLeft";
					}
					if (interpolationValue < 1)
					{
						this.IsShooting = true;
					}
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					if (interpolationValue < 1)
					{
						this.SpriteObjectCurrentChainName = "DashingAndShootingLeft";
					}
					break;
			}
			switch(secondState)
			{
				case  Animation.IdleAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "Idle";
					}
					break;
				case  Animation.MovingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "MovingRight";
					}
					break;
				case  Animation.MovingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "MovingLeft";
					}
					break;
				case  Animation.FallingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "FallingRight";
					}
					if (interpolationValue >= 1)
					{
						this.IsOnGround = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.FallingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "FallingLeft";
					}
					if (interpolationValue >= 1)
					{
						this.IsOnGround = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.StartMovingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "StartMovingRight";
					}
					break;
				case  Animation.StartMovingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "StartMovingLeft";
					}
					break;
				case  Animation.ClimbingAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "Climbing";
					}
					break;
				case  Animation.ShootingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "ShootingRight";
					}
					if (interpolationValue >= 1)
					{
						this.IsShooting = true;
					}
					break;
				case  Animation.DamagedRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DamagedRight";
					}
					break;
				case  Animation.DamagedLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DamagedLeft";
					}
					break;
				case  Animation.DashingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DashingRight";
					}
					DashingTimerSecondValue = 0f;
					if (interpolationValue >= 1)
					{
						this.IsDashing = true;
					}
					break;
				case  Animation.DashingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DashingLeft";
					}
					DashingTimerSecondValue = 0f;
					if (interpolationValue >= 1)
					{
						this.IsDashing = true;
					}
					break;
				case  Animation.IdleLowLifeAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "IdleLowLife";
					}
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "ClimbingAndShootingRight";
					}
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "ClimbingAndShootingLeft";
					}
					break;
				case  Animation.MovingAndShootingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "MovingAndShootingRight";
					}
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "MovingAndShootingLeft";
					}
					break;
				case  Animation.DashingAndShootingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DashingAndShootingRight";
					}
					break;
				case  Animation.JumpRightStartAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "JumpRightStart";
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = true;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.JumpLeftStartAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "JumpLeftStart";
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = true;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = true;
					}
					break;
				case  Animation.LandingRightAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "LandingRight";
					}
					if (interpolationValue >= 1)
					{
						this.IsOnGround = true;
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = false;
					}
					LandingTimerSecondValue = 0f;
					if (interpolationValue >= 1)
					{
						this.IsLanding = true;
					}
					break;
				case  Animation.LandingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "LandingLeft";
					}
					if (interpolationValue >= 1)
					{
						this.IsOnGround = true;
					}
					if (interpolationValue >= 1)
					{
						this.IsJumping = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsInAir = false;
					}
					LandingTimerSecondValue = 0f;
					if (interpolationValue >= 1)
					{
						this.IsLanding = true;
					}
					break;
				case  Animation.ShootingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "ShootingLeft";
					}
					if (interpolationValue >= 1)
					{
						this.IsShooting = true;
					}
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectCurrentChainName = "DashingAndShootingLeft";
					}
					break;
			}
			if (setDashingTimer)
			{
				DashingTimer = DashingTimerFirstValue * (1 - interpolationValue) + DashingTimerSecondValue * interpolationValue;
			}
			if (setLandingTimer)
			{
				LandingTimer = LandingTimerFirstValue * (1 - interpolationValue) + LandingTimerSecondValue * interpolationValue;
			}
			if (interpolationValue < 1)
			{
				mCurrentAnimationState = (int)firstState;
			}
			else
			{
				mCurrentAnimationState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (Direction stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<Direction>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
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
					if (interpolationValue < 1)
					{
						this.SpriteObjectFlipHorizontal = true;
					}
					if (interpolationValue < 1)
					{
						this.IsFacingRight = false;
					}
					break;
				case  Direction.Right:
					if (interpolationValue < 1)
					{
						this.SpriteObjectFlipHorizontal = false;
					}
					if (interpolationValue < 1)
					{
						this.IsFacingRight = true;
					}
					break;
			}
			switch(secondState)
			{
				case  Direction.Left:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectFlipHorizontal = true;
					}
					if (interpolationValue >= 1)
					{
						this.IsFacingRight = false;
					}
					break;
				case  Direction.Right:
					if (interpolationValue >= 1)
					{
						this.SpriteObjectFlipHorizontal = false;
					}
					if (interpolationValue >= 1)
					{
						this.IsFacingRight = true;
					}
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentDirectionState = (int)firstState;
			}
			else
			{
				mCurrentDirectionState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (JumpType stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  JumpType.NoJump:
					break;
				case  JumpType.IdleJump:
					break;
				case  JumpType.DashJump:
					break;
				case  JumpType.RunJump:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<JumpType>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (JumpType stateToStop)
		{
			switch(stateToStop)
			{
				case  JumpType.NoJump:
					break;
				case  JumpType.IdleJump:
					break;
				case  JumpType.DashJump:
					break;
				case  JumpType.RunJump:
					break;
			}
			CurrentJumpTypeState = stateToStop;
		}
		public void InterpolateBetween (JumpType firstState, JumpType secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  JumpType.NoJump:
					break;
				case  JumpType.IdleJump:
					break;
				case  JumpType.DashJump:
					break;
				case  JumpType.RunJump:
					break;
			}
			switch(secondState)
			{
				case  JumpType.NoJump:
					break;
				case  JumpType.IdleJump:
					break;
				case  JumpType.DashJump:
					break;
				case  JumpType.RunJump:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentJumpTypeState = (int)firstState;
			}
			else
			{
				mCurrentJumpTypeState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (BodyArmor stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  BodyArmor.X1Body:
					break;
				case  BodyArmor.X2Body:
					break;
				case  BodyArmor.X3Body:
					break;
				case  BodyArmor.None:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<BodyArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (BodyArmor stateToStop)
		{
			switch(stateToStop)
			{
				case  BodyArmor.X1Body:
					break;
				case  BodyArmor.X2Body:
					break;
				case  BodyArmor.X3Body:
					break;
				case  BodyArmor.None:
					break;
			}
			CurrentBodyArmorState = stateToStop;
		}
		public void InterpolateBetween (BodyArmor firstState, BodyArmor secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  BodyArmor.X1Body:
					break;
				case  BodyArmor.X2Body:
					break;
				case  BodyArmor.X3Body:
					break;
				case  BodyArmor.None:
					break;
			}
			switch(secondState)
			{
				case  BodyArmor.X1Body:
					break;
				case  BodyArmor.X2Body:
					break;
				case  BodyArmor.X3Body:
					break;
				case  BodyArmor.None:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentBodyArmorState = (int)firstState;
			}
			else
			{
				mCurrentBodyArmorState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (LegArmor stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  LegArmor.X1Leg:
					break;
				case  LegArmor.X2Leg:
					break;
				case  LegArmor.X3Leg:
					break;
				case  LegArmor.None:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<LegArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (LegArmor stateToStop)
		{
			switch(stateToStop)
			{
				case  LegArmor.X1Leg:
					break;
				case  LegArmor.X2Leg:
					break;
				case  LegArmor.X3Leg:
					break;
				case  LegArmor.None:
					break;
			}
			CurrentLegArmorState = stateToStop;
		}
		public void InterpolateBetween (LegArmor firstState, LegArmor secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  LegArmor.X1Leg:
					break;
				case  LegArmor.X2Leg:
					break;
				case  LegArmor.X3Leg:
					break;
				case  LegArmor.None:
					break;
			}
			switch(secondState)
			{
				case  LegArmor.X1Leg:
					break;
				case  LegArmor.X2Leg:
					break;
				case  LegArmor.X3Leg:
					break;
				case  LegArmor.None:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentLegArmorState = (int)firstState;
			}
			else
			{
				mCurrentLegArmorState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (HelmetArmor stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  HelmetArmor.X1Helmet:
					break;
				case  HelmetArmor.X2Helmet:
					break;
				case  HelmetArmor.X3Helmet:
					break;
				case  HelmetArmor.None:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<HelmetArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (HelmetArmor stateToStop)
		{
			switch(stateToStop)
			{
				case  HelmetArmor.X1Helmet:
					break;
				case  HelmetArmor.X2Helmet:
					break;
				case  HelmetArmor.X3Helmet:
					break;
				case  HelmetArmor.None:
					break;
			}
			CurrentHelmetArmorState = stateToStop;
		}
		public void InterpolateBetween (HelmetArmor firstState, HelmetArmor secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  HelmetArmor.X1Helmet:
					break;
				case  HelmetArmor.X2Helmet:
					break;
				case  HelmetArmor.X3Helmet:
					break;
				case  HelmetArmor.None:
					break;
			}
			switch(secondState)
			{
				case  HelmetArmor.X1Helmet:
					break;
				case  HelmetArmor.X2Helmet:
					break;
				case  HelmetArmor.X3Helmet:
					break;
				case  HelmetArmor.None:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentHelmetArmorState = (int)firstState;
			}
			else
			{
				mCurrentHelmetArmorState = (int)secondState;
			}
		}
		public FlatRedBall.Instructions.Instruction InterpolateToState (ArmArmor stateToInterpolateTo, double secondsToTake)
		{
			switch(stateToInterpolateTo)
			{
				case  ArmArmor.X1Arm:
					break;
				case  ArmArmor.X2Arm:
					break;
				case  ArmArmor.X3Arm:
					break;
				case  ArmArmor.None:
					break;
			}
			var instruction = new FlatRedBall.Instructions.DelegateInstruction<ArmArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = FlatRedBall.TimeManager.CurrentTime + secondsToTake;
			this.Instructions.Add(instruction);
			return instruction;
		}
		public void StopStateInterpolation (ArmArmor stateToStop)
		{
			switch(stateToStop)
			{
				case  ArmArmor.X1Arm:
					break;
				case  ArmArmor.X2Arm:
					break;
				case  ArmArmor.X3Arm:
					break;
				case  ArmArmor.None:
					break;
			}
			CurrentArmArmorState = stateToStop;
		}
		public void InterpolateBetween (ArmArmor firstState, ArmArmor secondState, float interpolationValue)
		{
			#if DEBUG
			if (float.IsNaN(interpolationValue))
			{
				throw new Exception("interpolationValue cannot be NaN");
			}
			#endif
			switch(firstState)
			{
				case  ArmArmor.X1Arm:
					break;
				case  ArmArmor.X2Arm:
					break;
				case  ArmArmor.X3Arm:
					break;
				case  ArmArmor.None:
					break;
			}
			switch(secondState)
			{
				case  ArmArmor.X1Arm:
					break;
				case  ArmArmor.X2Arm:
					break;
				case  ArmArmor.X3Arm:
					break;
				case  ArmArmor.None:
					break;
			}
			if (interpolationValue < 1)
			{
				mCurrentArmArmorState = (int)firstState;
			}
			else
			{
				mCurrentArmArmorState = (int)secondState;
			}
		}
		public static void PreloadStateContent (VariableState state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  VariableState.Idle:
					break;
				case  VariableState.Moving:
					break;
				case  VariableState.Jumping:
					break;
				case  VariableState.Shooting:
					break;
				case  VariableState.NotCharging:
					break;
				case  VariableState.Charging:
					break;
			}
		}
		public static void PreloadStateContent (Animation state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  Animation.IdleAnimation:
					{
						object throwaway = "Idle";
					}
					break;
				case  Animation.MovingRightAnimation:
					{
						object throwaway = "MovingRight";
					}
					break;
				case  Animation.MovingLeftAnimation:
					{
						object throwaway = "MovingLeft";
					}
					break;
				case  Animation.FallingRightAnimation:
					{
						object throwaway = "FallingRight";
					}
					break;
				case  Animation.FallingLeftAnimation:
					{
						object throwaway = "FallingLeft";
					}
					break;
				case  Animation.StartMovingRightAnimation:
					{
						object throwaway = "StartMovingRight";
					}
					break;
				case  Animation.StartMovingLeftAnimation:
					{
						object throwaway = "StartMovingLeft";
					}
					break;
				case  Animation.ClimbingAnimation:
					{
						object throwaway = "Climbing";
					}
					break;
				case  Animation.ShootingRightAnimation:
					{
						object throwaway = "ShootingRight";
					}
					break;
				case  Animation.DamagedRightAnimation:
					{
						object throwaway = "DamagedRight";
					}
					break;
				case  Animation.DamagedLeftAnimation:
					{
						object throwaway = "DamagedLeft";
					}
					break;
				case  Animation.DashingRightAnimation:
					{
						object throwaway = "DashingRight";
					}
					break;
				case  Animation.DashingLeftAnimation:
					{
						object throwaway = "DashingLeft";
					}
					break;
				case  Animation.IdleLowLifeAnimation:
					{
						object throwaway = "IdleLowLife";
					}
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					{
						object throwaway = "ClimbingAndShootingRight";
					}
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					{
						object throwaway = "ClimbingAndShootingLeft";
					}
					break;
				case  Animation.MovingAndShootingRightAnimation:
					{
						object throwaway = "MovingAndShootingRight";
					}
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					{
						object throwaway = "MovingAndShootingLeft";
					}
					break;
				case  Animation.DashingAndShootingRightAnimation:
					{
						object throwaway = "DashingAndShootingRight";
					}
					break;
				case  Animation.JumpRightStartAnimation:
					{
						object throwaway = "JumpRightStart";
					}
					break;
				case  Animation.JumpLeftStartAnimation:
					{
						object throwaway = "JumpLeftStart";
					}
					break;
				case  Animation.LandingRightAnimation:
					{
						object throwaway = "LandingRight";
					}
					break;
				case  Animation.LandingLeftAnimation:
					{
						object throwaway = "LandingLeft";
					}
					break;
				case  Animation.ShootingLeftAnimation:
					{
						object throwaway = "ShootingLeft";
					}
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					{
						object throwaway = "DashingAndShootingLeft";
					}
					break;
			}
		}
		public static void PreloadStateContent (Direction state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  Direction.Left:
					break;
				case  Direction.Right:
					break;
			}
		}
		public static void PreloadStateContent (JumpType state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  JumpType.NoJump:
					break;
				case  JumpType.IdleJump:
					break;
				case  JumpType.DashJump:
					break;
				case  JumpType.RunJump:
					break;
			}
		}
		public static void PreloadStateContent (BodyArmor state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  BodyArmor.X1Body:
					break;
				case  BodyArmor.X2Body:
					break;
				case  BodyArmor.X3Body:
					break;
				case  BodyArmor.None:
					break;
			}
		}
		public static void PreloadStateContent (LegArmor state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  LegArmor.X1Leg:
					break;
				case  LegArmor.X2Leg:
					break;
				case  LegArmor.X3Leg:
					break;
				case  LegArmor.None:
					break;
			}
		}
		public static void PreloadStateContent (HelmetArmor state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  HelmetArmor.X1Helmet:
					break;
				case  HelmetArmor.X2Helmet:
					break;
				case  HelmetArmor.X3Helmet:
					break;
				case  HelmetArmor.None:
					break;
			}
		}
		public static void PreloadStateContent (ArmArmor state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			switch(state)
			{
				case  ArmArmor.X1Arm:
					break;
				case  ArmArmor.X2Arm:
					break;
				case  ArmArmor.X3Arm:
					break;
				case  ArmArmor.None:
					break;
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static new object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "MegaManAnimationChainList":
					return MegaManAnimationChainList;
				case  "MegaManCollisionBox":
					return MegaManCollisionBox;
				case  "MegaManScene":
					return MegaManScene;
			}
			return null;
		}
		public static new object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "MegaManAnimationChainList":
					return MegaManAnimationChainList;
				case  "MegaManCollisionBox":
					return MegaManCollisionBox;
				case  "MegaManScene":
					return MegaManScene;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "MegaManAnimationChainList":
					return MegaManAnimationChainList;
				case  "MegaManCollisionBox":
					return MegaManCollisionBox;
				case  "MegaManScene":
					return MegaManScene;
			}
			return null;
		}
		public override void SetToIgnorePausing ()
		{
			base.SetToIgnorePausing();
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(SpriteObject);
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(CollisionBox);
		}
		public override void MoveToLayer (Layer layerToMoveTo)
		{
			base.MoveToLayer(layerToMoveTo);
			if (LayerProvidedByContainer != null)
			{
				LayerProvidedByContainer.Remove(SpriteObject);
			}
			SpriteManager.AddToLayer(SpriteObject, layerToMoveTo);
			LayerProvidedByContainer = layerToMoveTo;
		}

    }
	
	
	// Extra classes
	
}
