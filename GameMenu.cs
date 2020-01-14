using Godot;
using System;
using System.Collections.Generic;
using TrollPatrolMono.Framework;

namespace TrollPatrolMono
{
	public class GameMenu : Control
	{
		private Control gameMenuPanel;
		private Button playButton;
		private Button optionsButton;
		private Button exitButton;

		[Signal]
		public delegate void MenuSelectPlay();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print("GameMenu, _Ready() called");
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

		private void _playButton_Clicked()
		{
			//Propagate the "play" signal throughout the project
			Visible = false;
			GameState.AppGameStatus = Enums.GameStatus.GAME_STATUS_PLAY;
			EmitSignal(nameof(MenuSelectPlay));
			//Struggling with connecting this custom signal
			GD.Print("_playButton_Clicked() emit signal 'MenuSelectPlay'");
            Dictionary<string, string> menuProperties = Diagnostics.GetProperties(this);
            if (menuProperties != null)
            {
                Diagnostics.PrintStringDictionary("Properties for GameMenu", menuProperties);
            }
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
