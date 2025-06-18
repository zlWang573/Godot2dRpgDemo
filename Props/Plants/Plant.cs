using Godot;

public partial class Plant : Node2D
{
	[Export]
	public HitBox HitBox { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HitBox.Damaged += TakeDamage;
    }

	public void TakeDamage(HurtBox hurtBox)
	{
		QueueFree();
	}
}
