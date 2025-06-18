using Godot;

public partial class StunState : State
{
    [Export]
    public float KnockbackSpeed = 200f;
    [Export]
    public float DecelerateSpeed = 10f;
    [Export]
    public float InvulnerableDuration = 1f;

    private HurtBox hurtBox;

    private State AfterState = null;

    public override void Init()
    {
        player.PlayerDamaged += PlayerDamaged;
    }

    // What happens when the player enters this State
    public override void Enter()
    {
        player.animationPlayer.AnimationFinished += AnimationFinished;

        player.direction = player.GlobalPosition.DirectionTo(hurtBox.GlobalPosition);
        player.Velocity = player.direction * -KnockbackSpeed;
        player.SetCrossDirection();

        player.UpdateAnimation("stun");

        player.MakeInvulnerable(InvulnerableDuration);
        player.effectAnimationPlayer.Play("damaged");
    }

    // What happens when the player exits this State
    public override void Exit()
    {
        player.animationPlayer.AnimationFinished -= AnimationFinished;
        AfterState = null;
    }

    // What happens during the _process update in this State
    public override State Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public override State PhysicsProcess(double delta)
    {
        player.Velocity -= player.Velocity * DecelerateSpeed * (float)delta;
        return AfterState;
    }

    // What happens with input events in this State
    public override State HandleInput(InputEvent @event)
    {
        return null;
    }

    public void PlayerDamaged(HurtBox hurtBox)
    {
        this.hurtBox = hurtBox;
        stateMachine.ChangeState(this);
    }

    public void AnimationFinished(StringName a)
    {
        AfterState = PlayerStateMachine.states["Idle"];
    }
}
