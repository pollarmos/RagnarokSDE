using System;

namespace SDE.Databases.ItemRandomOptions.Features
{
    public class ItemRandomOption : ICloneable
    {
        public string Option;
        public string Script;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}