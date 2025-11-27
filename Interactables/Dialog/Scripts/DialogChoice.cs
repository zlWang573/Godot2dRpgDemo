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
        base._Ready();
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

    protected override void SetEditorDisplay()
    {
        SetRelatedText();
        if (DialogBranches.Count > 0)
        {
            ExampleDialog.SetDialogChoice(this);
        }
    }

    private void SetRelatedText()
    {
        var p = GetParent();
        if (p != null)
        {
            var t = p.GetChild(GetIndex() - 1);
            if (t != null && t is DialogText dt)
            {
                ExampleDialog.SetDialogText(dt);
                ExampleDialog.SetTextVisibleCharacters(-1);
            }
        }
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
