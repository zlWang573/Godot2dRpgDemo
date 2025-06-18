using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class PatrolLocation : Node2D
{
    [Export]
    public float WaitTime { get => waitTime; set => UpdateWaitTime(value); }

    public Vector2 TargetPosition = Vector2.Zero;

    public Sprite2D Sprite;
    public Label Label;
    public Label Label2;
    public Line2D Line;

    private float waitTime = 0f;

    public override void _Ready()
    {
        Label = GetNode<Label>("Sprite2D/Label");
        Label2 = GetNode<Label>("Sprite2D/Label2");
        Line = GetNode<Line2D>("Sprite2D/Line2D");
        Sprite = GetNode<Sprite2D>("Sprite2D");

        TargetPosition = GlobalPosition;
        UpdateWaitTimeLabel();

        if (Engine.IsEditorHint())
        {
            return;
        }

        Sprite.QueueFree();
    }

    public void UpdateLabel(string s)
    {
        if (Label == null)
        {
            Label = GetNode<Label>("Sprite2D/Label");
        }

        Label.Text = s;
    }

    public void UpdateLine(Vector2 nextLocation)
    {
        if (Line == null)
        {
            Line = GetNode<Line2D>("Sprite2D/Line2D");
        }

        var point = nextLocation - GlobalPosition;
        var points = Line.Points;
        points[1] = point;
        Line.Points = points;
    }

    public void UpdateWaitTime(float w)
    {
        waitTime = w;
        UpdateWaitTimeLabel();
    }

    public void UpdateWaitTimeLabel()
    {
        if (Engine.IsEditorHint())
        {
            if (Label2 != null)
            {
                Label2.Text = $"wait: {waitTime}s";
            }
        }
    }
}
