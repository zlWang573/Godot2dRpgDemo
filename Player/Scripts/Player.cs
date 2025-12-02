using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	public Vector2 direction = Vector2.Zero;
	public Vector2 crossDirection = Vector2.Down;

	public AnimationPlayer animationPlayer;
	public AnimationPlayer effectAnimationPlayer;
	public AudioStreamPlayer2D audioStreamPlayer;
	private Sprite2D sprite;
	private PlayerStateMachine stateMachine;
    private HitBox hitBox;

    [Signal]
	public delegate void DirectionChangedEventHandler(Vector2 newDirection);

    [Signal]
    public delegate void PlayerDamagedEventHandler(HurtBox hurtBox);

	private bool invulnerable = false;
	public int Hp = 6;
    public int MaxHp = 6;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GlobalPlayerManager.Instance.player = this;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        effectAnimationPlayer = GetNode<AnimationPlayer>("EffectAnimationPlayer");
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("Audio/AudioStreamPlayer2D");
        sprite = GetNode<Sprite2D>("Sprite2D");
        stateMachine = GetNode<PlayerStateMachine>("StateMachine");
        hitBox = GetNode<HitBox>("HitBox");

		hitBox.Damaged += TakeDamage;
        stateMachine.Initialize(this);

		UpdateHp(MaxHp);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
	{
		direction = new Vector2(Input.GetAxis("left", "right"), Input.GetAxis("up", "down")).Normalized();
		MoveAndSlide();
    }

	public bool SetCrossDirection()
	{
		if (direction == Vector2.Zero)
		{
			return false;
		}

		var newCrossDirection = crossDirection;

		if (direction.Y == 0f)
		{
			newCrossDirection = direction.X < 0f ? Vector2.Left : Vector2.Right;
		}
		else if (direction.X == 0f)
		{
            newCrossDirection = direction.Y < 0f ? Vector2.Up : Vector2.Down;
        }
		else
		{
			// If press left/right and up/down at same time, cross direction won't change by above code, need these codes to correct cross direction
            if (newCrossDirection == Vector2.Left && direction.X > 0f || newCrossDirection == Vector2.Right && direction.X < 0f)
            {
                newCrossDirection = direction.X < 0f ? Vector2.Left : Vector2.Right;
            }
            else if (newCrossDirection == Vector2.Up && direction.Y > 0f || newCrossDirection == Vector2.Down && direction.Y < 0f)
            {
                newCrossDirection = direction.Y < 0f ? Vector2.Up : Vector2.Down;
            }
        }

        if (newCrossDirection == crossDirection)
		{
			return false;
		}

		crossDirection = newCrossDirection;
		EmitSignal(SignalName.DirectionChanged, crossDirection);
		sprite.Scale = new Vector2(crossDirection == Vector2.Left ? -1f : 1f, 1f);
        return true;
	}

	public void UpdateAnimation(string state)
	{
        animationPlayer.Play(state + "_" + GetCrossDirectionName());
    }

	public string GetCrossDirectionName()
	{
		if (crossDirection == Vector2.Down)
		{
			return "down";
		}
		else if (crossDirection == Vector2.Up)
		{
			return "up";
		}
		else
		{
			return "side";
		}
	}

    private void TakeDamage(HurtBox hurtBox)
    {
        if (invulnerable == true)
        {
            return;
        }

		UpdateHp(-hurtBox.damage);

		if (Hp > 0)
		{
			EmitSignal(SignalName.PlayerDamaged, hurtBox);
		}
		else
		{
            EmitSignal(SignalName.PlayerDamaged, hurtBox);
			UpdateHp(MaxHp);
        }
    }

	public void UpdateHp(int delta)
	{
		Hp = Math.Clamp(Hp + delta, 0, MaxHp);
        PlayerHUD.Instance.UpdateHp(Hp, MaxHp);
	}

	public async void MakeInvulnerable(float duration = 1f)
	{
		invulnerable = true;
		hitBox.Monitoring = false;

        await ToSignal(GetTree().CreateTimer(duration), Timer.SignalName.Timeout);

        invulnerable = false;
        hitBox.Monitoring = true;
    }
}
