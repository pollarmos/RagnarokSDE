using Database;
using SDE.Databases.ItemGroups.Features;
using SDE.Databases.ItemGroups.Properties;

namespace SDE.Databases.ItemGroups {
    public sealed class ItemGroupAttributes : DbAttribute {
        public static readonly AttributeList AttributeList = new AttributeList();

        public static readonly DbAttribute Id = new ItemGroupAttributes(new PrimaryAttribute("Id", typeof(int), 0, "Id"));

        public static readonly DbAttribute Model = new ItemGroupAttributes(new ModelAttribute(typeof(ItemGroup)));

        public static readonly DbAttribute DisplayName2 = new ItemGroupAttributes(new DbAttribute("Name", typeof(ItemGroupNameBinding), null, "Name")) {IsDisplayAttribute = true, Visibility = VisibleState.Hidden};

        public static readonly DbAttribute FileKeyRef = new ItemGroupAttributes(new DbAttribute("FileKeyRef", typeof(string), null, "FileKeyRef")) { Visibility = VisibleState.Hidden };

        private ItemGroupAttributes(DbAttribute attribute) : base(attribute) {
            AttributeList.Add(this);
        }
    }
}
