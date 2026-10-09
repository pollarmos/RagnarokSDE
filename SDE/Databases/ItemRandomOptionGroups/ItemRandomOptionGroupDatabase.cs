using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Databases.ItemRandomOptionGroups.Parser;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using SDE.Databases.Generic.TabCommands;
using System;
using System.Windows;

namespace SDE.Databases.ItemRandomOptionGroups
{
    public class ItemRandomOptionGroupDatabase : ModelDatabase
    {
        public ItemRandomOptionGroupDatabase() : base(ItemRandomOptionGroupAttributes.Model)
        {
            Source = DataSources.ItemRandomOptionGroup;

            AttributeList = ItemRandomOptionGroupAttributes.AttributeList;

            Parser = new ItemRandomOptionGroupParserProvider();

            TabGenerator.OnInitSettings += (tab, settings, db) =>
                {
                    settings.AttIdWidth = 70;
                    settings.AttDisplayWrap = TextWrapping.NoWrap;
                    settings.AttDisplay = ItemRandomOptionGroupAttributes.DisplayName2;
                };

            TabGenerator.OnSetCustomCommands = delegate (DbTab tab, TabSettings settings, BaseDatabase db)
                {
                    settings.AddCommand(TabCommandAnchors.CopyTo, new CopyToImportTable(this));
                };
        }

        public override FrameworkElement OnCreateTab(FileType format, DbTab tab, TabSettings settings, BaseDatabase db)
        {
            switch (format)
            {
                case FileType.Yaml:
                    return new ItemRandomOptionGroupViewYaml();

                default:
                    throw new Exception(
                        $"Unknown table format for '{Source}'. " +
                        $"File type received: {format}.");
            }
        }
    }

    public class ItemRandomOptionGroupDatabaseImport : ItemRandomOptionGroupDatabase
    {
        public ItemRandomOptionGroupDatabaseImport()
        {
            Source = DataSources.ItemRandomOptionGroupImport;

            ThrowFileNotFoundException = false;
        }
    }
}