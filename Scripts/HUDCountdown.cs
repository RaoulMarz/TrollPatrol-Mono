using Godot;
using System;
using System.Collections.Generic;
using TrollSmasher.Framework;

namespace TrollSmasher
{
	enum CountdownHudValueTypes { HUD_VALUETYPE_NONE, HUD_VALUETYPE_HEALTH, HUD_VALUETYPE_COUNTDOWN, HUD_VALUETYPE_ALL };
	public class HUDCountdown : Control
	{
		private Control countdownPanel;
		private Label labelCountdownTime;
		private TextureProgress progressHealth;
		public int HealthValue { get; set; }
		public int CountdownValue { get; set; }
		private CountdownHudValueTypes setValueType;
		private Dictionary<string, int> numberValuesMap = new Dictionary<string, int>();
		private long processTicker = 0;

		public void InitDefaultDisplayValues()
		{
			HealthValue = 100;
			numberValuesMap["health#1"] = HealthValue;
			CountdownValue = 30;
			numberValuesMap["countdown#1"] = CountdownValue;
			setValueType = CountdownHudValueTypes.HUD_VALUETYPE_ALL;
			GD.Print($"HUDCountdown, InitDefaultDisplayValues() called, CountdownValue={CountdownValue}, HealthValue={HealthValue}");
		}

		public override void _Ready()
		{
			countdownPanel = this.GetNodeOrNull<Control>("panel-Countdown");
			Diagnostics.PrintNullValueMessage(countdownPanel, "countdownPanel");
			labelCountdownTime = countdownPanel.GetNodeOrNull<Label>("labelCountdownValue");
			progressHealth = countdownPanel.GetNodeOrNull<TextureProgress>("progress-Health");
			InitDefaultDisplayValues();
		}

		private bool PreviousValueExists(string key)
		{
			bool res = false;
			if (numberValuesMap != null)
			{
				res = numberValuesMap.ContainsKey(key);
			}
			return res;
		}

		public void AdjustHealthValue(int health)
		{
			HealthValue = HealthValue + health;
			GD.Print($"HUDCountdown, AdjustHealthValue() called, (change) health={health}, (total) HealthValue={HealthValue}");
			numberValuesMap["health#2"] = HealthValue;
		}

		public void AdjustCountdownValue(int countdown)
		{
			GD.Print($"HUDCountdown, AdjustCountdownValue() called, countdown={countdown}");
			numberValuesMap["countdown#2"] = countdown;
		}

		private int GetPreviousValue(string key)
		{
			int value = 0;
			if (numberValuesMap != null)
			{
				value = numberValuesMap[key];
			}
			return value;
		}

		private int GetValue(CountdownHudValueTypes valueType)
		{
			int res = 0;
			switch (setValueType)
			{
				case CountdownHudValueTypes.HUD_VALUETYPE_HEALTH:
					if (numberValuesMap.ContainsKey("health#2"))
						res = numberValuesMap["health#2"];
					else
						res = numberValuesMap["health#1"];
					break;
				case CountdownHudValueTypes.HUD_VALUETYPE_COUNTDOWN:
					if (numberValuesMap.ContainsKey("countdown#2"))
						res = numberValuesMap["countdown#2"];
					else
						res = numberValuesMap["countdown#1"];
					break;
			}
			return res;
		}

		private bool VisiblePropertyChanges()
		{
			bool res = false;
			int accumulateType = 0;
			setValueType = CountdownHudValueTypes.HUD_VALUETYPE_NONE;
			if (PreviousValueExists("health#1"))
			{
				int prevValue = GetPreviousValue("health#1");
				if (PreviousValueExists("health#2"))
					res = GetPreviousValue("health#2") != prevValue;
				if (res)
					accumulateType = 1;
					//setValueType = CountdownHudValueTypes.HUD_VALUETYPE_HEALTH;
			}
			if (PreviousValueExists("countdown#1"))
			{
				int prevValue = GetPreviousValue("countdown#1");
				bool res2 = false;
				if (PreviousValueExists("countdown#2"))
					res2 = GetPreviousValue("countdown#2") != prevValue;
				if (res2)
				{
					res = true;
					accumulateType += 2;
				}
			}
			switch (accumulateType) {
				case 0:
					setValueType = CountdownHudValueTypes.HUD_VALUETYPE_NONE;
					break;
				case 1:
					setValueType = CountdownHudValueTypes.HUD_VALUETYPE_HEALTH;
					break;
				case 2:
					setValueType = CountdownHudValueTypes.HUD_VALUETYPE_COUNTDOWN;
					break;
				case 3:
					setValueType = CountdownHudValueTypes.HUD_VALUETYPE_ALL;
					break;
			}
			if (setValueType != CountdownHudValueTypes.HUD_VALUETYPE_NONE)
				GD.Print($"HUDCountdown, VisiblePropertyChanges(), setValueType={setValueType}, result={res}");
			return res;
		}

		private void ResetChangedValues()
		{
			if (numberValuesMap.ContainsKey("health#2"))
				numberValuesMap["health#1"] = numberValuesMap["health#2"];
			numberValuesMap.Remove("health#2");
			if (numberValuesMap.ContainsKey("countdown#2"))
				numberValuesMap["countdown#1"] = numberValuesMap["countdown#2"];
			numberValuesMap.Remove("countdown#2");
		}

		public override void _Process(float delta)
		{
			processTicker += 1;
			//if (processTicker % 100 == 1)
			//	GD.Print($"HUDCountdown, _Process() called, processTicker={processTicker}");
			if (VisiblePropertyChanges())
			{
				GD.Print($"HUDCountdown, _Process() called, VisiblePropertyChanges(), setValueType={setValueType}");
				switch (setValueType)
				{
					case CountdownHudValueTypes.HUD_VALUETYPE_HEALTH:
						progressHealth.Value = GetValue(CountdownHudValueTypes.HUD_VALUETYPE_HEALTH);
						break;
					case CountdownHudValueTypes.HUD_VALUETYPE_COUNTDOWN:
						labelCountdownTime.Text = $"{GetValue(CountdownHudValueTypes.HUD_VALUETYPE_COUNTDOWN)}";
						break;
					case CountdownHudValueTypes.HUD_VALUETYPE_ALL:
						progressHealth.Value = GetValue(CountdownHudValueTypes.HUD_VALUETYPE_HEALTH);
						labelCountdownTime.Text = $"{GetValue(CountdownHudValueTypes.HUD_VALUETYPE_COUNTDOWN)}";
						break;
				}
				ResetChangedValues();
			}
		}
	}
}
