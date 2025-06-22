using Godot;

public partial class PauseMenu : CanvasLayer
{
	public Button SaveButton;
    public Button LoadButton;
    public Label ItemDescription;
    public AudioStreamPlayer2D AudioStreamPlayer2D;

	public bool isPaused = false;

    [Signal]
    public delegate void ShownEventHandler();

    [Signal]
    public delegate void HiddenEventHandler();

    public static PauseMenu Instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Instance = this;
        
        SaveButton = GetNode<Button>("Control/VBoxContainer/SaveButton");
        LoadButton = GetNode<Button>("Control/VBoxContainer/LoadButton");
        ItemDescription = GetNode<Label>("Control/ItemDescription");
        AudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("Control/AudioStreamPlayer2D");

        HidePauseMenu();
        SaveButton.Pressed += OnSavePressed;
        LoadButton.Pressed += OnLoadPressed;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            if (isPaused == false)
            {
                if (DialogSystemNode.Instance.isActive)
                {
                    return;
                }

                ShowPauseMenu();
            }
            else
            {
                HidePauseMenu();
            }

            GetViewport().SetInputAsHandled();
        }

        base._UnhandledInput(@event);
    }

    private void ShowPauseMenu()
    {
        GetTree().Paused = true;
        Visible = true;
        isPaused = true;
        SaveButton.GrabFocus();
        EmitSignal(SignalName.Shown);
    }

    private void HidePauseMenu()
    {
        GetTree().Paused = false;
        Visible = false;
        isPaused = false;
        EmitSignal(SignalName.Hidden);
    }

    private void OnSavePressed()
    {
        if (!isPaused)
        {
            return;
        }

        GlobalSaveManager.Instance.SaveGame();
        HidePauseMenu();
    }

    private async void OnLoadPressed()
    {
        if (!isPaused)
        {
            return;
        }

        GlobalSaveManager.Instance.LoadGame();
        await ToSignal(GlobalLevelManager.Instance, GlobalLevelManager.SignalName.LevelLoadStarted);
        HidePauseMenu();
    }

    public void UpdateItemDescription(string newText)
    {
        ItemDescription.Text = newText;
    }

    public void PlaySound(AudioStream audio)
    {
        AudioStreamPlayer2D.Stream = audio;
        AudioStreamPlayer2D.Play();
    }
}
