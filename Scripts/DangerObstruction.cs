using Godot;
using System;

namespace TrollSmasher
{
	public class DangerObstruction : Area2D
	{
		private Sprite spriteObstruction;
		private CollisionPolygon collisionZone;
		private string obstructionType = "default";
		private int damageDealt = -10;

		[Signal]
		public delegate void TrollHurtZoneEnter(string obstructionType, int damage);
		[Signal]
		public delegate void TrollHurtZoneExit(string obstructionType);

		private void _on_Obstruction_Body_Enter(PhysicsBody2D body)
		{
			GD.Print($"DangerObstruction, _on_Obstruction_Body_Enter() called, body={body}, name={body.Name}");
			if (body.Name == "Troll")
			{
				if (body.HasMethod("HitObstruction"))
				{
					if (!IsConnected(nameof(TrollHurtZoneEnter), body, "HitObstruction"))
						Connect(nameof(TrollHurtZoneEnter), body, "HitObstruction");
					EmitSignal(nameof(TrollHurtZoneEnter), obstructionType, damageDealt);
				}
			}
		}

		private void _on_Obstruction_Body_Exit(PhysicsBody2D body)
		{
			GD.Print($"DangerObstruction, _on_Obstruction_Body_Exit() called, body={body}, name={body.Name}");
			if (body.Name == "Troll")
			{
				if (body.HasMethod("LeaveObstruction"))
				{
					if (!IsConnected(nameof(TrollHurtZoneExit), body, "LeaveObstruction"))
						Connect(nameof(TrollHurtZoneExit), body, "LeaveObstruction");
					EmitSignal(nameof(TrollHurtZoneExit), obstructionType);
				}
			}
		}

		public override void _Ready()
		{
			collisionZone = this.GetNodeOrNull<CollisionPolygon>("collision-Zone");
			if (collisionZone != null)
			{
				//TO DO
				//Alter the shape for this character, using a polygon
			}
			spriteObstruction = this.GetNodeOrNull<Sprite>("sprite-Obstruction");
			if (spriteObstruction != null)
			{
				//spriteObstruction.Connect("", this, nameof(_on_Obstruction_Body_Enter));
			}
			Connect("body_entered", this, nameof(_on_Obstruction_Body_Enter));
			Connect("body_exited", this, nameof(_on_Obstruction_Body_Exit));
		}

		public void SetObstructionType(string obstructionTypeParam)
		{
			obstructionType = obstructionTypeParam;
		}

		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
