using Godot;
using System;

public partial class Coin : Area3D
{
	[Signal] public delegate void CollectedEventHandler();

	private void OnBodyEntered(Node body)
	{
		if(body is Player)
		{
			EmitSignal(nameof(Collected));
			QueueFree();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
