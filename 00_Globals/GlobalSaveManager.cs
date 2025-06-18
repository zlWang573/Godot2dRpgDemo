using Godot;
using Godot.Collections;
using static Godot.FileAccess;

public partial class GlobalSaveManager : Node
{
    public static GlobalSaveManager Instance;

    public const string SAVE_PATH = "user://";

	[Signal]
	public delegate void GameLoadedEventHandler();

    [Signal]
    public delegate void GameSavedEventHandler();

    public Dictionary<string, Variant> CurrentSave = new Dictionary<string, Variant>()
    {
        { "scene_path", "1" },
        { "player", new Dictionary<string, Variant>() { 
            { "hp", 1},
            { "max_hp", 1},
            { "pos_x", 0},
            { "pos_y", 0}} },
        { "items", new Array<Variant>() },
        { "persistence", new Array<string>() },
        { "quests", new Array<Node>() },
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
    }

    public void SaveGame()
	{
        UpdateScenePath();
        UpdatePlayerData();
        UpdateItemData();
		var file = FileAccess.Open(SAVE_PATH + "save.save", ModeFlags.Write);
		var saveJson = Json.Stringify(CurrentSave);
        file.StoreLine(saveJson);
        file.Close();
		EmitSignal(SignalName.GameSaved);
	}

	public async void LoadGame()
	{
        var file = FileAccess.Open(SAVE_PATH + "save.save", ModeFlags.Read);
        var json = file.GetLine();
        CurrentSave = (Dictionary<string, Variant>)Json.ParseString(json);
        var playerData = (Dictionary<string, Variant>)CurrentSave["player"];

        GlobalLevelManager.Instance.LoadNewLevel((string)CurrentSave["scene_path"], "", Vector2.Zero);

        await ToSignal(GlobalLevelManager.Instance, GlobalLevelManager.SignalName.LevelLoadStarted);

        GlobalPlayerManager.Instance.SetPlayerPosition(new Vector2((float)playerData["pos_x"], (float)playerData["pos_y"]));
        GlobalPlayerManager.Instance.SetPlayerHealth((int)playerData["hp"], (int)playerData["max_hp"]);
        GlobalPlayerManager.Instance.INVENTORY_DATA.LoadSaveData((Array<Variant>)CurrentSave["items"]);

        await ToSignal(GlobalLevelManager.Instance, GlobalLevelManager.SignalName.LevelLoaded);

        EmitSignal(SignalName.GameLoaded);
    }

    public void UpdatePlayerData()
    {
        var p = GlobalPlayerManager.Instance.player;
        var pData = (Dictionary<string, Variant>)CurrentSave["player"];
        pData["hp"] = p.Hp;
        pData["max_hp"] = p.MaxHp;
        pData["pos_x"] = p.GlobalPosition.X;
        pData["pos_y"] = p.GlobalPosition.Y;
    }

    public void UpdateScenePath()
    {
        var p = string.Empty;
        foreach (var c in GetTree().Root.GetChildren())
        {
            if (c is Level)
            {
                p = ((Level)c).SceneFilePath;
                break;
            }
        }

        CurrentSave["scene_path"] = p;
    }

    public void UpdateItemData()
    {
        CurrentSave["items"] = GlobalPlayerManager.Instance.INVENTORY_DATA.GetSaveData();
    }

    public void AddPersistentValue(string value)
    {
        if (!CheckPersistentValue(value))
        {
            var p = (Array<string>)CurrentSave["persistence"];
            p.Add(value);
        }
    }

    public bool CheckPersistentValue(string value)
    {
        var p = (Array<string>)CurrentSave["persistence"];
        return p.Contains(value);
    }
}
