using Godot;
using System;

[GlobalClass]
public partial class DropData : Resource
{
    [Export]
    public ItemData Item;

    [Export(PropertyHint.Range, "0,100,1")]
    public float Probability = 100;

    [Export(PropertyHint.Range, "1,10,1")]
    public int MinAmount = 1;

    [Export(PropertyHint.Range, "1,10,1")]
    public int MaxAmount = 1;

    public int GetDropCount()
    {
        if (GD.RandRange(0, 100) >= Probability)
        {
            return 0;
        }

        return GD.RandRange(MinAmount, MaxAmount);
    }
}
