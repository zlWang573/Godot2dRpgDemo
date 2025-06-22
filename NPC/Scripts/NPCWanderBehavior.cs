using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class NPCWanderBehavior : NPCBehavior
{
    public readonly Array<Vector2> DIRECTIONS = new Array<Vector2>() { Vector2.Up, Vector2.Right, Vector2.Down, Vector2.Left };

    [Export]
    public int WanderRange { get => wanderRange; set => SetWanderRange(value); }

    [Export]
    public float WanderSpeed = 30f;

    [Export]
    public float WanderDuration = 1f;

    [Export]
    public float IdleDuration = 1f;

    public CollisionShape2D CollisionShape;
    public Vector2 OriginalPosition;

    private int wanderRange = 2;

    public override void _Ready()
    {
        CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (Engine.IsEditorHint())
        {
            return;
        }
        
        base._Ready();
        OriginalPosition = this.NPC.GlobalPosition;
        CollisionShape.QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint())
        {
            return;
        }

        if (GlobalPosition.DistanceTo(OriginalPosition) > wanderRange * 32)
        {
            var diff = GlobalPosition - OriginalPosition;
            var newDir = Vector2.Zero;
            if (Mathf.Abs(diff.X) > Mathf.Abs(diff.Y))
            {
                newDir = diff.X < 0f ? Vector2.Right : Vector2.Left;
            }
            else
            {
                newDir = diff.Y < 0f ? Vector2.Down : Vector2.Up;
            }

            this.NPC.Velocity = WanderSpeed * newDir;
            this.NPC.Direction = newDir;
            this.NPC.UpdateDirection(GlobalPosition + this.NPC.Direction);
            this.NPC.UpdateAnimation();
        }
    }

    public async override void Start()
    {
        if (NPC.DoBehavior == false)
        {
            return;
        }

        this.NPC.State = "idle";
        this.NPC.Velocity = Vector2.Zero;
        this.NPC.UpdateAnimation();

        var timer = GetTree().CreateTimer(GD.Randf() * IdleDuration + IdleDuration);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        if (NPC.DoBehavior == false)
        {
            return;
        }

        this.NPC.State = "walk";
        var dir = DIRECTIONS[GD.RandRange(0, 3)];
        this.NPC.Direction = dir;
        this.NPC.Velocity = WanderSpeed * dir;
        this.NPC.UpdateDirection(GlobalPosition + dir);
        this.NPC.UpdateAnimation();

        timer = GetTree().CreateTimer(GD.Randf() * WanderDuration + WanderDuration);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);

        if (this.NPC.DoBehavior == false)
        {
            return;
        }

        // Loop do behavior
        this.NPC.EmitDoBehaviorSignal();
    }

    public void SetWanderRange(int range)
    {
        wanderRange = range;
        if (CollisionShape != null)
        {
            ((CircleShape2D)CollisionShape.Shape).Radius = range * 32;
        }
    }
}
