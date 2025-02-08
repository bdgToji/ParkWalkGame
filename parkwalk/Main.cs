using Godot;
using System;

public partial class Main : Node3D
{
	private int _score = 0;
	private int _totalCoins = 0;
	private Label _coinCounterLabel;

	[Export]
	public float TimeDuration { get; set; } = 100f;
	private float _timeRemaining;
	private Label _timerLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timeRemaining = TimeDuration;
		_timerLabel = GetNode<Label>("CanvasLayer/TimerLabel");


		_coinCounterLabel = GetNode<Label>("CanvasLayer/CoinCounterLabel");

		var coins = GetTree().GetNodesInGroup("Coins");
		_totalCoins = coins.Count;

		foreach(Node coin in coins)
		{
			coin.Connect(nameof(Coin.Collected), Callable.From(OnCoinCollected));
		}

		UpdateCoinCounter();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_timeRemaining > 0)
		{
			_timeRemaining -= (float)delta;
			UdateTimerLabel();

			if(_timeRemaining <=0)
			{
				ShowLoseScreen();
			}
		}
	}

	private void OnCoinCollected()
	{
		_score++;
		UpdateCoinCounter();

		if(_score >= _totalCoins){
			var winScreen = GD.Load<PackedScene>("res://win_screen.tscn").Instantiate();
			GetTree().Root.AddChild(winScreen);
			GetTree().Paused = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void UpdateCoinCounter()
	{
		_coinCounterLabel.Text = $"{_score}/{_totalCoins} Coins";
	}

	private void UdateTimerLabel()
	{
		_timerLabel.Text = $"Time: {Mathf.CeilToInt(_timeRemaining)}";
	}

	private void ShowLoseScreen()
	{
		var loseScreen = GD.Load<PackedScene>("res://lose_screen.tscn").Instantiate();
		GetTree().Root.AddChild(loseScreen);
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}
}
