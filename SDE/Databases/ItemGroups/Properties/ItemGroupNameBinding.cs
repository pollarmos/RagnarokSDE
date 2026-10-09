using Database;
using SDE.Databases.ItemGroups.Features;
using SDE.Databases.Items.Features;
using SDE.Editor.Database;
using SDE.View;
using System;
using System.Linq;

namespace SDE.Databases.ItemGroups.Properties {
    public class ItemGroupNameBinding : IBinding {
        #region IBinding Members
        public Database.Tuple Tuple { get; set; }
        public DbAttribute AttachedAttribute { get; set; }
        #endregion

        public override string ToString() {
            var model = Tuple.GetModel<ItemGroup>();

            if (model == null || string.IsNullOrEmpty(model.Group))
                return "";

            object itemId = CachedDbs.AegisNameItem.ToId(model.Group);

            if (itemId is int id)
                return DbUtilities.ItemId2Name(id);

            return model.Group;
        }
    }
}
