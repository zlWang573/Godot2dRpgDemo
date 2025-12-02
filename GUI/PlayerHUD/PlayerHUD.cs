using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHUD : CanvasLayer
{
	private List<HeartGUI> Hearts = new List<HeartGUI>();
    public static PlayerHUD Instance;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Instance = this;
        foreach (var child in GetNode<HFlowContainer>("Control/HFlowContainer").GetChildren())
		{
			if (child is HeartGUI)
			{
				var heart = child as HeartGUI;
                Hearts.Add(heart);
				heart.Visible = false;
			}
		}
	}

	public void UpdateHp(int hp, int maxHp)
	{
		UpdateMaxHp(maxHp);
		for (int i = 0; i < maxHp  / 2; i++)
		{
			UpdateHeart(i, hp);
		}
	}

	public void UpdateHeart(int index, int hp)
	{
		int value = Math.Clamp(hp - index * 2, 0, 2);
		Hearts[index].Value = value;
	}

	public void UpdateMaxHp(int maxHp)
	{
		int heartCount = Mathf.CeilToInt(maxHp * 0.5f);
		for (int i = 0; i < Hearts.Count; i++) 
		{
			if (i < heartCount)
			{
                Hearts[i].Visible = true;
            }
			else
			{
                Hearts[i].Visible = false;
            }
		}
	}
}
