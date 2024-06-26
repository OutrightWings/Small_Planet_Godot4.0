using Godot;

public partial class Housing : Node3D
{
	[Export]
	int MAX_PEOPLE = 1;
	private int current_people = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
