using Godot;
using System;

public partial class LoseScreen : CanvasLayer
{
	public override void _Ready()
	{
		GetNode<Button>("RestartButton").Pressed += OnRestartPressed;
		GetNode<Button>("QuitButton").Pressed += OnQuitPressed;
	}

	private void OnRestartPressed(){
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
		QueueFree();
	}

	private void OnQuitPressed(){
		GetTree().Quit();
	}
}
