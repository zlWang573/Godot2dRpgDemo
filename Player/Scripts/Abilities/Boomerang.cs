using Godot;
using System;

public partial class Boomerang : Node2D
{
    private enum State
    {
        INACTIVE = 0,
        THROW = 1,
        RETURN = 2
    }

    [Export]
    public float Acceleration = 500f;

    [Export]
    public float MaxSpeed = 400f;

    [Export]
    public AudioStream CatchAudio;

    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer2D audioStreamPlayer;

    private Player player;
    private Vector2 direction;
    private float speed = 0f;
    private State state;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        Visible = false;
        state = State.INACTIVE;
        player = GlobalPlayerManager.Instance.player;
    }

    public override void _PhysicsProcess(double delta)
    {
        float fd = (float)delta;
        if (state == State.THROW)
        {
            speed -= Acceleration * fd;
            Position += direction * speed * fd;
            if (speed <= 0f)
            {
                state = State.RETURN;
            }
        }
        else if (state == State.RETURN)
        {
            direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            speed += Acceleration * fd;
            Position += direction * speed * fd;
            if (GlobalPosition.DistanceTo(player.GlobalPosition) <= 10f)
            {
                GlobalPlayerManager.Instance.PlayAudio(CatchAudio);
                QueueFree();
            }
        }

        float speedRatio = speed / MaxSpeed;
        audioStreamPlayer.PitchScale = speedRatio * 0.75f + 0.75f;
        animationPlayer.SpeedScale = 1f + (speedRatio * 0.25f);
    }

    public void Throw(Vector2 throwDirection)
    {
        direction = throwDirection;
        speed = MaxSpeed;
        state = State.THROW;
        animationPlayer.Play("boomerang");
        GlobalPlayerManager.Instance.PlayAudio(CatchAudio);
        Visible = true;
    }
}
