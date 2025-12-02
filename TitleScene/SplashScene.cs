using Godot;
using System;

public partial class SplashScene : Control
{
	[Signal]
	public delegate void FinishedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("MichaelGamesLogo/AnimationPlayer").AnimationFinished += OnAnimationFinished;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnAnimationFinished(StringName name)
	{
		EmitSignal(SignalName.Finished);
	}
}
