using Godot;
using System;

public partial class HarvestArea : Node3D
{
	[Export]
	Color colorOfArea;
	[Export]
	Vector3 endPos;
	MeshInstance3D child;
	ShaderMaterial material;
	public override void _Ready()
	{
		child = (MeshInstance3D)GetNode("./MeshInstance3D");
		material = (ShaderMaterial)child.MaterialOverride;
		material.SetShaderParameter("albedo",Colors.Red);
	}
	//Pos of inner child needs to be half of width
	//Shader needs half of width
	//Scale is width in parent
	public void SetInitialCorner(Vector3 pos){
		pos.Y = 0;
		this.Position = pos;
		SetScaledCorner(pos);	
	}
	public void SetScaledCorner(Vector3 pos){
		Vector3 difference = pos - this.Position;
		difference.Y = 100;
		//((BoxMesh)child.Mesh).Size = difference;
		this.Scale = difference;
		
		//SetShaderParams(difference.X/2, difference.Z/2);
		endPos = pos;
	}
	private void SetShaderParams(float x, float z){
		material.SetShaderParameter("Cube Half X",x);
		material.SetShaderParameter("Cube Half Z",z);
	}
}
