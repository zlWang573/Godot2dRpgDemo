using Godot;
using System;

public partial class TitleScene : Node2D
{
	public const string START_LEVEL = "res://Levels/Area01/02.tscn";

	[Export]
	public AudioStream Music;

    [Export]
    public AudioStream ButtonFocusAudio;

    [Export]
    public AudioStream ButtonPressAudio;

	public AudioStreamPlayer AudioStreamPlayer {  get; set; }
    public Button NewGameButton { get; set; }
    public Button ContinueButton { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GetTree().Paused = true;
		GlobalPlayerManager.Instance.player.Visible = false;
		PlayerHUD.Instance.Visible = false;
		PauseMenu.Instance.ProcessMode = ProcessModeEnum.Disabled;

        AudioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        NewGameButton = GetNode<Button>("CanvasLayer/Control/NewGameButton");
        ContinueButton = GetNode<Button>("CanvasLayer/Control/ContinueButton");

		if (GlobalSaveManager.Instance.GetSaveFile() == null)
		{
			ContinueButton.Disabled = true;
			ContinueButton.Visible = false;
		}

		GetNode<SplashScene>("CanvasLayer/SplashScene").Finished += SetupTitleScreen;
		GlobalLevelManager.Instance.LevelLoadStarted += ExitTitleScreen;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetupTitleScreen()
	{
		GlobalAudioManager.Instance.PlayMusic(Music);

        NewGameButton.Pressed += StartGame;
		ContinueButton.Pressed += LoadGame;

        NewGameButton.GrabFocus();

        NewGameButton.FocusEntered += () => { PlayAudio(ButtonFocusAudio); };
        ContinueButton.FocusEntered += () => { PlayAudio(ButtonFocusAudio); };

        NewGameButton.Pressed += () => { PlayAudio(ButtonPressAudio); };
        ContinueButton.Pressed += () => { PlayAudio(ButtonPressAudio); };
    }

	private void StartGame()
	{
		GlobalLevelManager.Instance.LoadNewLevel(START_LEVEL, "", Vector2.Zero);
    }

	private void LoadGame()
	{
		GlobalSaveManager.Instance.LoadGame();
	}

	private void ExitTitleScreen()
	{
        GlobalPlayerManager.Instance.player.Visible = true;
        PlayerHUD.Instance.Visible = true;
        PauseMenu.Instance.ProcessMode = ProcessModeEnum.Always;
    }

	private void PlayAudio(AudioStream audio)
	{
        AudioStreamPlayer.Stream = audio;
		AudioStreamPlayer.Play();
    }
}
