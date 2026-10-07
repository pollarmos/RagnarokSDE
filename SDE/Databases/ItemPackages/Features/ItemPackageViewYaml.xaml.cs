using Database.Commands;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SDE.Databases.ItemPackages.Features
{
    public partial class ItemPackageViewYaml :
        UserControl, IDatabaseView
    {
        private ItemPackageViewModel _viewModel;
        private DbTab _tab;

        public ItemPackageViewYaml()
        {
            InitializeComponent();
        }

        public void Init(DbTab tab)
        {
            _tab = tab;

            tab.UpdateAction = _updateAction;

            _viewModel = new ItemPackageViewModel(tab);
            _viewModel.SetModel(null, null);

            DataContext = _viewModel;

            if (tab.SelectedItem != null)
                _updateAction(tab.SelectedItem);
        }

        private void _updateAction(ReadableTuple tuple)
        {
            if (_viewModel.IsLocked)
                return;

            if (tuple != null)
            {
                _viewModel.SetModel(
                    tuple,
                    tuple.GetModel<ItemPackage>());
            }
            else
            {
                _viewModel.SetModel(null, null);
            }
        }

        private void _buttonAddGroup_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_viewModel.Model == null)
                return;

            int groupId = 0;

            while (_viewModel.Model.Groups
                .Any(p => p.Group == groupId))
            {
                groupId++;
            }

            ItemPackageGroup group =
                new ItemPackageGroup
                {
                    Group = groupId
                };

            _viewModel.ChangeGroups(
                new List<ItemPackageGroup>
                {
                    group
                },
                ListCommandMode.Add);
        }

        private void _buttonDeleteGroup_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_viewModel.SelectedGroup == null)
                return;

            _viewModel.ChangeGroups(
                new List<ItemPackageGroup>
                {
                    _viewModel.SelectedGroup.Model
                },
                ListCommandMode.Remove);
        }

        private void _buttonAddItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            ItemPackageGroupViewModel group =
                _viewModel.SelectedGroup;

            if (group == null ||
                group.Model == null)
                return;

            ItemPackageEntry item =
                new ItemPackageEntry
                {
                    Item = ""
                };

            group.ChangeItems(
                new List<ItemPackageEntry>
                {
                    item
                },
                ListCommandMode.Add);
        }

        private void _buttonDeleteItem_Click(
            object sender,
            RoutedEventArgs e)
        {
            ItemPackageGroupViewModel group =
                _viewModel.SelectedGroup;

            if (group == null ||
                group.SelectedItem == null)
                return;

            group.ChangeItems(
                new List<ItemPackageEntry>
                {
                    group.SelectedItem.Model
                },
                ListCommandMode.Remove);
        }
    }
}