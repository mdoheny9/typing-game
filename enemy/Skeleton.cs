using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Skeleton : CharacterBody2D
{
	Player player; // reference to player
	
	[Export]
	// movement variables
	private int speed = 20;
	bool within_attack_range = false;
	Node currentScene;
	private AnimatedSprite2D _skeletonSprite;
	private Vector2 currentVelocity;
	private String direction = "down";
	
	// textlabel variables
	RichTextLabel prompt;
	String prompt_text;
	
	// colour variables 
	[Export] Color Green = new Color("#00ff00");
	[Export] Color White = new Color("#ffffff");

	public void setNextChar(int nextCharIndex) { // nonoptimal >.<
		string green_text = getBbcodeColourTag(Green) + prompt_text.Substring(0, nextCharIndex) + getBbcodeEndColourTag();
		// if (nextCharIndex == prompt_text.Length) white_text = ""
		string white_text = nextCharIndex != prompt_text.Length ? getBbcodeColourTag(White) + prompt_text.Substring(nextCharIndex, prompt_text.Length - nextCharIndex) + getBbcodeEndColourTag() : "";
		
		prompt.ParseBbcode("[center]" + green_text + white_text + "[/center]");
	}
	public string getBbcodeColourTag(Color color) {
		return "[color=#" + color.ToHtml(false) + "]"; // return unique colour string
	}
	public string getBbcodeEndColourTag() {
		return "[/color]";
	}
	
	public override void _Ready() {
		var currentScene = GetTree().CurrentScene;
		string sceneName = currentScene.Name;
		player = (Player)GetTree().Root.GetNode(sceneName).GetNode("Player");
		_skeletonSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		prompt = GetNode<RichTextLabel>("RichTextLabel");
		prompt_text = StripBBCode(prompt.Text); 
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
		// Compare absolute values to decide which component is dominant.
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
}
