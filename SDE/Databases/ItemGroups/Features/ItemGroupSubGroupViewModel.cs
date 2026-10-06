using Database.Commands;
using SDE.Databases.Generic.Features;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroupSubGroupViewModel : BaseModelView<ItemGroupSubGroup> {
        private readonly ItemGroupViewModel _vm;

        public RangeObservableCollection<ItemGroupEntryViewModel> Entries { get; } = new RangeObservableCollection<ItemGroupEntryViewModel>();

        private ItemGroupEntryViewModel _selectedEntry;

        public ItemGroupSubGroupViewModel(ItemGroupViewModel viewModel, ItemGroupSubGroup model) {
            Model = model;
            _vm = viewModel;
            OnEntriesListUpdated();
        }

        public int SubGroup {
            get => Model == null ? 0 : Model.SubGroup;
            set => ExecuteCommand(value);
        }

        public string Algorithm {
            get => Model?.Algorithm;
            set => ExecuteCommand(value);
        }

        public bool? Clear {
            get => Model?.Clear;
            set => ExecuteCommand(value);
        }

        public ItemGroupEntryViewModel SelectedEntry {
            get => _selectedEntry;
            set {
                if (_selectedEntry == value)
                    return;
                _selectedEntry = value;
                OnPropertyChanged(nameof(SelectedEntry));
                OnPropertyChanged(nameof(IsEntrySelected));
            }
        }

        public bool IsEntrySelected {
            get => _selectedEntry != null;
        }

        public void ExecuteCommand<T>(T value, [CallerMemberName] string fieldName = "") {
            if (Model == null || _vm.Tuple == null)
                return;

            try {
                _vm.IsLocked = true;
                _vm.Tab.Table.Commands.SetModelValue(_vm.Tuple, Model, fieldName, value);
            }
            finally {
                _vm.IsLocked = false;
            }
            OnPropertyChanged(fieldName);
        }

        public void ChangeEntries(List<ItemGroupEntry> entries, ListCommandMode mode) {
            if (entries == null || entries.Count == 0 || Model == null || _vm.Tuple == null)
                return;

            try {
                _vm.IsLocked = true;
                _vm.Tab.Table.Commands.SetModelListValue(_vm.Tuple, () => Model.List, entries, mode);
                _selectedEntry = null;
                OnEntriesListUpdated();
            }
            finally {
                _vm.IsLocked = false;
            }
            OnPropertyChanged(nameof(SelectedEntry));
            OnPropertyChanged(nameof(IsEntrySelected));
        }

        public void OnEntriesListUpdated() {
            _selectedEntry = null;

            if (Model == null) {
                Entries.ClearAndAddRange(new List<ItemGroupEntryViewModel>());
                OnPropertyChanged(nameof(SelectedEntry));
                OnPropertyChanged(nameof(IsEntrySelected));
                return;
            }
            Entries.ClearAndAddRange(Model.List.Select(p => new ItemGroupEntryViewModel(_vm, p)));
            OnPropertyChanged(nameof(SelectedEntry));
            OnPropertyChanged(nameof(IsEntrySelected));
        }
    }
}