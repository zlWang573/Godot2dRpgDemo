using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class NPCPatrolBehavior : NPCBehavior
{
    public readonly Array<Color> COLORS = new Array<Color>() { new Color(1, 0, 0), new Color(1, 1, 0), new Color(0, 1, 0), new Color(0, 1, 1), new Color(0, 0, 1), new Color(1, 0, 1) };

    [Export]
    public float WalkSpeed = 30f;

    public Array<PatrolLocation> PatrolLocations = new Array<PatrolLocation>();
    public int CurrentLocationIndex = 0;
    public PatrolLocation Target;

    public override void _Ready()
    {
        GatherPatrolLocations();
        if (Engine.IsEditorHint())
        {
            ChildEnteredTree += GatherPatrolLocationsWithNode;
            ChildOrderChanged += GatherPatrolLocations;
        }

        if (PatrolLocations.Count > 0)
        {
            Target = PatrolLocations[0];
            CurrentLocationIndex = 0;
        }

        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        if (GlobalPosition.DistanceTo(Target.TargetPosition) < 1f)
        {
            Start();
        }
    }

    public void GatherPatrolLocationsWithNode(Node _)
    {
        GatherPatrolLocations();
    }

    public void GatherPatrolLocations()
    {
        PatrolLocations.Clear();
        foreach (var c in GetChildren())
        {
            if (c is PatrolLocation)
            {
                var loc = (PatrolLocation)c;
                PatrolLocations.Add(loc);
            }
        }

        if (Engine.IsEditorHint())
        {
            if (PatrolLocations.Count > 0)
            {
                for (int i = 0; i < PatrolLocations.Count; i++)
                {
                    var p = PatrolLocations[i];
                    p.UpdateLabel(i.ToString());
                    p.Modulate = COLORS[i % COLORS.Count];

                    int next = (i + 1) % PatrolLocations.Count;
                    p.UpdateLine(PatrolLocations[next].GlobalPosition);
                }
            }
        }
    }

    public async override void Start()
    {
        if (NPC.DoBehavior == false && PatrolLocations.Count <= 1)
        {
            return;
        }

        this.NPC.GlobalPosition =  Target.TargetPosition;

        this.NPC.State = "idle";
        this.NPC.Velocity = Vector2.Zero;
        this.NPC.UpdateAnimation();

        float waitTime = Target.WaitTime;

        CurrentLocationIndex++;
        if (CurrentLocationIndex >= PatrolLocations.Count)
        {
            CurrentLocationIndex = 0;
        }

        Target = PatrolLocations[CurrentLocationIndex];

        var timer = GetTree().CreateTimer(waitTime);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        if (this.NPC.DoBehavior == false && PatrolLocations.Count <= 1)
        {
            return;
        }

        this.NPC.State = "walk";
        var dir = GlobalPosition.DirectionTo(Target.TargetPosition);
        this.NPC.Direction = dir;
        this.NPC.Velocity = WalkSpeed * dir;
        this.NPC.UpdateDirection(Target.TargetPosition);
        this.NPC.UpdateAnimation();
    }
}
