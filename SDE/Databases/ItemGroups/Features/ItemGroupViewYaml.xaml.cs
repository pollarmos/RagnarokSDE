using Database.Commands;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SDE.Databases.ItemGroups.Features {
    public partial class ItemGroupViewYaml : UserControl, IDatabaseView {
        private ItemGroupViewModel _viewModel;
        private DbTab _tab;

        public ItemGroupViewYaml() {
            InitializeComponent();
        }

        public void Init(DbTab tab) {
            _tab = tab;

            tab.UpdateAction = _updateAction;

            _viewModel = new ItemGroupViewModel(tab);
            _viewModel.SetModel(null, null);

            DataContext = _viewModel;

            if (tab.SelectedItem != null)
                _updateAction(tab.SelectedItem);
        }

        private void _updateAction(ReadableTuple tuple) {
            if (_viewModel.IsLocked)
                return;

            if (tuple != null)  {
                _viewModel.SetModel(tuple, tuple.GetModel<ItemGroup>());
            }
            else {
                _viewModel.SetModel(null, null);
            }
        }

        private void _buttonAddSubGroup_Click(object sender, RoutedEventArgs e) {
            if (_viewModel.Model == null)
                return;

            int subGroupId = 0;

            while (_viewModel.Model.SubGroups.Any(p => p.SubGroup == subGroupId))
                subGroupId++;

            ItemGroupSubGroup subGroup = new ItemGroupSubGroup { SubGroup = subGroupId, Algorithm = "SharedPool" };

            _viewModel.ChangeSubGroups(new List<ItemGroupSubGroup> { subGroup }, ListCommandMode.Add);
        }

        private void _buttonDeleteSubGroup_Click(object sender, RoutedEventArgs e) {
            if (_viewModel.SelectedSubGroup == null)
                return;

            _viewModel.ChangeSubGroups(new List<ItemGroupSubGroup> { _viewModel.SelectedSubGroup.Model }, ListCommandMode.Remove);
        }

        private void _buttonAddEntry_Click(object sender, RoutedEventArgs e) {
            ItemGroupSubGroupViewModel subGroup = _viewModel.SelectedSubGroup;

            if (subGroup == null || subGroup.Model == null)
                return;

            int index = 0;

            while (subGroup.Model.List.Any(p => p.Index == index))
                index++;

            ItemGroupEntry entry = new ItemGroupEntry {
                Index = index,
                Item = ""
            };

            subGroup.ChangeEntries(new List<ItemGroupEntry> { entry }, ListCommandMode.Add);
        }

        private void _buttonDeleteEntry_Click(object sender, RoutedEventArgs e) {
            ItemGroupSubGroupViewModel subGroup = _viewModel.SelectedSubGroup;

            if (subGroup == null || subGroup.SelectedEntry == null)
                return;

            subGroup.ChangeEntries(new List<ItemGroupEntry> { subGroup.SelectedEntry.Model }, ListCommandMode.Remove);
        }
    }
}