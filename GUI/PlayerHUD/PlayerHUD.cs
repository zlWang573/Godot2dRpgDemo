using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHUD : CanvasLayer
{
	public static List<HeartGUI> Hearts = new List<HeartGUI>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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

	public static void UpdateHp(int hp, int maxHp)
	{
		UpdateMaxHp(maxHp);
		for (int i = 0; i < maxHp  / 2; i++)
		{
			UpdateHeart(i, hp);
		}
	}

	public static void UpdateHeart(int index, int hp)
	{
		int value = Math.Clamp(hp - index * 2, 0, 2);
		Hearts[index].Value = value;
	}

	public static void UpdateMaxHp(int maxHp)
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
