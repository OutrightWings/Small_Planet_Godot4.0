using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public partial class ResourceArea : Node
{
    public ResourceMap resourceMap = new();
    public bool isAboveGround = false;
    public int row, col;
    public float height;
    Spline alphaSpline;
    public ResourceArea():this(0,0,0){}
    public ResourceArea(int row, int col, float height){
        this.row = row;
        this.col = col;
        this.height = height;
        Vector2[] points = {
            new Vector2(0,0.3f),
            new Vector2(1,1)
        };
        alphaSpline = new Spline(points);
        resourceMap.SetResource("wood",0);
		resourceMap.SetResource("plantGrowthRate",GlobalData.BASE_PLANT_GROWTH_RATE);
        resourceMap.SetResource("alphaVarience",(float)GD.RandRange(0,0.2f));
    }
    public void GrowResource(ref ResourceArea[] neighbors){
        if(isAboveGround){
            float wood = resourceMap.GetResourceAmount("wood");
            float deltaWood = wood * resourceMap.GetResourceAmount("plantGrowthRate") * (1 - (wood / GlobalData.MAX_WOOD));
            resourceMap.AddToResource("wood",deltaWood);
            if(wood >= 0.4f * GlobalData.MAX_WOOD){
                for(int itr = 1; itr < 8; itr+=2){
                    ResourceArea neighbor = neighbors[itr];
                    if(neighbor?.resourceMap.GetResourceAmount("wood") == 0){
                        neighbor.resourceMap.AddToResource("wood",0.01f);
                    }
                }
            }
        }
    }
    public void DrawTexture(ref Image image){
        if(isAboveGround){
            Color color = Colors.Transparent;
            float wood = resourceMap.GetResourceAmount("wood");
            if(wood > 0){
                color = Colors.DarkGreen;
                var alpha = wood/GlobalData.MAX_WOOD;
                alpha = alphaSpline.Interpolate(alpha);
                color.A = alpha;
                color = color.Darkened(resourceMap.GetResourceAmount("alphaVarience"));
            }
            image.SetPixel(row,col,color);
        }
    }
    
}
