using Godot;
using System;

[Tool]
public partial class TreasureChest : Node2D
{
    [Export]
    public ItemData ItemData { get => itemData; set => SetItemData(value); }

    [Export]
    public int Quantity { get => quantity; set => SetQuantity(value); }

    public bool IsOpen = false;

    public Sprite2D ItemSprite;
    public Label Label;
    public AnimationPlayer AnimationPlayer;
    public Area2D InteractArea;
    public PersistentDataHandler IsOpenData;

    private ItemData itemData;
    private int quantity;

    public override void _Ready()
    {
        ItemSprite = GetNode<Sprite2D>("ItemSprite");
        Label = GetNode<Label>("ItemSprite/Label");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        InteractArea = GetNode<Area2D>("Area2D");

        UpdateTexture();
        UpdateLabel();

        if (Engine.IsEditorHint())
        {
            return;
        }

        IsOpenData = GetNode<PersistentDataHandler>("IsOpen");

        InteractArea.AreaEntered += OnAreaEntered;
        InteractArea.AreaExited += OnAreaExited;
        IsOpenData.DataLoaded += SetChestState;
        SetChestState();
    }

    public void SetChestState()
    {
        IsOpen = IsOpenData.Value;
        if (IsOpen)
        {
            AnimationPlayer.Play("opened");
        }
        else
        {
            AnimationPlayer.Play("closed");
        }
    }

    private void SetItemData(ItemData data)
    {
        itemData = data;
        UpdateTexture();
    }

    private void SetQuantity(int value)
    {
        quantity = value;
        UpdateLabel();
    }

    private void PlayerInteract()
    {
        if (IsOpen)
        {
            return;
        }

        IsOpen = true;
        IsOpenData.SetValue();
        AnimationPlayer.Play("open_chest");
        if (itemData != null && quantity > 0)
        {
            GlobalPlayerManager.Instance.INVENTORY_DATA.AddItem(itemData, quantity);
        }
        else
        {
            GD.PrintErr("No Items in Chest!");
            GD.PushError($"No Items in Chest! Chest Name: {Name}");
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        GlobalPlayerManager.Instance.InteractPressed += PlayerInteract;
    }

    private void OnAreaExited(Area2D area)
    {
        GlobalPlayerManager.Instance.InteractPressed -= PlayerInteract;
    }

    private void UpdateTexture()
    {
        if (itemData != null && ItemSprite != null)
        {
            ItemSprite.Texture = itemData.Texture;
        }
    }

    private void UpdateLabel()
    {
        if (Label != null)
        {
            if (quantity > 1)
            {
                Label.Text = "x" + quantity.ToString();
            }
            else
            {
                Label.Text = string.Empty;
            }
        }
    }
}
