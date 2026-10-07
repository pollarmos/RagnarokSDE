using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemPackages.Features;
using SDE.Editor.Database;
using SDE.Editor.Parsers;
using System;
using System.Linq;

namespace SDE.Databases.ItemPackages.Parser
{
    public class ItemPackageReaderYaml : DatabaseReaderYaml<int>
    {
        public override string KeyField => "Item";

        public override void ReadEntry(
            DbLoadContext context,
            ParserObject itemPackage)
        {
            var table = context.AbsractDb.Table;

            ParserObject itemObject = itemPackage[KeyField];

            if (itemObject == null)
                throw new Exception("Missing Item field.");

            int id = CachedDbs.AegisNameItem.ToIntId(
                itemObject.ObjectValue);

            ItemPackage model = new ItemPackage();

            foreach (var entry in itemPackage.OfType<ParserKeyValue>())
            {
                switch (entry.Key)
                {
                    case "Item":
                        // 최상위 Item은 Tuple Key로 사용하므로
                        // 모델에는 별도로 저장하지 않는다.
                        break;

                    case "Groups":
                        foreach (var groupObject in entry.Value)
                        {
                            ItemPackageGroup group =
                                new ItemPackageGroup();

                            foreach (var groupEntry
                                in groupObject.OfType<ParserKeyValue>())
                            {
                                switch (groupEntry.Key)
                                {
                                    case "Group":
                                        group.Group =
                                            Int32.Parse(
                                                groupEntry.ObjectValue);
                                        break;

                                    case "Items":
                                        foreach (var itemEntryObject
                                            in groupEntry.Value)
                                        {
                                            ItemPackageEntry packageEntry =
                                                new ItemPackageEntry();

                                            foreach (var itemEntry
                                                in itemEntryObject
                                                    .OfType<ParserKeyValue>())
                                            {
                                                switch (itemEntry.Key)
                                                {
                                                    case "Item":
                                                        packageEntry.Item =
                                                            CachedDbs
                                                                .AegisNameItem
                                                                .ToStringId(
                                                                    itemEntry.ObjectValue);
                                                        break;

                                                    case "Amount":
                                                        packageEntry.Amount =
                                                            Int32.Parse(
                                                                itemEntry.ObjectValue);
                                                        break;

                                                    case "RentalHours":
                                                        packageEntry.RentalHours =
                                                            Int32.Parse(
                                                                itemEntry.ObjectValue);
                                                        break;

                                                    case "Refine":
                                                        packageEntry.Refine =
                                                            Int32.Parse(
                                                                itemEntry.ObjectValue);
                                                        break;

                                                    case "Grade":
                                                        packageEntry.Grade =
                                                            itemEntry.ObjectValue;
                                                        break;

                                                    case "RandomOptionGroup":
                                                        packageEntry.RandomOptionGroup =
                                                            itemEntry.ObjectValue;
                                                        break;
                                                }
                                            }

                                            group.Items.Add(packageEntry);
                                        }
                                        break;
                                }
                            }

                            model.Groups.Add(group);
                        }
                        break;
                }
            }

            table.SetRaw(
                id,
                ItemPackageAttributes.Model,
                model);
        }
    }
}