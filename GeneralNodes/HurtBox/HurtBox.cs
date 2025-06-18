using Godot;

public partial class HurtBox : Area2D
{
	[Export]
	public int damage = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += _AreaEntered;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _AreaEntered(Area2D a)
	{
		if (a is HitBox)
		{
			((HitBox)a).TakeDamage(this);
		}
	}
}
