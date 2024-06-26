using Godot;
using System;

public partial class HarvestArea : Node3D
{
	[Export]
	Color colorOfArea;
	MeshInstance3D child;
	ShaderMaterial material;
	Vector3 origin;
	public override void _Ready()
	{
		child = (MeshInstance3D)GetNode("./MeshInstance3D");
		material = (ShaderMaterial)child.MaterialOverride;
		material.SetShaderParameter("albedo",colorOfArea);
	}
	//Pos of inner child needs to be half of width
	//Shader needs half of width
	public void SetInitialCorner(Vector3 pos){
		pos.Y = 0;
		origin = FloorVector(pos);
		this.Position = origin;
		SetScaledCorner(pos);	
	}
	public void SetScaledCorner(Vector3 pos){
		Vector3 difference = pos - this.Position;
		Vector3 newPos = origin;
		if(difference.X < 0){
			newPos.X += GlobalData.SCALE;
			difference.X = FloorNearest(difference.X);
		} else{
			difference.X = CeilNearest(difference.X);
		}
		if(difference.Z < 0){
			newPos.Z += GlobalData.SCALE;
			difference.Z = FloorNearest(difference.Z);
		} else{
			difference.Z = CeilNearest(difference.Z);
		}
		this.Position = newPos;

		difference.Y = 100;
		((BoxMesh)child.Mesh).Size = difference;
		difference.Y = 0;
		child.Position = difference/2;
		
		SetShaderParams(difference.X/2, difference.Z/2);
	}
	private void SetShaderParams(float x, float z){
		material.SetShaderParameter("cube_half_x",Math.Abs(x));
		material.SetShaderParameter("cube_half_z",Math.Abs(z));
	}
	private static Vector3 FloorVector(Vector3 vec){
		return new Vector3(FloorNearest(vec.X), FloorNearest(vec.Y), FloorNearest(vec.Z));
	}
	private static float FloorNearest(float x){
		return Mathf.Floor(x /GlobalData.SCALE) * GlobalData.SCALE;
	}
	private static Vector3 CeilVector(Vector3 vec){
		return new Vector3(CeilNearest(vec.X), CeilNearest(vec.Y), CeilNearest(vec.Z));
	}
	private static float CeilNearest(float x){
		return Mathf.Ceil(x /GlobalData.SCALE) * GlobalData.SCALE;
	}
}
