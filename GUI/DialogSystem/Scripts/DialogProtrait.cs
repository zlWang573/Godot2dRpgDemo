using Godot;
using System;

[Tool]
public partial class DialogProtrait : Sprite2D
{
	public bool Blink { get => blink; set => SetBlink(value); }
    public bool OpenMouth { get => openMouth; set => SetOpenMouth(value); }
    public float audioPitchBase = 1.0f;

    private bool blink = false;
    private bool openMouth = false;
	private int mouthOpenFrames = 0;
    private float blinkTime;

    private string soundChars = "aeiouy1234567890";
    private string closeChars = ".,!?";

    private AudioStreamPlayer audioStreamPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        audioStreamPlayer = GetNode<AudioStreamPlayer>("../AudioStreamPlayer");

        if (Engine.IsEditorHint())
        {
            return;
        }

        var dialogSystem = GetParent().GetParent() as DialogSystemNode;
        dialogSystem.LetterAdded += CheckMouthOpen;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        float fd = (float)delta;
        blinkTime -= fd;
        if (blinkTime <= 0f)
        {
            if (blink)
            {
                blinkTime = (float)GD.RandRange(0.1, 3);
            }
            else
            {
                blinkTime = 0.15f;
            }

            Blink = !Blink;
        }
    }

	private void SetBlink(bool value)
	{
        if (blink != value)
        {
            blink = value;
            UpdatePortrait();
        }
    }

    private void SetOpenMouth(bool value)
    {
        if (openMouth != value)
        {
            openMouth = value;
            UpdatePortrait();
        }
    }

    private void UpdatePortrait()
    {
        if (openMouth)
        {
            Frame = 2;
        }
        else
        {
            Frame = 0;
        }

        if (blink)
        {
            Frame += 1;
        }
    }

    private void CheckMouthOpen(string letter)
    {
        if (soundChars.Contains(letter))
        {
            OpenMouth = true;
            mouthOpenFrames += 3;
            audioStreamPlayer.PitchScale = (float)GD.RandRange(audioPitchBase - 0.05f, audioPitchBase + 0.05f);
            audioStreamPlayer.Play();
        }
        else if (closeChars.Contains(letter))
        {
            mouthOpenFrames = 0;
        }

        if (mouthOpenFrames > 0)
        {
            mouthOpenFrames--;
        }

        if (mouthOpenFrames <= 0)
        {
            OpenMouth = false;
        }
    }
}
