using Godot;
using System;
using System.Collections.Generic;

public partial class MeshArea : Area3D
{
    [Signal]
    public delegate void AreaSelectedEventHandler(int row, int col);

    public void OnAreaInputEvent(Node camera, InputEvent e, Vector3 pos, Vector3 normal, int shape){
       if(e is InputEventMouseButton click && click.IsPressed()){
           if(click.ButtonIndex == MouseButton.Left){ //Left mouse click 
                var row = (int)(pos.X/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
                var col = (int)(pos.Z/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
                EmitSignal("AreaSelected",row, col);
           }
       }
    }
}
