using Database;
using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Databases.ItemRandomOptionGroups.Properties;

namespace SDE.Databases.ItemRandomOptionGroups
{
    public sealed class ItemRandomOptionGroupAttributes
        : DbAttribute
    {
        public static readonly AttributeList AttributeList =
            new AttributeList();

        public static readonly DbAttribute Id =
            new ItemRandomOptionGroupAttributes(
                new PrimaryAttribute(
                    "Id",
                    typeof(int),
                    0,
                    "Id"));

        public static readonly DbAttribute Model =
            new ItemRandomOptionGroupAttributes(
                new ModelAttribute(
                    typeof(ItemRandomOptionGroup)));

        public static readonly DbAttribute DisplayName2 =
            new ItemRandomOptionGroupAttributes(
                new DbAttribute(
                    "Group",
                    typeof(ItemRandomOptionGroupNameBinding),
                    null,
                    "Group"))
            {
                IsDisplayAttribute = true,
                Visibility = VisibleState.Hidden
            };

        private ItemRandomOptionGroupAttributes(
            DbAttribute attribute)
            : base(attribute)
        {
            AttributeList.Add(this);
        }
    }
}