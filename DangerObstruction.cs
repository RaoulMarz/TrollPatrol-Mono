using Godot;
using System;

public class DangerObstruction : Area2D
{
	private Sprite spriteObstruction;
	private CollisionPolygon collisionZone;

	[Signal]
	public delegate void TrollHurtZoneEnter();
	[Signal]
	public delegate void TrollHurtZoneExit();

	private void _on_Obstruction_Body_Enter(PhysicsBody2D body)
	{
		GD.Print($"DangerObstruction, _on_Obstruction_Body_Enter() called, body={body}, name={body.Name}");
		if (body.Name == "Troll")
		{
			EmitSignal(nameof(TrollHurtZoneEnter));
		}
	}

	private void _on_Obstruction_Body_Exit(PhysicsBody2D body)
	{
		GD.Print($"DangerObstruction, _on_Obstruction_Body_Exit() called, body={body}, name={body.Name}");
		if (body.Name == "Troll")
		{
			EmitSignal(nameof(TrollHurtZoneExit));
		}
	}

	// Called when the node enters the scene tree for the first time.
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

//  public override void _Process(float delta)
//  {
//      
//  }
}
