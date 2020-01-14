using Godot;
using System;
using TrollPatrolMono.Framework;

namespace TrollPatrolMono
{
	public class GamePauseMenu : Control
	{
		private Control panelMaster;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			panelMaster = this.GetNodeOrNull<Control>("panel-Master");
			Diagnostics.PrintNullValueMessage(panelMaster, "panelMaster");
		}

	}
}
