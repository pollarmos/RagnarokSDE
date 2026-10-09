using SDE.Databases.ItemRandomOptions.Features;
using SDE.Databases.ItemRandomOptions.Parser;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using SDE.Databases.Generic.TabCommands;
using System;
using System.Windows;

namespace SDE.Databases.ItemRandomOptions
{
    public class ItemRandomOptionDatabase
        : ModelDatabase
    {
        public ItemRandomOptionDatabase()
            : base(ItemRandomOptionAttributes.Model)
        {
            Source =
                DataSources.ItemRandomOption;

            AttributeList =
                ItemRandomOptionAttributes.AttributeList;

            Parser =
                new ItemRandomOptionParserProvider();

            TabGenerator.OnInitSettings +=
                (tab, settings, db) =>
                {
                    settings.AttIdWidth = 70;

                    settings.AttDisplayWrap =
                        TextWrapping.NoWrap;

                    settings.AttDisplay =
                        ItemRandomOptionAttributes.DisplayName2;
                };

            TabGenerator.OnSetCustomCommands = delegate (DbTab tab, TabSettings settings, BaseDatabase db)
               {
                   settings.AddCommand(TabCommandAnchors.CopyTo, new CopyToImportTable(this));
               };
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
                    return new ItemRandomOptionViewYaml();

                default:
                    throw new Exception(
                        $"Unknown table format for '{Source}'. " +
                        $"File type received: {format}.");
            }
        }
    }

    public class ItemRandomOptionDatabaseImport
        : ItemRandomOptionDatabase
    {
        public ItemRandomOptionDatabaseImport()
        {
            Source =
                DataSources.ItemRandomOptionImport;

            ThrowFileNotFoundException = false;
        }
    }
}