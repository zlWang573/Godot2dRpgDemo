using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/question_bubble.svg")]
public partial class DialogChoice : DialogItem
{
	[Export]
	public Array<string> Choices = new Array<string>() { "Yes", "No" };
}
