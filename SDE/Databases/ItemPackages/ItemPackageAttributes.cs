using Database;
using SDE.Databases.ItemPackages.Features;
using SDE.Databases.ItemPackages.Properties;

namespace SDE.Databases.ItemPackages
{
    public sealed class ItemPackageAttributes : DbAttribute
    {
        public static readonly AttributeList AttributeList =
            new AttributeList();

        public static readonly DbAttribute Id =
            new ItemPackageAttributes(
                new PrimaryAttribute(
                    "Id",
                    typeof(int),
                    0,
                    "Id"));

        public static readonly DbAttribute Model =
            new ItemPackageAttributes(
                new ModelAttribute(
                    typeof(ItemPackage)));

        public static readonly DbAttribute DisplayName2 =
            new ItemPackageAttributes(
                new DbAttribute(
                    "Name",
                    typeof(ItemPackageNameBinding),
                    null,
                    "Name"))
            {
                IsDisplayAttribute = true,
                Visibility = VisibleState.Hidden
            };

        private ItemPackageAttributes(DbAttribute attribute)
            : base(attribute)
        {
            AttributeList.Add(this);
        }
    }
}