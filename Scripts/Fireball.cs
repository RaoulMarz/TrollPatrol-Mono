using Godot;
using System;

namespace TrollSmasher
{
	public class Fireball : KinematicBody2D
	{
		private AnimatedSprite animatedFireball;
		private float lifeTime;
		private Path2D trackPath;
		private PathFollow2D followPathTracker;
		public bool Started { get; set; } = false;

		public override void _Ready()
		{

		}

		public void StartAnimation(Path2D fireFollowPath)
		{
			Started = true;
			trackPath = fireFollowPath;
			followPathTracker = new PathFollow2D();
			trackPath.AddChild(followPathTracker);
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
