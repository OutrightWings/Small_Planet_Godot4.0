using Godot;
using System;

public partial class CameraController : Camera3D
{
    [Export]
    public int SPEED = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        MoveCamera((float)delta);
    }

    private void MoveCamera(float delta){
        Vector3 velocity = Vector3.Zero;
        if(Input.IsActionPressed("Camera_Forward")){
            velocity.Z -= 1;
        }
        if(Input.IsActionPressed("Camera_Back")){
            velocity.Z += 1;
        }
        if(Input.IsActionPressed("Camera_Left")){
            velocity.X -= 1;
        }
        if(Input.IsActionPressed("Camera_Right")){
            velocity.X += 1;
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
        
        Translate(velocity);
    }
}
