using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Editor.Database;
using System;
using System.Text;

namespace SDE.Databases.ItemRandomOptionGroups.Parser
{
    public class ItemRandomOptionGroupWriterYaml
        : DatabaseWriterYaml
    {
        public override string KeyField => "Id";

        public override void WriteEntry(
            StringBuilder builder,
            ReadableTuple tuple)
        {
            if (tuple == null)
                return;

            ItemRandomOptionGroup model =
                tuple.GetModel<ItemRandomOptionGroup>();

            if (model == null)
                return;

            builder.AppendLine(
                "  - Id: " + tuple.GetKey<int>());

            if (!String.IsNullOrEmpty(model.Group))
            {
                builder.AppendLine(
                    "    Group: " + model.Group);
            }

            WriteSlots(
                builder,
                model);

            if (model.MaxRandom.HasValue)
            {
                builder.AppendLine(
                    "    MaxRandom: " +
                    model.MaxRandom.Value);
            }

            WriteRandom(
                builder,
                model);
        }

        private void WriteSlots(
            StringBuilder builder,
            ItemRandomOptionGroup model)
        {
            if (model.Slots == null ||
                model.Slots.Count == 0)
                return;

            builder.AppendLine(
                "    Slots:");

            foreach (ItemRandomOptionGroupSlot slot
                in model.Slots)
            {
                builder.AppendLine(
                    "      - Slot: " +
                    slot.Slot);

                if (slot.Options == null ||
                    slot.Options.Count == 0)
                    continue;

                builder.AppendLine(
                    "        Options:");

                foreach (ItemRandomOptionGroupOption option
                    in slot.Options)
                {
                    WriteOption(
                        builder,
                        option,
                        "          ");
                }
            }
        }

        private void WriteRandom(
            StringBuilder builder,
            ItemRandomOptionGroup model)
        {
            if (model.Random == null ||
                model.Random.Count == 0)
                return;

            builder.AppendLine(
                "    Random:");

            foreach (ItemRandomOptionGroupOption option
                in model.Random)
            {
                WriteOption(
                    builder,
                    option,
                    "      ");
            }
        }

        private void WriteOption(
            StringBuilder builder,
            ItemRandomOptionGroupOption option,
            string indent)
        {
            if (option == null)
                return;

            builder.AppendLine(
                indent +
                "- Option: " +
                (option.Option ?? ""));

            if (option.MinValue.HasValue)
            {
                builder.AppendLine(
                    indent +
                    "  MinValue: " +
                    option.MinValue.Value);
            }

            if (option.MaxValue.HasValue)
            {
                builder.AppendLine(
                    indent +
                    "  MaxValue: " +
                    option.MaxValue.Value);
            }

            if (option.Param.HasValue)
            {
                builder.AppendLine(
                    indent +
                    "  Param: " +
                    option.Param.Value);
            }

            if (option.Chance.HasValue)
            {
                builder.AppendLine(
                    indent +
                    "  Chance: " +
                    option.Chance.Value);
            }
        }
    }
}