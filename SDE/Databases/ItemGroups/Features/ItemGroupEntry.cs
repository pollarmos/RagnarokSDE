using System;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroupEntry : ICloneable {
        public int Index;
        public string Item = "";

        public int? Rate;
        public int? Amount;
        public int? Duration;

        public bool? Announced;
        public bool? UniqueId;
        public bool? Stacked;
        public bool? Named;

        public string Bound;
        public string RandomOptionGroup;

        public int? RefineMinimum;
        public int? RefineMaximum;

        public string GradeMinimum;
        public string GradeMaximum;

        public bool? Clear;

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
