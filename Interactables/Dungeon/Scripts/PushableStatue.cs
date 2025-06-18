using Godot;
using System;

public partial class PushableStatue : RigidBody2D
{
    [Export]
    public float PushSpeed = 30f;

    public AudioStreamPlayer2D AudioStreamPlayer;

    public Vector2 PushDirection { get => pushDirection; set => SetPushDirection(value); }
    private Vector2 pushDirection = Vector2.Zero;

    public override void _Ready()
    {
        AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        LinearVelocity = PushDirection * PushSpeed;
    }

    public void SetPushDirection(Vector2 direction)
    {
        pushDirection = direction;
        if (pushDirection != Vector2.Zero)
        {
            AudioStreamPlayer.Play();
        }
        else
        {
            AudioStreamPlayer.Stop();
        }
    }
}
