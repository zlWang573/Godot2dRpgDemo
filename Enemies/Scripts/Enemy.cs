using Godot;
using Godot.Collections;

public partial class Enemy : CharacterBody2D
{
    public Vector2 direction = Vector2.Zero;
    private Vector2 crossDirection = Vector2.Down;
    public bool Invulnerable = false;

    public AnimationPlayer animationPlayer;
    private Sprite2D sprite;
    private EnemyStateMachine stateMachine;
    public Player player;
    public HitBox hitBox;
    public HurtBox hurtBox;

    [Signal]
    public delegate void DirectionChangedEventHandler(Vector2 newDirection);

    [Signal]
    public delegate void EnemyDamagedEventHandler(HurtBox hurtBox);

    [Signal]
    public delegate void EnemyDestroyedEventHandler(HurtBox hurtBox);

    [Export]
    public int Hp = 3;

    public Array<Vector2> DIR_4 = new Array<Vector2>() { Vector2.Left, Vector2.Right, Vector2.Up, Vector2.Down };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        sprite = GetNode<Sprite2D>("Sprite2D");
        stateMachine = GetNode<EnemyStateMachine>("EnemyStateMachine");
        hitBox = GetNode<HitBox>("HitBox");
        hurtBox = GetNode<HurtBox>("HurtBox");
        player = GlobalPlayerManager.Instance.player;

        hitBox.Damaged += TakeDamage;

        stateMachine.Initialize(this);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public bool SetCrossDirection(Vector2 _direction)
    {
        direction = _direction;
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
        if (Invulnerable == true)
        {
            return;
        }

        Hp -= hurtBox.damage;
        if (Hp > 0)
        {
            EmitSignal(SignalName.EnemyDamaged, hurtBox);
        }
        else
        {
            EmitSignal(SignalName.EnemyDestroyed, hurtBox);
        }
    }
}
