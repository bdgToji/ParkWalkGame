using System;
using Godot;

public partial class Player : CharacterBody3D
{
	[Export] public float Speed { get; set; } = 10.0f;
	[Export] public float JumpForce { get; set; } = 15.0f;
	[Export] public float Gravity { get; set; } = 50.0f;
	
	[Export] public float MouseSensitivity {get; set;} = 0.1f;
	
	private Node3D _cameraPivot;
	private Camera3D _camera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		
		_cameraPivot = GetNode<Node3D>("CameraPivot");
		_camera = GetNode<Camera3D>("CameraPivot/Camera");
	}
	
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Visible? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
		}

		if(@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * MouseSensitivity));

			var pitchRotaton = _cameraPivot.RotationDegrees;
			pitchRotaton.X -= mouseMotion.Relative.Y * MouseSensitivity;
			pitchRotaton.X = Mathf.Clamp(pitchRotaton.X, -90, 90);
			_cameraPivot.RotationDegrees = pitchRotaton;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = Vector3.Zero;

		if (Input.IsActionPressed("move_forward"))
			direction -= Transform.Basis.Z;
		if (Input.IsActionPressed("move_back"))
			direction += Transform.Basis.Z;
		if (Input.IsActionPressed("move_left"))
			direction -= Transform.Basis.X;
		if (Input.IsActionPressed("move_right"))
			direction += Transform.Basis.X;

		direction = direction.Normalized();
		Velocity = new Vector3(direction.X * Speed, Velocity.Y, direction.Z * Speed);

		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("jump"))
				Velocity = new Vector3(Velocity.X, JumpForce, Velocity.Z);
		}
		else
		{
			Velocity = new Vector3(Velocity.X, Velocity.Y - Gravity * (float)delta, Velocity.Z);
		}

		MoveAndSlide();
	}
}
