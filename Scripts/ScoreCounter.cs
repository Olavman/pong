using Godot;
using System;

public partial class ScoreCounter : Label
{
	public int Score { get; set; }
    public override void _Ready()
	{
		Score = 0;
		UpdateScore();
	}

	private void UpdateScore()
	{
		Text = Score.ToString();
	}
	public void AddToScore()
	{
		Score++;
		UpdateScore();
	}
}
