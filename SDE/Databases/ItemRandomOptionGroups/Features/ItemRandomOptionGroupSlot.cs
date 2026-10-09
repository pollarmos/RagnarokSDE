using System;
using System.Collections.Generic;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroupSlot
        : ICloneable
    {
        public int Slot;

        public List<ItemRandomOptionGroupOption> Options
            = new List<ItemRandomOptionGroupOption>();

        public object Clone()
        {
            ItemRandomOptionGroupSlot clone =
                (ItemRandomOptionGroupSlot)MemberwiseClone();

            clone.Options =
                new List<ItemRandomOptionGroupOption>();

            foreach (ItemRandomOptionGroupOption option
                in Options)
            {
                clone.Options.Add(
                    (ItemRandomOptionGroupOption)
                    option.Clone());
            }

            return clone;
        }
    }
}