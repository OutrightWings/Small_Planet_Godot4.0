using Godot;
using System;
using System.Collections.Generic;

public partial class MeshArea : Area3D
{
    public ModeSelect modeSelect = ModeSelect.SELECT;
    public enum ModeSelect{
        SELECT,
        BUILD
    }
    [Signal]
    public delegate void AreaSelectedEventHandler(int row, int col);

    public void OnAreaInputEvent(Node camera, InputEvent e, Vector3 pos, Vector3 normal, int shape){
       if(e is InputEventMouseButton click && click.IsPressed()){
           if(click.ButtonIndex == MouseButton.Left){
                if(ModeSelect.SELECT == modeSelect){
                    SelectArea(pos);
                }
                else if(ModeSelect.BUILD == modeSelect){
                    PlaceBuilding(pos);
                }
           }
       }
    }
    private void SelectArea(Vector3 pos){
        var row = (int)(pos.X/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        var col = (int)(pos.Z/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        EmitSignal("AreaSelected",row, col);
    }
    private void PlaceBuilding(Vector3 pos){

    }
    public void SetSelectionMode(ModeSelect select){
        modeSelect = select;
    }
}
