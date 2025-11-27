using Godot;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/chat_bubble.svg")]
public partial class DialogItem : Node
{
    public static readonly PackedScene DIALOG_SYSTEM = ResourceLoader.Load<PackedScene>("res://GUI/DialogSystem/DialogSystem.tscn");

    [Export]
	public NPCResource NpcInfo { get; set; }

	public EditorSelection EditorSelection { get; set; }
	public DialogSystemNode ExampleDialog { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			EditorSelection = EditorInterface.Singleton.GetSelection();
            EditorSelection.Connect(EditorSelection.SignalName.SelectionChanged, Callable.From(OnSelectionChanged));
			return; 
		}
		
		CheckNpcData();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	private void CheckNpcData()
	{
		if (NpcInfo == null)
		{
			Node p = this;
			while (true)
			{
				p = p.GetParent();
				if (p == null)
				{
                    return;
				}

				if (p is NPC npc && npc.NPCResource != null)
				{
					NpcInfo = npc.NPCResource;
					return;
				}
			}
		}
	}

	private void OnSelectionChanged()
	{
		if (EditorSelection == null)
		{
			return;
		}

        //GD.Print("in");
        var sel = EditorSelection.GetSelectedNodes();

		if (ExampleDialog!= null)
		{
			//GD.Print("queue");
			RemoveChild(ExampleDialog);
			ExampleDialog.QueueFree();
			ExampleDialog = null;
        }

		if (sel.Count > 0)
		{
			if (this == sel[0])
			{
                //GD.Print("new");
                ExampleDialog = DIALOG_SYSTEM.Instantiate<DialogSystemNode>();
				if (ExampleDialog == null)
				{
					return;
				}

                //GD.Print("Add");
                AddChild(ExampleDialog);
				ExampleDialog.Offset = GetParentGlobalPosition() + new Vector2(32, -200);
				CheckNpcData();
				SetEditorDisplay();
            }
		}
	}

	private Vector2 GetParentGlobalPosition()
	{
        Node p = this;
        while (true)
        {
            p = p.GetParent();
            if (p == null)
            {
				break;
            }

            if (p is Node2D node)
            {
                return node.GlobalPosition;
            }
        }

		return Vector2.Zero;
    }

	protected virtual void SetEditorDisplay()
	{
	}
}
