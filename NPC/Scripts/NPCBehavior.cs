using Godot;
using System;

[Icon("res://NPC/Icons/npc_behavior.svg")]
public partial class NPCBehavior : Node2D
{
    public NPC NPC;

    public override void _Ready()
    {
        var p = GetParent();
        if (p is NPC)
        {
            NPC = (NPC)p;
            NPC.DoBehaviorEnabled += Start;
        }
    }

    public virtual void Start()
    {

    }
}
