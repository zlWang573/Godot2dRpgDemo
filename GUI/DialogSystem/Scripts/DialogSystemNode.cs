using Godot;
using Godot.Collections;
using System;

[Tool]
[Icon("res://GUI/DialogSystem/Icons/star_bubble.svg")]
public partial class DialogSystemNode : CanvasLayer
{
	[Signal]
	public delegate void FinishedEventHandler();

	[Signal]
	public delegate void LetterAddedEventHandler(string letter);

	public static DialogSystemNode Instance;

	private string longChars = ".!?:;";
	private string halfLongChars = ", ";

    public bool isActive = false;
	private Array<DialogItem> dialogItems;
	private int dialogIndex = 0;

	private bool textInProgress = false;
	private float textSpeed = 0.02f;
	private int textLength = 0;
	private string plainText;

	private bool waitingForChoice = false;

	private Control dialogUI;
	private RichTextLabel content;
	private Label nameLabel;
	private DialogProtrait portraitSprite;
	private PanelContainer dialogProgressIndicator;
	private Label dialogProgressIndicatorLabel;
	private Timer timer;
	private AudioStreamPlayer audioStreamPlayer;
	private VBoxContainer choiceOptions;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
        dialogUI = GetNode<Control>("DialogUI");
        content = GetNode<RichTextLabel>("DialogUI/PanelContainer/RichTextLabel");
        nameLabel = GetNode<Label>("DialogUI/NameLabel");
        portraitSprite = GetNode<DialogProtrait>("DialogUI/PortraitSprite");
        dialogProgressIndicator = GetNode<PanelContainer>("DialogUI/DialogProgressIndicator");
        dialogProgressIndicatorLabel = GetNode<Label>("DialogUI/DialogProgressIndicator/Label");
        timer = GetNode<Timer>("DialogUI/Timer");
        audioStreamPlayer = GetNode<AudioStreamPlayer>("DialogUI/AudioStreamPlayer");
        choiceOptions = GetNode<VBoxContainer>("DialogUI/VBoxContainer");

        if (Engine.IsEditorHint())
		{
			if (GetViewport() is Window)
			{
				GetParent().RemoveChild(this);
				return;
			}

			return;
		}

		timer.Timeout += OnTimerTimeout;
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
			if (textInProgress)
			{
				content.VisibleCharacters = textLength;
				timer.Stop();
				textInProgress = false;
				ShowDialogButtonIndicator(true);
                return;
			}
			else if (waitingForChoice)
			{
				return;
			}

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
		dialogUI.ProcessMode = Node.ProcessModeEnum.Always;
		dialogItems = items;
		dialogIndex = 0;

		GetTree().Paused = true;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        dialogUI.Visible = true;
        StartDialog();
    }

    public void HideDialog() 
	{
		isActive = false;
		choiceOptions.Visible = false;
        dialogUI.Visible = false;
        dialogUI.ProcessMode = Node.ProcessModeEnum.Disabled;
		EmitSignal(SignalName.Finished);

        GetTree().Paused = false;
    }

	public void StartDialog()
	{
		waitingForChoice = false;
		ShowDialogButtonIndicator(false);
		var dialogItem = dialogItems[dialogIndex];

		if (dialogItem is DialogText dt)
		{
			SetDialogText(dt);
		}
		else if (dialogItem is DialogChoice dc)
		{
            SetDialogChoice(dc);
        }
	}

    public void SetDialogText(DialogText dt)
    {
        content.Text = dt.Text;
        portraitSprite.Texture = dt.NpcInfo.Portrait;
        portraitSprite.audioPitchBase = dt.NpcInfo.DialogAudioPitch;
        nameLabel.Text = dt.NpcInfo.NPCName;
        content.VisibleCharacters = 0;
        textLength = content.GetTotalCharacterCount();
        plainText = content.GetParsedText();
        textInProgress = true;
        StartTimer();
    }

	public async void SetDialogChoice(DialogChoice dc)
	{
		choiceOptions.Visible = true;
		waitingForChoice = true;
		foreach (var c in choiceOptions.GetChildren())
		{
			c.QueueFree();
		}

		foreach (var db in dc.DialogBranches)
		{
			var newChoice = new Button();
			newChoice.Text = db.Text;
			newChoice.Alignment = HorizontalAlignment.Left;
			newChoice.Pressed += () => { DialogChoiceSelected(db); };
			choiceOptions.AddChild(newChoice);
		}

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		choiceOptions.GetChild<Button>(0).GrabFocus();
    }

	public void DialogChoiceSelected(DialogBranch db)
	{
		choiceOptions.Visible = false;
		waitingForChoice = false;
		ShowDialog(db.DialogItems);
	}

    public void OnTimerTimeout()
	{
		content.VisibleCharacters += 1;
		if (content.VisibleCharacters > textLength)
		{
            ShowDialogButtonIndicator(true);
			textInProgress = false;
			return;
        }

		EmitSignal(SignalName.LetterAdded, plainText[content.VisibleCharacters - 1].ToString());
		StartTimer();
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

	public void StartTimer()
	{
		timer.WaitTime = textSpeed;
		// manipulate wait time
		if (content.VisibleCharacters > 0)
		{
			var c = plainText[content.VisibleCharacters - 1];
			if (longChars.Contains(c))
			{
				timer.WaitTime *= 4;
			}
			else if (halfLongChars.Contains(c))
			{
				timer.WaitTime *= 2;
			}
		}

		timer.Start();
    }
}
