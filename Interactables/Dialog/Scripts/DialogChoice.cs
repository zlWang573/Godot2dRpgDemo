using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/question_bubble.svg")]
public partial class DialogChoice : DialogItem
{
    public Array<DialogBranch> DialogBranches = new Array<DialogBranch>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        foreach (var c in GetChildren())
        {
            if (c is DialogBranch d)
            {
                DialogBranches.Add(d);
            }
        }
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (!CheckForDialogBranches())
        {
            return new string[] { "Requires at least 2 DialogBranch node." };
        }

        return new string[0];
    }

    private bool CheckForDialogBranches()
    {
        int count = 0;
        foreach (var c in GetChildren())
        {
            if (c is DialogBranch)
            {
                count++;
            }
        }

        return count >= 2;
    }
}
