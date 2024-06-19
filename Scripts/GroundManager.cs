using Godot;
using System;
using System.Collections.Generic;

public partial class GroundManager : ResourceManager
{
	[Export]
	public Image biome_image;
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
    }
	public override void CreateMeshImages(){
        for(int row = 0; row < GlobalData.DIMENSIONS; row++){
            for(int col = 0; col < GlobalData.DIMENSIONS; col++){
                ResourceArea resource = GetResourceArea(row,col);
                
                float rand = (float)GD.RandRange(0,0.2f);

                float steep = resource.height;
                biome_image.SetPixel(row,col,Colors.Transparent);
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
                     float veg_noise  = resource.GetResourceAmount("noise_veg");
                    if(veg_noise > 0){
                        resource.AddToResource("wood",(int)(veg_noise * GlobalData.MAX_WOOD));
                        resource.DrawTexture(ref biome_image);
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
                var river = resource.GetResourceAmount("noise_river");
                var mountain = resource.GetResourceAmount("noise_mountain");
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
