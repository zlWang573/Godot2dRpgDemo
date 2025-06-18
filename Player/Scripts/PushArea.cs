using Godot;
using System;

public partial class PushArea : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is PushableStatue)
        {
            var ps = (PushableStatue)body;
            ps.PushDirection = GlobalPlayerManager.Instance.player.direction;
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if (body is PushableStatue)
        {
            var ps = (PushableStatue)body;
            ps.PushDirection = Vector2.Zero;
        }
    }
}
