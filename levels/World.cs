using Godot;
using System;

public partial class World : Node2D
{
	// ===== references =====
	Player player; // reference to player
	private Skeleton active_enemy = null; // reference to "active" skeleton in range 
	private Node2D enemy_container;
	private Node2D enemy_spawner;
	
	// ==== prompt variables ====
	int cur_letter_index = -1;
	
	public override void _Ready() {
		player = GetNode<Player>("Player");
		enemy_container = GetNode<Node2D>("EnemyContainer");
		enemy_spawner = GetNode<Node2D>("EnemySpawner");
		
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
			char typedChar = (char)eventKey.Unicode; // get char value of key input
			
			if (active_enemy == null) {
				FindNewActiveEnemy(typedChar);
			} else {
				string prompt = active_enemy.getPrompt();
				char nextChar = prompt[cur_letter_index];
				
				if (typedChar == nextChar) { // if key input is successful,
					GD.Print("successfully typed " + typedChar);
					cur_letter_index += 1;
					active_enemy.setNextChar(cur_letter_index);
					if (cur_letter_index == prompt.Length) { // if word fully typed,
						playAttack();
						GD.Print("~~ Enemy Defeated </3 ~~");
						active_enemy.QueueFree(); // remove enemy from scene
						active_enemy = null; 
						cur_letter_index = 1; // reset letter index 
					}
				} else {
					GD.Print("incorrectly typed " + typedChar + " instead of " + nextChar);
				}
			}
		}
	}
	public void playAttack() {
		if (active_enemy != null) {
			Vector2 direction = (active_enemy.GlobalPosition - player.GlobalPosition).Normalized();
			
			string attack_direction = "down"; // forward facing on default
			// Compare absolute values to decide which component is dominant.
			if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y)) {
				attack_direction = direction.X < 0 ? "left" : "right";
			} else {
				attack_direction = direction.Y < 0 ? "up" : "down";
			}
			player.playDirectedAttack(attack_direction);
		}
		
	}
}
