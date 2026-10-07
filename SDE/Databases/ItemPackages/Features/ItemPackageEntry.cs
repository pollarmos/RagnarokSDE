using System;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackageEntry : ICloneable
    {
        public string Item = "";

        public int? Amount;
        public int? RentalHours;
        public int? Refine;

        public string Grade;
        public string RandomOptionGroup;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}