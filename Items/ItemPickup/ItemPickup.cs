using Godot;
using System;

[Tool]
public partial class ItemPickup : CharacterBody2D
{
    [Signal]
    public delegate void PickedUpEventHandler();

    [Export]
    public ItemData ItemData { get => itemData; set => SetItemData(value as ItemData); }

    public Area2D Area2D;
    public Sprite2D Sprite2D;
    public AudioStreamPlayer2D AudioStreamPlayer2D;

    private ItemData itemData;

    public override void _Ready()
    {
        Area2D = GetNode<Area2D>("Area2D");
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        AudioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        UpdateTexture();

        if (Engine.IsEditorHint())
        {
            return;
        }

        Area2D.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        var collisionInfo = MoveAndCollide(Velocity * (float)delta);
        if (collisionInfo != null)
        {
            Velocity = Velocity.Bounce(collisionInfo.GetNormal());
        }

        Velocity -= Velocity * (float)delta * 4;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            if (itemData != null)
            {
                if (GlobalPlayerManager.Instance.INVENTORY_DATA.AddItem(itemData))
                {
                    ItemPickedUp();
                }
            }
        }
    }

    private async void ItemPickedUp()
    {
        Area2D.BodyEntered -= OnBodyEntered;
        AudioStreamPlayer2D.Play();
        Visible = false;
        EmitSignal(SignalName.PickedUp);
        await ToSignal(AudioStreamPlayer2D, AudioStreamPlayer2D.SignalName.Finished);
        QueueFree();
    }

    private void SetItemData(ItemData value)
    {
        itemData = value;
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        if (itemData != null && Sprite2D != null)
        {
            Sprite2D.Texture = itemData.Texture;
        }
    }
}
