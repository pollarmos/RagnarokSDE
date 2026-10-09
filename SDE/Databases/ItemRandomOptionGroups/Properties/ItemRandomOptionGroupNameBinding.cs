using Database;
using SDE.Databases.ItemRandomOptionGroups.Features;

namespace SDE.Databases.ItemRandomOptionGroups.Properties
{
    public class ItemRandomOptionGroupNameBinding
        : IBinding
    {
        public Database.Tuple Tuple { get; set; }

        public DbAttribute AttachedAttribute { get; set; }

        public override string ToString()
        {
            if (Tuple == null)
                return "";

            ItemRandomOptionGroup model =
                Tuple.GetModel<ItemRandomOptionGroup>();

            if (model == null)
                return "";

            return model.Group ?? "";
        }
    }
}