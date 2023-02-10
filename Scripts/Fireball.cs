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
		private DateTime syncFrameReference = DateTime.Now;
		private int frameCounter = 0;
		private float progressDelta;

		public override void _Ready()
		{
			progressDelta = 0.025f;
		}

		public void StartAnimation(Path2D fireFollowPath)
		{
			Started = true;
			trackPath = fireFollowPath;
			followPathTracker = new PathFollow2D();
			trackPath.AddChild(followPathTracker);
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			if (Started)
			{
				var frameSpanDifference = DateTime.Now - syncFrameReference;
				if (frameSpanDifference.Milliseconds >= 50)
				{
					syncFrameReference = DateTime.Now;
					var progressMark = progressDelta * frameCounter;
					followPathTracker.UnitOffset = progressMark;
					frameCounter += 1;
					if (progressMark >= 1.0f)
					{
						Started = false;
					}
				}
			}
		}
	}
}
