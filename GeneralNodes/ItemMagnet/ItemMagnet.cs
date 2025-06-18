using Godot;
using System;
using System.Collections.Generic;

public partial class ItemMagnet : Area2D
{
    public List<ItemPickup> Items = new List<ItemPickup>();
    public List<float> Speeds = new List<float>();

    [Export]
    public float MagnetStrength = 100f;

    [Export]
    public bool PlayMagnetAudio = false;

    private AudioStreamPlayer2D audioStreamPlayer;

    public override void _Ready()
    {
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        AreaEntered += OnAreaEntered;
    }

    public override void _Process(double delta)
    {
        float fd = (float)delta;
        for (int i = 0; i < Items.Count;)
        {
            var item = Items[i];
            if (!IsInstanceValid(item))
            {
                Items.RemoveAt(i);
                Speeds.RemoveAt(i);
                continue;
            }
            else if (item.GlobalPosition.DistanceTo(GlobalPosition) > Speeds[i] * fd)
            {
                Speeds[i] += MagnetStrength * fd;
                item.Position += item.GlobalPosition.DirectionTo(GlobalPosition) * Speeds[i] * fd;
            }
            else
            {
                item.GlobalPosition = GlobalPosition;
            }

            i++;
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area.GetParent() is ItemPickup)
        {
            var newItem = (ItemPickup)area.GetParent();
            Items.Add(newItem);
            Speeds.Add(MagnetStrength);
            newItem.SetPhysicsProcess(false);
            if (PlayMagnetAudio)
            {
                audioStreamPlayer.Play(0);
            }
        }
    }
}
