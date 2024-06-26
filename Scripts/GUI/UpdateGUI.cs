using Godot;
using System;
using System.Linq;

public partial class UpdateGUI : Control
{
    private Label selected_region_text, harvested_resources_text;
    public override void _Ready(){
        selected_region_text = (Label)GetNode("InfoBox/PanelContainer/Label");
        harvested_resources_text = (Label)GetNode("InfoHarvestedBox/PanelContainer/Label");
    }
    public void PopulateAreaDataInGUI(ResourceArea[] area){
        float wood = 0;
        area.ToList().ForEach( pixel => {
            if(pixel != null){
                wood += pixel.resourceMap.GetResourceAmount("wood");
                //GD.Print($"{pixel.row},{pixel.col}");
            }
        });
        //testTextArea.Text = $"({area.row},{area.col})\n";
        selected_region_text.Text = $"Wood:{wood}\n";
    }
    public void UpdateHarvestedInfo(ResourceMap resources){
        harvested_resources_text.Text = resources.ToString();
    }
    public void ClearAreaInfo(){
        selected_region_text.Text = "";
    }
}
