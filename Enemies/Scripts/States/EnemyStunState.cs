using Godot;

public partial class EnemyStunState : EnemyState
{
    [ExportCategory("AI")]
    [Export] public EnemyState AfterState;

    private string animName = "stun";

    [Export]
    public float KnockbackSpeed = 200f;
    [Export]
    public float DecelerateSpeed = 10f;

    private Vector2 damagePosition;
    private Vector2 _direction;
    private bool _animationFinish = false;

    public override void Init()
    {
        Enemy.EnemyDamaged += OnEnemyDamaged;
    }

    // What happens when the player enters this State
    public override void Enter()
    {
        Enemy.Invulnerable = true;
        _animationFinish = false;

        _direction = Enemy.GlobalPosition.DirectionTo(damagePosition);

        Enemy.Velocity = _direction * -KnockbackSpeed;
        Enemy.SetCrossDirection(_direction);
        Enemy.UpdateAnimation(animName);
        Enemy.animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    // What happens when the player exits this State
    public override void Exit()
    {
        Enemy.animationPlayer.AnimationFinished -= OnAnimationFinished;
        Enemy.Invulnerable = false;
    }

    // What happens during the _process update in this State
    public override EnemyState Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public override EnemyState PhysicsProcess(double delta)
    {
        if (_animationFinish == true)
        {
            return AfterState;
        }
        Enemy.Velocity -= Enemy.Velocity * DecelerateSpeed * (float)delta;

        return null;
    }

    private void OnEnemyDamaged(HurtBox hurtBox)
    {
        damagePosition = hurtBox.GlobalPosition;
        StateMachine.ChangeState(this);
    }

    private void OnAnimationFinished(StringName _a)
    {
        _animationFinish = true;
    }
}
