using Godot;
using System;
using System.Collections.Generic;
using TrollPatrolMono.Framework;

namespace TrollPatrolMono
{
	public class HUDCountdown : Control
	{
		private Control countdownPanel;
		private Label labelCountdownTime;
		private TextureProgress progressHealth;
		public int HealthValue { get; set; }
		public int CountdownValue { get; set; }
		private Dictionary<string, int> numberValuesMap = new Dictionary<string, int>();

		public override void _Ready()
		{
			countdownPanel = this.GetNodeOrNull<Control>("panel-Countdown");
			Diagnostics.PrintNullValueMessage(countdownPanel, "countdownPanel");
			labelCountdownTime = countdownPanel.GetNodeOrNull<Label>("labelCountdown");
			progressHealth = countdownPanel.GetNodeOrNull<TextureProgress>("progress-Health");

		}

		private bool PreviousValueExists(string key)
		{
			bool res = false;
			if (numberValuesMap != null) {
				res = numberValuesMap.ContainsKey(key);
			}
			return res;
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

		private bool VisiblePropertyChanges()
		{
			bool res = false;
			if (PreviousValueExists("health"))
			{
				int prevValue = GetPreviousValue("health");
			}
			if (PreviousValueExists("countdown"))
			{
				int prevValue = GetPreviousValue("countdown");
			}
			return res;
		}

		public override void _Process(float delta)
		{
			if (VisiblePropertyChanges())
			{

			}
		}
	}
}
