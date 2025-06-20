using Godot;
using System;

[Tool]
[Icon("res://NPC/Icons/npc.svg")]
public partial class NPC : CharacterBody2D
{
    [Signal]
    public delegate void DoBehaviorEnabledEventHandler();

    public string State = "idle";
    public Vector2 Direction = Vector2.Down;
    public string DirectionName = "down";
    public bool DoBehavior = true;

    [Export]
    public NPCResource NPCResource { get => npcResource; set => SetNPCResource(value); }

    public Sprite2D Sprite;
    public AnimationPlayer AnimationPlayer;

    private NPCResource npcResource;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite2D");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        SetUpNPC();
        if (Engine.IsEditorHint())
        {
            return;
        }

        GatherInteractables();
        EmitDoBehaviorSignal();
    }

    public override void _PhysicsProcess(double delta)
    {
        float fd = (float)delta;
        MoveAndSlide();
    }

    public void GatherInteractables()
    {
        foreach (var c in GetChildren())
        {
            if (c is DialogInteraction d)
            {
                d.PlayerInteracted += OnPlayerInteracted;
                d.Finished += OnInteractionFinished;
            }
        }
    }

    public void OnPlayerInteracted()
    {
        UpdateDirection(GlobalPlayerManager.Instance.player.GlobalPosition);
        State = "idle";
        Velocity = Vector2.Zero;
        UpdateAnimation();
        DoBehavior = false;
    }

    public void OnInteractionFinished()
    {
        State = "idle";
        UpdateAnimation();
        DoBehavior = true;
        EmitDoBehaviorSignal();
    }

    public void EmitDoBehaviorSignal()
    {
        EmitSignal(SignalName.DoBehaviorEnabled);
    }

    public void UpdateAnimation()
    {
        AnimationPlayer.Play(State + "_" + DirectionName);
    }

    public void UpdateDirection(Vector2 targetPosition)
    {
        Direction = GlobalPosition.DirectionTo(targetPosition);
        UpdateDirectionName();
        if (DirectionName == "side" && Direction.X < 0)
        {
            Sprite.FlipH = true;
        }
        else
        {
            Sprite.FlipH = false;
        }
    }

    public void UpdateDirectionName()
    {
        float threshold = 0.45f;
        if (Direction.Y < -threshold)
        {
            DirectionName = "up";
        }
        else if (Direction.Y > threshold)
        {
            DirectionName = "down";
        }
        else if (Direction.X > threshold || Direction.X < -threshold)
        {
            DirectionName = "side";
        }
    }

    public void SetNPCResource(NPCResource resource)
    {
        npcResource = resource;
        SetUpNPC();
    }

    public void SetUpNPC()
    {
        if (npcResource != null)
        {
            if (Sprite != null)
            {
                Sprite.Texture = npcResource.Sprite;
            }
        }
    }
}
