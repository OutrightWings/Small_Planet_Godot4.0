using Godot;

public partial class GlobalData : Node{
    #region Global Vars
    public static int DIMENSIONS = 250;
    public static float HEIGHT = 100;
    public static float FLOOR = -5;
    public static float WATER_LEVEL = 2;
    public static float SCALE = 0.3f;
    public static int RESOURCE_GRID_WIDTH = DIMENSIONS;
    public static float MAX_WOOD = 10;
    public static float BASE_PLANT_GROWTH_RATE = 0.1f;
    public static int REGION_SELECTION_RADIOUS = 2;
    #endregion
    #region Signals
    [Signal]
    public delegate void ClockEventHandler();
    #endregion
    #region Ready
    public Terrain water_mesh, ground_mesh;
    public override void _Ready()
    {
       water_mesh = (Terrain)GetNode("Water/WaterMesh");
        ground_mesh = (Terrain)GetNode("Ground/GroundMesh");
        //GenerateMeshes();
    }
    private void GenerateMeshes(){
        ground_mesh.GenerateTerrain();
        water_mesh.GenerateTerrain();
        EmitSignal("Clock");
    }
    #endregion
    
    #region Util
    public static int Index(int row, int col){
        return Index(row,col,DIMENSIONS);
    }
    public static int Index(int row, int col, int dim){
        return (row * dim) + col;
    }
    #endregion
}