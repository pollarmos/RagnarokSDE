using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemRandomOptions.Features;
using SDE.Editor.Database;
using SDE.Editor.Parsers;
using System;
using System.Linq;

namespace SDE.Databases.ItemRandomOptions.Parser
{
    public class ItemRandomOptionReaderYaml
        : DatabaseReaderYaml<int>
    {
        public override string KeyField => "Id";

        public override void ReadEntry(
            DbLoadContext context,
            ParserObject item)
        {
            ParserObject idObject = item[KeyField];

            if (idObject == null)
                throw new Exception("Missing Id field.");

            int id = Int32.Parse(
                idObject.ObjectValue);

            ItemRandomOption model =
                new ItemRandomOption();

            foreach (var entry
                in item.OfType<ParserKeyValue>())
            {
                switch (entry.Key)
                {
                    case "Id":
                        break;

                    case "Option":
                        model.Option =
                            entry.ObjectValue;
                        break;

                    case "Script":
                        model.Script =
                            entry.ObjectValue;
                        break;
                }
            }

            context.AbsractDb.Table.SetRaw(
                id,
                ItemRandomOptionAttributes.Model,
                model);
        }
    }
}