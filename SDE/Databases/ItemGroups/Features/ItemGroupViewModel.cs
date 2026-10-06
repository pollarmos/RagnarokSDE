using Database.Commands;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroupViewModel : BaseModelView<ItemGroup> {
        public RangeObservableCollection<ItemGroupSubGroupViewModel> SubGroups { get; } = new RangeObservableCollection<ItemGroupSubGroupViewModel>();

        private ItemGroupSubGroupViewModel _selectedSubGroup;

        public bool IsLocked { get; set; }

        public ItemGroupViewModel(DbTab tab) {
            Tab = tab;
        }

        public ItemGroupViewModel(ReadableTuple tuple, ItemGroup model) {
            Tuple = tuple;
            Model = model;

            OnSubGroupsListUpdated();
            OnPropertyChanged("");
        }

        public void SetModel(ReadableTuple tuple, ItemGroup model) {
            if (IsLocked)
                return;

            Tuple = tuple;
            Model = model;

            _selectedSubGroup = null;

            OnSubGroupsListUpdated();
            OnPropertyChanged("");
        }

        public string Group {
            get => Model?.Group;
            set => ExecuteCommand(value);
        }

        public ItemGroupSubGroupViewModel SelectedSubGroup {
            get => _selectedSubGroup;
            set {
                if (_selectedSubGroup == value)
                    return;
                _selectedSubGroup = value;
                OnPropertyChanged(nameof(SelectedSubGroup));
                OnPropertyChanged(nameof(IsSubGroupSelected));
            }
        }

        public bool IsSubGroupSelected {
            get => _selectedSubGroup != null;
        }

        public void ExecuteCommand<T>(T value, [CallerMemberName] string fieldName = "") {
            Execute(Model, value, fieldName, v => IsLocked = v);
        }

        public void ChangeSubGroups(List<ItemGroupSubGroup> subGroups, ListCommandMode mode) {
            if (subGroups == null || subGroups.Count == 0 || Model == null || Tuple == null)
                return;

            try {
                IsLocked = true;
                Tab.Table.Commands.SetModelListValue(Tuple, () => Model.SubGroups, subGroups, mode);
                _selectedSubGroup = null;
                OnSubGroupsListUpdated();
            }
            finally {
                IsLocked = false;
            }
            OnPropertyChanged(nameof(SelectedSubGroup));
            OnPropertyChanged(nameof(IsSubGroupSelected));
        }

        public void OnSubGroupsListUpdated() {
            if (Model == null) {
                SubGroups.ClearAndAddRange(new List<ItemGroupSubGroupViewModel>());

                SelectedSubGroup = null;
                return;
            }

            SubGroups.ClearAndAddRange(Model.SubGroups.Select(p => new ItemGroupSubGroupViewModel(this, p)));

            SelectedSubGroup = SubGroups.FirstOrDefault();
        }
    }
}