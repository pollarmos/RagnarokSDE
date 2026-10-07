using Database.Commands;
using SDE.Databases.Generic.Features;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackageGroupViewModel :
        BaseModelView<ItemPackageGroup>
    {
        private readonly ItemPackageViewModel _vm;

        public RangeObservableCollection<ItemPackageEntryViewModel> Items { get; }
            = new RangeObservableCollection<ItemPackageEntryViewModel>();

        private ItemPackageEntryViewModel _selectedItem;

        public ItemPackageGroupViewModel(
            ItemPackageViewModel viewModel,
            ItemPackageGroup model)
        {
            Model = model;
            _vm = viewModel;

            OnItemsListUpdated();
        }

        public int Group
        {
            get => Model == null ? 0 : Model.Group;
            set => ExecuteCommand(value);
        }

        public ItemPackageEntryViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value)
                    return;

                _selectedItem = value;

                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));
            }
        }

        public bool IsItemSelected
        {
            get => _selectedItem != null;
        }

        public void ExecuteCommand<T>(
            T value,
            [CallerMemberName] string fieldName = "")
        {
            if (Model == null || _vm.Tuple == null)
                return;

            try
            {
                _vm.IsLocked = true;

                _vm.Tab.Table.Commands.SetModelValue(
                    _vm.Tuple,
                    Model,
                    fieldName,
                    value);
            }
            finally
            {
                _vm.IsLocked = false;
            }

            OnPropertyChanged(fieldName);
        }

        public void ChangeItems(
            List<ItemPackageEntry> items,
            ListCommandMode mode)
        {
            if (items == null ||
                items.Count == 0 ||
                Model == null ||
                _vm.Tuple == null)
                return;

            try
            {
                _vm.IsLocked = true;

                _vm.Tab.Table.Commands.SetModelListValue(
                    _vm.Tuple,
                    () => Model.Items,
                    items,
                    mode);

                _selectedItem = null;

                OnItemsListUpdated();
            }
            finally
            {
                _vm.IsLocked = false;
            }

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(IsItemSelected));
        }

        public void OnItemsListUpdated()
        {
            _selectedItem = null;

            if (Model == null)
            {
                Items.ClearAndAddRange(
                    new List<ItemPackageEntryViewModel>());

                OnPropertyChanged(nameof(SelectedItem));
                OnPropertyChanged(nameof(IsItemSelected));

                return;
            }

            Items.ClearAndAddRange(
                Model.Items.Select(
                    p => new ItemPackageEntryViewModel(_vm, p)));

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(IsItemSelected));
        }
    }
}