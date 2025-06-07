using Godot;
using System;

public static class Boundry
{
	public static Vector2 GetLevelSize(Node requester)
	{
		return requester.GetViewport().GetVisibleRect().Size;
	}
	public static float GetLevelCenterX(Node requester)
	{
		return requester.GetViewport().GetVisibleRect().Size.X / 2;
	}
	public static float GetLevelCenterY(Node requester)
	{
		return requester.GetViewport().GetVisibleRect().Size.Y / 2;
	}
}
