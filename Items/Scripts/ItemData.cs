using Godot;
using Godot.Collections;

[GlobalClass]
[Tool]
public partial class ItemData : Resource
{
    [Export]
    public string Name = string.Empty;

    [Export]
    public string Description = string.Empty;

    [Export]
    public Texture2D Texture;

    [ExportCategory("Item Use Effects")]
    [Export]
    public Array<ItemEffect> Effects;

    public bool Use()
    {
        if (Effects != null && Effects.Count > 0)
        {
            foreach (var effect in Effects)
            {
                if (effect != null)
                {
                    effect.Use();
                }
            }

            return true;
        }

        return false;
    }
}
