using Godot;

public partial class Level : Node2D
{
	[Export]
	public AudioStream Music;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.YSortEnabled = true;
		GlobalPlayerManager.Instance.SetAsParent(this);
		GlobalLevelManager.Instance.LevelLoadStarted += FreeLevel;
		GlobalAudioManager.Instance.PlayMusic(Music);
	}

	public void FreeLevel()
	{
        GlobalLevelManager.Instance.LevelLoadStarted -= FreeLevel;
        GlobalPlayerManager.Instance.UnparentPlayer(this);
		QueueFree();
	}
}
