using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	// ===== references =====
	private Node2D enemy_container;
	
	// == spawn time variables ==
	private float spawn_rate;
	private float time_until_spawn = 0;
	
	// === configuration ===
	[Export] PackedScene enemy_scn;
	[Export] Node2D[] spawn_points;
	[Export] float eps = 1f; // enemies per second


	public override void _Ready()
	{
		spawn_rate = 1/eps;
		enemy_container = GetParent().GetNode<Node2D>("EnemyContainer");
	}

	public override void _Process(double delta)
	{
		if (time_until_spawn >= spawn_rate) {
			Spawn();
			time_until_spawn = 0;
		} else {
			time_until_spawn += (float)delta;
		}
	}
	
	private void Spawn() {
		RandomNumberGenerator rng = new RandomNumberGenerator();
		Vector2 location = spawn_points[rng.Randi() % spawn_points.Length].GlobalPosition; // random number between 1 and 4
		
		Skeleton enemy = (Skeleton)enemy_scn.Instantiate();
		enemy.GlobalPosition = location;
		enemy_container.AddChild(enemy);
		
		time_until_spawn = 0;
	}
}
