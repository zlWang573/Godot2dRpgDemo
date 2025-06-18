using Godot;

public partial class EnemyState : Node
{
    // Reference to the enemy that this state belongs to
    public Enemy Enemy;
    public EnemyStateMachine StateMachine;

    public virtual void Init()
    {

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // What happens when the enemy enters this State
    public virtual void Enter()
    {

    }

    // What happens when the enemy exits this State
    public virtual void Exit()
    {

    }

    // What happens during the _process update in this State
    public virtual EnemyState Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public virtual EnemyState PhysicsProcess(double delta)
    {
        return null;
    }
}
