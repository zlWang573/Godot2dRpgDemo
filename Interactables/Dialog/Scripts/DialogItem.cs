using Godot;
using System;

[Tool]
[GlobalClass]
[Icon("res://GUI/DialogSystem/Icons/chat_bubble.svg")]
public partial class DialogItem : Node
{
	[Export]
	public NPCResource NpcInfo { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
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
}
