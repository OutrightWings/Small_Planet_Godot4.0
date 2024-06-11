using Godot;
using System;

public partial class Water : ResourceManager
{
	public override void CreateMeshImages(){
		for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                var color = Colors.Blue;
                color.A = 0.5f;
                base_image.SetPixel(row,col,color);
            }
        }
        EmitSignal("UpdateBaseImage",base_image);
    }
    public override void UpdateTick()
    {
        
    }
}
