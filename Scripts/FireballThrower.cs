using Godot;
using System;

namespace TrollSmasher
{
	public class FireballThrower : Node2D
	{
		private Path2D walkPath;
		private PathFollow2D pathFollower;
		private Fireball fireballChild;
		private Timer animateTimer;
		public bool Started { get; set; } = false;
		public float _duration = 2.5f;
		public float duration { get => _duration; 
			set {
			if ((value > 0) && (value < 13))
			{
				_duration = value;
			}
			} }
		private float trackPathRatio = 0.0f;
		private int timerCounter = 0;
		private float timer_period = 0.05f;

		public override void _Ready()
		{
			walkPath = this.GetNodeOrNull<Path2D>("Path2D");
			pathFollower = walkPath.GetNodeOrNull<PathFollow2D>("PathFollow2D");
			fireballChild = pathFollower.GetNodeOrNull<Fireball>("Fireball");
			animateTimer = new Timer();
			animateTimer.WaitTime = timer_period;
			animateTimer.Autostart = false;
			animateTimer.Connect("timeout", this, nameof(_on_AnimateTimer_timeout));
			this.AddChild(animateTimer);
		}

		public void _on_AnimateTimer_timeout()
		{
			timerCounter += 1;
			trackPathRatio = timerCounter * (timer_period / duration);
			SetWalkPathPosition(trackPathRatio);
			GD.Print($"FireballThrower, _on_AnimateTimer_timeout(), trackPathRatio = {trackPathRatio}");
			if (trackPathRatio > 0.985f)
			{
				fireballChild.Visible = false;
				animateTimer.Stop();
				//this.QueueFree();
				//TODO : Send signal "Finished" to indicate that this object can be re-used
				Started = false;
			}
		}

		public void SetDuration(float timerDuration) {

		}

		public void SetWalkPathPosition(float unitOffset)
		{
			if (pathFollower != null)
			{
				pathFollower.UnitOffset = unitOffset;
			}
		}

		public void StartAnimation(Path2D fireFollowPath)
		{
			Started = true;
			GD.Print($"FireballThrower, StartAnimation(), animateTimer = {animateTimer}, fireFollowPath = {fireFollowPath}, walkPath = {walkPath}");
			GD.Print($"FireballThrower, StartAnimation(), follow path #points = {fireFollowPath.Curve.GetPointCount()}");
			if (animateTimer != null)
				animateTimer.Start();
			if ( (fireFollowPath != null) && (walkPath != null) )
			{
				Curve2D plotData = fireFollowPath.Curve;
				walkPath.Curve = (Curve2D)plotData.Duplicate();
				SetWalkPathPosition(0.0f);
				trackPathRatio = 0.0f;
			}
		}
		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
