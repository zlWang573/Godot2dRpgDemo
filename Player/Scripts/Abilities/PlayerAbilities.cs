using Godot;
using System;

public partial class PlayerAbilities : Node
{
    public enum Abilities
    { 
        BOOMERANG = 0,
        GRAPPLE = 1,
    }

    public PackedScene BOOMERANG = ResourceLoader.Load<PackedScene>("res://Player/Boomerang.tscn");

    public Abilities selectedAbility = Abilities.BOOMERANG;
    public Boomerang boomerangInstance = null;

    private Player player;

    public override void _Ready()
    {
        player = GlobalPlayerManager.Instance.player;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ability"))
        {
            if (selectedAbility == Abilities.BOOMERANG)
            {
                BoomerangAbility();
            }
        }
    }

    public void BoomerangAbility()
    {
        if (boomerangInstance != null && IsInstanceValid(boomerangInstance))
        {
            return;
        }

        var b = BOOMERANG.Instantiate<Boomerang>();
        player.AddSibling(b);
        b.GlobalPosition = player.GlobalPosition;

        var throwDirection = player.direction;
        if (throwDirection == Vector2.Zero)
        {
            throwDirection = player.crossDirection;
        }

        b.Throw(throwDirection);
        boomerangInstance = b;
    }
}
