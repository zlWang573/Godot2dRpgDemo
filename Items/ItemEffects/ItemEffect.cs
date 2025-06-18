using Godot;
using System;

[GlobalClass]
public partial class ItemEffect : Resource
{
    [Export]
    public string UseDescription;

    public virtual void Use()
    {

    }
}
