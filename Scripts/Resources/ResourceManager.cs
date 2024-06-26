using Godot;
using System;
using System.Collections.Generic;
public abstract partial class ResourceManager : Node3D
{
	[Export]
	public Image base_image;
    private List<ResourceArea> resources = new List<ResourceArea>();
    [Signal]
    public delegate void UpdateBaseImageEventHandler(Image image);
	[Signal]
    public delegate void UpdateOverlayImageEventHandler(Image image);
    [Signal]
    public delegate void UpdateResourcesEventHandler();
	[Signal]
    public delegate void EmitSelectedResourcesEventHandler(ResourceArea[] areas);
	[Signal]
    public delegate void EmitHarvestedResourcesEventHandler(ResourceMap resources);

    #region Getters/Setters
    //Takes unscaled pos, returns unscaled
    public float GetHeightAtPoint(float row, float col){
		float unscaledHeightAtPoint = GetResourceArea((int)row,(int)col).height;
		return unscaledHeightAtPoint;
	}
	public void EmptyResources(){
		resources.ForEach(resource => resource.Free());
		resources.Clear();
	}
	public ResourceArea AddNewResourceArea(int row, int col, float height){
        ResourceArea resource = new(row, col, height);
		resources.Add(resource);
		return GetResourceArea(row,col);
	}
	public void AddResourceToSection(int row, int col, string name, float amount){
        GetResourceArea(row,col).resourceMap.AddToResource(name,amount);
	}
	public ResourceArea GetResourceArea(int row, int col){
		int index = GlobalData.Index(row,col);
		return resources[index];
	}
	public float GetResourceAmount(int row, int col, string name){
		return GetResourceArea(row,col).resourceMap.GetResourceAmount(name);
	}
	public ResourceArea[] GetNeighbors(int selected_row, int selected_col,int radious){
		int dimension = radious*2+1;
        var selected = new ResourceArea[dimension*dimension];
        int pos = 0;
        for(int row = -radious; row < radious+1; row++){
            for(int col = -radious; col < radious+1; col++){
                int index = GlobalData.Index(selected_row+row,selected_col+col);
                if(index >= 0 && index < GlobalData.DIMENSIONS*GlobalData.DIMENSIONS){
                    selected[pos] = resources[index];
                }
                else{
                    selected[pos] = null;
                }
                pos++;
            }
        }
		return selected;
	}
	public void SelectedResource(int row, int col){
		EmitSignal("EmitSelectedResources",GetNeighbors(row,col,GlobalData.REGION_SELECTION_RADIOUS));
	}
	#endregion
	public abstract void CreateMeshImages();
    public abstract void UpdateTick();
}