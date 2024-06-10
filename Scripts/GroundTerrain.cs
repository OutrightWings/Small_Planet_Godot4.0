using System.Drawing.Drawing2D;
using System.Linq;
using Godot;
public partial class GroundTerrain : Terrain
{
    [Export]
    Image island_mask;
    [Export]
    Image biome_mask;
    FastNoiseLite noiseGround;
    FastNoiseLite noiseFall;
    FastNoiseLite noiseVegetation;
    Spline terrainSpline;
    Spline woodSpline;
    public ResourceArea[] resources;
    float FALL_DISTANCE = 1f;
     [Signal]
     public delegate void EmitResourceEventHandler(ResourceArea[] areas);
     [Signal]
     public delegate void EmitCoordEventHandler(Vector3 pos);
    public override void _Ready()
    {
	    noiseGround = CreateNoise(3,0.5f,2,64);
        noiseFall = CreateNoise(3,0.5f,2,30);
        noiseVegetation = CreateNoise(3,0.5f,2,100);
        FALL_DISTANCE *= GlobalData.SCALE;
        Vector2[] terrainPoints = {
            new Vector2(0,0),
            new Vector2(1,0.3f),
            new Vector2(3,4),
            new Vector2(5,5),
            new Vector2(9,6.6f),
            new Vector2(9.5f,6.6f),
            new Vector2(14,6.6f),
            new Vector2(100,6.6f)
        };
        terrainSpline = new Spline(terrainPoints);
        Vector2[] woodPoints = {
            new Vector2(0,0),
            new Vector2(0.5f,0),
            new Vector2(1,1)
        };
        woodSpline = new Spline(woodPoints);
        resources = new ResourceArea[(GlobalData.DIMENSIONS* GlobalData.DIMENSIONS)];
        base._Ready();
    }
    public void Update()
    {
        var image = biome_mask;
        //image.Lock();
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                ResourceArea area = resources[row*GlobalData.DIMENSIONS + col];
                if(area == null) break;
                var neighbors = new ResourceArea[4]{null,null,null,null};
                if(col > 0)
                    neighbors[0] = resources[row*GlobalData.DIMENSIONS + col-1];
                if(col < GlobalData.DIMENSIONS-1)
                    neighbors[1] = resources[row*GlobalData.DIMENSIONS + col+1];
                if(row > 0)
                    neighbors[2] = resources[(row-1)*GlobalData.DIMENSIONS + col];
                if(row < GlobalData.DIMENSIONS-1)
                    neighbors[3] = resources[(row+1)*GlobalData.DIMENSIONS + col];
                area.GrowResource(neighbors);
                area.DrawTexture(ref image);
            }
        }
        var texture = ImageTexture.CreateFromImage(image);
        //texture.Flags -= 4;
        ((StandardMaterial3D)this.MaterialOverlay).AlbedoTexture = texture;
    }
    public void AreaSelected(int selected_row, int selected_col){
        int dimension = GlobalData.REGION_SELCTION_SIZE*2+1;
        var selected = new ResourceArea[dimension*dimension];
        int pos = 0;
        for(int row = -GlobalData.REGION_SELCTION_SIZE; row < GlobalData.REGION_SELCTION_SIZE+1; row++){
            for(int col = -GlobalData.REGION_SELCTION_SIZE; col < GlobalData.REGION_SELCTION_SIZE+1; col++){
                int index = ((selected_row+row) * GlobalData.DIMENSIONS) + (selected_col+col);
                if(index >= 0 && index < GlobalData.DIMENSIONS*GlobalData.DIMENSIONS){
                    selected[pos] = resources[index];
                }
                else{
                    selected[pos] = null;
                }
                pos++;
            }
        }
        EmitSignal("EmitResource", selected);
    }
    #region Mesh Generation
    private FastNoiseLite CreateNoise(int octaves, float persistence, float lacunarity, float period){ //(3,0.5f,2,64)
        var noise = new FastNoiseLite(){
            FractalOctaves = octaves,
            FractalLacunarity = lacunarity
        };
        return noise;
    }
    public override void GenerateTerrain(){
        noiseGround.Seed = (int)GD.Randi();
        noiseFall.Seed = (int)GD.Randi();
        noiseVegetation.Seed = (int)GD.Randi();
        base.GenerateTerrain();
        ((CollisionShape3D)this.GetNode("Area/CollisionShape")).Shape = Mesh.CreateTrimeshShape();
    }
    protected override void GenerateVert(int row, int col){
        float steep;

        Color maskColor = island_mask.GetPixel(row,col);
        float maskHeight = maskColor.R;
        var noise = Mathf.Abs(noiseGround.GetNoise2D(row,col));
        steep = Mathf.Pow(noise,1)*GlobalData.HEIGHT;
        steep *= maskHeight;
        steep += maskHeight*GlobalData.WATER_LEVEL;
        steep -= noise < 0.5f ? (1 - noise) * 3 : 0; 
        steep = terrainSpline.Interpolate(steep);
        var vec = new Vector3(row*GlobalData.SCALE, steep*GlobalData.SCALE, col*GlobalData.SCALE);
        verts.Add(vec);
        normals.Add(vec.Normalized());
    }
    protected override void Errode(int times){
        if(times <= 0) return;
        for(int row = 1; row < GlobalData.DIMENSIONS-1; row++){
            for(int col = 1; col < GlobalData.DIMENSIONS-1; col++){
                int index = row*GlobalData.DIMENSIONS + col;
                Vector3 vert_center = verts[index];
                
                for(int j = -1; j <= 1; j++){
                    for(int k = -1; k <= 1; k++){
                        if(j == 0 && k == 0) continue;
                        int i = (row+j)*GlobalData.DIMENSIONS + (col+k);
                        Vector3 vert_side = verts[i];
                        float difference = vert_center.Y - vert_side.Y;
                        if(difference <= FALL_DISTANCE && difference > 0){
                            float fall = Mathf.Abs(noiseFall.GetNoise2D(row,col));
                            vert_center.Y -= difference*fall;
                            vert_side.Y += difference*fall;
                            verts[i] = vert_side;
                        } 
                    }
                }
                verts[index] = vert_center;
            }
        }
        Errode(times-1);
    }
    #endregion
    protected override void Biome(){

        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                ResourceArea resource;
                int index = row*GlobalData.DIMENSIONS + col;
                resources[index]?.Free();
                resource = new ResourceArea(row,col,verts[index].Y);
                resources[index] = resource;
                
                float rand = (float)GD.RandRange(0,0.2f);

                
                Vector3 vert_center = verts[index];
                float steep = vert_center.Y/GlobalData.SCALE;
                biome_mask.SetPixel(row,col,Colors.Transparent);
                Color c;
                if(steep <= GlobalData.WATER_LEVEL-3.5){
                     c = Colors.DarkGray;
                }
                else if(steep <= GlobalData.WATER_LEVEL+1.5f){
                     c = Colors.Tan;
                }
                else if(steep <= GlobalData.HEIGHT/5f){
                    resource.isAboveGround = true;
                     c = Colors.DarkGoldenrod;
                     float veg_noise = Mathf.Clamp(noiseVegetation.GetNoise2D(row,col)*5f,-1,1);
                     veg_noise = woodSpline.Interpolate(veg_noise);
                    if(veg_noise > 0){
                        resource.wood = (int)(veg_noise * GlobalData.MAX_WOOD);
                        resource.DrawTexture(ref biome_mask);
                    }
                }
                else if(steep <= GlobalData.HEIGHT/3f){
                     c = Colors.Gray;
                }
                else if(steep <= GlobalData.HEIGHT){
                     c = Colors.White;
                }
                else{
                     c = Colors.Black;
                }
                c = c.Darkened(rand);
                image.SetPixel(row,col,c);
            }
        }

        var texture =  ImageTexture.CreateFromImage(biome_mask);
        ((StandardMaterial3D)this.MaterialOverlay).AlbedoTexture = texture;
    }

}
