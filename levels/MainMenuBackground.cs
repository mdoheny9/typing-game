using Godot;
using System;

public partial class MainMenuBackground : ParallaxBackground
{
	 [Export]
	private float scrollingSpeed = 100f;

	public override void _Process(double delta)
	{
		ScrollOffset -= new Vector2((float)(scrollingSpeed * delta), 0);
	}
}
