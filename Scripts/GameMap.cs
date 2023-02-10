using Godot;
using System;
using System.Collections.Generic;
using TrollSmasher.Enums;
using TrollSmasher.DataTypes;
using TrollSmasher.Framework;

namespace TrollSmasher
{
	public class GameMap : Node2D
	{
		private bool gameCountdownStarted = false;
		private Timer gameTimer;
		private int gameTimerCounter = 0;
		private uint markerFireballsLaunch = 0;
		public uint simultaneousMaxFireballs { get; set; } = 12;
		private uint minFireballsFlarePeriod = 4;
		private uint maxFireballsFlarePeriod = 12;
		private uint flarePeriodCounter = 0;
		private uint flareCounter = 10;
		private GameMenu gameMenu;
		private GamePauseMenu pauseMenu;
		private bool gamePlayActive = false;
		//private MovementEntity movementEntity = MovementEntity.MOVEMENT_ENTITY_NONE;
		private TileMap tileMapWorld;
		private HUDCountdown hudCountdown;
		private Troll trollPlayer;
		private CanvasLayer canvasLayer;
		private FireballThrower fireballThrowExample;
		private CountdownGameTimer fireballCountdownTimer = null;
		private bool gamePaused = true;
		/* DangerObstructions should be added manually */
		private SpawnFireballsPool fireballsPool = null;
		private bool startNewFireballsCascade = false;
		private int secondsDurationFireball = 0;
		private DateTime refFireballCountdownTime = DateTime.Now;
		private Label debugLauncherCount;
		private RandomWalker fireballPathWalker;
		const string gameMenuResource = "res://Scenes/GameMenu.tscn";
		const string hudCountdownResource = "res://Scenes/HUDCountdown.tscn";
		const string trollResource = "res://Scenes/Troll.tscn";
		const string pauseGameResource = "res://Scenes/GamePauseMenu.tscn";
		/* Monitor signal "game_started", and perform the required game set up */

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Randomize();
			GD.Print("GameMap, _Ready() called");
			GameState.AppGameStatus = Enums.GameStatus.GAME_STATUS_MENU;
			//Link up with the children nodes defined in the Map scene
			gameTimer = this.GetNodeOrNull<Timer>("gameTimer");
			Diagnostics.PrintNullValueMessage(gameTimer, "gameTimer");
			if (gameTimer != null)
			{
				gameTimer.WaitTime = 0.05f;
				gameTimer.Connect("timeout", this, nameof(_on_GameTimer_timeout));
				//gameTimer.Start();
			}

			tileMapWorld = this.GetNodeOrNull<TileMap>("tileMapWorld");
			Diagnostics.PrintNullValueMessage(tileMapWorld, "tileMapWorld");
			canvasLayer = this.GetNodeOrNull<CanvasLayer>("canvasLayer");
			Diagnostics.PrintNullValueMessage(canvasLayer, "canvasLayer");
			if (canvasLayer != null)
			{
				hudCountdown = canvasLayer.GetNodeOrNull<HUDCountdown>("hudCountdown");
				Diagnostics.PrintNullValueMessage(hudCountdown, "hudCountdown");
				debugLauncherCount = new Label();
				debugLauncherCount.Visible = true;
				debugLauncherCount.RectPosition = new Vector2(620, 90);
				debugLauncherCount.Text = "999";
				fireballThrowExample = canvasLayer.GetNodeOrNull<FireballThrower>("FireballThrower");
				Diagnostics.PrintNullValueMessage(fireballThrowExample, "fireballThrowExample");
				fireballThrowExample.Visible = true;
				fireballThrowExample.SetWalkPathPosition(0.05f);
			}

			/* VERY IMPORTANT */
			/* When a class has children node the children are already instantiated
			 * at the point when the "_Ready" of the parent scene gets executed
			 * thus child1.Connect("ready", this, nameof(_on_Child1_Ready)) serves no logical purpose
			 * BECAUSE the child is already "READY" and this connect won't be triggered!
			 * --- ONLY use the connect with "ready" on dynamically added children nodes of the scene
			/* VERY IMPORTANT */

			var random = new RandomNumberGenerator();
			random.Randomize();
			GD.Print(random.Randfn());

			markerFireballsLaunch = (GD.Randi() % 200) + 50;
			//LoadHudCountdown();
			LoadTrollPlayer();
			LoadGameMenu();
			LoadPauseMenu();
			InitialiseFireballPool();
			Rect2 windowExtent = SceneUtilities.GetApplicationWindowExtent(this);
			Vector2 bottomFireballZone = new Vector2(90.0f + GD.Randi() % (int)(windowExtent.Size.x - 120), windowExtent.Size.y);
			fireballPathWalker = new RandomWalker(new Vector2(80.0f + (GD.Randi() % (int)((windowExtent.Size.x / 2) - 120)), 10.0f + (GD.Randi() % 50)), bottomFireballZone, 30, 6.5f);
			GD.Print($"GameMap, _Ready() end, markerFireballsLaunch = {markerFireballsLaunch}");
		}

		private void AnimateOnScreen(List<PairTimestamp<FireballThrower>> fireballsFall, uint fireballs, float topOffsetVariability)
		{
			if (fireballsFall != null)
			{
				var random = new RandomNumberGenerator();
				random.Randomize();
				uint selectedCount = 0;
				uint itemCounter = 1;
				GD.Print($"GameMap, AnimateOnScreen(), param fireballsFall list count = {fireballsFall.Count}");
				Rect2 appExtent = SceneUtilities.GetApplicationWindowExtent(this);
				foreach (PairTimestamp<FireballThrower> fireballRec in fireballsFall)
				{
					float yTopOffset = random.RandfRange(5.0f, 25.0f);
					float xTopOffset = random.RandfRange(10.0f, appExtent.Size.x * 0.75f * topOffsetVariability);
					var offsetPosition = new Vector2(xTopOffset, yTopOffset);									
					fireballRec.pairX.Position += offsetPosition;
					GD.Print($"GameMap, AnimateOnScreen(), fbthrower[{itemCounter}] = {fireballRec.pairX}, position-offset = {offsetPosition}");
					canvasLayer.AddChild(fireballRec.pairX);
					itemCounter += 1;
					if (!fireballRec.pairX.Started)
					{
						fireballPathWalker.Generate();
						Path2D fireFallPath = fireballPathWalker.GetVerticesPath();
						if ( (fireFallPath == null) || (fireFallPath.Curve.GetPointCount() <= 1) )
						{
							GD.Print($"GameMap, AnimateOnScreen(), fireFallPath == null or <too few points>, fireballs = {fireballs}");
							return;
						}
						fireballRec.pairX.StartAnimation(fireFallPath);
						selectedCount += 1;
						if (selectedCount >= fireballs)
							break;
					}
				}
			}
		}

		private void SpawnRandomFireballs(AlignmentArea alignment, uint fireballs, float topOffsetVariability)
		{
			var allocatedFireballs = fireballsPool.SpawnFireballArray(fireballs);
			GD.Print($"GameMap, SpawnRandomFireballs(), param fireballs = {fireballs}, param topOffsetVariability = {topOffsetVariability}, alloced fireballs = {allocatedFireballs}");
			if (alignment == AlignmentArea.ALIGNMENT_TOP)
			{
				AnimateOnScreen(allocatedFireballs, fireballs, topOffsetVariability);
			}
		}

		private void StartTimedGame(MiniGameCategory gameCategory)
		{
			switch (gameCategory)
			{
				case MiniGameCategory.MINIGAME_FIREBALLS_CRUNCH:
					{
						fireballCountdownTimer = new CountdownGameTimer(DateTime.Now, 90);
						fireballCountdownTimer.Start();
						break;
					}
				default:
					{
						break;
					}
			}
		}

		private void _on_GameMenu_Ready()
		{
			GD.Print("GameMap, _on_GameMenu_Ready() called");
			gameMenu.Connect("MenuSelectPlay", this, nameof(_on_GameMenu_GamePlay_Started));
			Vector2 temp1 = SceneUtilities.GetExtentOffsetsForCenter(this, gameMenu);
			if (trollPlayer != null)
			{
				Vector2 trollPos = trollPlayer.spriteTroll.Position;
				Vector2 camPos = trollPlayer.followCamera.Position;
				GD.Print($"GameMap, _on_GameMenu_Ready(), center offset={temp1}, troll position={trollPos}");
			}
			gameMenu.RectPosition = temp1;
		}

		private void _on_GameMenu_Start()
		{

		}

		private void InitialiseFireballPool()
		{
			GD.Print("GameMap, InitialiseFireballPool() called");
			fireballsPool = new SpawnFireballsPool(80);
			if (fireballsPool != null)
			{
				//fireballsPool.ItemInUse()
			}
		}

		private void LoadPauseMenu()
		{
			PackedScene menuPauseScene = (PackedScene)ResourceLoader.Load(pauseGameResource);
			GD.Print("GameMap, LoadGameMenu() started");
			Diagnostics.PrintNullValueMessage(menuPauseScene, "menuPauseScene");
			if (menuPauseScene != null)
			{
				pauseMenu = (GamePauseMenu)menuPauseScene.Instance();
				pauseMenu.Visible = false;
				SceneUtilities.PlaceControlCentered(this, pauseMenu);
				if (canvasLayer != null)
					canvasLayer.AddChild(pauseMenu);
			}
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
				//gameMenu.Connect("start", this, nameof(_on_GameMenu_Start));
				GD.Print($"GameMap, LoadGameMenu() connect 'ready' signal, canvasLayer={canvasLayer}");
				if (canvasLayer != null)
					canvasLayer.AddChild(gameMenu);
				//AddChild(gameMenu);
			}
		}

		private void LoadTrollPlayer()
		{
			PackedScene trollPlayerScene = (PackedScene)ResourceLoader.Load(trollResource);
			GD.Print("GameMap, LoadTrollPlayer() started");
			Diagnostics.PrintNullValueMessage(trollPlayerScene, "trollPlayerScene");
			if (trollPlayerScene != null)
			{
				trollPlayer = (Troll)trollPlayerScene.Instance();
				trollPlayer.GlobalPosition = new Vector2(500.0f, 380.0f);
				(trollPlayer as Node2D).Connect(/*nameof(Troll.DealDamage)*/ "DealDamage", hudCountdown, nameof(HUDCountdown.AdjustHealthValue));
				AddChild(trollPlayer);
				GD.Print($"GameMap, LoadTrollPlayer() Adding {trollPlayer} to scene");
			}
		}

		public void _on_HUD_Countdown_Ready()
		{
			GD.Print("GameMap, _on_HUD_Countdown_Ready() called");
		}

		private void AdjustHudCountdown()
		{
			if ((trollPlayer != null) && (hudCountdown != null))
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
			gamePaused = false;
			gameTimer.Start();
		}

		public void _on_GameTimer_timeout()
		{
			if (gamePaused)
				return;
			gameTimerCounter += 1;
			if ((gameTimerCounter >= 20) && (gameTimerCounter < 220))
			{
				fireballThrowExample.SetWalkPathPosition(0.005f * (gameTimerCounter - 20));
				if (gameTimerCounter == 219)
					fireballThrowExample.Visible = false;
			}
			if (gameTimerCounter % 100 == 1)
				GD.Print($"GameMap, _on_GameTimer_timeout(), gameTimerCounter = {gameTimerCounter}");
			debugLauncherCount.Text = $"{gameTimerCounter - markerFireballsLaunch}";
			if (secondsDurationFireball >= 1)
			{
				TimeSpan diffTime = DateTime.Now - refFireballCountdownTime;
				int durationChangeValue = 90 - (int)diffTime.TotalSeconds;
				if (durationChangeValue != secondsDurationFireball)
				{
					GD.Print($"GameMap, _on_GameTimer_timeout(), update secondsDurationFireball = {secondsDurationFireball}");
					secondsDurationFireball = durationChangeValue;
					hudCountdown.AdjustCountdownValue(secondsDurationFireball);
					flarePeriodCounter += 1;
					if (flarePeriodCounter >= flareCounter)
					{
						GD.Print($"GameMap, _on_GameTimer_timeout(), unallocated fireball pool = {fireballsPool.UnallocatedCount()}");
						flarePeriodCounter = 0;
						flareCounter = (GD.Randi() % (maxFireballsFlarePeriod - minFireballsFlarePeriod) ) + minFireballsFlarePeriod;
						uint fireballs = (GD.Randi() % simultaneousMaxFireballs) + 4;
						float topOffsetVariability = 0.335f;
						SpawnRandomFireballs(AlignmentArea.ALIGNMENT_TOP, fireballs, topOffsetVariability);
					}
				}
			}
			if (gameTimerCounter == markerFireballsLaunch)
			{
				GD.Print($"GameMap, _on_GameTimer_timeout(), markerFireballsLaunch = {markerFireballsLaunch} reached");
				secondsDurationFireball = 90;
				hudCountdown.AdjustCountdownValue(secondsDurationFireball);
				refFireballCountdownTime = DateTime.Now;
				flareCounter = (uint)(secondsDurationFireball * 0.1);
			}
		}

		public override void _Process(float delta)
		{
			if (gameCountdownStarted)
			{
				gameTimer.Start();
			}
			if ((gamePlayActive) && (trollPlayer != null))
			{
				trollPlayer._Process(delta);
				AdjustHudCountdown();
			}
			if (GameState.AppGameStatus == GameStatus.GAME_STATUS_PLAY)
			{
				if (GameState.movementEntity == MovementEntity.MOVEMENT_ENTITY_PLAYERCHARACTER)
				{
					if (Input.IsActionPressed("ui_cancel"))
					{
						if (InputAssistance.KeyBounceCheck("cancel_key", 0.375f, 0.65f))
						{

							if (pauseMenu != null)
							{
								var pauseMenuVisible = pauseMenu.Visible;
								pauseMenu.Visible = !pauseMenuVisible;
								gamePaused = pauseMenu.Visible;
							}
							GD.Print("ui_cancel pressed");
						}
					}
				}
			}
		}
	}
}
