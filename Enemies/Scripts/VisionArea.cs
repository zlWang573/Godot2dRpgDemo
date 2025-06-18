using Godot;
using System;

public partial class VisionArea : Area2D
{
    [Signal]
    public delegate void PlayerEnteredEventHandler();

    [Signal]
    public delegate void PlayerExitedEventHandler();

    public override void _Ready()
    {
        BodyEntered += OnBodyEnter;
        BodyExited += OnBodyExit;

        var p = GetParent();
        if (p is Enemy)
        {
            var e = (Enemy)p;
            e.DirectionChanged += OnDirectionChange;
        }
    }

    public void OnBodyEnter(Node2D body)
    {
        if (body is Player)
        {
            EmitSignal(SignalName.PlayerEntered);
        }
    }

    public void OnBodyExit(Node2D body)
    {
        if (body is Player)
        {
            EmitSignal(SignalName.PlayerExited);
        }
    }

    public void OnDirectionChange(Vector2 newDirection)
    {
        if (newDirection == Vector2.Down)
        {
            RotationDegrees = 0;
        }
        else if (newDirection == Vector2.Up)
        {
            RotationDegrees = 180;
        }
        else if (newDirection == Vector2.Left)
        {
            RotationDegrees = 90;
        }
        else if (newDirection == Vector2.Right)
        {
            RotationDegrees = -90;
        }
        else
        {
            RotationDegrees = 0;
        }
    }
}
