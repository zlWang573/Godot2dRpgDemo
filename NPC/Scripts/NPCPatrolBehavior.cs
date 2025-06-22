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

    private bool hasStarted = false;
    private Vector2 direction;
    private string lastPhase = string.Empty;

    private Timer timer;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");

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
            IdlePhase();
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
                    if (!p.IsConnected(PatrolLocation.SignalName.TransformChanged, Callable.From(GatherPatrolLocations)))
                    {
                        p.Connect(PatrolLocation.SignalName.TransformChanged, Callable.From(GatherPatrolLocations));
                    }

                    int next = (i + 1) % PatrolLocations.Count;
                    p.UpdateLine(PatrolLocations[next].GlobalPosition);
                }
            }
        }
    }

    public async override void Start()
    {
        if (NPC.DoBehavior == false || PatrolLocations.Count <= 1)
        {
            return;
        }

        if (hasStarted)
        {
            if (timer.TimeLeft <= 0f)
            {
                WalkPhase();
            }

            return;
        }

        hasStarted = true;
        IdlePhase();
    }

    private async void IdlePhase()
    {
        this.NPC.GlobalPosition = Target.TargetPosition;

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

        if (waitTime > 0f)
        {
            timer.Start(waitTime);
            await ToSignal(timer, Timer.SignalName.Timeout);
        }

        if (this.NPC.DoBehavior == false || PatrolLocations.Count <= 1)
        {
            return;
        }

        WalkPhase();
    }

    private void WalkPhase()
    {
        this.NPC.State = "walk";
        direction = GlobalPosition.DirectionTo(Target.TargetPosition);
        this.NPC.Direction = direction;
        this.NPC.Velocity = WalkSpeed * direction;
        this.NPC.UpdateDirection(Target.TargetPosition);
        this.NPC.UpdateAnimation();
    }
}
