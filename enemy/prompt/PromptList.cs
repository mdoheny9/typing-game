using Godot;
using System;

public partial class PromptList : Node
{
	[Export]
	public PromptResource PromptData;

	public string getPrompt() {
		RandomNumberGenerator rng = new RandomNumberGenerator();
		int index = (int)(rng.Randi() % PromptData.prompts.Count);
		return PromptData.prompts[index];
	}
}
