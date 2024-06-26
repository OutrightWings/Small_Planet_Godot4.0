using Godot;
using System;

public partial class ModeSelectionButton : Button
{
	[Signal]
    public delegate void UpdateModeEventHandler(MeshArea.ModeSelect select);
	[Export]
	MeshArea.ModeSelect selection;

	public void OnClick(){
		EmitSignal("UpdateMode",(int)selection);
	}
}
