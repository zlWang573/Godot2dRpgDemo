using Godot;
using System;

[GlobalClass]
public partial class ItemEffectHeal : ItemEffect
{
    [Export]
    public int HealAmount = 1;

    [Export]
    public AudioStream AudioStream = null;

    public override void Use()
    {
        GlobalPlayerManager.Instance.player.UpdateHp(HealAmount);
        PauseMenu.Instance.PlaySound(AudioStream);
    }
}
