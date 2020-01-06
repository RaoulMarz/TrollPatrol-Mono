using Godot;
using System;

namespace TrollPatrolMono
{
	public class HUDCountdown : Control
	{
		private Control countdownPanel;
		private Label labelCountdownTime;
		public override void _Ready()
		{
			countdownPanel = this.GetNodeOrNull<Control>("panel-Countdown");
			Diagnostics.PrintNullValueMessage(countdownPanel, "countdownPanel");
			labelCountdownTime = countdownPanel.GetNodeOrNull<Label>("labelCountdown");
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
