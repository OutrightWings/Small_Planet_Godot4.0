using Godot;
using System;

public partial class SliderControl : Control
{
    Label growthRateLabel;
    Label mountainnessLabel;
    public override void _Ready()
    {
        growthRateLabel = (Label)GetNode("GrowthRate/HBoxContainer/Label");
        mountainnessLabel = (Label)GetNode("Mountainness/HBoxContainer/Label");
        
    }
    public void UpdateGrowthRate(float value){
        GlobalData.BASE_PLANT_GROWTH_RATE = value;
        growthRateLabel.Text = $"{value}";
    }
    public void UpdateMountainness(float value){
        GlobalData.HEIGHT = value;
        mountainnessLabel.Text = $"{value}";
    }
}
