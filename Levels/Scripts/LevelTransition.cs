using Godot;

public enum SIDE
{
	LEFT, RIGHT, TOP, BOTTOM
}

[Tool]
public partial class LevelTransition : Area2D
{
	[Export]
	public string LevelPath;

	[Export]
	public string TargetTransitionArea = "LevelTransition";

	[Export]
	public bool CenterPlayer = false;

	[ExportCategory("Collision Area Settings")]
	[Export] public SIDE Side { get => side; set { side = value; UpdateArea(); } }
    [Export] public bool SnapToGrid { get => true; set { _SnapToGrid(); } }
	[Export] public int Size { get => size;  set { size = value; UpdateArea(); } }

	private int size = 2;
	private SIDE side = SIDE.LEFT;
	public CollisionShape2D CollisionShape;


	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		UpdateArea();

		if (Engine.IsEditorHint())
		{
			return;
		}

		Monitoring = false;
		PlacePlayer();

        await ToSignal(GlobalLevelManager.Instance, GlobalLevelManager.SignalName.LevelLoaded);

        Monitoring = true;
        BodyEntered += PlayerEntered;
    }

	public void PlayerEntered(Node2D player)
	{
		GlobalLevelManager.Instance.LoadNewLevel(LevelPath, TargetTransitionArea, GetOffset());
	}

	public void PlacePlayer()
	{
		if (Name != GlobalLevelManager.Instance.TargetTransition)
		{
			return;
		}

		GlobalPlayerManager.Instance.SetPlayerPosition(GlobalLevelManager.Instance.PositionOffset + GlobalPosition);
	}

	public Vector2 GetOffset()
	{
		var offset = Vector2.Zero;
		var playerPosition = GlobalPlayerManager.Instance.player.GlobalPosition;

		if (Side == SIDE.LEFT || Side == SIDE.RIGHT)
		{
			if (CenterPlayer)
			{
				offset.Y = 0;
			}
			else
			{
                offset.Y = playerPosition.Y - GlobalPosition.Y;
            }

            offset.X = Side == SIDE.LEFT ? -16 : 16;
        }
        else
        {
            if (CenterPlayer)
            {
                offset.X = 0;
            }
            else
            {
                offset.X = playerPosition.X - GlobalPosition.X;
            }

            offset.Y = Side == SIDE.TOP ? -16 : 16;
        }

        return offset;
	}

    public void UpdateArea()
	{
		Vector2 newRect = new Vector2(32, 32);
		Vector2 newPosition = Vector2.Zero;

		if (Side == SIDE.TOP)
		{
			newRect.X *= size;
			newPosition.Y -= 16;
		}
		else if (Side == SIDE.BOTTOM)
        {
            newRect.X *= size;
            newPosition.Y += 16;
        }
        else if (Side == SIDE.LEFT)
        {
            newRect.Y *= size;
            newPosition.X -= 16;
        }
        else if (Side == SIDE.RIGHT)
        {
            newRect.Y *= size;
            newPosition.X += 16;
        }

		if (CollisionShape == null)
		{
            CollisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        }

        if (CollisionShape?.Shape is RectangleShape2D rect)
        {
			rect.Size = newRect;
        }

		CollisionShape.Position = newPosition;
    }

	private void _SnapToGrid()
	{
        Position = new Vector2(Mathf.RoundToInt(Position.X / 16) * 16, Mathf.RoundToInt(Position.Y / 16) * 16);
	}
}
