using Godot;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/text_bubble.svg")]
public partial class DialogText : DialogItem
{
	[Export(PropertyHint.MultilineText)]
	public string Text { get; set; } = "Placeholder text";
}
