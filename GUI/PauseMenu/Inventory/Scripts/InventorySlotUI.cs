using Godot;
using System;

public partial class InventorySlotUI : Button
{
    public SlotData SlotData { get => slotData; set { SetSlotData(value); } }

    public TextureRect TextureRect;
    public Label Label;

    private SlotData slotData;

    public PauseMenu PauseMenu;

    public override void _Ready()
    {
        TextureRect = GetNode<TextureRect>("TextureRect");
        Label = GetNode<Label>("Label");
        PauseMenu = GetNode<PauseMenu>("../../../..");

        TextureRect.Texture = null;
        Label.Text = string.Empty;
        FocusEntered += ItemFocused;
        FocusExited += ItemUnfocused;
        Pressed += ItemPressed;
    }

    public void SetSlotData(SlotData value)
    {
        slotData = value;
        if (SlotData == null)
        {
            return;
        }

        TextureRect.Texture = SlotData.ItemData.Texture;
        Label.Text = SlotData.Quantity.ToString();
    }

    private void ItemFocused()
    {
        if (slotData != null && slotData.ItemData != null)
        {
            PauseMenu.UpdateItemDescription(slotData.ItemData.Description);
        }
    }

    private void ItemUnfocused()
    {
        PauseMenu.UpdateItemDescription(string.Empty);
    }

    private void ItemPressed()
    {
        if (slotData != null)
        {
            if (slotData.ItemData != null)
            {
                var wasUsed = slotData.ItemData.Use();
                if (wasUsed)
                {
                    slotData.SetQuantity(slotData.Quantity - 1);
                    Label.Text = slotData.Quantity.ToString();
                }
            }
        }
    }
}
