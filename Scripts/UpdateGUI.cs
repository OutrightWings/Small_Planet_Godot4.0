using Godot;
using System;
using System.Linq;

public partial class UpdateGUI : Control
{
    private Label testTextArea;
    public override void _Ready(){
        testTextArea = (Label)GetNode("AspectRatioContainer/PanelContainer/Label");
    }
    public void PopulateAreaDataInGUI(ResourceArea[] area){
        float wood = 0;
        area.ToList().ForEach( pixel => {
            if(pixel != null){
                wood += pixel.wood;
                //GD.Print($"{pixel.row},{pixel.col}");
            }
        });
        //testTextArea.Text = $"({area.row},{area.col})\n";
        testTextArea.Text = $"Wood:{wood}\n";
    }
}
