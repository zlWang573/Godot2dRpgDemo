using Godot;

public partial class State : Node
{
	// Reference to the player that this state belongs to
	public static Player player;
    public static PlayerStateMachine stateMachine;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    // What happens when initialize this State
    public virtual void Init()
    {

    }

    // What happens when the player enters this State
    public virtual void Enter()
	{

	}

    // What happens when the player exits this State
    public virtual void Exit()
	{

	}

    // What happens during the _process update in this State
    public virtual State Process(double delta)
	{
		return null;
	}

    // What happens during the _physicsProcess update in this State
    public virtual State PhysicsProcess(double delta)
	{
		return null;
	}

    // What happens with input events in this State
    public virtual State HandleInput(InputEvent @event)
	{
		return null;
	}
}
