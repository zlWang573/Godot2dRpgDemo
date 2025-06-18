using Godot;

public partial class IdleState : State
{
	// What happens when the player enters this State
	public override void Enter()
	{
		player.UpdateAnimation("idle");
	}

    // What happens when the player exits this State
    public override void Exit()
	{

	}

    // What happens during the _process update in this State
    public override State Process(double delta)
	{
		return null;
	}

    // What happens during the _physicsProcess update in this State
    public override State PhysicsProcess(double delta)
	{
		if (player.direction != Vector2.Zero)
		{
			return PlayerStateMachine.states["Walk"];
		}

        player.Velocity = Vector2.Zero;
        return null;
	}

    // What happens with input events in this State
    public override State HandleInput(InputEvent @event)
	{
		if (@event.IsActionPressed("attack"))
		{
            return PlayerStateMachine.states["Attack"];
        }

		if (@event.IsActionPressed("interact"))
		{
			GlobalPlayerManager.Instance.EmitSignal(GlobalPlayerManager.SignalName.InteractPressed);
		}

		return null;
	}
}
