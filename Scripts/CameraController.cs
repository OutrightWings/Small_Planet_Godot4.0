using Godot;
using System;

public partial class CameraController : Camera3D
{
    [Export]
    public int SPEED = 10;
    private Node3D node_rotation, node_zoom;

    private Vector3 velocity = Vector3.Zero;
    private int rotation = 0;
    public override void _Ready()
    {
        node_rotation = (Node3D)GetNode("../..");
        node_zoom = (Node3D)GetNode("..");
    }

    public override void _Process(double delta)
    {
        MoveCamera((float)delta);
        RotateCamera((float)delta);
    }
    private void RotateCamera(float delta){
        float r = 0;
        if(rotation != 0){
            r = rotation * delta;
        }
        node_rotation.RotateY(r);
    }
    private void MoveCamera(float delta){
        Vector3 v = Vector3.Zero;

        if(velocity.Length() > 0){
            v = velocity.Normalized() * SPEED * delta;
        }
        
        node_zoom.Translate(v);
    }

    public void UpdateRotation(int x){
        rotation += x;
    }
    public void UpdateVelocity(Vector2 vec){ //X maps to z, Y to y
        velocity.Z += vec.X;
        velocity.Y += vec.Y;
    }
}
