using Database.Commands;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Collections.Generic;
using System.Linq;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackageViewModel : BaseModelView<ItemPackage>
    {
        public RangeObservableCollection<ItemPackageGroupViewModel> Groups { get; }
            = new RangeObservableCollection<ItemPackageGroupViewModel>();

        private ItemPackageGroupViewModel _selectedGroup;

        public bool IsLocked { get; set; }

        public ItemPackageViewModel(DbTab tab)
        {
            Tab = tab;
        }

        public void SetModel(ReadableTuple tuple, ItemPackage model)
        {
            if (IsLocked)
                return;

            Tuple = tuple;
            Model = model;

            _selectedGroup = null;

            OnGroupsListUpdated();
            OnPropertyChanged("");
        }

        public ItemPackageGroupViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup == value)
                    return;

                _selectedGroup = value;

                OnPropertyChanged(nameof(SelectedGroup));
                OnPropertyChanged(nameof(IsGroupSelected));
            }
        }

        public bool IsGroupSelected
        {
            get => _selectedGroup != null;
        }

        public void ChangeGroups(
            List<ItemPackageGroup> groups,
            ListCommandMode mode)
        {
            if (groups == null ||
                groups.Count == 0 ||
                Model == null ||
                Tuple == null)
                return;

            try
            {
                IsLocked = true;

                Tab.Table.Commands.SetModelListValue(
                    Tuple,
                    () => Model.Groups,
                    groups,
                    mode);

                _selectedGroup = null;
                OnGroupsListUpdated();
            }
            finally
            {
                IsLocked = false;
            }

            OnPropertyChanged(nameof(SelectedGroup));
            OnPropertyChanged(nameof(IsGroupSelected));
        }

        public void OnGroupsListUpdated()
        {
            if (Model == null)
            {
                Groups.ClearAndAddRange(
                    new List<ItemPackageGroupViewModel>());

                SelectedGroup = null;
                return;
            }

            Groups.ClearAndAddRange(
                Model.Groups.Select(
                    p => new ItemPackageGroupViewModel(this, p)));

            SelectedGroup = Groups.FirstOrDefault();
        }
    }
}