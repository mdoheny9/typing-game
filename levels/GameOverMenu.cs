using Godot;
using System;

public partial class GameOverMenu : Control
{
	// references
	private Button play_again_button;
	
	public override void _Ready() {
		play_again_button = GetNode<Button>("VBoxContainer/PlayAgainButton");
		play_again_button.GrabFocus();
	}
	private void OnPlayAgainButtonPressed() {
		GetTree().ChangeSceneToFile("res://levels/world.tscn");
	}
	
	private void OnMainMenuButtonPressed() {
		GetTree().ChangeSceneToFile("res://levels/main_menu.tscn");
	}
}
