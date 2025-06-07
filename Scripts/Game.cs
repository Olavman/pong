using Godot;
using System;

public partial class Game : Node
{
	private Vector2 _levelSize;
	private Bat _bat1;
	private Bat _bat2;
	private Ball _ball;
	private ScoreCounter _scoreCounter1;
	private ScoreCounter _scoreCounter2;
	private Countdown _countdown;
	private AudioStreamPlayer _musicPlayer;
	private AudioStreamPlayer _pointSoundPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelSize = GetViewport().GetVisibleRect().Size;

		_ball = GetNode<Ball>($"Ball");
		_ball.GivePoint += OnGivePoint;

		_bat1 = GetNode<Bat>($"Bat1");
		_bat1.SetBall(_ball);

		_bat2 = GetNode<Bat>($"Bat2");
		_bat2.SetBall(_ball);

		_scoreCounter1 = GetNode<ScoreCounter>($"ScoreCounter1");

		_scoreCounter2 = GetNode<ScoreCounter>($"ScoreCounter2");

		_countdown = GetNode<Countdown>($"Countdown");
		_countdown.CountingComplete += OnCountingComplete;

		_musicPlayer = GetNode<AudioStreamPlayer>($"MusicPlayer");
		_pointSoundPlayer = GetNode<AudioStreamPlayer>($"PointSoundPlayer");

		_bat1.SetBatNumber(true); // Bat is bat1
		_bat1.SetBatNumber(false); // Bat is bat2

		ResetGame();
	}

	private void ResetGame()
	{
		_ball.StopBall();
		_countdown.StartCountDown();
		InitializePositions();
	}

	private void OnCountingComplete()
	{
		_ball.SetRandomVelocity();
	}


	private void OnGivePoint(bool leftTeamPoint)
	{
		if (leftTeamPoint)
		{
			_scoreCounter1.AddToScore();
		}
		else
		{
			_scoreCounter2.AddToScore();
		}
		_pointSoundPlayer.Play();
		ResetGame();
	}

	private void InitializePositions()
	{
		float xOffset = 48;
		_bat1.GlobalPosition = new Vector2(xOffset, Boundry.GetLevelCenterY(this)); // Bat 1 (left)
		_bat2.GlobalPosition = new Vector2(Boundry.GetLevelSize(this).X - xOffset, Boundry.GetLevelCenterY(this)); // Bat 2 (right)
		_ball.GlobalPosition = new Vector2(Boundry.GetLevelCenterX(this), Boundry.GetLevelCenterY(this)); // Ball
		//_countdown.Position = new Vector2(Boundry.GetLevelCenterX(this), Boundry.GetLevelCenterY(this)); // Countdown label
	}
}
