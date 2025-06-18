using Godot;

public partial class EnemyIdleState : EnemyState
{
    [ExportCategory("AI")]
    [Export] public float MinStateDuration = 0.5f;
    [Export] public float MaxStateDuration = 1.5f;
    [Export] public EnemyState AfterIdleState;

    private string animName = "idle";

    private float _timer = 0f;

    // What happens when the player enters this State
    public override void Enter()
    {
        Enemy.Velocity = Vector2.Zero;
        _timer = new RandomNumberGenerator().RandfRange(MinStateDuration, MaxStateDuration);
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
            return AfterIdleState;
        }

        return null;
    }
}
