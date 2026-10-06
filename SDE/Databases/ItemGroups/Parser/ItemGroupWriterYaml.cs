using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemGroups.Features;
using SDE.Editor.Database;
using SDE.Editor.Files;
using SDE.Editor.Parsers;
using SDE.Editor.Parsers.Libconfig;
using SDE.Editor.Parsers.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDE.Databases.ItemGroups.Parser {
    public class ItemGroupWriterYaml : DatabaseWriterYaml {
        public override string KeyField => "Group";

        public override void Writer(DbSaveContext context, BaseDatabase db) {
            base.Writer(context, db);

            try {
                YamlParser parser = new YamlParser(context.OldPath, ParserMode.Write, KeyField);

                List<string> allLines = parser.AllLines;

                Dictionary<string, ParserObject> entries = new Dictionary<string, ParserObject>(StringComparer.OrdinalIgnoreCase);

                ParserObject body = parser.Output?["Body"];

                if (body != null) {
                    foreach (ParserObject entry in body) {
                        ParserObject groupEntry = entry["Group"];

                        if (groupEntry == null)
                            continue;

                        string group = groupEntry.ObjectValue;

                        if (String.IsNullOrEmpty(group))
                            continue;

                        if (!entries.ContainsKey(group))
                            entries[group] = entry;
                    }
                }

                HashSet<string> currentOriginalGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (ReadableTuple tuple in db.Table.FastItems) {
                    string fileKey = tuple.GetValue<string>(ItemGroupAttributes.FileKeyRef);

                    if (!String.IsNullOrEmpty(fileKey))
                        currentOriginalGroups.Add(fileKey);
                }

                // 삭제된 Group 제거
                foreach (var entry in entries) {
                    if (!currentOriginalGroups.Contains(entry.Key))
                        ClearLines(entry.Value, allLines);
                }

                List<string> entriesToAdd = new List<string>();

                // 추가 또는 수정된 Group 처리
                foreach (ReadableTuple tuple in db.Table.FastItems.Where(p => !p.Normal).OrderBy(p => p.GetKey<int>())) {

                    ItemGroup model = tuple.GetModel<ItemGroup>();

                    if (model == null)
                        continue;

                    StringBuilder entryBuilder = new StringBuilder();
                    WriteItemGroup(entryBuilder, model);

                    string entryData = entryBuilder.ToString().Trim('\r', '\n');

                    string oldGroup = tuple.GetValue<string>(
                        ItemGroupAttributes.FileKeyRef);

                    if (!String.IsNullOrEmpty(oldGroup) && entries.TryGetValue(oldGroup, out ParserObject parserEntry)) {
                        ReplaceLines(parserEntry, allLines, entryData);
                    }
                    else {
                        entriesToAdd.Add(entryData);
                    }
                }

                if (entriesToAdd.Count > 0) {
                    ParserObject footer = parser.Output?["Footer"];

                    int insertIndex = footer != null ? footer.Line - 1 : allLines.Count;

                    foreach (string entry in entriesToAdd) {
                        allLines.Insert(insertIndex, entry);
                        insertIndex++;
                    }
                }

                if (parser.Output?["Header"] == null) {
                    while (allLines.Count > 0 && String.IsNullOrWhiteSpace(allLines[0])) {
                        allLines.RemoveAt(0);
                    }

                    allLines.Insert(0, "Header:\r\n" + "  Type: ITEM_GROUP_DB\r\n" + "  Version: 5");
                    allLines.Insert(1, "");
                }

                StringBuilder builder = new StringBuilder();

                foreach (string line in allLines)
                {
                    if (line != null)
                        builder.AppendLine(line);
                }

                IOHelper.WriteAllText(context.FilePath, builder.ToString());

                foreach (ReadableTuple tuple in db.Table.FastItems) {
                    ItemGroup model = tuple.GetModel<ItemGroup>();

                    if (model == null)
                        continue;

                    tuple.SetRawValue(ItemGroupAttributes.FileKeyRef, model.Group);
                }
            }
            catch (Exception err) {
                context.ReportException(err);
            }
        }

        private void ClearLines(ParserObject parserObject, List<string> allLines) {
            for (int i = 0; i < parserObject.Length; i++) {
                int index = parserObject.Line - 1 + i;

                if (index >= 0 && index < allLines.Count)
                    allLines[index] = null;
            }
        }

        private void ReplaceLines(ParserObject parserObject, List<string> allLines, string content) {
            ClearLines(parserObject, allLines);

            int index = parserObject.Line - 1;

            if (index >= 0 && index < allLines.Count)
                allLines[index] = content;
        }

        public override void WriteEntry(StringBuilder builder, ReadableTuple tuple) {
            if (tuple == null)
                return;

            ItemGroup model = tuple.GetModel<ItemGroup>();

            if (model == null)
                return;

            WriteItemGroup(builder, model);
        }

        public void WriteItemGroup(StringBuilder builder, ItemGroup itemGroup) {

            builder.AppendLine("  - Group: " + itemGroup.Group);

            if (itemGroup.SubGroups == null || itemGroup.SubGroups.Count == 0) {
                return;
            }

            builder.AppendLine("    SubGroups:");

            foreach (ItemGroupSubGroup subGroup in itemGroup.SubGroups) {
                builder.AppendLine("      - SubGroup: " + subGroup.SubGroup);

                if (!String.IsNullOrEmpty(subGroup.Algorithm)) {
                    builder.AppendLine("        Algorithm: " + subGroup.Algorithm);
                }

                if (subGroup.List != null && subGroup.List.Count > 0) {
                    builder.AppendLine("        List:");

                    foreach (ItemGroupEntry entry in subGroup.List) {
                        WriteItemGroupEntry(builder, entry);
                    }
                }

                if (subGroup.Clear.HasValue) {
                    builder.AppendLine("        Clear: " + ToYamlBoolean(subGroup.Clear.Value));
                }
            }
        }

        private void WriteItemGroupEntry(StringBuilder builder, ItemGroupEntry entry) {

            builder.AppendLine("          - Index: " + entry.Index);

            builder.AppendLine("            Item: " + (entry.Item ?? ""));

            if (entry.Rate.HasValue) {
                builder.AppendLine("            Rate: " + entry.Rate.Value);
            }

            if (entry.Amount.HasValue) {
                builder.AppendLine("            Amount: " + entry.Amount.Value);
            }

            if (entry.Duration.HasValue) {
                builder.AppendLine("            Duration: " + entry.Duration.Value);
            }

            if (entry.Announced.HasValue) {
                builder.AppendLine("            Announced: " + ToYamlBoolean(entry.Announced.Value));
            }

            if (entry.UniqueId.HasValue) {
                builder.AppendLine("            UniqueId: " + ToYamlBoolean(entry.UniqueId.Value));
            }

            if (entry.Stacked.HasValue) {
                builder.AppendLine("            Stacked: " + ToYamlBoolean(entry.Stacked.Value));
            }

            if (entry.Named.HasValue) {
                builder.AppendLine("            Named: " + ToYamlBoolean(entry.Named.Value));
            }

            if (!String.IsNullOrEmpty(entry.Bound)) {
                builder.AppendLine("            Bound: " + entry.Bound);
            }

            if (!String.IsNullOrEmpty(entry.RandomOptionGroup)) {
                builder.AppendLine("            RandomOptionGroup: " +
                    entry.RandomOptionGroup);
            }

            if (entry.RefineMinimum.HasValue) {
                builder.AppendLine("            RefineMinimum: " + entry.RefineMinimum.Value);
            }

            if (entry.RefineMaximum.HasValue) {
                builder.AppendLine("            RefineMaximum: " + entry.RefineMaximum.Value);
            }

            if (!String.IsNullOrEmpty(entry.GradeMinimum)) {
                builder.AppendLine("            GradeMinimum: " + entry.GradeMinimum);
            }

            if (!String.IsNullOrEmpty(entry.GradeMaximum)) {
                builder.AppendLine("            GradeMaximum: " + entry.GradeMaximum);
            }

            if (entry.Clear.HasValue) {
                builder.AppendLine("            Clear: " + ToYamlBoolean(entry.Clear.Value));
            }
        }

        private string ToYamlBoolean(bool value)  {
            return value ? "true" : "false";
        }
    }
}