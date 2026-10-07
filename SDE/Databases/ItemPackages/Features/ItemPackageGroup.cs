using System;
using System.Collections.Generic;
using System.Linq;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackageGroup : ICloneable
    {
        public int Group;

        public List<ItemPackageEntry> Items =
            new List<ItemPackageEntry>();

        public object Clone()
        {
            ItemPackageGroup clone =
                (ItemPackageGroup)MemberwiseClone();

            clone.Items = Items
                .Select(p => (ItemPackageEntry)p.Clone())
                .ToList();

            return clone;
        }
    }
}