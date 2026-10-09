using Database;
using SDE.Editor.Database;

namespace SDE.Databases.ItemPackages.Properties
{
    public class ItemPackageNameBinding : IBinding
    {
        public Database.Tuple Tuple { get; set; }

        public DbAttribute AttachedAttribute { get; set; }

        public override string ToString()
        {
            if (Tuple == null)
                return "";

            return DbUtilities.ItemId2Name(
                Tuple.GetKey<int>());
        }
    }
}