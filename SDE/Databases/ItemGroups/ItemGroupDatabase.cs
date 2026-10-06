using SDE.Databases.ItemGroups.Features;
using SDE.Databases.ItemGroups.Parser;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using SDE.Databases.Generic.TabCommands;
using System;
using System.Windows;

namespace SDE.Databases.ItemGroups
{
    public class ItemGroupDatabase : ModelDatabase
    {
        public ItemGroupDatabase() : base(ItemGroupAttributes.Model)
        {
            Source = DataSources.ItemGroup;
            AttributeList = ItemGroupAttributes.AttributeList;
            Parser = new ItemGroupParserProvider();

            TabGenerator.OnInitSettings += (tab, settings, db) => {
                settings.AttIdWidth = 80;
                settings.AttDisplayWrap = TextWrapping.NoWrap;
                settings.AttDisplay = ItemGroupAttributes.DisplayName2;
            };

            TabGenerator.OnSetCustomCommands = delegate (DbTab tab, TabSettings settings, BaseDatabase db) {
                settings.HasUniqueId = true;
                settings.RemoveCommand(TabCommandAnchors.ChangeId);

                settings.AddCommand(
                    TabCommandAnchors.CopyTo,
                    new CopyToImportTable(this)
                );
            };

            UseUniqueId = true;
        }

        public override FrameworkElement OnCreateTab(
            FileType format,
            DbTab tab,
            TabSettings settings,
            BaseDatabase db)
        {

            switch (format)
            {
                case FileType.Yaml:
                    return new ItemGroupViewYaml();

                default:
                    throw new Exception(
                        $"Unknown table format for '{Source}'. File type received: {format}.");
            }
        }
    }

    public class ItemGroupDatabaseImport : ItemGroupDatabase
    {
        public ItemGroupDatabaseImport() {
            Source = DataSources.ItemGroupImport;
            ThrowFileNotFoundException = false;
        }
    }
}