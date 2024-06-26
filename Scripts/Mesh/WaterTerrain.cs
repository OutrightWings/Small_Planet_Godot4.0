using Godot;
using System.Collections.Generic;
using System.Data;
public partial class WaterTerrain : Terrain
{
    protected override void Errode(int times)
    {
        
    }
    protected override void GenerateEdge()
    {
        base.GenerateEdge();
        var groundMesh = (GroundTerrain)GetNode("/root/Root/Ground/GroundMesh");
        var verts_ground = groundMesh.verts;
        int count = 0;
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                if(row > 0 && row < GlobalData.DIMENSIONS-1){
                    if(col > 0 && col < GlobalData.DIMENSIONS-1){
                        continue;
                    }
                }
                var pos_ground = row*GlobalData.DIMENSIONS+col;
                var pos_here = GlobalData.DIMENSIONS*GlobalData.DIMENSIONS;
                var vec_ground = verts_ground[pos_ground];
                var vec_edge = verts[pos_here+count];
                vec_edge.Y = vec_ground.Y;
                verts[pos_here+count] = vec_edge;
                count++;
            }
        }
    }

    protected override void GenerateVert(int row, int col){
        var vec = new Vector3(row*GlobalData.SCALE,GlobalData.WATER_LEVEL*GlobalData.SCALE,col*GlobalData.SCALE);
        verts.Add(vec);
        normals.Add(vec.Normalized());
    }
}
