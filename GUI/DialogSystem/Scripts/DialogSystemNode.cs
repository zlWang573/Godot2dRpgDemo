using Godot;
using System;

[Tool]
[Icon("res://GUI/DialogSystem/Icons/star_bubble.svg")]
public partial class DialogSystemNode : CanvasLayer
{
	private bool isActive = false;

	private Control dialogUI;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dialogUI = GetNode<Control>("DialogUI");
		if (Engine.IsEditorHint())
		{
			if (GetViewport() is Window)
			{
				GetParent().RemoveChild(this);
				return;
			}

			return;
		}

		HideDialog();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (isActive == false)
		{
			// return;
		}

		if (@event.IsActionPressed("test"))
		{
			if (isActive == false)
			{
				ShowDialog();
			}
			else
			{
				HideDialog();
			}
		}
    }

	private void ShowDialog()
	{
		isActive = true;
		dialogUI.Visible = true;
		dialogUI.ProcessMode = Node.ProcessModeEnum.Always;
		GetTree().Paused = true;
	}

	private void HideDialog() 
	{
		isActive = false;
        dialogUI.Visible = false;
        dialogUI.ProcessMode = Node.ProcessModeEnum.Disabled;
        GetTree().Paused = false;
    }
}
