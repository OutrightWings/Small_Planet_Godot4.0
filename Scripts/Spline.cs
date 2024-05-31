using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Spline
{
    List<Vector2> points;
    public Spline(Vector2[] points){
        this.points = points.ToList();
    }
    public float Interpolate(float noiseValue){
        for(int i = 0; i < points.Count -1; i++){
            if(noiseValue >= points[i].X && noiseValue <= points[i+1].X){
                float distanceTo = 1 - ((points[i+1].X - noiseValue) / (points[i+1].X - points[i].X));
                return Mathf.Lerp(points[i].Y,points[i+1].Y,distanceTo);
            }
        }
        return noiseValue;
    }
}
