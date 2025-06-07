using Godot;
using System;

public partial class Ball : Node2D
{
	[Export] public float Speed = 300f;
	private Vector2 _velocity = Vector2.Zero;
	private RayCast2D _rayCast2D;
	[Signal] public delegate void GivePointEventHandler(bool leftTeamPoint);
	private AudioStreamPlayer2D _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rayCast2D = GetNode<RayCast2D>($"RayCast2D");
		_player = GetNode<AudioStreamPlayer2D>($"AudioStreamPlayer2D");
	}

	public void StopBall()
	{
		_velocity = Vector2.Zero;
	}
	public void SetRandomVelocity()
	{
		_velocity = new Vector2(1, GD.Randf()).Normalized();
		GD.Print(_velocity);
	}
	
	private void CheckCollision()
	{
		_rayCast2D.TargetPosition = _velocity * Speed * (float)GetPhysicsProcessDeltaTime();

		if (_rayCast2D.IsColliding())
		{
			// Get the bat hit by the ball
			Node2D collider = _rayCast2D.GetCollider() as Node2D;

			// Change angle based on the difference in position of the ball from the center of the bat + xOffset
			float xOffset = Mathf.Sign(GlobalPosition.X - collider.GlobalPosition.X) * 16;
			_velocity = new Vector2(GlobalPosition.X - collider.GlobalPosition.X + xOffset, GlobalPosition.Y - collider.GlobalPosition.Y).Normalized();

			// Make sure the max angle is 45 degrees
			_velocity.X += Mathf.Sign(_velocity.X);
			_velocity = _velocity.Normalized();

			// Play sound
			_player.Play();
		}
	}

	// Get the balls velocity
	public Vector2 GetVelocity()
	{
		return _velocity;
	}

	// Checks if the ball is outside the boundry
	private void CheckPlayAreaPosition()
	{
		if (GlobalPosition.X < 0)
		{
			EmitSignal(nameof(GivePoint), false);
			GD.Print("Point to right side");
		}
		else if (GlobalPosition.X > Boundry.GetLevelSize(this).X)
		{
			EmitSignal(nameof(GivePoint), true);
			GD.Print("Point to left side");
		}
		else if (GlobalPosition.Y < 0 || GlobalPosition.Y > Boundry.GetLevelSize(this).Y)
		{
			_velocity.Y *= -1;
		}
	}

    public override void _PhysicsProcess(double delta)
    {
		CheckPlayAreaPosition();

		// Check collision with bat
		CheckCollision();

		GlobalPosition += _velocity * Speed * (float)delta;
    }


	
}
