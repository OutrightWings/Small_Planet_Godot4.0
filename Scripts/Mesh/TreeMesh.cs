using Godot;
using System;

public partial class TreeMesh : MultiMeshInstance3D
{
    GroundManager ground;
    bool initilized = false;
    public override void _Ready()
    {
        ground = (GroundManager)GetNode("/root/Root/Ground");
        InitalizeTrees();
    }
    public void Update(){
        for(int row = 0; row < GlobalData.RESOURCE_GRID_WIDTH; row++){
            for(int col = 0; col < GlobalData.RESOURCE_GRID_WIDTH; col++){
                int index = GlobalData.Index(row,col,GlobalData.RESOURCE_GRID_WIDTH);
                float wood = ground.GetResourceAmount(row,col,"wood") / GlobalData.MAX_WOOD;
                var color = Colors.Green.Darkened(ground.GetResourceAmount(row,col,"alphaVarience")*4);
                color.A = wood;
                this.Multimesh.SetInstanceCustomData(index,color);
            }
        }
    }
    private void InitalizeTrees(){
        this.Multimesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        this.Multimesh.InstanceCount = GlobalData.RESOURCE_GRID_WIDTH * GlobalData.RESOURCE_GRID_WIDTH;
    }
    public void SetTreePositions(){
        for(int row = 0; row < GlobalData.RESOURCE_GRID_WIDTH; row++){
            for(int col = 0; col < GlobalData.RESOURCE_GRID_WIDTH; col++){
                float box_width = GlobalData.DIMENSIONS / GlobalData.RESOURCE_GRID_WIDTH;
                float box_pos_x = box_width * row + 0.5f * box_width; 
                float box_pos_z = box_width * col + 0.5f * box_width;
                float rand_x = (float)GD.RandRange(-0.5,0.5);
                float rand_z = (float)GD.RandRange(-0.5,0.5);
                box_pos_x += rand_x;
                box_pos_z += rand_z;

                int index = GlobalData.Index(row,col,GlobalData.RESOURCE_GRID_WIDTH);
                float height = ground.GetHeightAtPoint(box_pos_x,box_pos_z);
                
                Vector3 position = new Vector3(box_pos_x*GlobalData.SCALE,height*GlobalData.SCALE,box_pos_z*GlobalData.SCALE);
                Transform3D transform = new Transform3D(Basis.Identity, position);
                this.Multimesh.SetInstanceTransform(index,transform);
            }
        }
    }
}
