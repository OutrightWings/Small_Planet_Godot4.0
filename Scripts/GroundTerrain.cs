using System;
using Godot;
public partial class GroundTerrain : Terrain
{
    [Export]
    Image island_mask;
    FastNoiseLite noise_mountain;
    FastNoiseLite noise_bumps;
    FastNoiseLite noise_river;
    FastNoiseLite noise_veg;
    FastNoiseLite noiseFall;

    Spline mountain_height_spline, mountain_bumps_spline, bumps_spline, river_height_spline, river_bumps_spline;
	Spline vegSpline;

    private GroundManager ground;
    float FALL_DISTANCE = 1f;

    public override void _Ready()
    {
	    noise_mountain = CreateNoise(0.005f);
        noise_bumps = CreateNoise(0.02f);
        noise_river = CreateNoise(0.01f);
        noise_veg = CreateNoise(0.01f);

        noiseFall = CreateNoise(0.01f);
        FALL_DISTANCE *= GlobalData.SCALE;
        ground = (GroundManager)GetNode("..");
       

        float river_depth=0.2f;
        float river_width=0.05f;
        float river_valley_width=0.1f;
        float mountain_height = 1f;
        float mountain_width=0.3f;
        float foothills_width=0.4f;
        Vector2[] noise_veg_points = {
            new Vector2(0,0),
            new Vector2(0.5f,0),
            new Vector2(1,1)
        };
        vegSpline = new Spline(noise_veg_points);
        
        Vector2[] noise_mountain_height_points = {
            new Vector2(-1000,river_depth),
            new Vector2(-1,river_depth),
            new Vector2(-foothills_width,mountain_height/4),
            new Vector2(-mountain_width,mountain_height/2),
            new Vector2(0,mountain_height),
            new Vector2(mountain_width,mountain_height/2),
            new Vector2(foothills_width,mountain_height/4),
            new Vector2(1,river_depth),
            new Vector2(1000,river_depth),

        };
        mountain_height_spline = new Spline(noise_mountain_height_points);

        Vector2[] noise_mountain_bumps_points = {
            new Vector2(-1000,.5f),
            new Vector2(-foothills_width,.5f),
            new Vector2(-mountain_width,1f),
            new Vector2(mountain_width,1f),
            new Vector2(foothills_width,.5f),
            new Vector2(1000,.5f),
        };
        mountain_bumps_spline = new Spline(noise_mountain_bumps_points);
        
        Vector2[] noise_bumps_points = {
            new Vector2(-1000,2),
            new Vector2(-1,2f),
            new Vector2(0,0),
            new Vector2(1,2f),
            new Vector2(1000,2),

        };
        bumps_spline = new Spline(noise_bumps_points);
        
        Vector2[] noise_river_height_points = {
            new Vector2(-1000,0),
            new Vector2(-1,0),
            new Vector2(-river_valley_width,-river_depth/2),
            new Vector2(-river_width,-river_depth),
            new Vector2(river_width,-river_depth),
            new Vector2(river_valley_width,-river_depth/2),
            new Vector2(1,0),
            new Vector2(1000,0),
  
        };
        river_height_spline = new Spline(noise_river_height_points);

        Vector2[] noise_river_bumps_points = {
            new Vector2(-1000,1),
            new Vector2(-river_valley_width*2,1),
            new Vector2(-river_valley_width,0),
            new Vector2(river_valley_width,0),
            new Vector2(river_valley_width*2,1),
            new Vector2(1000,1),
        };
        river_bumps_spline = new Spline(noise_river_bumps_points);

        base._Ready();
    }
    
    public void UpdateOverlayTexture(Image image){
        var texture = ImageTexture.CreateFromImage(image);
        ((StandardMaterial3D)this.MaterialOverlay).AlbedoTexture = texture;
    }
    #region Mesh Generation
    public static FastNoiseLite CreateNoise(float frequency){
        var noise = new FastNoiseLite(){
            Frequency = frequency
        };
        return noise;
    }
    public override void GenerateTerrain(){
        noise_mountain.Seed = (int)GD.Randi();
        noise_bumps.Seed = (int)GD.Randi();
        noise_veg.Seed = (int)GD.Randi();
        noiseFall.Seed = (int)GD.Randi();
        ground.EmptyResources();
        base.GenerateTerrain();
        ((CollisionShape3D)this.GetNode("Area/CollisionShape")).Shape = Mesh.CreateTrimeshShape();
        
    }
    protected override void GenerateVert(int row, int col){
        float steep;

        float veg_noise = Mathf.Clamp(noise_veg.GetNoise2D(row,col)*5f,-1,1);
        veg_noise = vegSpline.Interpolate(veg_noise);

        float mountain = noise_mountain.GetNoise2D(row,col);
        float bumps = noise_bumps.GetNoise2D(row,col);
        float river = noise_river.GetNoise2D(row,col);

        //GD.Print($"{mountain}, {bumps}, {river}");

        Color maskColor = island_mask.GetPixel(row,col);
        float maskHeight = maskColor.R;
        river *= maskHeight;
        mountain -= 1-maskHeight;

        float mountain_height = mountain_height_spline.Interpolate(mountain);
        float mountain_bumps = mountain_bumps_spline.Interpolate(mountain);
        float river_height = river_height_spline.Interpolate(river);
        float river_bumps = river_bumps_spline.Interpolate(river);
        float bumps_splined = bumps_spline.Interpolate(bumps);

        steep = mountain_height + river_height + (mountain_bumps * river_bumps * bumps_splined);

        steep *= .2f*GlobalData.HEIGHT;

        var vec = new Vector3(row*GlobalData.SCALE, steep*GlobalData.SCALE, col*GlobalData.SCALE);
        verts.Add(vec);
        normals.Add(vec.Normalized());

        var resource = ground.AddNewResourceArea(row,col,steep);
        resource.resourceMap.SetResource("noise_veg",veg_noise);
        resource.resourceMap.SetResource("noise_mountain",mountain);
        resource.resourceMap.SetResource("noise_bumps",bumps);
        resource.resourceMap.SetResource("noise_river",river);
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
