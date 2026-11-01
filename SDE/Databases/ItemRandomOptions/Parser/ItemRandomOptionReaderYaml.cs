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

            var table = context.AbsractDb.Table;
            table.EnsureExists(id);
            var tuple = table.GetTuple(id);
            var model = tuple.GetModel<ItemRandomOption>();
            var previousModel = model;

            if (table.EnableEvents)
                model = (ItemRandomOption)model.Clone();

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

            if (table.EnableEvents && previousModel != null)
                table.Commands.Set(tuple, ItemRandomOptionAttributes.Model, model, false);
        }
    }
}