using Godot;

public partial class EnemyChaseState : EnemyState
{
    [ExportCategory("AI")]
    [Export] public VisionArea VisionArea;
    [Export] public HurtBox AttackArea;
    [Export] public float StateAggroDuration = 0.5f;
    [Export] public EnemyState NextState;

    private string animName = "chase";

    [Export]
    public float ChaseSpeed = 40f;

    [Export]
    public float TurnRate = 0.25f;

    private float _timer = 0f;
    private Vector2 _direction;
    private bool canSeePlayer = false;

    public override void Init()
    {
        if (VisionArea != null)
        {
            VisionArea.PlayerEntered += OnPlayerEnter;
            VisionArea.PlayerExited += OnPlayerExit;
        }
    }

    // What happens when the player enters this State
    public override void Enter()
    {
        _timer = StateAggroDuration;
        
        Enemy.UpdateAnimation(animName);
        if (AttackArea != null)
        {
            AttackArea.Monitoring = true;
        }
    }

    // What happens when the player exits this State
    public override void Exit()
    {
        if (AttackArea != null)
        {
            AttackArea.Monitoring = false;
        }

        canSeePlayer = false;
    }

    // What happens during the _process update in this State
    public override EnemyState Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public override EnemyState PhysicsProcess(double delta)
    {
        var newDir = Enemy.GlobalPosition.DirectionTo(GlobalPlayerManager.Instance.player.GlobalPosition);
        _direction = _direction.Lerp(newDir, TurnRate);
        Enemy.Velocity = _direction * ChaseSpeed;
        if (Enemy.SetCrossDirection(_direction))
        {
            Enemy.UpdateAnimation(animName);
        }

        if (canSeePlayer == false)
        {
            _timer -= (float)delta;
            if (_timer <= 0f)
            {
                return NextState;
            }
        }
        else
        {
            _timer = StateAggroDuration;
        }

        return null;
    }

    public void OnPlayerEnter()
    {
        canSeePlayer = true;
        if (StateMachine.currentState is EnemyStunState || StateMachine.currentState is EnemyDestroyState)
        {
            return;
        }

        StateMachine.ChangeState(this);
    }

    public void OnPlayerExit()
    {
        canSeePlayer = false;
    }
}
