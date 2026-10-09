using SDE.Databases.Generic.Common;
using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemRandomOptions.Features;
using SDE.Editor.Database;
using System;
using System.Text;

namespace SDE.Databases.ItemRandomOptions.Parser
{
    public class ItemRandomOptionWriterYaml
        : DatabaseWriterYaml
    {
        public override string KeyField => "Id";

        public override void WriteEntry(
            StringBuilder builder,
            ReadableTuple tuple)
        {
            if (tuple == null)
                return;

            ItemRandomOption model =
                tuple.GetModel<ItemRandomOption>();

            if (model == null)
                return;

            builder.AppendLine(
                "  - Id: " + tuple.Key);

            if (!String.IsNullOrEmpty(
                    model.Option))
            {
                builder.AppendLine(
                    "    Option: " +
                    model.Option);
            }

            if (!String.IsNullOrEmpty(
                    model.Script))
            {
                builder.AppendLine(
                    "    Script: |");

                builder.AppendLine(
                    DbWriter.ToYamlScript(
                        model.Script,
                        "      "));
            }
        }
    }
}