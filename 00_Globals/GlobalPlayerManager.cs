using Godot;

public partial class GlobalPlayerManager : Node
{
    public static GlobalPlayerManager Instance;

    public static readonly PackedScene PLAYER = ResourceLoader.Load<PackedScene>("res://Player//Player.tscn");
    public readonly InventoryData INVENTORY_DATA = ResourceLoader.Load<InventoryData>("res://GUI/PauseMenu/Inventory/PlayerInventory.tres");

    [Signal]
    public delegate void InteractPressedEventHandler();

    public Player player;
    public bool PlayerSpawned = false;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        Instance = this;
        AddPlayerInstance();
        await ToSignal(GetTree().CreateTimer(0.2), Timer.SignalName.Timeout);
        PlayerSpawned = true;
    }

    public void AddPlayerInstance()
    {
        player = PLAYER.Instantiate<Player>();
        AddChild(player);
    }

    public void SetPlayerPosition(Vector2 newPos)
    {
        player.GlobalPosition = newPos;
    }

    public void SetPlayerHealth(int hp, int maxHp)
    {
        player.MaxHp = maxHp;
        player.Hp = hp;
        player.UpdateHp(0);
    }

    public void SetAsParent(Node2D p)
    {
        if (player.GetParent() != null)
        {
            player.GetParent().RemoveChild(player);
        }

        p.AddChild(player);
    }

    public void UnparentPlayer(Node2D p)
    {
        p.RemoveChild(player);
    }

    public void PlayAudio(AudioStream audio)
    {
        player.audioStreamPlayer.Stream = audio;
        player.audioStreamPlayer.Play();
    }
}
