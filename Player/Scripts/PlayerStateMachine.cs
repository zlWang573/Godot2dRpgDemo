using Godot;
using Godot.Collections;
using System.Linq;

public partial class PlayerStateMachine : Node
{
    public static Dictionary<string, State> states { get; private set; }
    
	private State prevState;
	private State currentState;

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

    public override void _UnhandledInput(InputEvent @event)
    {
        ChangeState(currentState.HandleInput(@event));
    }

    public void Initialize(Player player)
	{
		states = new Dictionary<string, State>();

		foreach (var child in GetChildren())
		{
			if (child is State)
			{
				states.Add(child.Name, (State)child);
			}
		}

		if (states.Count <= 0)
		{
			return;
		}

        State.player = player;
		State.stateMachine = this;

		foreach (var state in states.Values)
		{
			state.Init();
		}

		ChangeState(states.Values.First());
		ProcessMode = Node.ProcessModeEnum.Inherit;
	}

	public void ChangeState(State newState)
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
