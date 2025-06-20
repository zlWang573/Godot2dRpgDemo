using Godot;
using Godot.Collections;
using System;

[Tool]
[Icon("res://GUI/DialogSystem/Icons/star_bubble.svg")]
public partial class DialogSystemNode : CanvasLayer
{
	[Signal]
	public delegate void FinishedEventHandler();

	public static DialogSystemNode Instance;

    private bool isActive = false;
	private Array<DialogItem> dialogItems;
	private int dialogIndex = 0;

	private Control dialogUI;
	private RichTextLabel content;
	private Label nameLabel;
	private Sprite2D portraitSprite;
	private PanelContainer dialogProgressIndicator;
	private Label dialogProgressIndicatorLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
        dialogUI = GetNode<Control>("DialogUI");
        content = GetNode<RichTextLabel>("DialogUI/PanelContainer/RichTextLabel");
        nameLabel = GetNode<Label>("DialogUI/NameLabel");
        portraitSprite = GetNode<Sprite2D>("DialogUI/PortraitSprite");
        dialogProgressIndicator = GetNode<PanelContainer>("DialogUI/DialogProgressIndicator");
        dialogProgressIndicatorLabel = GetNode<Label>("DialogUI/DialogProgressIndicator/Label");

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
			return;
		}

		if (@event.IsActionPressed("interact") || @event.IsActionPressed("attack") || @event.IsActionPressed("ui_accept"))
		{
			dialogIndex += 1;
			if (dialogIndex < dialogItems.Count)
			{
				StartDialog();
			}
            else
            {
				HideDialog();
            }
        }
    }

	public async void ShowDialog(Array<DialogItem> items)
	{
		isActive = true;
		dialogUI.Visible = true;
		dialogUI.ProcessMode = Node.ProcessModeEnum.Always;
		dialogItems = items;
		dialogIndex = 0;

		GetTree().Paused = true;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		StartDialog();
    }

    public void HideDialog() 
	{
		isActive = false;
        dialogUI.Visible = false;
        dialogUI.ProcessMode = Node.ProcessModeEnum.Disabled;
		EmitSignal(SignalName.Finished);

        GetTree().Paused = false;
    }

	public void StartDialog()
	{
		ShowDialogButtonIndicator(true);
		var dialogItem = dialogItems[dialogIndex];
		SetDialogData(dialogItem);
	}

	public void SetDialogData(DialogItem dialogItem)
	{
        if (dialogItem is DialogText dt)
        {
            content.Text = dt.Text;
        }

		portraitSprite.Texture = dialogItem.NpcInfo.Portrait;
		nameLabel.Text = dialogItem.NpcInfo.NPCName;
    }

	public void ShowDialogButtonIndicator(bool isVisible)
	{
		dialogProgressIndicator.Visible = isVisible;
		if (dialogIndex + 1 < dialogItems.Count)
		{
			dialogProgressIndicatorLabel.Text = "NEXT";
        }
		else
		{
			dialogProgressIndicatorLabel.Text = "END";
        }
	}
}
