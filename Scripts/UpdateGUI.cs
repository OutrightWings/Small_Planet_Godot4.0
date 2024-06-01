using Godot;
using System;

public partial class UpdateGUI : Control
{
    private Label testTextArea;

    private ResourceArea currentlySelectedArea;
    public override void _Ready(){
        testTextArea = (Label)GetNode("AspectRatioContainer/PanelContainer/Label");
    }
    public void PopulateAreaDataInGUI(ResourceArea area){
        testTextArea.Text = $"({area.row},{area.col})\n";
        testTextArea.Text += $"Wood:{area.wood}\n";
    }
}
