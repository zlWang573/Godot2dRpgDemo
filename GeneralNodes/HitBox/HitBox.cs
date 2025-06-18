using Godot;

public partial class HitBox : Area2D
{
    [Signal]
    public delegate void DamagedEventHandler(HurtBox hurtbox);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(HurtBox hurtbox)
	{
		EmitSignal(SignalName.Damaged, hurtbox);
    }
}
