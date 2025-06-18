using Godot;
using Godot.Collections;

public partial class EnemyDestroyState : EnemyState
{
    private string animName = "destroy";

    public readonly PackedScene PICKUP = ResourceLoader.Load<PackedScene>("res://Items/ItemPickup/ItemPickup.tscn");

    [Export]
    public float KnockbackSpeed = 200f;
    [Export]
    public float DecelerateSpeed = 10f;

    private Vector2 damagePosition;
    private Vector2 _direction;

    [ExportCategory("ItemDrops")]
    [Export]
    public Array<DropData> Drops;

    public override void Init()
    {
        Enemy.EnemyDestroyed += OnEnemyDestroyed;
    }

    // What happens when the player enters this State
    public override void Enter()
    {
        Enemy.Invulnerable = true;

        _direction = Enemy.GlobalPosition.DirectionTo(damagePosition);

        Enemy.Velocity = _direction * -KnockbackSpeed;
        Enemy.SetCrossDirection(_direction);
        Enemy.UpdateAnimation(animName);
        Enemy.animationPlayer.AnimationFinished += OnAnimationFinished;

        Enemy.hurtBox.Monitoring = false;
        Enemy.hitBox.Monitoring = false;

        DropItems();
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
        Enemy.Velocity -= Enemy.Velocity * DecelerateSpeed * (float)delta;

        return null;
    }

    private void OnEnemyDestroyed(HurtBox hurtBox)
    {
        damagePosition = hurtBox.GlobalPosition;
        StateMachine.ChangeState(this);
    }

    private void OnAnimationFinished(StringName _a)
    {
        Enemy.QueueFree();
    }

    private void DropItems()
    {
        if (Drops == null || Drops.Count <= 0)
        {
            return;
        }

        foreach (var drop in Drops)
        {
            if (drop != null && drop.Item != null)
            {
                int count = drop.GetDropCount();
                for (int i = 0; i < count; i++)
                {
                    var item = PICKUP.Instantiate<ItemPickup>();
                    item.ItemData = drop.Item;
                    Enemy.GetParent().CallDeferred("add_child", item);
                    item.GlobalPosition = Enemy.GlobalPosition;
                    item.Velocity = Enemy.Velocity.Rotated((float)GD.RandRange(-1.5, 1.5)) * (float)GD.RandRange(0.9, 1.5);
                }
            }
        }
    }
}
