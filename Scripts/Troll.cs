using Godot;
using System;
using System.Collections.Generic;
using TrollSmasher.Enums;

namespace TrollSmasher
{
	public struct CharMoveRecord
	{
		DateTime timestamp;
		Vector2 position;

		public CharMoveRecord(DateTime timestampParam, Vector2 positionParam) {
			timestamp = timestampParam;
			position = positionParam;
		}
	}

	public class Troll : KinematicBody2D
	{
		private Vector2 motion = new Vector2();
		private CollisionShape2D collisionShape;
		public Sprite spriteTroll;
		public Camera2D followCamera;
		const int MOTION_SPEED_WALK = 120;
		//private MovementEntity movementEntity = MovementEntity.MOVEMENT_ENTITY_NONE;
		private List<CharMoveRecord> listTrollMoveHistory = new List<CharMoveRecord>();
		private Vector2 playerLastSyncPosition = new Vector2(100, 100);

		[Signal]
		public delegate void DealDamage(int damagePoints);

		public override void _Ready()
		{
			collisionShape = this.GetNodeOrNull<CollisionShape2D>("collisionShape");
			if (collisionShape != null)
			{
				
			}
			spriteTroll = this.GetNodeOrNull<Sprite>("spriteTroll");
			if (spriteTroll != null)
			{

			}
			followCamera = this.GetNodeOrNull<Camera2D>("cameraFollow");
			if (followCamera != null)
			{

			}
			//Connect(nameof(DealDamage), body, "UpdateHealth");
		}

		private void AppendPlayerPosition(Vector2 position)
		{
			if (listTrollMoveHistory != null)
			{
				GD.Print($"Troll, AppendPlayerPosition(), position={position}");
				if (listTrollMoveHistory.Count >= 10) {
					List<CharMoveRecord> tempRemoveList = new List<CharMoveRecord>();
					int cutoff = 0;
					foreach (CharMoveRecord recPos in listTrollMoveHistory)
					{
						cutoff += 1;
						if (cutoff >= 6)
							tempRemoveList.Add(recPos);
					}
					foreach (CharMoveRecord recPos in tempRemoveList)
						listTrollMoveHistory.Remove(recPos);
				}
				listTrollMoveHistory.Add(new CharMoveRecord(DateTime.Now, position));
			}
		}

		public override void _Process(float delta)
		{
			if (GameState.AppGameStatus == GameStatus.GAME_STATUS_PLAY)
			{
				if (GameState.movementEntity == MovementEntity.MOVEMENT_ENTITY_PLAYERCHARACTER)
				{
					// GD.Print($"Troll, _Process(), movementEntity={GameState.movementEntity}");
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
					}

					//MOVE UP
					if (Input.IsActionPressed("move_up"))
					{
						motion = new Vector2(0, -1);
						moving = true;
					}

					//MOVE DOWN
					if (Input.IsActionPressed("move_bottom"))
					{
						motion = new Vector2(0, 1);
						moving = true;
					}

					if (moving)
					{
						motion = motion.Normalized() * MOTION_SPEED_WALK;
						this.MoveAndSlide(motion);
						if (Position.DistanceTo(playerLastSyncPosition) >= 30.0f) {
							AppendPlayerPosition(Position);
							playerLastSyncPosition = Position;
						}
					}
				}
			}
		}

		public void HitObstruction(string obsType, int damage)
		{
			GD.Print($"Troll, [Signal] HitObstruction(), obsType={obsType}");
			//int damage = -10;
			//Connect(nameof(DealDamage), body, "UpdateHealth");
			EmitSignal(nameof(DealDamage), damage);
		}

		public void LeaveObstruction(string obsType)
		{
			GD.Print($"Troll, [Signal] LeaveObstruction(), obsType={obsType}");
		}

		public void CameraShake()
		{
			if (followCamera != null)
			{
				Vector2 shakeFudge = new Vector2(20.0f, 15.0f);
				followCamera.Translate(shakeFudge);
			}
		}
	}
}
