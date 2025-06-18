using Godot;
using System;

public partial class PressurePlate : Node2D
{
	[Signal]
	public delegate void ActivatedEventHandler();

	[Signal]
	public delegate void DeactivatedEventHandler();

	private int bodies = 0;
	private bool isActive = false;
	private Rect2 offRect;

	public Area2D Area2D;
	public AudioStreamPlayer2D AudioStreamPlayer;
	public Sprite2D Sprite2D;

	public AudioStream ActivatedAudio = ResourceLoader.Load<AudioStream>("res://Interactables/Dungeon/lever-01.wav");
	public AudioStream DeactivatedAudio = ResourceLoader.Load<AudioStream>("res://Interactables/Dungeon/lever-02.wav");

	public override void _Ready()
	{
		Area2D = GetNode<Area2D>("Area2D");
		AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		Sprite2D = GetNode<Sprite2D>("Sprite2D");

		Area2D.BodyEntered += OnBodyEntered;
		Area2D.BodyExited += OnBodyExited;

		offRect = Sprite2D.RegionRect;
	}

	private void OnBodyEntered(Node2D body)
	{
		bodies++;
		CheckIsActivated();
	}

	private void OnBodyExited(Node2D body)
	{
		bodies--;
		CheckIsActivated();
	}

	private void CheckIsActivated()
	{
		if (bodies > 0)
		{
			if (!isActive)
			{
				isActive = true;
				Sprite2D.RegionRect = new Rect2(new Vector2(offRect.Position.X - 32, offRect.Position.Y), offRect.Size.X, offRect.Size.Y);
				PlayAudio(ActivatedAudio);
				EmitSignal(SignalName.Activated);
			}
		}
		else
		{
			if (isActive)
			{
				isActive = false;
				Sprite2D.RegionRect = offRect;
				PlayAudio(DeactivatedAudio);
				EmitSignal(SignalName.Deactivated);
			}
		}
	}

	private void PlayAudio(AudioStream audio)
	{
		AudioStreamPlayer.Stream = audio;
		AudioStreamPlayer.Play();
	}
}
