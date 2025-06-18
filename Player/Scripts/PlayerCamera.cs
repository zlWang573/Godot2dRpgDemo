using Godot;
using Godot.Collections;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalLevelManager.Instance.TileMapBoundsChanged += UpdateLimits;
		// UpdateLimits(GlobalLevelManager.Instance.CurrentTilemapBounds);
	}

	public void UpdateLimits(Array<Vector2I> bounds)
	{
		LimitLeft = bounds[0].X;
        LimitRight = bounds[1].X;
        LimitTop = bounds[0].Y;
        LimitBottom = bounds[1].Y;
    }
}
