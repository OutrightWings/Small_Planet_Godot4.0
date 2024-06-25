using Godot;
using System;
using System.Collections.Generic;

public partial class MeshArea : Area3D
{
    private ModeSelect modeSelect = ModeSelect.SELECT;
    GroundManager ground;
    [Export]
    PackedScene temp_object;
    [Export]
    PackedScene resourceCollectionRegion;
    public enum ModeSelect{
        SELECT,
        BUILD,
        HARVEST_WOOD_SELECT
    }
    [Signal]
    public delegate void AreaSelectedEventHandler(int row, int col);
    [Signal]
    public delegate void AreaDeSelectedEventHandler();
    bool isPlacing = false;
    HarvestArea currentPlacing = null;
    public override void _Ready()
    {
        base._Ready();
        ground = (GroundManager)GetNode("../..");
    }
    public void OnAreaInputEvent(Node camera, InputEvent e, Vector3 pos, Vector3 normal, int shape){
       if(e is InputEventMouseButton click && click.IsPressed()){
           if(click.ButtonIndex == MouseButton.Left){
                if(ModeSelect.SELECT == modeSelect){
                    SelectArea(pos);
                }
                else if(ModeSelect.BUILD == modeSelect){
                    PlaceBuilding(pos);
                } else if(ModeSelect.HARVEST_WOOD_SELECT == modeSelect){
                    if(isPlacing){
                        currentPlacing.SetScaledCorner(pos);
                        isPlacing = false;
                    }else {
                        isPlacing = true;
                        currentPlacing = (HarvestArea) resourceCollectionRegion.Instantiate();
                        AddChild(currentPlacing);
                        currentPlacing.SetInitialCorner(pos);
                    }
                    
                    
                }
           } else if(click.ButtonIndex == MouseButton.Right){
                if(isPlacing){
                    FreePlacement();
                } else {
                    EmitSignal("AreaDeSelected");
                }
           }
       } else if(e is InputEventMouseMotion drag){
            if(isPlacing){
                currentPlacing.SetScaledCorner(pos);
            }
       }
    }
    private void SelectArea(Vector3 pos){
        var row = (int)(pos.X/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        var col = (int)(pos.Z/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        EmitSignal("AreaSelected",row, col);
    }
    private void PlaceBuilding(Vector3 pos){
        var obj = (Node3D)temp_object.Instantiate();
        obj.Position = pos;
        AddChild(obj);
    }
    public void SetSelectionMode(ModeSelect select){
        if(modeSelect == select){
            modeSelect = ModeSelect.SELECT;
            FreePlacement();
        } else{
            modeSelect = select;
        }
    }
    private void FreePlacement(){
        if(isPlacing){
            currentPlacing?.Free();
            isPlacing = false;
        }
    }
}
