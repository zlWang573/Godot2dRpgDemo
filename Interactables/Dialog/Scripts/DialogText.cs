using Godot;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/text_bubble.svg")]
public partial class DialogText : DialogItem
{
	[Export(PropertyHint.MultilineText)]
	public string Text { get => text; set => SetText(value); }

    private string text = "Placeholder text";

    protected override void SetEditorDisplay()
    {
        ExampleDialog.SetDialogText(this);
        ExampleDialog.SetTextVisibleCharacters(-1);
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
