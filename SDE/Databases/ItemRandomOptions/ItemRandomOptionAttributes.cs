using Database;
using SDE.Databases.ItemRandomOptions.Features;
using SDE.Databases.ItemRandomOptions.Properties;

namespace SDE.Databases.ItemRandomOptions
{
    public sealed class ItemRandomOptionAttributes : DbAttribute
    {
        public static readonly AttributeList AttributeList =
            new AttributeList();

        public static readonly DbAttribute Id =
            new ItemRandomOptionAttributes(
                new PrimaryAttribute(
                    "Id",
                    typeof(int),
                    0,
                    "Id"));

        public static readonly DbAttribute Model =
            new ItemRandomOptionAttributes(
                new ModelAttribute(
                    typeof(ItemRandomOption)));

        public static readonly DbAttribute DisplayName2 =
            new ItemRandomOptionAttributes(
                new DbAttribute(
                    "Option",
                    typeof(ItemRandomOptionNameBinding),
                    null,
                    "Option"))
            {
                IsDisplayAttribute = true,
                Visibility = VisibleState.Hidden
            };

        private ItemRandomOptionAttributes(
            DbAttribute attribute)
            : base(attribute)
        {
            AttributeList.Add(this);
        }
    }
}