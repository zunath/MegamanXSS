
namespace MegaManXSS.DataTypes
{
	public partial class MovementValues
	{
		public string Name;
		public float MaxSpeedX;
		public float AccelerationTimeX;
		public float DecelerationTimeX;
		public float Gravity;
		public float MaxFallSpeed;
		public float JumpVelocity;
		public float JumpApplyLength;
		public bool JumpApplyByButtonHold;
		public const string AccelerationOnGround = "AccelerationOnGround";
		public const string AccelerationBeforeDoubleJump = "AccelerationBeforeDoubleJump";
		public const string AccelerationInAir = "AccelerationInAir";
		public const string Water = "Water";
		public const string ImmediateVelocityOnGround = "ImmediateVelocityOnGround";
		public const string ImmediateVelocityBeforeDoubleJump = "ImmediateVelocityBeforeDoubleJump";
		public const string ImmediateVelocityInAir = "ImmediateVelocityInAir";
		public static System.Collections.Generic.List<System.String> OrderedList = new System.Collections.Generic.List<System.String>
		{
		AccelerationOnGround
		,AccelerationBeforeDoubleJump
		,AccelerationInAir
		,Water
		,ImmediateVelocityOnGround
		,ImmediateVelocityBeforeDoubleJump
		,ImmediateVelocityInAir
		};
		
		
	}
}
