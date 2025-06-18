using Godot;
using Godot.Collections;

public partial class GlobalLevelManager : Node
{
    public static GlobalLevelManager Instance;

    public Array<Vector2I> CurrentTilemapBounds;

	public string TargetTransition;
	public Vector2 PositionOffset;

	[Signal]
	public delegate void TileMapBoundsChangedEventHandler(Array<Vector2I> bounds);

	[Signal]
	public delegate void LevelLoadStartedEventHandler();

    [Signal]
    public delegate void LevelLoadedEventHandler();

    // Called when the node enters the scene tree for the first time.
    public async override void _Ready()
    {
        Instance = this;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        EmitSignal(SignalName.LevelLoaded);
    }

    public void ChangeTilemapBounds(Array<Vector2I> bounds)
    {
        CurrentTilemapBounds = bounds;
        EmitSignal(SignalName.TileMapBoundsChanged, bounds);
    }

    public async void LoadNewLevel(string levelPath, string targetTransition, Vector2 positionOffect)
    {
        GetTree().Paused = true;
        TargetTransition = targetTransition;
        PositionOffset = positionOffect;

        await SceneTransition.Instance.FadeOut();

        EmitSignal(SignalName.LevelLoadStarted);

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        GetTree().ChangeSceneToFile(levelPath);

        await SceneTransition.Instance.FadeIn();

        GetTree().Paused = false;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        EmitSignal(SignalName.LevelLoaded);
    }
}
