using Godot;
using System;

public partial class CameraController : Camera3D
{
    [Export]
    public int SPEED = 10;
    private Node3D node_rotation, node_zoom;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        node_rotation = (Node3D)GetNode("../..");
        node_zoom = (Node3D)GetNode("..");
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        MoveCamera((float)delta);
        RotateCamera((float)delta);
    }
    private void RotateCamera(float delta){
        float velocity = 0;
        if(Input.IsActionPressed("Camera_Left")){
            velocity += 1;
        }
        if(Input.IsActionPressed("Camera_Right")){
            velocity -= 1;
        }

        if(velocity != 0){
            velocity = velocity * delta;
        }
        
        node_rotation.RotateY(velocity);
    }
    private void MoveCamera(float delta){
        Vector3 velocity = Vector3.Zero;
        if(Input.IsActionPressed("Camera_Forward")){
            velocity.Z -= 1;
        }
        if(Input.IsActionPressed("Camera_Back")){
            velocity.Z += 1;
        }
        if(Input.IsActionPressed("Camera_Up")){
            velocity.Y += 1;
        }
        if(Input.IsActionPressed("Camera_Down")){
            velocity.Y -= 1;
        }

        if(velocity.Length() > 0){
            velocity = velocity.Normalized() * SPEED * delta;
        }
        
        node_zoom.Translate(velocity);
    }
}
