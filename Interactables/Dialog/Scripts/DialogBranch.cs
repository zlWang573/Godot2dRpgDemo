using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/answer_bubble.svg")]
public partial class DialogBranch : DialogItem
{
	[Export]
	public string Text = "ok...";

	public Array<DialogItem> DialogItems = new Array<DialogItem>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        foreach (var c in GetChildren())
        {
            if (c is DialogItem d)
            {
                DialogItems.Add(d);
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
