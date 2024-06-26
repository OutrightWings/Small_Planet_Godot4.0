using Godot;
using System;
using System.Reflection;

public partial class InputManager : Node3D
{
	#region Signals
    [Signal]
    public delegate void AreaSelectedEventHandler(int row, int col);
    [Signal]
    public delegate void AreaDeSelectedEventHandler();
	[Signal]
    public delegate void CameraRotationEventHandler(int x);
	[Signal]
    public delegate void CameraVelocityEventHandler(Vector2 x);
	[Signal]
    public delegate void ReloadEventHandler();
	#endregion
    [Export]
    PackedScene temp_object;
    [Export]
    PackedScene resourceCollectionRegion;
    public enum ModeSelect{
        SELECT,
        BUILD,
        HARVEST_WOOD_SELECT
    }
	#region Ready
	ModeSelect modeSelect = ModeSelect.SELECT;
	GroundManager ground;
    bool isPlacing = false;
    HarvestArea currentPlacing = null;
    public override void _Ready()
    {
        base._Ready();
        ground = (GroundManager)GetNode("../Ground");
    }
	#endregion
	//KeyPresses
    public override void _UnhandledInput(InputEvent @event)
    {	
		//Camera Keys Pressed
		if(@event.IsActionPressed("Camera_Left")){
			EmitSignal("CameraRotation",1);
		} else if(@event.IsActionPressed("Camera_Right")){
			EmitSignal("CameraRotation",-1);
		} else if(@event.IsActionPressed("Camera_Up")){
			EmitSignal("CameraVelocity",Vector2.Down);
		} else if(@event.IsActionPressed("Camera_Down")){
			EmitSignal("CameraVelocity",Vector2.Up);
		} else if(@event.IsActionPressed("Camera_Back")){
			EmitSignal("CameraVelocity",Vector2.Right);
		} else if(@event.IsActionPressed("Camera_Forward")){
			EmitSignal("CameraVelocity",Vector2.Left);
		}
		//Camera  Keys Released
		else if(@event.IsActionReleased("Camera_Left")){
			EmitSignal("CameraRotation",-1);
		} else if(@event.IsActionReleased("Camera_Right")){
			EmitSignal("CameraRotation",1);
		} else if(@event.IsActionReleased("Camera_Up")){
			EmitSignal("CameraVelocity",Vector2.Up);
		} else if(@event.IsActionReleased("Camera_Down")){
			EmitSignal("CameraVelocity",Vector2.Down);
		} else if(@event.IsActionReleased("Camera_Back")){
			EmitSignal("CameraVelocity",Vector2.Left);
		} else if(@event.IsActionReleased("Camera_Forward")){
			EmitSignal("CameraVelocity",Vector2.Right);
		} 
		//Other Keys
		else if(@event.IsActionPressed("Reload")){
			EmitSignal("Reload");
		}
    }
    public void MeshClicked(Node camera, InputEvent e, Vector3 pos, Vector3 normal, int shape){
       if(e is InputEventMouseButton click && click.IsPressed()){
           if(click.ButtonIndex == MouseButton.Left){
                if(ModeSelect.SELECT == modeSelect){
                    SelectArea(pos);
                }
                else if(ModeSelect.BUILD == modeSelect){
                    PlaceBuilding(pos);
                } else if(ModeSelect.HARVEST_WOOD_SELECT == modeSelect){
                    if(isPlacing){
                        currentPlacing.SetScaledCorner(pos);
                        isPlacing = false;
                    }else {
                        isPlacing = true;
                        currentPlacing = (HarvestArea) resourceCollectionRegion.Instantiate();
                        AddChild(currentPlacing);
                        currentPlacing.SetInitialCorner(pos);
                    }
                    
                }
           } else if(click.ButtonIndex == MouseButton.Right){
                if(isPlacing){
                    FreePlacement();
                } else {
                    EmitSignal("AreaDeSelected");
                }
           }
       } else if(e is InputEventMouseMotion drag){
            if(isPlacing){
                currentPlacing.SetScaledCorner(pos);
            }
       }
    }
	#region Actions
    private void SelectArea(Vector3 pos){
        var row = (int)(pos.X/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        var col = (int)(pos.Z/GlobalData.SCALE / (GlobalData.DIMENSIONS/GlobalData.RESOURCE_GRID_WIDTH));
        EmitSignal("AreaSelected",row, col);
    }
    private void PlaceBuilding(Vector3 pos){
        var obj = (Node3D)temp_object.Instantiate();
        obj.Position = pos;
        AddChild(obj);
    }
    public void SetSelectionMode(ModeSelect select){
        if(modeSelect == select){
            modeSelect = ModeSelect.SELECT;
            FreePlacement();
        } else{
            modeSelect = select;
        }
    }
    private void FreePlacement(){
        if(isPlacing){
            currentPlacing?.Free();
            isPlacing = false;
        }
    }
	#endregion
}
