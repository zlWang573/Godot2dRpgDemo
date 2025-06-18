using Godot;
using System;

public partial class InventoryUI : Node
{
    public static readonly PackedScene INVENTORY_SLOT = ResourceLoader.Load<PackedScene>("res://GUI/PauseMenu/Inventory/InventorySlot.tscn");

    [Export]
    public InventoryData Data;

    public PauseMenu PauseMenu;

    private int focusIndex = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PauseMenu = GetNode<PauseMenu>("../../..");

        PauseMenu.Shown += () => { ClearInventory(); UpdateInventory(); };
        PauseMenu.Hidden += ClearInventory;
        Data.Changed += OnInventoryChanged;

        Data.Init();
    }

    public void ClearInventory()
    {
        foreach (var item in GetChildren())
        {
            item.QueueFree();
        }
    }

    public async void UpdateInventory(int index = 0)
    {
        foreach (var s in Data.Slots)
        {
            var newSlot = INVENTORY_SLOT.Instantiate<InventorySlotUI>();
            AddChild(newSlot);
            newSlot.SlotData = s;
            newSlot.FocusEntered += ItemFocused;
        }

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        GetChild<Button>(index).GrabFocus();
    }

    private void ItemFocused()
    {
        int count = GetChildCount();
        for (int i = 0; i < count; i++)
        {
            if (GetChild<Button>(i).HasFocus())
            {
                focusIndex = i;
                return;
            }
        }
    }

    private async void OnInventoryChanged()
    {
        var index = focusIndex;
        ClearInventory();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        UpdateInventory(index);
    }
}