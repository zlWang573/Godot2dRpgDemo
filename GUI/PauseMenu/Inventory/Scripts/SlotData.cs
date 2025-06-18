using Godot;
using System;

[GlobalClass]
public partial class SlotData : Resource
{
    [Export]
    public ItemData ItemData;

    [Export]
    public int Quantity { get => quantity; set => SetQuantity(value); }

    private int quantity;

    public void SetQuantity(int value)
    {
        quantity = value;
        if (Quantity <= 0)
        {
            EmitSignal(SignalName.Changed);
        }
    }
}
