using Godot;
using System;

public partial class World : Node2D
{
	private Skeleton active_enemy = null; // reference to "active" skeleton in range 
	int cur_letter_index = -1;
	Node2D enemy_container;
	
	public override void _Ready() {
		enemy_container = GetNode<Node2D>("EnemyContainer");
	}
	
	public void FindNewActiveEnemy(char typed_character) {
		foreach (Skeleton enemy in enemy_container.GetChildren()) {
			// string prompt = (string)GetNode<Skeleton>("Skeleton").getPrompt(); // find active skeleton in range 
			string prompt = enemy.getPrompt(); // find active skeleton in range
			char nextChar = prompt[0];
			if (nextChar == typed_character) {
				GD.Print("found new enemy that starts with " + nextChar);
				active_enemy = enemy; // GetNode<Skeleton>("Skeleton"); // select skeleton based on key input 
				cur_letter_index = 1;
				active_enemy.setNextChar(cur_letter_index);
				break;
			}
		}
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		// cast event to InputEventKey and eliminate echo
		if (@event is InputEventKey eventKey && eventKey.Pressed && !eventKey.Echo) {
			char typedChar = (char)eventKey.Unicode;
			
			if (active_enemy == null) {
				FindNewActiveEnemy(typedChar);
			} else {
				string prompt = active_enemy.getPrompt();
				char nextChar = prompt[cur_letter_index];
				
				if (typedChar == nextChar) { // successful key input
					GD.Print("successfully typed " + typedChar);
					cur_letter_index += 1;
					active_enemy.setNextChar(cur_letter_index);
					if (cur_letter_index == prompt.Length) {
						GD.Print("~~ Enemy Defeated </3 ~~");
						active_enemy.QueueFree(); // enemy defeated!!! 
						active_enemy = null;
						cur_letter_index = 1; // reset letter index 
					}
				} else {
					GD.Print("incorrectly typed " + typedChar + " instead of " + nextChar);
				}
			}
		}
	}
}
