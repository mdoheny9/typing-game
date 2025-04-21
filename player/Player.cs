using Godot;
using System;

public partial class Player : CharacterBody2D
{	
	// ======= references =======
	private AnimatedSprite2D _playerSprite;
	
	// === movement variables ===
	[Export]
	private int speed = 50;
	private Vector2 currentVelocity;
	private String direction = "down";
	
	// === animation variables ===
	private bool isAttacking = false;

	public override void _Ready() {
		_playerSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// connect signals
		_playerSprite.AnimationFinished += OnAttackAnimationFinished;
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		handleInput();

		Velocity = currentVelocity;
		MoveAndSlide();

		updateAnimation();
	}
	
	public void playDirectedAttack(string direction) {
		isAttacking = true;
		_playerSprite.Animation = "attack_" + direction;
		_playerSprite.Play();
	}
	
	private void OnAttackAnimationFinished() {
		string animationString = _playerSprite.Animation.ToString().Substring(0, 6); // get first 6 characters of animation name
		if (animationString == "attack") {
			isAttacking = false;
		}
	}

	private void updateAnimation() {
		if (isAttacking) {
			return;
		}
		if (currentVelocity.Length() == 0) { // if currentVelocity's magnitude is zero,
			_playerSprite.Animation = "idle_" + direction; // play idle animation
			_playerSprite.Play();
			return;
		}

		string new_direction = "down"; // forward facing on default
		if (currentVelocity.X < 0) new_direction = "left";
		else if (currentVelocity.X > 0) new_direction = "right";
		else if (currentVelocity.Y < 0) new_direction = "up";
		else if (currentVelocity.Y > 0) new_direction = "down";
		
		string cur_animation = _playerSprite.Animation.ToString();
		if (direction != new_direction || cur_animation.StartsWith("idle_")) { // prevents frames from reseting
			direction = new_direction;
			_playerSprite.Animation = "walk_" + direction; // resets frame count to 0 and changes animation to correct direction
		}

		_playerSprite.Play();
	}

	private void handleInput() {
		currentVelocity = Input.GetVector("move_left","move_right", "move_up", "move_down"); //sets unit vector determining movement direction
		currentVelocity = currentVelocity.Normalized(); // avoids diagonal movement being faster
		currentVelocity *= speed; 
	}
}
