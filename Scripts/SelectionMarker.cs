using Godot;
using System;

public partial class SelectionMarker : Node3D
{
	public void MoveMarker(ResourceArea[] areas){
		int length = GlobalData.REGION_SELCTION_SIZE*2+1;
		int index = length*length;
		index /= 2;
		ResourceArea area = areas[index];
		float half = GlobalData.SCALE/2f;
		Vector3 pos = new Vector3((area.row*GlobalData.SCALE)+half,area.height+half,(area.col*GlobalData.SCALE)+half);
		this.Position = pos;
	}
}
