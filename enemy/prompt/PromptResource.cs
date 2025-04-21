using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PromptResource : Resource
{
	[Export] 
	public Godot.Collections.Array<string> prompts { get; set; }
}
