using System;
using System.Collections.Generic;
using System.Linq;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroupSubGroup : ICloneable {
        public int SubGroup;
        public string Algorithm;
        public List<ItemGroupEntry> List = new List<ItemGroupEntry>();
        public bool? Clear;

        public object Clone() {
            ItemGroupSubGroup clone = (ItemGroupSubGroup)MemberwiseClone();

            clone.List = List.Select(p => (ItemGroupEntry)p.Clone()).ToList();

            return clone;
        }
    }
}
