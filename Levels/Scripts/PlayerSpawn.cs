using Godot;

public partial class PlayerSpawn : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
		if (GlobalPlayerManager.Instance.PlayerSpawned == false)
		{
			GlobalPlayerManager.Instance.SetPlayerPosition(GlobalPosition);
			GlobalPlayerManager.Instance.PlayerSpawned = true;
        }
	}
}
