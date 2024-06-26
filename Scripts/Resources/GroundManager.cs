using Godot;
using System;
using System.Collections.Generic;

public partial class GroundManager : ResourceManager
{
	[Export]
	public Image biome_image;
    public ResourceMap harvestedResources = new();
    public override void _Ready()
    {
        base._Ready();
    }
	public override void UpdateTick()
    {
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                ResourceArea area = GetResourceArea(row,col);
                var neighbors = GetNeighbors(row,col,1);
                area.GrowResource(ref neighbors);
                area.DrawTexture(ref biome_image);
            }
        }
        EmitSignal("UpdateOverlayImage",biome_image);
        EmitSignal("EmitHarvestedResources",harvestedResources);
    }
	public override void CreateMeshImages(){
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                ResourceArea resourceArea = GetResourceArea(row,col);
                
                float rand = (float)GD.RandRange(0,0.2f);

                float steep = resourceArea.height;
                biome_image.SetPixel(row,col,Colors.Transparent);
                Color c;
                if(steep <= GlobalData.WATER_LEVEL-3.5){
                     c = Colors.DarkGray;
                }
                else if(steep <= GlobalData.WATER_LEVEL+1.5f){
                     c = Colors.Tan;
                }
                else if(steep <= GlobalData.HEIGHT/5f){
                    resourceArea.isAboveGround = true;
                     c = Colors.DarkGoldenrod;
                     float veg_noise  = resourceArea.resourceMap.GetResourceAmount("noise_veg");
                    if(veg_noise > 0){
                        resourceArea.resourceMap.AddToResource("wood",(int)(veg_noise * GlobalData.MAX_WOOD));
                        resourceArea.DrawTexture(ref biome_image);
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
                var river = resourceArea.resourceMap.GetResourceAmount("noise_river");
                var mountain = resourceArea.resourceMap.GetResourceAmount("noise_mountain");
                //RIVER TEST: INTERIM VALUES
                if(Mathf.Abs(river)+0.1f*(1-Mathf.Abs(mountain)) <= .1f){
                    c = Colors.BlueViolet;
                }
                c = c.Darkened(rand);
                base_image.SetPixel(row,col,c);
            }
        }
        EmitSignal("UpdateBaseImage",base_image);
		EmitSignal("UpdateOverlayImage",biome_image);
		EmitSignal("UpdateResources");
    }
}
