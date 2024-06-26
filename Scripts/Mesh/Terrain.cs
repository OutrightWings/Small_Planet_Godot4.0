using Godot;
using System.Collections.Generic;
public abstract partial class Terrain : MeshInstance3D
{
    protected Godot.Collections.Array surfaceArray = new Godot.Collections.Array();
    public List<Vector3> verts = new List<Vector3>();
    protected List<Vector2> uvs = new List<Vector2>();
    protected List<Vector3> normals = new List<Vector3>();
    protected List<int> indices = new List<int>();
     [Signal]
    public delegate void CreateImagesEventHandler();
    public virtual void GenerateTerrain(){
        ((ArrayMesh)this.Mesh).ClearSurfaces();
        GenerateMesh();
    }
    protected void GenerateMesh(){
        surfaceArray.Clear(); 
        surfaceArray.Resize((int)Mesh.ArrayType.Max);
        verts.Clear();
        uvs.Clear();
        normals.Clear();
        indices.Clear();
        //image.Lock();
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            var u = ((float)row)/GlobalData.DIMENSIONS;
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                var v = ((float)col)/GlobalData.DIMENSIONS;
                uvs.Add(new Vector2(u,v));
                if(row > 0 && col > 0){
                    indices.Add((row-1)*GlobalData.DIMENSIONS +col-1);
                    indices.Add((row)*GlobalData.DIMENSIONS +col);
                    indices.Add((row-1)*GlobalData.DIMENSIONS +col);
                    
                    indices.Add((row-1)*GlobalData.DIMENSIONS +col-1);
                    indices.Add((row)*GlobalData.DIMENSIONS +col-1);
                    indices.Add((row)*GlobalData.DIMENSIONS +col);
                    
                }
                GenerateVert(row,col);
            }
        }
        Errode(3);
        GenerateEdge();
        // Convert Lists to arrays and assign to surface array
        surfaceArray[(int)Mesh.ArrayType.Vertex] = verts.ToArray();
        surfaceArray[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Normal] = normals.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = indices.ToArray();
        ((ArrayMesh)this.Mesh).AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        EmitSignal("CreateImages");
    }
    public void UpdateBaseTexture(Image image){
        var texture =  ImageTexture.CreateFromImage(image);
        ((StandardMaterial3D)this.MaterialOverride).AlbedoTexture = texture;
    }
    protected abstract void GenerateVert(int row, int col);
    protected virtual void GenerateEdge(){
        int count = 0;
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            var u = ((float)row)/GlobalData.DIMENSIONS;
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                if(row > 0 && row < GlobalData.DIMENSIONS-1){
                    if(col > 0 && col < GlobalData.DIMENSIONS-1){
                        continue;
                    }
                }

                var v = ((float)col)/GlobalData.DIMENSIONS;
                uvs.Add(new Vector2(u,v));
                if((row == 0 || row == GlobalData.DIMENSIONS-1) && col > 0){
                    int a = row * GlobalData.DIMENSIONS + col -1;
                    int b = GlobalData.DIMENSIONS * GlobalData.DIMENSIONS + count-1;
                    int c = b + 1;
                    int d = a + 1;
                    if(row == 0){
                        indices.Add(a);
                        indices.Add(c);
                        indices.Add(b);
                    
                        indices.Add(a);
                        indices.Add(d);
                        indices.Add(c);
                    } else {
                        indices.Add(a);
                        indices.Add(b);
                        indices.Add(c);
                    
                        indices.Add(a);
                        indices.Add(c);
                        indices.Add(d);
                    }
                    
                } 
                if((col == 0 || col == GlobalData.DIMENSIONS-1) && row > 0){
                    int a = (row-1) * GlobalData.DIMENSIONS + col;
                    int c = GlobalData.DIMENSIONS * GlobalData.DIMENSIONS + count;
                    int d = row * GlobalData.DIMENSIONS + col;
                    int b;
                    if((row == 1 && col == 0) || (row == GlobalData.DIMENSIONS -1 && col == GlobalData.DIMENSIONS -1)){
                        b = c - GlobalData.DIMENSIONS;
                    } else {
                        b = c - 2;
                    }
                    
                    if(col == 0){
                        indices.Add(a);
                        indices.Add(b);
                        indices.Add(c);

                        indices.Add(a);
                        indices.Add(c);
                        indices.Add(d);
                    } else {
                        indices.Add(a);
                        indices.Add(c);
                        indices.Add(b);
                    
                        indices.Add(a);
                        indices.Add(d);
                        indices.Add(c);
                    }
                }
                var vec = new Vector3(row*GlobalData.SCALE,GlobalData.FLOOR,col*GlobalData.SCALE);
                verts.Add(vec);
                normals.Add(vec.Normalized());
                count++;
            }
        }
    }
    protected abstract void Errode(int times);
}
