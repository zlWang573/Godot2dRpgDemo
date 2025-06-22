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

    [Export(PropertyHint.Range, "0.5, 1.8, 0.02")]
    public float DialogAudioPitch = 1.0f;
}
