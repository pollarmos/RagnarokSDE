using SDE.Databases.ItemPackages.Parser;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using SDE.Databases.ItemPackages.Features;
using SDE.Databases.Generic.TabCommands;
using System;
using System.Windows;

namespace SDE.Databases.ItemPackages
{
    public class ItemPackageDatabase : ModelDatabase
    {
        public ItemPackageDatabase()
            : base(ItemPackageAttributes.Model)
        {
            Source = DataSources.ItemPackage;
            AttributeList = ItemPackageAttributes.AttributeList;
            Parser = new ItemPackageParserProvider();

            TabGenerator.OnInitSettings +=
                (tab, settings, db) =>
                {
                    settings.AttIdWidth = 80;
                    settings.AttDisplayWrap = TextWrapping.NoWrap;
                    settings.AttDisplay =
                        ItemPackageAttributes.DisplayName2;
                };
            TabGenerator.OnSetCustomCommands =
                delegate (DbTab tab, TabSettings settings, BaseDatabase db)
                {
                    settings.AddCommand(
                        TabCommandAnchors.CopyTo,
                        new CopyToImportTable(this)
                    );
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
                    return new ItemPackageViewYaml();

                default:
                    throw new Exception(
                        $"Unknown table format for '{Source}'. " +
                        $"File type received: {format}.");
            }
        }
    }

    public class ItemPackageDatabaseImport
        : ItemPackageDatabase
    {
        public ItemPackageDatabaseImport()
        {
            Source = DataSources.ItemPackageImport;
            ThrowFileNotFoundException = false;
        }
    }
}