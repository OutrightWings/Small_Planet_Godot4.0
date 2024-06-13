using Godot;
public partial class GroundTerrain : Terrain
{
    [Export]
    Image island_mask;
    FastNoiseLite noiseGround;
    FastNoiseLite noiseFall;

    Spline terrainSpline;

    private GroundManager ground;
    float FALL_DISTANCE = 1f;

    public override void _Ready()
    {
	    noiseGround = CreateNoise(3,0.5f,2,64);
        noiseFall = CreateNoise(3,0.5f,2,30);
        FALL_DISTANCE *= GlobalData.SCALE;
        Vector2[] terrainPoints = {
            new Vector2(0,0),
            new Vector2(1,0.3f),
            new Vector2(3,4),
            new Vector2(5,5),
            new Vector2(9,6.6f),
            new Vector2(9.5f,6.6f),
            new Vector2(14,10f),
            new Vector2(100,GlobalData.HEIGHT/2)
        };
        terrainSpline = new Spline(terrainPoints);
        ground = (GroundManager)GetNode("..");
        base._Ready();
    }
    
    public void UpdateOverlayTexture(Image image){
        var texture = ImageTexture.CreateFromImage(image);
        ((StandardMaterial3D)this.MaterialOverlay).AlbedoTexture = texture;
    }
    #region Mesh Generation
    public static FastNoiseLite CreateNoise(int octaves, float persistence, float lacunarity, float period){ //(3,0.5f,2,64)
        var noise = new FastNoiseLite(){
            FractalOctaves = octaves,
            FractalLacunarity = lacunarity
        };
        return noise;
    }
    public override void GenerateTerrain(){
        noiseGround.Seed = (int)GD.Randi();
        noiseFall.Seed = (int)GD.Randi();
        ground.EmptyResources();
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

        ground.AddNewResourceArea(row,col,steep);
    }
    protected override void Errode(int times){
        if(times <= 0) return;
        for(int row = 1; row < GlobalData.DIMENSIONS-1; row++){
            for(int col = 1; col < GlobalData.DIMENSIONS-1; col++){
                int index = GlobalData.Index(row,col);
                Vector3 vert_center = verts[index];
                
                for(int j = -1; j <= 1; j++){
                    for(int k = -1; k <= 1; k++){
                        if(j == 0 && k == 0) continue;
                        int i = GlobalData.Index(row+j,col+k);
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

}
