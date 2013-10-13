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

namespace MegaManXSS.Entities.GameObjectInstances.PlayerEntities
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
		public enum Animation
		{
			Uninitialized, //This exists so that the first set call actually does something
			IdleAnimation, 
			MovingRightAnimation, 
			MovingLeftAnimation, 
			FallingRightAnimation, 
			FallingLeftAnimation, 
			StartMovingRightAnimation, 
			StartMovingLeftAnimation, 
			ClimbingAnimation, 
			ShootingRightAnimation, 
			DamagedRightAnimation, 
			DamagedLeftAnimation, 
			DashingRightAnimation, 
			DashingLeftAnimation, 
			IdleLowLifeAnimation, 
			ClimbingAndShootingRightAnimation, 
			ClimbingAndShootingLeftAnimation, 
			MovingAndShootingRightAnimation, 
			MovingAndShootingLeftAnimation, 
			DashingAndShootingRightAnimation, 
			JumpRightStartAnimation, 
			JumpLeftStartAnimation, 
			LandingRightAnimation, 
			LandingLeftAnimation, 
			ShootingLeftAnimation, 
			DashingAndShootingLeftAnimation
		}
		Animation mCurrentAnimationState = Animation.Uninitialized;
		public Animation CurrentAnimationState
		{
			get
			{
				return mCurrentAnimationState;
			}
			set
			{
				mCurrentAnimationState = value;
				switch(mCurrentAnimationState)
				{
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
			Uninitialized, //This exists so that the first set call actually does something
			NoJump, 
			IdleJump, 
			DashJump, 
			RunJump
		}
		JumpType mCurrentJumpTypeState = JumpType.Uninitialized;
		public JumpType CurrentJumpTypeState
		{
			get
			{
				return mCurrentJumpTypeState;
			}
			set
			{
				mCurrentJumpTypeState = value;
				switch(mCurrentJumpTypeState)
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
		}
		public enum BodyArmor
		{
			Uninitialized, //This exists so that the first set call actually does something
			X1Body, 
			X2Body, 
			X3Body, 
			None
		}
		BodyArmor mCurrentBodyArmorState = BodyArmor.Uninitialized;
		public BodyArmor CurrentBodyArmorState
		{
			get
			{
				return mCurrentBodyArmorState;
			}
			set
			{
				mCurrentBodyArmorState = value;
				switch(mCurrentBodyArmorState)
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
		}
		public enum LegArmor
		{
			Uninitialized, //This exists so that the first set call actually does something
			X1Leg, 
			X2Leg, 
			X3Leg, 
			None
		}
		LegArmor mCurrentLegArmorState = LegArmor.Uninitialized;
		public LegArmor CurrentLegArmorState
		{
			get
			{
				return mCurrentLegArmorState;
			}
			set
			{
				mCurrentLegArmorState = value;
				switch(mCurrentLegArmorState)
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
		}
		public enum HelmetArmor
		{
			Uninitialized, //This exists so that the first set call actually does something
			X1Helmet, 
			X2Helmet, 
			X3Helmet, 
			None
		}
		HelmetArmor mCurrentHelmetArmorState = HelmetArmor.Uninitialized;
		public HelmetArmor CurrentHelmetArmorState
		{
			get
			{
				return mCurrentHelmetArmorState;
			}
			set
			{
				mCurrentHelmetArmorState = value;
				switch(mCurrentHelmetArmorState)
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
		}
		public enum ArmArmor
		{
			Uninitialized, //This exists so that the first set call actually does something
			X1Arm, 
			X2Arm, 
			X3Arm, 
			None
		}
		ArmArmor mCurrentArmArmorState = ArmArmor.Uninitialized;
		public ArmArmor CurrentArmArmorState
		{
			get
			{
				return mCurrentArmArmorState;
			}
			set
			{
				mCurrentArmArmorState = value;
				switch(mCurrentArmArmorState)
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
		}
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		private static FlatRedBall.Graphics.Animation.AnimationChainList MegaManAnimationChainList;
		private static FlatRedBall.Math.Geometry.ShapeCollection MegaManCollisionBox;
		private static FlatRedBall.Scene MegaManScene;
		
		private FlatRedBall.Sprite mSpriteObject;
		public FlatRedBall.Sprite SpriteObject
		{
			get
			{
				return mSpriteObject;
			}
		}
		private FlatRedBall.Math.Geometry.AxisAlignedRectangle mCollisionBox;
		public FlatRedBall.Math.Geometry.AxisAlignedRectangle CollisionBox
		{
			get
			{
				return mCollisionBox;
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
		public virtual bool IsMoving { get; set; }
		public virtual float MaxFallSpeed { get; set; }
		public virtual float Gravity { get; set; }
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
		public virtual int MaxNumberOfBullets { get; set; }
		public int Index { get; set; }
		public bool Used { get; set; }

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
		public override void AddToManagers (Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
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
				SpriteObject.Detach(); SpriteManager.RemoveSprite(SpriteObject);
			}
			if (CollisionBox != null)
			{
				CollisionBox.Detach(); ShapeManager.Remove(CollisionBox);
			}


			CustomDestroy();
		}

		// Generated Methods
		public override void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			base.PostInitialize();
			if (mSpriteObject!= null && mSpriteObject.Parent == null)
			{
				mSpriteObject.CopyAbsoluteToRelative();
				mSpriteObject.AttachTo(this, false);
			}
			if (mCollisionBox!= null && mCollisionBox.Parent == null)
			{
				mCollisionBox.CopyAbsoluteToRelative();
				mCollisionBox.AttachTo(this, false);
			}
			CollisionBox.Visible = true;
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
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public override void AddToManagersBottomUp (Layer layerToAddTo)
		{
			base.AddToManagersBottomUp(layerToAddTo);
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
			SpriteManager.AddToLayer(mSpriteObject, layerToAddTo);
			ShapeManager.AddToLayer(mCollisionBox, layerToAddTo);
			mCollisionBox.Visible = true;
			X = oldX;
			Y = oldY;
			Z = oldZ;
			RotationX = oldRotationX;
			RotationY = oldRotationY;
			RotationZ = oldRotationZ;
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
		public Instruction InterpolateToState (Animation stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<Animation>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public Instruction InterpolateToState (JumpType stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<JumpType>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public Instruction InterpolateToState (BodyArmor stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<BodyArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public Instruction InterpolateToState (LegArmor stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<LegArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public Instruction InterpolateToState (HelmetArmor stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<HelmetArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public Instruction InterpolateToState (ArmArmor stateToInterpolateTo, double secondsToTake)
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
			var instruction = new DelegateInstruction<ArmArmor>(StopStateInterpolation, stateToInterpolateTo);
			instruction.TimeToExecute = TimeManager.CurrentTime + secondsToTake;
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
		}
		public static void PreloadStateContent (Animation state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			object throwaway;
			switch(state)
			{
				case  Animation.IdleAnimation:
					throwaway = "Idle";
					break;
				case  Animation.MovingRightAnimation:
					throwaway = "MovingRight";
					break;
				case  Animation.MovingLeftAnimation:
					throwaway = "MovingLeft";
					break;
				case  Animation.FallingRightAnimation:
					throwaway = "FallingRight";
					break;
				case  Animation.FallingLeftAnimation:
					throwaway = "FallingLeft";
					break;
				case  Animation.StartMovingRightAnimation:
					throwaway = "StartMovingRight";
					break;
				case  Animation.StartMovingLeftAnimation:
					throwaway = "StartMovingLeft";
					break;
				case  Animation.ClimbingAnimation:
					throwaway = "Climbing";
					break;
				case  Animation.ShootingRightAnimation:
					throwaway = "ShootingRight";
					break;
				case  Animation.DamagedRightAnimation:
					throwaway = "DamagedRight";
					break;
				case  Animation.DamagedLeftAnimation:
					throwaway = "DamagedLeft";
					break;
				case  Animation.DashingRightAnimation:
					throwaway = "DashingRight";
					break;
				case  Animation.DashingLeftAnimation:
					throwaway = "DashingLeft";
					break;
				case  Animation.IdleLowLifeAnimation:
					throwaway = "IdleLowLife";
					break;
				case  Animation.ClimbingAndShootingRightAnimation:
					throwaway = "ClimbingAndShootingRight";
					break;
				case  Animation.ClimbingAndShootingLeftAnimation:
					throwaway = "ClimbingAndShootingLeft";
					break;
				case  Animation.MovingAndShootingRightAnimation:
					throwaway = "MovingAndShootingRight";
					break;
				case  Animation.MovingAndShootingLeftAnimation:
					throwaway = "MovingAndShootingLeft";
					break;
				case  Animation.DashingAndShootingRightAnimation:
					throwaway = "DashingAndShootingRight";
					break;
				case  Animation.JumpRightStartAnimation:
					throwaway = "JumpRightStart";
					break;
				case  Animation.JumpLeftStartAnimation:
					throwaway = "JumpLeftStart";
					break;
				case  Animation.LandingRightAnimation:
					throwaway = "LandingRight";
					break;
				case  Animation.LandingLeftAnimation:
					throwaway = "LandingLeft";
					break;
				case  Animation.ShootingLeftAnimation:
					throwaway = "ShootingLeft";
					break;
				case  Animation.DashingAndShootingLeftAnimation:
					throwaway = "DashingAndShootingLeft";
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
		public static void PreloadStateContent (JumpType state, string contentManagerName)
		{
			ContentManagerName = contentManagerName;
			object throwaway;
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
			object throwaway;
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
			object throwaway;
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
			object throwaway;
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
			object throwaway;
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
			InstructionManager.IgnorePausingFor(SpriteObject);
			InstructionManager.IgnorePausingFor(CollisionBox);
		}

    }
	
	
	// Extra classes
	public static class MegaManEntityExtensionMethods
	{
	}
	
}
