using Godot;
using Godot.Collections;
using System;

[Tool]
[Icon("res://GUI/DialogSystem/Icons/chat_bubbles.svg")]
public partial class DialogInteraction : Area2D
{
	[Signal]
	public delegate void PlayerInteractedEventHandler();

	[Signal]
	public delegate void FinishedEventHandler();

	[Export]
	public bool Enabled = true;

	private Array<DialogItem> DialogItems = new Array<DialogItem>();

	private AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (Engine.IsEditorHint())
		{
			return;
		}

		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

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

	public async void PlayerInteract()
	{
		EmitSignal(SignalName.PlayerInteracted);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        DialogSystemNode.Instance.ShowDialog(DialogItems);
		DialogSystemNode.Instance.Finished += OnDialogFinished;
    }

	public void OnAreaEntered(Area2D area)
	{
		if (!Enabled || DialogItems.Count == 0)
		{
			return;
		}

		animationPlayer.Play("show");
		GlobalPlayerManager.Instance.InteractPressed += PlayerInteract;
    }

    public void OnAreaExited(Area2D area)
    {
        if (!Enabled || DialogItems.Count == 0)
        {
            return;
        }

        animationPlayer.Play("hide");
        GlobalPlayerManager.Instance.InteractPressed -= PlayerInteract;
    }

	public void OnDialogFinished()
	{
        DialogSystemNode.Instance.Finished -= OnDialogFinished;
		EmitSignal(SignalName.Finished);
    }

    public override string[] _GetConfigurationWarnings()
    {
        if (!CheckForDialogItems())
		{
			return new string[] { "Requires at least one DialogItem node." };
		}

        return new string[0];
    }

	private bool CheckForDialogItems()
	{
        foreach (var c in GetChildren())
        {
            if (c is DialogItem)
            {
				return true;
            }
        }

		return false;
    }
}
