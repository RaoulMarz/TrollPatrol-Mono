using Godot;
using System;
using TrollPatrolMono.Enums;

namespace TrollPatrolMono
{
 
	public class Troll : KinematicBody2D
	{
		private Vector2 motion = new Vector2();
		private CollisionShape2D collisionShape;
		public Sprite spriteTroll;
		public Camera2D followCamera;
		const int MOTION_SPEED_WALK = 120;
		//private MovementEntity movementEntity = MovementEntity.MOVEMENT_ENTITY_NONE;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			collisionShape = this.GetNodeOrNull<CollisionShape2D>("collisionShape");
			if (collisionShape != null)
			{
				//TO DO
				//Alter the shape for this character, using a polygon
			}
			spriteTroll = this.GetNodeOrNull<Sprite>("spriteTroll");
			if (spriteTroll != null)
			{

			}
			followCamera = this.GetNodeOrNull<Camera2D>("cameraFollow");
			if (followCamera != null)
			{

			}
		}

		public override void _Process(float delta)
		{
			if (GameState.AppGameStatus == GameStatus.GAME_STATUS_PLAY)
			{
				if (GameState.movementEntity == MovementEntity.MOVEMENT_ENTITY_PLAYERCHARACTER)
				{
					// qGD.Print($"Troll, _Process(), movementEntity={GameState.movementEntity}");
					bool moving = false;
					//MOVE LEFT
					if (Input.IsActionPressed("move_left"))
					{
						/* += */
						motion = new Vector2(-1, 0);
						moving = true;
						//GD.Print("move_left pressed");
					}

					//MOVE RIGHT
					if (Input.IsActionPressed("move_right"))
					{
						motion = new Vector2(1, 0);
						moving = true;
						//GD.Print("move_right pressed");
					}

					//MOVE UP
					if (Input.IsActionPressed("move_up"))
					{
						motion = new Vector2(0, -1);
						moving = true;
						//GD.Print("move_up pressed");
					}

					//MOVE DOWN
					if (Input.IsActionPressed("move_bottom"))
					{
						motion = new Vector2(0, 1);
						moving = true;
						//GD.Print("move_bottom pressed");
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
