using Database;
using SDE.Databases.ItemRandomOptions.Features;

namespace SDE.Databases.ItemRandomOptions.Properties
{
    public class ItemRandomOptionNameBinding : IBinding
    {
        public Database.Tuple Tuple { get; set; }

        public DbAttribute AttachedAttribute { get; set; }

        public override string ToString()
        {
            if (Tuple == null)
                return "";

            ItemRandomOption model =
                Tuple.GetModel<ItemRandomOption>();

            if (model == null)
                return "";

            return model.Option ?? "";
        }
    }
}