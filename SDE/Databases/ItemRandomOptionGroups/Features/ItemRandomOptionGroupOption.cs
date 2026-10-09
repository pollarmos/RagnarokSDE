using System;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroupOption
        : ICloneable
    {
        public string Option;

        public int? MinValue;

        public int? MaxValue;

        public int? Param;

        public int? Chance;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}