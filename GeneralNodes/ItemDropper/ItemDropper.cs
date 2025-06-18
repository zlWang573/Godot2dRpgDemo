using Godot;
using System;

[Tool]
public partial class ItemDropper : Node2D
{
    public PackedScene PICKUP = ResourceLoader.Load<PackedScene>("res://Items/ItemPickup/ItemPickup.tscn");

    [Export]
    public ItemData ItemData { get => itemData; set => SetItemData(value); }

    private Sprite2D sprite2D;
    private PersistentDataHandler hasDroppedData;
    private AudioStreamPlayer audioStreamPlayer;

    private ItemData itemData;
    private bool hasDropped = false;

    public override void _Ready()
    {
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        if (Engine.IsEditorHint())
        {
            UpdateTexture();
            return;
        }

        hasDroppedData = GetNode<PersistentDataHandler>("PersistentDataHandler");

        sprite2D.Visible = false;
        hasDroppedData.DataLoaded += OnDataLoaded;
        OnDataLoaded();
    }

    public void SetItemData(ItemData value)
    {
        itemData = value;
        UpdateTexture();
    }

    public void DropItem()
    {
        if (!hasDropped)
        {
            hasDropped = true;
            var pickup = PICKUP.Instantiate<ItemPickup>();
            pickup.ItemData = itemData;
            AddChild(pickup);
            pickup.PickedUp += OnDropPickup;
            audioStreamPlayer.Play();
        }
    }

    private void OnDropPickup()
    {
        hasDroppedData.SetValue();
    }

    private void UpdateTexture()
    {
        if (Engine.IsEditorHint())
        {
            if (itemData != null && sprite2D != null)
            {
                sprite2D.Texture = itemData.Texture;
            }
        }
    }

    private void OnDataLoaded()
    {
        hasDropped = hasDroppedData.Value;
    }
}
