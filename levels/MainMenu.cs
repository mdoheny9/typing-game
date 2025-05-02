using Godot;
using System;

public partial class MainMenu : Control
{
	// references
	private Button start_button;
	
	public override void _Ready() {
		start_button = GetNode<Button>("VBoxContainer/StartButton");
		start_button.GrabFocus();
	}
	private void OnStartButtonPressed() {
		GetTree().ChangeSceneToFile("res://levels/world.tscn");
	}
	
	private void OnMainMenuButtonPressed() {
		GetTree().ChangeSceneToFile("res://levels/main_menu.tscn");
	}
}
