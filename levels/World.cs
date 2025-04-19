using Godot;
using System;

public partial class World : Node2D
{
	private Skeleton active_enemy = null; // reference to "active" skeleton in range 
	int cur_letter_index = -1;
	
	public void FindNewActiveEnemy(char typed_character) {
		string prompt = (string)GetNode<Skeleton>("Skeleton").getPrompt(); // find active skeleton in range 
		if (prompt[0] == typed_character) {
			active_enemy =  GetNode<Skeleton>("Skeleton"); // select skeleton based on key input 
			cur_letter_index = 1;
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
					cur_letter_index += 1;
					if (cur_letter_index == prompt.Length) {
						active_enemy.QueueFree(); // enemy defeated!!! 
						active_enemy = null;
						cur_letter_index = 1; // reset letter index 
					}
				}
			}
		}
	}
}
