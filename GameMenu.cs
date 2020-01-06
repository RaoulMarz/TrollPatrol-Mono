using Godot;
using System;

namespace TrollPatrolMono
{
	public class GameMenu : Control
	{
		private Control gameMenuPanel;
		private Button playButton;
		private Button optionsButton;
		private Button exitButton;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			gameMenuPanel = this.GetNodeOrNull<Control>("panel-Main-Menu");
			Diagnostics.PrintNullValueMessage(gameMenuPanel, "gameMenuPanel");
			if (gameMenuPanel != null)
			{
				playButton = gameMenuPanel.GetNodeOrNull<Button>("playButton");
				optionsButton = gameMenuPanel.GetNodeOrNull<Button>("optionsButton");
				exitButton = gameMenuPanel.GetNodeOrNull<Button>("exitButton");
				playButton.Connect("pressed", this, nameof(_playButton_Clicked));
				optionsButton.Connect("pressed", this, nameof(_optionsButton_Clicked));
				exitButton.Connect("pressed", this, nameof(_exitButton_Clicked));
			}
		}

		[Signal]
		public delegate void GameStarted();


		private void _playButton_Clicked()
		{
			//Propagate the "play" signal throughout the project
			Visible = false;
			GameState.AppGameStatus = Enums.GameStatus.GAME_STATUS_PLAY;
			EmitSignal(nameof(GameStarted));
			GD.Print("_playButton_Clicked() emit signal 'GameStarted'");
		}

		private void _optionsButton_Clicked()
		{

		}

		private void _exitButton_Clicked()
		{
			SceneUtilities.ExitApplication(this);
		}

		public override void _Process(float delta)
		{
		}

	}
}