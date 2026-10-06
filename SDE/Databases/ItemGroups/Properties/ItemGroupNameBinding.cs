using Database;
using SDE.Databases.ItemGroups.Features;
using SDE.Editor.Database;

namespace SDE.Databases.ItemGroups.Properties {
    public class ItemGroupNameBinding : IBinding {
        #region IBinding Members
        public Database.Tuple Tuple { get; set; }
        public DbAttribute AttachedAttribute { get; set; }
        #endregion

        public override string ToString() {
            var model = Tuple.GetModel<ItemGroup>();

            if (model == null)
                return "";

            return model.Group ?? "";
        }
    }
}
