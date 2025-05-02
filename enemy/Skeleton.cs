using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Skeleton : CharacterBody2D
{
	// ======= references =======
	Player player; // reference to player
	Node currentScene;
	private AnimatedSprite2D _skeletonSprite;
	private RichTextLabel prompt;
	PromptList prompt_list;
	
	// === movement variables ===
	[Export]
	private int speed = 20;
	private Vector2 currentVelocity;
	private String direction = "down";
	// private bool within_attack_range = false;
	
	// ==== prompt variables ====
	private string prompt_text;
	
	// ==== colour variables ====
	[Export] public Color Green = new Color("#00ff00");
	[Export] public Color White = new Color("#ffffff");
	[Export] public Color Grey = new Color("#878787");
	
	// ==== attack variables ====
	[Export] float damage = 50f;
	[Export] float aps = 2f; // attacks per second
	float attack_speed;
	float time_until_attack;
	bool within_attack_range = false;
	
	
	public override void _Ready() {
		var currentScene = GetTree().CurrentScene;
		string sceneName = currentScene.Name;
		player = (Player)GetTree().Root.GetNode(sceneName).GetNode("Player");
		_skeletonSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		prompt = GetNode<RichTextLabel>("RichTextLabel");
		prompt_list = GetTree().Root.GetNode<PromptList>("World/PromptList"); // adjust the path as needed
		prompt_text = prompt_list.getPrompt();
		
		displayPrompt();
		
		attack_speed = 1/aps;
		time_until_attack = attack_speed;
		
		// connect signals
		var attackRange = GetNode<Area2D>("AttackRange");
		attackRange.BodyEntered += OnAttackRangeBodyEnter;
		attackRange.BodyExited += OnAttackRangeBodyExit;
	}
	
	public override void _Process(double delta) {
		if (within_attack_range && time_until_attack <= 0) {
			Attack();
			time_until_attack = attack_speed;
		} else {
			time_until_attack -= (float)delta;
		}
	}
	
	public void Attack() {
		player.GetNode<Health>("Health").Damage(damage);
		// add player animation too
	}
	
	public void displayPrompt() { // nonoptimal >.<
		prompt.ParseBbcode("[center]" + getBbcodeColourTag(Grey) + prompt_text + getBbcodeEndColourTag() + "[/center]");
	}

	public void setNextChar(int nextCharIndex) { // nonoptimal >.<
		string green_text = getBbcodeColourTag(Green) + prompt_text.Substring(0, nextCharIndex) + getBbcodeEndColourTag();
		// if (nextCharIndex == prompt_text.Length) white_text = "",
		string white_text = nextCharIndex != prompt_text.Length ? getBbcodeColourTag(White) + prompt_text.Substring(nextCharIndex, prompt_text.Length - nextCharIndex) + getBbcodeEndColourTag() : "";
		
		prompt.ParseBbcode("[center]" + green_text + white_text + "[/center]");
	}
	
	public string getBbcodeColourTag(Color color) {
		return "[color=#" + color.ToHtml(false) + "]"; // return unique colour string
	}
	
	public string getBbcodeEndColourTag() {
		return "[/color]";
	}
	
	public string getPrompt() { 
		return prompt_text;
	}
	
	public static string StripBBCode(string bbcodeText) { // extracts text value of BBCode string
		return Regex.Replace(bbcodeText, @"\[(.*?)\]", ""); // removes anything inside brackets
	}
	
	public override void _PhysicsProcess(double delta) {
		if (player != null) {
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			currentVelocity = direction * speed;
			Velocity = currentVelocity;
		}
		MoveAndSlide();
		updateAnimation();
	}
	
	private void updateAnimation() {
		if (currentVelocity.Length() == 0) { // if currentVelocity's magnitude is zero,
				_skeletonSprite.Animation = "idle_" + direction; // play idle animation
				_skeletonSprite.Play();
				return;
		}
		string new_direction = "down"; // forward facing on default
		if (Mathf.Abs(currentVelocity.X) > Mathf.Abs(currentVelocity.Y)) {
			new_direction = currentVelocity.X < 0 ? "left" : "right";
		} else {
			new_direction = currentVelocity.Y < 0 ? "up" : "down";
		}
		string cur_animation = _skeletonSprite.Animation.ToString();
		if (direction != new_direction || cur_animation.StartsWith("idle_")) { // prevents frames from reseting
			direction = new_direction;
			_skeletonSprite.Animation = "walk_" + direction; //resets frame count to 0 and changes animation to correct direction
		}
		_skeletonSprite.Play();
	}
	
	public void OnAttackRangeBodyEnter(Node2D body) {
		if (body.IsInGroup("player")) {
			GD.Print("player in range");
			within_attack_range = true;
		}
	}
	
	public void OnAttackRangeBodyExit(Node2D body) {
		if (body.IsInGroup("player")) {
			within_attack_range = false;
			time_until_attack = attack_speed;
		}
	}
}
