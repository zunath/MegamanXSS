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
using Microsoft.Xna.Framework.Graphics;

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

namespace MegaManXSS.Entities.Debugging
{
	public partial class DebugWindowEntity : PositionedObject, IDestroyable
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
		static object mLockObject = new object();
		static List<string> mRegisteredUnloads = new List<string>();
		static List<string> LoadedContentManagers = new List<string>();
		protected static FlatRedBall.Graphics.BitmapFont debugFont;
		protected static Microsoft.Xna.Framework.Graphics.Texture2D debugFont_0;
		
		private FlatRedBall.Graphics.Text DebugWindowObject;
		bool mIsEnabled = true;
		public bool IsEnabled
		{
			set
			{
				mIsEnabled = value;
			}
			get
			{
				return mIsEnabled;
			}
		}
		public virtual string DebugMessage { get; set; }
		protected Layer LayerProvidedByContainer = null;

        public DebugWindowEntity()
            : this(FlatRedBall.Screens.ScreenManager.CurrentScreen.ContentManagerName, true)
        {

        }

        public DebugWindowEntity(string contentManagerName) :
            this(contentManagerName, true)
        {
        }


        public DebugWindowEntity(string contentManagerName, bool addToManagers) :
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
			DebugWindowObject = new FlatRedBall.Graphics.Text();
			DebugWindowObject.Name = "DebugWindowObject";
			
			PostInitialize();
			if (addToManagers)
			{
				AddToManagers(null);
			}


		}

// Generated AddToManagers
		public virtual void ReAddToManagers (Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			SpriteManager.AddPositionedObject(this);
			TextManager.AddToLayer(DebugWindowObject, LayerProvidedByContainer);
			if (DebugWindowObject.Font != null)
			{
				DebugWindowObject.SetPixelPerfectScale(LayerProvidedByContainer);
			}
		}
		public virtual void AddToManagers (Layer layerToAddTo)
		{
			LayerProvidedByContainer = layerToAddTo;
			SpriteManager.AddPositionedObject(this);
			TextManager.AddToLayer(DebugWindowObject, LayerProvidedByContainer);
			if (DebugWindowObject.Font != null)
			{
				DebugWindowObject.SetPixelPerfectScale(LayerProvidedByContainer);
			}
			AddToManagersBottomUp(layerToAddTo);
			CustomInitialize();
		}

		public virtual void Activity()
		{
			// Generated Activity
			
			CustomActivity();
			
			// After Custom Activity
		}

		public virtual void Destroy()
		{
			// Generated Destroy
			SpriteManager.RemovePositionedObject(this);
			
			if (DebugWindowObject != null)
			{
				TextManager.RemoveText(DebugWindowObject);
			}


			CustomDestroy();
		}

		// Generated Methods
		public virtual void PostInitialize ()
		{
			bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.CopyAbsoluteToRelative();
				DebugWindowObject.AttachTo(this, false);
			}
			DebugWindowObject.Font = debugFont;
			DebugWindowObject.NewLineDistance = 5f;
			DebugWindowObject.Scale = 5f;
			DebugWindowObject.Spacing = 5f;
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.X = 50f;
			}
			else
			{
				DebugWindowObject.RelativeX = 50f;
			}
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.Y = 50f;
			}
			else
			{
				DebugWindowObject.RelativeY = 50f;
			}
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.Z = 0f;
			}
			else
			{
				DebugWindowObject.RelativeZ = 0f;
			}
			FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
		}
		public virtual void AddToManagersBottomUp (Layer layerToAddTo)
		{
			AssignCustomVariables(false);
		}
		public virtual void RemoveFromManagers ()
		{
			SpriteManager.ConvertToManuallyUpdated(this);
			if (DebugWindowObject != null)
			{
				TextManager.RemoveTextOneWay(DebugWindowObject);
			}
		}
		public virtual void AssignCustomVariables (bool callOnContainedElements)
		{
			if (callOnContainedElements)
			{
			}
			DebugWindowObject.Font = debugFont;
			DebugWindowObject.NewLineDistance = 5f;
			DebugWindowObject.Scale = 5f;
			DebugWindowObject.Spacing = 5f;
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.X = 50f;
			}
			else
			{
				DebugWindowObject.RelativeX = 50f;
			}
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.Y = 50f;
			}
			else
			{
				DebugWindowObject.RelativeY = 50f;
			}
			if (DebugWindowObject.Parent == null)
			{
				DebugWindowObject.Z = 0f;
			}
			else
			{
				DebugWindowObject.RelativeZ = 0f;
			}
			IsEnabled = true;
		}
		public virtual void ConvertToManuallyUpdated ()
		{
			this.ForceUpdateDependenciesDeep();
			SpriteManager.ConvertToManuallyUpdated(this);
			TextManager.ConvertToManuallyUpdated(DebugWindowObject);
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
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DebugWindowEntityStaticUnload", UnloadStaticContent);
						mRegisteredUnloads.Add(ContentManagerName);
					}
				}
				if (!FlatRedBallServices.IsLoaded<FlatRedBall.Graphics.BitmapFont>(@"content/entities/debugging/debugwindowentity/debugfont.fnt", ContentManagerName))
				{
					registerUnload = true;
				}
				debugFont = FlatRedBallServices.Load<FlatRedBall.Graphics.BitmapFont>(@"content/entities/debugging/debugwindowentity/debugfont.fnt", ContentManagerName);
				if (!FlatRedBallServices.IsLoaded<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/entities/debugging/debugwindowentity/debugfont_0.png", ContentManagerName))
				{
					registerUnload = true;
				}
				debugFont_0 = FlatRedBallServices.Load<Microsoft.Xna.Framework.Graphics.Texture2D>(@"content/entities/debugging/debugwindowentity/debugfont_0.png", ContentManagerName);
			}
			if (registerUnload && ContentManagerName != FlatRedBallServices.GlobalContentManager)
			{
				lock (mLockObject)
				{
					if (!mRegisteredUnloads.Contains(ContentManagerName) && ContentManagerName != FlatRedBallServices.GlobalContentManager)
					{
						FlatRedBallServices.GetContentManagerByName(ContentManagerName).AddUnloadMethod("DebugWindowEntityStaticUnload", UnloadStaticContent);
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
				if (debugFont != null)
				{
					debugFont= null;
				}
				if (debugFont_0 != null)
				{
					debugFont_0= null;
				}
			}
		}
		[System.Obsolete("Use GetFile instead")]
		public static object GetStaticMember (string memberName)
		{
			switch(memberName)
			{
				case  "debugFont":
					return debugFont;
				case  "debugFont_0":
					return debugFont_0;
			}
			return null;
		}
		public static object GetFile (string memberName)
		{
			switch(memberName)
			{
				case  "debugFont":
					return debugFont;
				case  "debugFont_0":
					return debugFont_0;
			}
			return null;
		}
		object GetMember (string memberName)
		{
			switch(memberName)
			{
				case  "debugFont":
					return debugFont;
				case  "debugFont_0":
					return debugFont_0;
			}
			return null;
		}
		protected bool mIsPaused;
		public override void Pause (FlatRedBall.Instructions.InstructionList instructions)
		{
			base.Pause(instructions);
			mIsPaused = true;
		}
		public virtual void SetToIgnorePausing ()
		{
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(this);
			FlatRedBall.Instructions.InstructionManager.IgnorePausingFor(DebugWindowObject);
		}
		public virtual void MoveToLayer (Layer layerToMoveTo)
		{
			if (LayerProvidedByContainer != null)
			{
				LayerProvidedByContainer.Remove(DebugWindowObject);
			}
			TextManager.AddToLayer(DebugWindowObject, layerToMoveTo);
			LayerProvidedByContainer = layerToMoveTo;
		}

    }
	
	
	// Extra classes
	
}
