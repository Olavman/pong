using Godot;
using System;

public partial class Countdown : Label
{
	public float timer = 0;
	const float countdownTime = 3f;
	private bool _isCounting = false;
	private bool _isCountingUp = false;
	[Signal] public delegate void CountingCompleteEventHandler();
	private AudioStreamPlayer _player;

    public override void _Ready()
    {
        _player = GetNode<AudioStreamPlayer>($"AudioStreamPlayer");
    }


	public override void _Process(double delta)
	{
		if (_isCounting)
		{
			timer = _isCountingUp ? timer += (float)delta : timer -= (float)delta; // Add to count if counting up, and subtract to count if counting down
			if (timer > countdownTime || timer < 0)
			{
				EmitSignal(nameof(CountingComplete));
				StopCounting();
			}
			UpdateLabel();
		}
	}

	private void UpdateLabel()
	{
		int offset = _isCountingUp ? 0 : 1;
		int currentCount = Mathf.FloorToInt(timer + offset);
		if (currentCount != Text.ToInt())
		{
			_player.Play();
		}
		Text = currentCount.ToString();
	} 

	public void StartCountUp()
	{
		Visible = true;
		_isCounting = true;
		_isCountingUp = true;
		timer = 0;
	}
	public void StartCountDown()
	{
		Visible = true;
		_isCounting = true;
		_isCountingUp = false;
		timer = countdownTime;
	}
	public void StopCounting()
	{
		Visible = false;
		_isCounting = false;
	}
}
