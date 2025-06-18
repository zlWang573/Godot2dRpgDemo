using Godot;
using System;

public partial class PersistentDataHandler : Node
{
    [Signal]
    public delegate void DataLoadedEventHandler();

    public bool Value = false;

    public override void _Ready()
    {
        GetValue();
    }

    public void SetValue()
    {
        GlobalSaveManager.Instance.AddPersistentValue(GetDataName());
    }

    public void GetValue()
    {
        Value = GlobalSaveManager.Instance.CheckPersistentValue(GetDataName());
        EmitSignal(SignalName.DataLoaded);
    }

    public string GetDataName()
    {
        return $"{GetTree().CurrentScene.SceneFilePath}/{GetParent().Name}/{Name}";
    }
}
