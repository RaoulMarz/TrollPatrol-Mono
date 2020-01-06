using Godot;
using System;
using TrollPatrolMono.Enums;

namespace TrollPatrolMono
{
 
	public class Troll : KinematicBody2D
	{
		private Vector2 motion = new Vector2();
		private CollisionShape2D collisionShape;
		const int MOTION_SPEED_WALK = 160;
		private MovementEntity movementEntity = MovementEntity.MOVEMENT_ENTITY_NONE;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			collisionShape = this.GetNodeOrNull<CollisionShape2D>("collisionShape");
			if (collisionShape != null)
			{
				//TO DO
				//Alter the shape for this character, using a polygon
			}
		}

		public override void _Process(float delta)
		{
			if (GameState.AppGameStatus == GameStatus.GAME_STATUS_PLAY)
			{
				if (movementEntity == MovementEntity.MOVEMENT_ENTITY_PLAYERCHARACTER)
				{
					bool moving = false;
					//MOVE LEFT
					if (Input.IsActionPressed("move_left"))
					{
						motion += new Vector2(-1, 0);
						moving = true;
						GD.Print("move_left pressed");
					}

					//MOVE RIGHT
					if (Input.IsActionPressed("move_right"))
					{
						motion += new Vector2(1, 0);
						moving = true;
						GD.Print("move_right pressed");
					}

					//MOVE UP
					if (Input.IsActionPressed("move_up"))
					{
						motion += new Vector2(0, -1);
						moving = true;
						GD.Print("move_up pressed");
					}

					//MOVE DOWN
					if (Input.IsActionPressed("move_bottom"))
					{
						motion += new Vector2(0, 1);
						moving = true;
						GD.Print("move_bottom pressed");
					}

					if (moving)
					{
						motion = motion.Normalized() * MOTION_SPEED_WALK;
						this.MoveAndSlide(motion);
					}
				}
			}
		}
	}
}
