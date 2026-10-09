using System;
using System.Collections.Generic;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroup
        : ICloneable
    {
        public string Group;

        public int? MaxRandom;

        public List<ItemRandomOptionGroupSlot> Slots
            = new List<ItemRandomOptionGroupSlot>();

        public List<ItemRandomOptionGroupOption> Random
            = new List<ItemRandomOptionGroupOption>();

        public object Clone()
        {
            ItemRandomOptionGroup clone =
                (ItemRandomOptionGroup)MemberwiseClone();

            clone.Slots =
                new List<ItemRandomOptionGroupSlot>();

            foreach (ItemRandomOptionGroupSlot slot
                in Slots)
            {
                clone.Slots.Add(
                    (ItemRandomOptionGroupSlot)
                    slot.Clone());
            }

            clone.Random =
                new List<ItemRandomOptionGroupOption>();

            foreach (ItemRandomOptionGroupOption option
                in Random)
            {
                clone.Random.Add(
                    (ItemRandomOptionGroupOption)
                    option.Clone());
            }

            return clone;
        }
    }
}