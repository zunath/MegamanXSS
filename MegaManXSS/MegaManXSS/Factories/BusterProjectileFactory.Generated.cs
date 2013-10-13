using MegaManXSS.Entities.GameObjectInstances.ProjectileEntities.PlayerProjectiles;
using System;
using FlatRedBall.Math;
using FlatRedBall.Graphics;
using MegaManXSS.Performance;

namespace MegaManXSS.Factories
{
	public static class BusterProjectileFactory
	{
		public static string mContentManagerName;
		static PositionedObjectList<BusterProjectile> mScreenListReference;
		static PoolList<BusterProjectile> mPool = new PoolList<BusterProjectile>();
		public static Action<BusterProjectile> EntitySpawned;
		public static BusterProjectile CreateNew ()
		{
			return CreateNew(null);
		}
		public static BusterProjectile CreateNew (Layer layer)
		{
			if (string.IsNullOrEmpty(mContentManagerName))
			{
				throw new System.Exception("You must first initialize the factory to use it.");
			}
			BusterProjectile instance = null;
			instance = mPool.GetNextAvailable();
			if (instance == null)
			{
				mPool.AddToPool(new BusterProjectile(mContentManagerName, false));
				instance =  mPool.GetNextAvailable();
			}
			instance.AddToManagers(layer);
			if (mScreenListReference != null)
			{
				mScreenListReference.Add(instance);
			}
			if (EntitySpawned != null)
			{
				EntitySpawned(instance);
			}
			return instance;
		}
		
		public static void Initialize (PositionedObjectList<BusterProjectile> listFromScreen, string contentManager)
		{
			mContentManagerName = contentManager;
			mScreenListReference = listFromScreen;
			FactoryInitialize();
		}
		
		public static void Destroy ()
		{
			mContentManagerName = null;
			mScreenListReference = null;
			mPool.Clear();
			EntitySpawned = null;
		}
		
		private static void FactoryInitialize ()
		{
			const int numberToPreAllocate = 20;
			for (int i = 0; i < numberToPreAllocate; i++)
			{
				BusterProjectile instance = new BusterProjectile(mContentManagerName, false);
				mPool.AddToPool(instance);
			}
		}
		
		/// <summary>
		/// Makes the argument objectToMakeUnused marked as unused.  This method is generated to be used
		/// by generated code.  Use Destroy instead when writing custom code so that your code will behave
		/// the same whether your Entity is pooled or not.
		/// </summary>
		public static void MakeUnused (BusterProjectile objectToMakeUnused)
		{
			MakeUnused(objectToMakeUnused, true);
		}
		
		/// <summary>
		/// Makes the argument objectToMakeUnused marked as unused.  This method is generated to be used
		/// by generated code.  Use Destroy instead when writing custom code so that your code will behave
		/// the same whether your Entity is pooled or not.
		/// </summary>
		public static void MakeUnused (BusterProjectile objectToMakeUnused, bool callDestroy)
		{
			mPool.MakeUnused(objectToMakeUnused);
			
			if (callDestroy)
			{
				objectToMakeUnused.Destroy();
			}
		}
		
		
	}
}
