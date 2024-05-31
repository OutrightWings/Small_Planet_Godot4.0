using Godot;
using System;

public partial class ResourceArea : Node
{
    public bool isAboveGround = false;
    public float plantGrowthRate;
    public float wood = 0;
    public float alphaVarience;
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
        alphaVarience = (float)GD.RandRange(0,0.2f);
        plantGrowthRate = GlobalData.BASE_PLANT_GROWTH_RATE;
    }
    public void GrowResource(ResourceArea[] neighbors){
        if(isAboveGround){
            float deltaWood = wood * plantGrowthRate * (1 - (wood / GlobalData.MAX_WOOD));
            wood += deltaWood;
            if(wood >= 0.4f * GlobalData.MAX_WOOD){
                for(int itr = 0; itr < 4; itr++){
                    ResourceArea neighbor = neighbors[itr];
                    if(neighbor!= null && neighbor.wood == 0){
                        neighbor.wood = 0.01f;
                    }
                }
            }
        }
    }
    public void DrawTexture(ref Image image){
        if(isAboveGround){
            Color color = Colors.Transparent;
            if(wood > 0){
                color = Colors.DarkGreen;
                var alpha = wood/GlobalData.MAX_WOOD;
                alpha = alphaSpline.Interpolate(alpha);
                color.A = alpha;
                color = color.Darkened(alphaVarience);
            }
            image.SetPixel(row,col,color);
        }
        
    }
}
