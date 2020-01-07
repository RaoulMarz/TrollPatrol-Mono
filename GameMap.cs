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
		private TileMap tileMapWorld;
		private HUDCountdown hudCountdown;
		private Troll trollPlayer;
		private CanvasLayer canvasLayer;
		/* DangerObstructions should be added manually */
		private SpawnFireballsPool fireballsPool = null;
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
			Diagnostics.PrintNullValueMessage(gameTimer, "gameTimer");
			if (gameTimer != null)
			{
				/* If Windows then reduce timer resolution */
				//sceneTimer.WaitTime = 0.1f;				
				gameTimer.Connect("timeout", this, nameof(_on_GameTimer_timeout));
				gameTimer.Start();
			}
			
			tileMapWorld = this.GetNodeOrNull<TileMap>("tileMapWorld");
			Diagnostics.PrintNullValueMessage(tileMapWorld, "tileMapWorld");
			canvasLayer = this.GetNodeOrNull<CanvasLayer>("canvasLayer");
			Diagnostics.PrintNullValueMessage(canvasLayer, "canvasLayer");
			if (canvasLayer != null)
			{
				hudCountdown = canvasLayer.GetNodeOrNull<HUDCountdown>("hudCountdown");
				Diagnostics.PrintNullValueMessage(hudCountdown, "hudCountdown");
			}

			/* VERY IMPORTANT */
			/* When a class has children node the children are already instantiated
			 * at the point when the "_Ready" of the parent scene gets executed
			 * thus child1.Connect("ready", this, nameof(_on_Child1_Ready)) serves no logical purpose
			 * BECAUSE the child is already "READY" and this connect won't be triggered!
			 * --- ONLY use the connect with "ready" on dynamically added children nodes of the scene
			/* VERY IMPORTANT */

			//LoadHudCountdown();
			LoadTrollPlayer();
			LoadGameMenu();
			InitialiseFireballPool();
		}

		private void _on_GameMenu_Ready()
		{
			GD.Print("GameMap, _on_GameMenu_Ready() called");
			gameMenu.Connect("MenuSelectPlay", this, nameof(_on_GameMenu_GamePlay_Started));
			Vector2 temp1 = SceneUtilities.GetExtentOffsetsForCenter(this, gameMenu);
			if (trollPlayer != null)
			{
				Vector2 trollPos = trollPlayer.spriteTroll.Position; //.RectPosition;
				Vector2 camPos = trollPlayer.followCamera.Position;
				GD.Print($"GameMap, _on_GameMenu_Ready(), center offset={temp1}, troll position={trollPos}");
				//temp1 += camPos;
			}
			gameMenu.RectPosition = temp1;
		}

		private void InitialiseFireballPool()
		{
			GD.Print("GameMap, InitialiseFireballPool() called");
			fireballsPool = new SpawnFireballsPool(80);
		}

		private void LoadGameMenu()
		{
			PackedScene menuScene = (PackedScene)ResourceLoader.Load(gameMenuResource);
			GD.Print("GameMap, LoadGameMenu() started");
			Diagnostics.PrintNullValueMessage(menuScene, "menuScene");
			if (menuScene != null)
			{
				gameMenu = (GameMenu)menuScene.Instance();
				gameMenu.Visible = true;
				SceneUtilities.ThreadSleep(800);
				gameMenu.Connect("ready", this, nameof(_on_GameMenu_Ready));
				GD.Print("GameMap, LoadGameMenu() connect 'ready' signal");
				if (canvasLayer != null)
					canvasLayer.AddChild(gameMenu);
				//AddChild(gameMenu);
			}
		}

		/*
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
		*/

		private void LoadTrollPlayer()
		{
			PackedScene trollPlayerScene = (PackedScene)ResourceLoader.Load(trollResource);
			GD.Print("GameMap, LoadTrollPlayer() started");
			Diagnostics.PrintNullValueMessage(trollPlayerScene, "trollPlayerScene");
			if (trollPlayerScene != null)
			{
				trollPlayer = (Troll)trollPlayerScene.Instance();
				AddChild(trollPlayer);
				GD.Print("GameMap, LoadTrollPlayer() Adding {trollPlayer} to scene");
			}
		}

		public void _on_HUD_Countdown_Ready()
		{
			GD.Print("GameMap, _on_HUD_Countdown_Ready() called");
		}

		private void AdjustHudCountdown()
		{
			if ( (trollPlayer != null) && (hudCountdown != null) )
			{
				//if (followMode == FOLLOW_PLAYER)
				//    hudCountdown.RectPosition = trollPlayer.followCamera.GlobalPosition + new Vector2(-300.0f, -200.0f);

			   //hudCountdown.RectPosition = tileMapWorld.GlobalPosition + new Vector2(300.0f, 100.0f);
			   hudCountdown.SetGlobalPosition(GlobalPosition + new Vector2(40.0f, 30.0f));
			   //GD.Print($"AdjustHudCountdown() called, HUD position={hudCountdown.RectPosition}");
			}
		}

		public void _on_GameMenu_GamePlay_Started()
		{
			GD.Print("_on_GameMenu_GamePlay_Started() called");
			GameState.movementEntity = MovementEntity.MOVEMENT_ENTITY_PLAYERCHARACTER;
			if (hudCountdown != null)
				hudCountdown.Visible = true;
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
			if ( (gamePlayActive) && (trollPlayer != null) )
			{
				trollPlayer._Process(delta);
				AdjustHudCountdown();
			}
		}
	}
}
