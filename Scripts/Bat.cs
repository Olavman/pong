using Godot;
using System;

public partial class Bat : CharacterBody2D
{
	[Export] public float Speed = 300.0f; // Pixels pr frame
	[Export] public bool IsAI = false;
	private bool _isPlayerOne = true;
	private Ball _ball;
	private Vector2 _velocity = new Vector2(0, 0);
	[Export] private float _AiReactionTime = 0.15f;
	private float _AiReactionTimer = 0;
	private float _AiPrecision = 0.5f;

	private Sprite2D _targetSprite;

    public override void _Ready()
    {
        _targetSprite = GetNode<Sprite2D>($"TargetSprite");
    }


	public override void _PhysicsProcess(double delta)
	{
		// Move the bat
		if (!IsAI) _velocity = GetPlayerMovement();
		else _velocity = GetAiMovement();
		GlobalPosition += _velocity * Speed * (float)delta;

		// Clamp the position within the y boundry
		float batHalfHeight = 32;
		float posY = Mathf.Clamp(GlobalPosition.Y, batHalfHeight, (Boundry.GetLevelSize(this).Y) - batHalfHeight);
		GlobalPosition = new Vector2(GlobalPosition.X, posY);
	}

	public void SetBall(Ball ball)
	{
		_ball = ball;
	}

	public void SetBatNumber(bool isPlayerOne)
	{
		_isPlayerOne = isPlayerOne;
		if (_isPlayerOne)
		{
			GD.Print("This is bat one");
		}
		else
		{
			GD.Print("This is bat two");
		}
	}

	private Vector2 GetPlayerMovement()
	{
		Vector2 velocity = new Vector2(0, 0);
		if (_isPlayerOne)
		{
			if (Input.IsActionPressed("bat_one_up"))
			{
				velocity += new Vector2(0, -1);
			}
			if (Input.IsActionPressed("bat_one_down"))
			{
				velocity += new Vector2(0, 1);
			}
		}
		else
		{
			if (Input.IsActionPressed("bat_two_up"))
			{
				velocity += new Vector2(0, -1);
			}
			if (Input.IsActionPressed("bat_two_down"))
			{
				velocity += new Vector2(0, 1);
			}
		}
		return velocity;
	}

	private Vector2 GetAiMovement()
	{
		_AiReactionTimer += (float)GetProcessDeltaTime();

		if (_AiReactionTimer <= _AiReactionTime) return _velocity; // Don't change velocity as the AI does not have the reaction time to do it

		Vector2 velocity = new Vector2(0, 0);
		float targetY;

		// Reduce precision when far from the ball
		float xDist = Mathf.Abs(_ball.GlobalPosition.X - GlobalPosition.X);

		// Reset the timer as the AI did it's move
		_AiReactionTimer = GD.Randf() * _AiReactionTime;

		// Go towards the middle if the ball is moving away
		float futureXDist = Mathf.Abs(_ball.GlobalPosition.X - GlobalPosition.X + _ball.GetVelocity().X);
		if (xDist < futureXDist)
		{
			targetY = Boundry.GetLevelCenterY(this);
		}
		else targetY = _ball.GetVelocity().Y *Mathf.Min(100 *_AiPrecision, xDist) +_ball.GlobalPosition.Y;

		// Debug
		_targetSprite.GlobalPosition = new Vector2(GlobalPosition.X, targetY);

		float yDistance = targetY - GlobalPosition.Y;

		if (Mathf.Abs(yDistance) < 32) return velocity; // Choose to stay still if sufficiantly close to the balls predicted position

		velocity = new Vector2(0, yDistance).Normalized(); // Move towards the ball, only in the Y axis
		return velocity;
	}
}
