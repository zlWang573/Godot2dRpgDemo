using Godot;
using System;

public partial class BarredDoor : Node2D
{
	public AnimationPlayer AnimationPlayer;

	public override void _Ready()
	{
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void OpenDoor()
	{
		AnimationPlayer.Play("open_door");
	}

	public void CloseDoor()
	{
		AnimationPlayer.Play("close_door");
	}
}
