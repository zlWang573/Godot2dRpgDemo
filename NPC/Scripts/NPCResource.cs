using Godot;
using System;

[Tool]
[GlobalClass]
public partial class NPCResource : Resource
{
    [Export]
    public string NPCName = string.Empty;

    [Export]
    public Texture2D Sprite;

    [Export]
    public Texture2D Portrait;

    [Export]
    public float DialogAudioPitch = 1.0f;
}
