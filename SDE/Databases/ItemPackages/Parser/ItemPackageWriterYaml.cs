using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemPackages.Features;
using SDE.Editor.Database;
using SDE.Editor.Files;
using SDE.Editor.Parsers;
using SDE.Editor.Parsers.Libconfig;
using SDE.Editor.Parsers.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDE.Databases.ItemPackages.Parser
{
    public class ItemPackageWriterYaml : DatabaseWriterYaml
    {
        public override string KeyField => "Item";

        public override string KeyToYamlKey(int key)
        {
            return DbUtilities.ItemId2AegisName(key, ItemDb);
        }

        public override void Writer(
            DbSaveContext context,
            BaseDatabase db)
        {
            //
            // 중요:
            // base.Writer(context, db)는 호출하지 않는다.
            //
            // item_packages.yml의 KeyField인 Item은 AegisName이므로
            // YamlParser.WriteFile()의 신규 엔트리 int.Parse 정렬과
            // 호환되지 않는다.
            //

            context.MetaMobDb = MobDb;
            context.MetaItemDb = ItemDb;

            try
            {
                YamlParser parser =
                    new YamlParser(
                        context.OldPath,
                        ParserMode.Write,
                        KeyField);

                if (parser.Output == null)
                    return;

                List<string> allLines =
                    parser.AllLines;

                //
                // 기존 파일의 Package들을 실제 Item ID 기준으로 매핑
                //
                Dictionary<int, ParserObject> entries =
                    new Dictionary<int, ParserObject>();

                ParserObject body =
                    parser.Output["Body"];

                if (body != null)
                {
                    foreach (ParserObject entry in body)
                    {
                        ParserObject itemEntry =
                            entry["Item"];

                        if (itemEntry == null)
                            continue;

                        string itemName =
                            itemEntry.ObjectValue;

                        if (String.IsNullOrEmpty(itemName))
                            continue;

                        string idString =
                            CachedDbs.AegisNameItem
                                .ToStringId(itemName);

                        if (!Int32.TryParse(
                                idString,
                                out int itemId))
                        {
                            continue;
                        }

                        if (!entries.ContainsKey(itemId))
                            entries[itemId] = entry;
                    }
                }

                //
                // 현재 SDE 테이블에 존재하는 Package ID
                //
                HashSet<int> currentIds =
                    new HashSet<int>(
                        db.Table.FastItems
                            .Select(p => p.Key));

                //
                // 파일에는 있지만 현재 Table에는 없는 Package 제거
                //
                foreach (var entry in entries)
                {
                    if (!currentIds.Contains(entry.Key))
                    {
                        ClearLines(
                            entry.Value,
                            allLines);
                    }
                }

                //
                // 기존 파일에 없던 Package는 나중에 추가
                //
                List<string> entriesToAdd =
                    new List<string>();

                //
                // 추가 또는 수정된 Package 처리
                //
                foreach (ReadableTuple tuple
                    in db.Table.FastItems
                        .Where(p => !p.Normal)
                        .OrderBy(p => p.Key))
                {
                    ItemPackage model =
                        tuple.GetModel<ItemPackage>();

                    if (model == null)
                        continue;

                    StringBuilder entryBuilder =
                        new StringBuilder();

                    WriteEntry(
                        entryBuilder,
                        tuple);

                    string entryData =
                        entryBuilder
                            .ToString()
                            .Trim('\r', '\n');

                    //
                    // 기존 Package 수정
                    //
                    if (entries.TryGetValue(
                            tuple.Key,
                            out ParserObject parserEntry))
                    {
                        ReplaceLines(
                            parserEntry,
                            allLines,
                            entryData);
                    }
                    //
                    // Import 복사 등 새 Package
                    //
                    else
                    {
                        entriesToAdd.Add(
                            entryData);
                    }
                }

                //
                // 신규 Package 추가
                //
                if (entriesToAdd.Count > 0)
                {
                    ParserObject footer =
                        parser.Output["Footer"];

                    int insertIndex =
                        footer != null
                            ? footer.Line - 1
                            : allLines.Count;

                    foreach (string entry
                        in entriesToAdd)
                    {
                        allLines.Insert(
                            insertIndex,
                            entry);

                        insertIndex++;
                    }
                }

                //
                // Header가 없는 새 파일에 대한 방어 처리
                //
                if (parser.Output["Header"] == null)
                {
                    while (allLines.Count > 0 &&
                           String.IsNullOrWhiteSpace(
                               allLines[0]))
                    {
                        allLines.RemoveAt(0);
                    }

                    allLines.Insert(
                        0,
                        "Header:\r\n" +
                        "  Type: ITEM_PACKAGE_DB\r\n" +
                        "  Version: 2");

                    allLines.Insert(1, "");
                }

                //
                // 실제 파일 저장
                //
                StringBuilder builder =
                    new StringBuilder();

                foreach (string line in allLines)
                {
                    if (line != null)
                        builder.AppendLine(line);
                }

                IOHelper.WriteAllText(
                    context.FilePath,
                    builder.ToString());
            }
            catch (Exception err)
            {
                context.ReportException(err);
            }
        }

        private void ClearLines(
            ParserObject parserObject,
            List<string> allLines)
        {
            for (int i = 0;
                 i < parserObject.Length;
                 i++)
            {
                int index =
                    parserObject.Line - 1 + i;

                if (index >= 0 &&
                    index < allLines.Count)
                {
                    allLines[index] = null;
                }
            }
        }

        private void ReplaceLines(
            ParserObject parserObject,
            List<string> allLines,
            string content)
        {
            ClearLines(
                parserObject,
                allLines);

            int index =
                parserObject.Line - 1;

            if (index >= 0 &&
                index < allLines.Count)
            {
                allLines[index] = content;
            }
        }

        public override void WriteEntry(
            StringBuilder builder,
            ReadableTuple tuple)
        {
            if (tuple == null)
                return;

            ItemPackage model =
                tuple.GetModel<ItemPackage>();

            if (model == null)
                return;

            builder.AppendLine(
                "  - Item: " +
                DbUtilities.ItemId2AegisName(
                    tuple.Key,
                    ItemDb));

            if (model.Groups == null ||
                model.Groups.Count == 0)
            {
                builder.AppendLine(
                    "    Groups: []");

                return;
            }

            builder.AppendLine(
                "    Groups:");

            foreach (ItemPackageGroup group
                in model.Groups)
            {
                builder.AppendLine(
                    "      - Group: " +
                    group.Group);

                if (group.Items == null ||
                    group.Items.Count == 0)
                {
                    builder.AppendLine(
                        "        Items: []");

                    continue;
                }

                builder.AppendLine(
                    "        Items:");

                foreach (ItemPackageEntry item
                    in group.Items)
                {
                    WriteItem(
                        builder,
                        item);
                }
            }
        }

        private void WriteItem(
            StringBuilder builder,
            ItemPackageEntry item)
        {
            if (item == null)
                return;

            builder.AppendLine(
                "          - Item: " +
                DbUtilities.ItemId2AegisName(
                    item.Item,
                    ItemDb));

            if (item.Amount.HasValue)
            {
                builder.AppendLine(
                    "            Amount: " +
                    item.Amount.Value);
            }

            if (item.RentalHours.HasValue)
            {
                builder.AppendLine(
                    "            RentalHours: " +
                    item.RentalHours.Value);
            }

            if (item.Refine.HasValue)
            {
                builder.AppendLine(
                    "            Refine: " +
                    item.Refine.Value);
            }

            if (!String.IsNullOrEmpty(
                    item.Grade))
            {
                builder.AppendLine(
                    "            Grade: " +
                    item.Grade);
            }

            if (!String.IsNullOrEmpty(
                    item.RandomOptionGroup))
            {
                builder.AppendLine(
                    "            RandomOptionGroup: " +
                    item.RandomOptionGroup);
            }
        }
    }
}