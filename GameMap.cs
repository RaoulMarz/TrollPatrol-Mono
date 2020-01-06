using Godot;
using System;
using TrollPatrolMono.Enums;

namespace TrollPatrolMono
{
	public class GameMap : Node2D
	{
		private bool gameCountdownStarted = false;
		private Timer gameTimer;
		private int gameTimerCounter = 0;
		private GameMenu gameMenu;
		private bool gamePlayActive = false;
		private MovementEntity movementEntity = MovementEntity.MOVEMENT_ENTITY_NONE;
		private HUDCountdown hudCountdown = null;
		private Troll trollPlayer = null;
		const string gameMenuResource = "res://GameMenu.tscn";
		const string hudCountdownResource = "res://HUDCountdown.tscn";
		const string trollResource = "res://Troll.tscn";
		/* Monitor signal "game_started", and perform the required game set up */

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GameState.AppGameStatus = Enums.GameStatus.GAME_STATUS_MENU;
			//Link up with the children nodes defined in the Map scene
			gameTimer = this.GetNodeOrNull<Timer>("gameTimer");
			//Diagnostics.PrintNullValueMessage(gameTimer, "gameTimer");
			if (gameTimer != null)
			{
				/* If Windows then reduce timer resolution */
				//sceneTimer.WaitTime = 0.1f;				
				gameTimer.Connect("timeout", this, nameof(_on_GameTimer_timeout));
				gameTimer.Start();
			}
			LoadGameMenu();
			LoadHudCountdown();
			LoadTrollPlayer();
		}

		private void _on_GameMenu_Ready()
		{
			GD.Print("_on_GameMenu_Ready() called");
			gameMenu.Connect("GameStarted", this, nameof(_on_GameMenu_Game_Started));
		}

		private void LoadGameMenu()
		{
			PackedScene menuScene = (PackedScene)ResourceLoader.Load(gameMenuResource);
			GD.Print("LoadGameMenu() started");
			Diagnostics.PrintNullValueMessage(menuScene, "menuScene");
			if (menuScene != null)
			{
				gameMenu = (GameMenu)menuScene.Instance();
				//gameMenu.Connect("GameStarted", this, nameof(_on_GameMenu_Started));
				gameMenu.Connect("ready", this, nameof(_on_GameMenu_Ready));
				GD.Print("LoadGameMenu() connect 'ready' signal");
			}
		}

		private void LoadHudCountdown()
		{
			PackedScene hudScene = (PackedScene)ResourceLoader.Load(hudCountdownResource);
			GD.Print("LoadHudCountdown() started");
			Diagnostics.PrintNullValueMessage(hudScene, "hudScene");
			if (hudScene != null)
			{
				hudCountdown = (HUDCountdown)hudScene.Instance();
				hudCountdown.Connect("ready", this, nameof(_on_HUD_Countdown_Ready));
			}
		}

		private void LoadTrollPlayer()
		{
			PackedScene trollPlayerScene = (PackedScene)ResourceLoader.Load(trollResource);
			GD.Print("LoadTrollPlayer() started");
			Diagnostics.PrintNullValueMessage(trollPlayerScene, "trollPlayerScene");
			if (trollPlayerScene != null)
			{
				trollPlayer = (Troll)trollPlayerScene.Instance();
			}
		}

		public void _on_HUD_Countdown_Ready()
		{
			GD.Print("_on_HUD_Countdown_Ready() called");
		}


		public void _on_GameMenu_Game_Started()
		{
			GD.Print("_on_GameMenu_Game_Started() called");
			gamePlayActive = true;
		}

		public void _on_GameTimer_timeout()
		{
			gameTimerCounter += 1;
		}

		public override void _Process(float delta)
		{
			if (gameCountdownStarted)
			{
				gameTimer.Start();
			}
			if (gamePlayActive)
			{
				trollPlayer._Process(delta);
			}
		}
	}
}
