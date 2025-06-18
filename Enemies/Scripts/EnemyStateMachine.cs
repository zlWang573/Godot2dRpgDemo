using Godot;
using Godot.Collections;
using System.Linq;

public partial class EnemyStateMachine : Node
{
    public static Dictionary<string, EnemyState> states { get; private set; }

    public EnemyState prevState;
    public EnemyState currentState;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Disabled;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ChangeState(currentState.Process(delta));
    }

    public override void _PhysicsProcess(double delta)
    {
        ChangeState(currentState.PhysicsProcess(delta));
    }

    public void Initialize(Enemy enemy)
    {
        states = new Dictionary<string, EnemyState>();

        foreach (var child in GetChildren())
        {
            if (child is EnemyState)
            {
                states.Add(child.Name, (EnemyState)child);
            }
        }

        foreach (var state in states)
        {
            state.Value.Enemy = enemy;
            state.Value.StateMachine = this;
            state.Value.Init();
        }

        if (states.Count > 0)
        {
            ChangeState(states.Values.First());
            ProcessMode = Node.ProcessModeEnum.Inherit;
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == null || newState == currentState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.Exit();
        }

        prevState = currentState;
        currentState = newState;
        currentState.Enter();
    }
}
