using Godot;
using System;

public partial class TreeMesh : MultiMeshInstance3D
{
    GroundTerrain groundTerrain;
    [Export]
    public Image image;
    bool initilized = false;
    public override void _Ready()
    {
        groundTerrain = (GroundTerrain)GetNode("/root/Root/Ground/GroundMesh");
        InitalizeTrees();
    }
    public override void _Process(double delta)
    {
    }
    public void Update(){
        for(int row = 0; row < GlobalData.RESOURCE_GRID_WIDTH; row++){
            for(int col = 0; col < GlobalData.RESOURCE_GRID_WIDTH; col++){
                int index = row * GlobalData.RESOURCE_GRID_WIDTH + col;
                ResourceArea resource = groundTerrain.resources[index];
                float wood = resource.wood / GlobalData.MAX_WOOD;
                //image.SetPixel(row,col,new Color(1,1,1,wood));
                this.Multimesh.SetInstanceCustomData(index,new Color(1,1,1,wood));
            }
        }
    }
    private void InitalizeTrees(){
        this.Multimesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        this.Multimesh.InstanceCount = GlobalData.RESOURCE_GRID_WIDTH * GlobalData.RESOURCE_GRID_WIDTH;
    }
    public void SetTreePositions(){
        var verts_ground = groundTerrain.verts;
        for(int row = 0; row < GlobalData.RESOURCE_GRID_WIDTH; row++){
            for(int col = 0; col < GlobalData.RESOURCE_GRID_WIDTH; col++){
                float box_width = GlobalData.DIMENSIONS * GlobalData.SCALE / GlobalData.RESOURCE_GRID_WIDTH;
                float box_pos_x = box_width * row + 0.5f * box_width; 
                float box_pos_z = box_width * col + 0.5f * box_width;
                int index = row*GlobalData.RESOURCE_GRID_WIDTH+col;
                Vector3 position = new Vector3(box_pos_x,verts_ground[index].Y,box_pos_z);
                Transform3D transform = new Transform3D(Basis.Identity, position);
                this.Multimesh.SetInstanceTransform(index,transform);
            }
        }
    }
}
