using Godot;
using System;
using TrollSmasher.Framework;

namespace TrollSmasher
{
	public class GamePauseMenu : Control
	{
		private Panel panelMaster;
		private Button buttonContinue;
		private Button buttonExit;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			panelMaster = this.GetNodeOrNull<Panel>("panel-Master");
			Diagnostics.PrintNullValueMessage(panelMaster, "panelMaster");
			if (panelMaster != null) {
				buttonContinue = panelMaster.GetNodeOrNull<Button>("button-Continue");
				buttonExit = panelMaster.GetNodeOrNull<Button>("button-Exit");
				buttonExit.Connect("pressed", this, nameof(_exitButton_Clicked));
				buttonContinue.Connect("pressed", this, nameof(_continueButton_Clicked));
			}
		}

		private void _continueButton_Clicked()
		{
			//QueueFree();
			Visible = false;
		}

		private void _exitButton_Clicked()
		{
			SceneUtilities.ExitApplication(this);
		}
	}
}
