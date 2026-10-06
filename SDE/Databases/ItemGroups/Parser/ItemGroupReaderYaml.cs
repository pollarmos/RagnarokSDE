using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemGroups.Features;
using SDE.Editor.Database;
using SDE.Editor.Parsers;
using System;
using System.Linq;

namespace SDE.Databases.ItemGroups.Parser {
    public class ItemGroupReaderYaml : DatabaseReaderYaml<int>  {
        public override string KeyField => "Group";

        public override void ReadEntry(DbLoadContext context, ParserObject itemGroup) {
            var table = context.AbsractDb.Table;

            ItemGroup model = new ItemGroup();

            foreach (var entry in itemGroup.OfType<ParserKeyValue>()) {
                switch (entry.Key) {
                    case "Group":
                        model.Group = entry.ObjectValue;
                        break;

                    case "SubGroups":
                        foreach (var subGroupObject in entry.Value) {
                            ItemGroupSubGroup subGroup = new ItemGroupSubGroup();

                            foreach (var subEntry in subGroupObject.OfType<ParserKeyValue>()) {
                                switch (subEntry.Key) {
                                    case "SubGroup":
                                        subGroup.SubGroup = Int32.Parse(subEntry.ObjectValue);
                                        break;

                                    case "Algorithm":
                                        subGroup.Algorithm = subEntry.ObjectValue;
                                        break;

                                    case "Clear":
                                        subGroup.Clear = Boolean.Parse(subEntry.ObjectValue);
                                        break;

                                    case "List":
                                        foreach (var listObject in subEntry.Value) {
                                            ItemGroupEntry groupEntry = new ItemGroupEntry();

                                            foreach (var listEntry in listObject.OfType<ParserKeyValue>()) {
                                                switch (listEntry.Key) {
                                                    case "Index":
                                                        groupEntry.Index = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Item":
                                                        groupEntry.Item = listEntry.ObjectValue;
                                                        break;

                                                    case "Rate":
                                                        groupEntry.Rate = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Amount":
                                                        groupEntry.Amount = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Duration":
                                                        groupEntry.Duration = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Announced":
                                                        groupEntry.Announced = Boolean.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "UniqueId":
                                                        groupEntry.UniqueId = Boolean.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Stacked":
                                                        groupEntry.Stacked = Boolean.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Named":
                                                        groupEntry.Named = Boolean.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "Bound":
                                                        groupEntry.Bound = listEntry.ObjectValue;
                                                        break;

                                                    case "RandomOptionGroup":
                                                        groupEntry.RandomOptionGroup = listEntry.ObjectValue;
                                                        break;

                                                    case "RefineMinimum":
                                                        groupEntry.RefineMinimum = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "RefineMaximum":
                                                        groupEntry.RefineMaximum = Int32.Parse(listEntry.ObjectValue);
                                                        break;

                                                    case "GradeMinimum":
                                                        groupEntry.GradeMinimum = listEntry.ObjectValue;
                                                        break;

                                                    case "GradeMaximum":
                                                        groupEntry.GradeMaximum = listEntry.ObjectValue;
                                                        break;

                                                    case "Clear":
                                                        groupEntry.Clear = Boolean.Parse(listEntry.ObjectValue);
                                                        break;
                                                }
                                            }
                                            subGroup.List.Add(groupEntry);
                                        }
                                        break;
                                }
                            }
                            model.SubGroups.Add(subGroup);
                        }
                        break;
                }
            }

            int uid = table.GenerateUniqueId();

            table.SetRaw(uid, ItemGroupAttributes.Model, model);
            table.SetRaw(uid, ItemGroupAttributes.FileKeyRef, model.Group);
        }
    }
}
