using Godot;

public partial class GlobalData : Node{
    public static int DIMENSIONS = 250;
    public static float HEIGHT = 100;
    public static float FLOOR = -5;
    public static float WATER_LEVEL = 5;
    public static float SCALE = 0.3f;
    public static int RESOURCE_GRID_WIDTH = DIMENSIONS;
    public static float MAX_WOOD = 10;
    public static float BASE_PLANT_GROWTH_RATE = 0.1f;
    public static int REGION_SELCTION_SIZE = 2;
    
    public Terrain water_mesh, ground_mesh;
    public override void _Ready()
    {
       water_mesh = (Terrain)GetNode("/root/Root/Water/WaterMesh");
        ground_mesh = (Terrain)GetNode("/root/Root/Ground/GroundMesh");
        GenerateMeshes();
    }
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("Reload")){
           GenerateMeshes();
        }
    }
    private void GenerateMeshes(){
        ground_mesh.GenerateTerrain();
       water_mesh.GenerateTerrain();
    }
}