using Godot;
using System.Threading.Tasks;

public partial class AttackState : State
{
    [Export]
    private AudioStream attackSound;

    [Export]
    private float decelerateSpeed = 5f;

    private bool attacking = false;

    private AnimationPlayer animationPlayer;
    private AnimationPlayer attackAnimationPlayer;
    private AudioStreamPlayer2D audioStreamPlayer;
    private HurtBox hurtBox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("../../AnimationPlayer");
        attackAnimationPlayer = GetNode<AnimationPlayer>("../../Sprite2D/AttackEffectSprite/AnimationPlayer");
        audioStreamPlayer = GetNode<AudioStreamPlayer2D>("../../Audio/AudioStreamPlayer2D");
        hurtBox = GetNode<HurtBox>("../../Interactions/HurtBox");
    }

    // What happens when the player enters this State
    public async override void Enter()
    {
        player.UpdateAnimation("attack");
        attackAnimationPlayer.Play("attack_" + player.GetCrossDirectionName());
        animationPlayer.AnimationFinished += EndAttack;

        audioStreamPlayer.Stream = attackSound;
        audioStreamPlayer.PitchScale = new RandomNumberGenerator().RandfRange(0.9f, 1.1f);
        audioStreamPlayer.Play();

        attacking = true;

        await ToSignal(GetTree().CreateTimer(0.075), Timer.SignalName.Timeout);
        if (attacking)
        {
            hurtBox.Monitoring = true;
        }
    }

    // What happens when the player exits this State
    public override void Exit()
    {
        animationPlayer.AnimationFinished -= EndAttack;
        attacking = false;
        hurtBox.Monitoring = false;
    }

    // What happens during the _process update in this State
    public override State Process(double delta)
    {
        return null;
    }

    // What happens during the _physicsProcess update in this State
    public override State PhysicsProcess(double delta)
    {
        player.Velocity -= player.Velocity * decelerateSpeed * (float)delta;
        if (!attacking)
        {
            if (player.direction == Vector2.Zero)
            {
                return PlayerStateMachine.states["Idle"];
            }
            else
            {
                return PlayerStateMachine.states["Walk"];
            }
        }
        return null;
    }

    // What happens with input events in this State
    public override State HandleInput(InputEvent @event)
    {
        return null;
    }

    private void EndAttack(StringName animName)
    {
        attacking = false;
        hurtBox.Monitoring = false;
    }

    private void SetHurtBoxMonitoring(bool value)
    {
        hurtBox.Monitoring = value;
    }
}
