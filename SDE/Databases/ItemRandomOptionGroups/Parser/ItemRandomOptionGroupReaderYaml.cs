using SDE.Databases.Generic.Parser;
using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Editor.Database;
using SDE.Editor.Parsers;
using System;
using System.Linq;

namespace SDE.Databases.ItemRandomOptionGroups.Parser
{
    public class ItemRandomOptionGroupReaderYaml
        : DatabaseReaderYaml<int>
    {
        public override string KeyField => "Id";

        public override void ReadEntry(
            DbLoadContext context,
            ParserObject item)
        {
            int id = Int32.Parse(item[KeyField]);

            var table = context.AbsractDb.Table;

            table.EnsureExists(id);

            var tuple = table.GetTuple(id);

            ItemRandomOptionGroup model =
                tuple.GetModel<ItemRandomOptionGroup>();

            ItemRandomOptionGroup previousModel = model;

            if (table.EnableEvents)
            {
                model =
                    (ItemRandomOptionGroup)model.Clone();
            }

            foreach (var entry
                in item.OfType<ParserKeyValue>())
            {
                switch (entry.Key)
                {
                    case "Group":
                        model.Group =
                            entry.ObjectValue;
                        break;

                    case "Slots":
                        model.Slots.Clear();

                        foreach (var slotObject
                            in entry.Value)
                        {
                            ItemRandomOptionGroupSlot slot =
                                new ItemRandomOptionGroupSlot();

                            foreach (var slotEntry
                                in slotObject.OfType<ParserKeyValue>())
                            {
                                switch (slotEntry.Key)
                                {
                                    case "Slot":
                                        slot.Slot =
                                            Int32.Parse(
                                                slotEntry.ObjectValue);
                                        break;

                                    case "Options":
                                        foreach (var optionObject
                                            in slotEntry.Value)
                                        {
                                            slot.Options.Add(
                                                ReadOption(
                                                    optionObject));
                                        }
                                        break;
                                }
                            }

                            model.Slots.Add(slot);
                        }
                        break;

                    case "MaxRandom":
                        model.MaxRandom =
                            Int32.Parse(
                                entry.ObjectValue);
                        break;

                    case "Random":
                        model.Random.Clear();

                        foreach (var optionObject
                            in entry.Value)
                        {
                            model.Random.Add(
                                ReadOption(
                                    optionObject));
                        }
                        break;
                }
            }

            if (table.EnableEvents &&
                previousModel != null)
            {
                table.Commands.Set(
                    tuple,
                    ItemRandomOptionGroupAttributes.Model,
                    model,
                    false);
            }
        }

        private static ItemRandomOptionGroupOption
            ReadOption(ParserObject optionObject)
        {
            ItemRandomOptionGroupOption option =
                new ItemRandomOptionGroupOption();

            foreach (var entry
                in optionObject.OfType<ParserKeyValue>())
            {
                switch (entry.Key)
                {
                    case "Option":
                        option.Option =
                            entry.ObjectValue;
                        break;

                    case "MinValue":
                        option.MinValue =
                            Int32.Parse(
                                entry.ObjectValue);
                        break;

                    case "MaxValue":
                        option.MaxValue =
                            Int32.Parse(
                                entry.ObjectValue);
                        break;

                    case "Param":
                        option.Param =
                            Int32.Parse(
                                entry.ObjectValue);
                        break;

                    case "Chance":
                        option.Chance =
                            Int32.Parse(
                                entry.ObjectValue);
                        break;
                }
            }

            return option;
        }
    }
}