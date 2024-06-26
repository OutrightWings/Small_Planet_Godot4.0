using Godot;
using System;

public partial class ModeSelectionButton : Button
{
	[Signal]
    public delegate void UpdateModeEventHandler(int select);
	[Export]
	InputManager.ModeSelect selection;

	public void OnClick(){
		EmitSignal("UpdateMode",(int)selection);
	}
}
