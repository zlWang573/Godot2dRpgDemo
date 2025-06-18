using Godot;
using Godot.Collections;
using System.Linq;

public partial class LevelTileMap : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GlobalLevelManager.Instance.ChangeTilemapBounds(GetTilemapBounds());
	}

	public Array<Vector2I> GetTilemapBounds()
	{
		var bounds = new Array<Vector2I>();
		bounds.Add(GetUsedRect().Position * RenderingQuadrantSize);
        bounds.Add(GetUsedRect().End * RenderingQuadrantSize);

        return bounds;
	}
}
