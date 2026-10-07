using System;
using System.Collections.Generic;
using System.Linq;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackage : ICloneable
    {
        public List<ItemPackageGroup> Groups =
            new List<ItemPackageGroup>();

        public object Clone()
        {
            ItemPackage clone =
                (ItemPackage)MemberwiseClone();

            clone.Groups = Groups
                .Select(p => (ItemPackageGroup)p.Clone())
                .ToList();

            return clone;
        }
    }
}