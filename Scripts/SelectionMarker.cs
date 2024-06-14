using Godot;
using System;

public partial class SelectionMarker : Node3D
{
	public void MoveMarker(ResourceArea[] areas){
		int length = GlobalData.REGION_SELECTION_RADIOUS*2+1;
		int index = length*length;
		index /= 2;
		ResourceArea area = areas[index];
		Vector3 pos = new Vector3((area.row+0.5f)*GlobalData.SCALE,(area.height+0.5f)*GlobalData.SCALE,(area.col+0.5f)*GlobalData.SCALE);
		this.Position = pos;
	}
	public void DisableMarker(){
		this.Position = new Vector3(-10,0,-10);
	}
}
