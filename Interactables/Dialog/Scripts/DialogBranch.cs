using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/answer_bubble.svg")]
public partial class DialogBranch : DialogItem
{
	[Export]
    public string Text { get => text; set => SetText(value); }

    private string text = "ok...";

    public Array<DialogItem> DialogItems = new Array<DialogItem>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
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

    protected override void SetEditorDisplay()
    {
        var p = GetParent();
        if (p is DialogChoice dc)
        {
            SetRelatedText();
            if (dc.DialogBranches.Count > 0)
            {
                ExampleDialog.SetDialogChoice(dc);
            }
        }
    }

    private void SetRelatedText()
    {
        var p = GetParent();
        var pp = p.GetParent();
        if (pp != null)
        {
            var t = pp.GetChild(p.GetIndex() - 1);
            if (t != null && t is DialogText dt)
            {
                ExampleDialog.SetDialogText(dt);
                ExampleDialog.SetTextVisibleCharacters(-1);
            }
        }
    }

    private void SetText(string text)
    {
        this.text = text;
        if (Engine.IsEditorHint())
        {
            if (ExampleDialog != null)
            {
                SetEditorDisplay();
            }
        }
    }
}
