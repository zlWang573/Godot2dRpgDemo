using Godot;

public partial class EnemyWanderState : EnemyState
{
    [ExportCategory("AI")]
    [Export] public float StateAnimationDuration = 0.7f;
    [Export] public int MinStateCycles = 1;
    [Export] public int MaxStateCycles = 3;
    [Export] public EnemyState AfterWanderState;

    private string animName = "walk";

    [Export]
    public float WanderSpeed = 20f;

    private float _timer = 0f;
    private Vector2 _direction;

    // What happens when the player enters this State
    public override void Enter()
    {
        _timer = new RandomNumberGenerator().RandiRange(MinStateCycles, MaxStateCycles) * StateAnimationDuration;
        var rand = new RandomNumberGenerator().RandiRange(0, 3);
        _direction = Enemy.DIR_4[rand];
        Enemy.Velocity = _direction * WanderSpeed;
        Enemy.SetCrossDirection(_direction);
        Enemy.UpdateAnimation(animName);
    }

    // What happens when the player exits this State
    public override void Exit()
    {

    }

    // What happens during the _process update in this State
    public override EnemyState Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public override EnemyState PhysicsProcess(double delta)
    {
        _timer -= (float)delta;
        if (_timer <= 0f)
        {
            return AfterWanderState;
        }

        return null;
    }
}
