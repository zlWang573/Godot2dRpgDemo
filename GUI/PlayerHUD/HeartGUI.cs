using Godot;

public partial class HeartGUI : Control
{
	public Sprite2D sprite2D;
	public int Value { get => _value; set { _value = value; UpdateSprite(); } }

	private int _value = 2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        _value = 2;
	}

	public void UpdateSprite()
	{
		sprite2D.Frame = _value;
	}
}
