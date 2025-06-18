using Godot;
using Godot.Collections;

[GlobalClass]
public partial class InventoryData : Resource
{
    [Export]
    public Array<SlotData> Slots;

    public void Init()
    {
        ConnectSlots();
    }

    public bool AddItem(ItemData item, int count = 1)
    {
        foreach (var s in Slots)
        {
            if (s != null)
            {
                if (s.ItemData == item)
                {
                    s.Quantity += count;

                    return true;
                }
            }
        }

        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i] == null)
            {
                var newSlot = new SlotData();
                newSlot.ItemData = item;
                newSlot.Quantity = count;
                Slots[i] = newSlot;
                newSlot.Changed += SlotChanged;
                return true;
            }
        }

        GD.Print("Inventory is full");
        return false;
    }

    public void ConnectSlots()
    {
        foreach(var s in Slots)
        {
            if (s != null)
            {
                s.Changed += SlotChanged;
            }
        }
    }

    public void SlotChanged()
    {
        foreach(var s in Slots)
        {
            if (s != null && s.Quantity <= 0)
            {
                int index = Slots.IndexOf(s);
                Slots[index] = null;
                EmitSignal(SignalName.Changed);
            }
        }
    }

    public Array<Variant> GetSaveData()
    {
        var data = new Array<Variant>();
        foreach (var s in Slots)
        {
            data.Add(ItemToSave(s));
        }

        return data;
    }

    public Dictionary<string, Variant> ItemToSave(SlotData slot)
    {
        var result = new Dictionary<string, Variant>()
        {
            { "item", string.Empty},
            { "quantity", 0},
        };

        if (slot != null)
        {
            result["quantity"] = slot.Quantity;
            if (slot.ItemData != null)
            {
                result["item"] = slot.ItemData.ResourcePath;
            }
        }

        return result;
    }

    public void LoadSaveData(Array<Variant> data)
    {
        var size = Slots.Count;
        Slots.Clear();
        Slots.Resize(size);
        for (int i = 0; i < size; i++)
        {
            Slots[i] = ItemFromSave((Dictionary<string, Variant>)data[i]);
        }

        ConnectSlots();
    }

    public SlotData ItemFromSave(Dictionary<string, Variant> itemData)
    {
        var path = (string)itemData["item"];
        var quantity = (int)itemData["quantity"];
        if (!string.IsNullOrEmpty(path))
        {
            var slot = new SlotData();
            slot.Quantity = quantity;
            slot.ItemData = ResourceLoader.Load<ItemData>(path);

            return slot;
        }

        return null;
    }

    public bool TryUseItem(ItemData item, int count = 1)
    {
        foreach (var s in Slots)
        {
            if (s != null)
            {
                if (s.ItemData == item && s.Quantity >= count)
                {
                    s.Quantity -= count;
                    return true;
                }
            }
        }

        return false;
    }
}
