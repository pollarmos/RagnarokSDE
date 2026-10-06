using System;
using System.Collections.Generic;
using System.Linq;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroup : ICloneable {
        public string Group = "";
        public List<ItemGroupSubGroup> SubGroups = new List<ItemGroupSubGroup>();

        public object Clone() {
            ItemGroup clone = (ItemGroup)MemberwiseClone();

            clone.SubGroups = SubGroups
                .Select(p => (ItemGroupSubGroup)p.Clone())
                .ToList();

            return clone;
        }
    }
}
