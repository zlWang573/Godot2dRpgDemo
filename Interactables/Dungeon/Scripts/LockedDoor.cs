using Godot;
using System;

public partial class LockedDoor : Node2D
{
    [Export]
    public ItemData KeyItem; // What type of item can open the door

    [Export]
    public AudioStream LockedAudio;

    [Export]
    public AudioStream OpenAudio;

    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D audioStreamPlayer;
    private PersistentDataHandler isOpenData;
    private Area2D interactArea;

    private bool isOpen = false;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        isOpenData = GetNode<PersistentDataHandler>("PersistentDataHandler");
        interactArea = GetNode<Area2D>("InteractArea2D");

        interactArea.AreaEntered += OnAreaEntered;
        interactArea.AreaExited += OnAreaExited;

        isOpenData.DataLoaded += SetState;
        SetState();
    }

    private void OpenDoor()
    {
        if (KeyItem == null)
        {
            return;
        }

        bool doorUnlocked = GlobalPlayerManager.Instance.INVENTORY_DATA.TryUseItem(KeyItem);
        if (doorUnlocked)
        {
            animationPlayer.Play("open_door");
            audioStreamPlayer.Stream = OpenAudio;
            isOpenData.SetValue();
        }
        else
        {
            animationPlayer.Play("closed");
            audioStreamPlayer.Stream = LockedAudio;
        }

        audioStreamPlayer.Play();
    }

    private void CloseDoor()
    {
        animationPlayer.Play("close_door");
        audioStreamPlayer.Stream = LockedAudio;
        audioStreamPlayer.Play();
    }

    private void SetState()
    {
        isOpen = isOpenData.Value;
        if (isOpen)
        {
            animationPlayer.Play("opened");
        }
        else
        {
            animationPlayer.Play("closed");
        }
    }

    private void OnAreaEntered(Area2D a)
    {
        GlobalPlayerManager.Instance.InteractPressed += OpenDoor;
    }

    private void OnAreaExited(Area2D a)
    {
        GlobalPlayerManager.Instance.InteractPressed -= OpenDoor;
    }
}
