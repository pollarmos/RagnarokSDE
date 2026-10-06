using SDE.Databases.Generic.Features;
using System.Runtime.CompilerServices;

namespace SDE.Databases.ItemGroups.Features {
    public class ItemGroupEntryViewModel : BaseModelView<ItemGroupEntry> {
        private readonly ItemGroupViewModel _vm;

        public ItemGroupEntryViewModel(ItemGroupViewModel viewModel, ItemGroupEntry model) {
            Model = model;
            _vm = viewModel;
        }

        public int Index {
            get => Model == null ? 0 : Model.Index;
            set => ExecuteCommand(value);
        }

        public string Item {
            get => Model?.Item;
            set => ExecuteCommand(value);
        }

        public int? Rate {
            get => Model?.Rate;
            set => ExecuteCommand(value);
        }

        public int? Amount {
            get => Model?.Amount;
            set => ExecuteCommand(value);
        }

        public int? Duration {
            get => Model?.Duration;
            set => ExecuteCommand(value);
        }

        public bool? Announced {
            get => Model?.Announced;
            set => ExecuteCommand(value);
        }

        public bool? UniqueId {
            get => Model?.UniqueId;
            set => ExecuteCommand(value);
        }

        public bool? Stacked {
            get => Model?.Stacked;
            set => ExecuteCommand(value);
        }

        public bool? Named {
            get => Model?.Named;
            set => ExecuteCommand(value);
        }

        public string Bound {
            get => Model?.Bound;
            set => ExecuteCommand(value);
        }

        public string RandomOptionGroup {
            get => Model?.RandomOptionGroup;
            set => ExecuteCommand(value);
        }

        public int? RefineMinimum {
            get => Model?.RefineMinimum;
            set => ExecuteCommand(value);
        }

        public int? RefineMaximum {
            get => Model?.RefineMaximum;
            set => ExecuteCommand(value);
        }

        public string GradeMinimum {
            get => Model?.GradeMinimum;
            set => ExecuteCommand(value);
        }

        public string GradeMaximum {
            get => Model?.GradeMaximum;
            set => ExecuteCommand(value);
        }

        public bool? Clear {
            get => Model?.Clear;
            set => ExecuteCommand(value);
        }

        public void ExecuteCommand<T>(T value, [CallerMemberName] string fieldName = "") {
            if (Model == null || _vm.Tuple == null)
                return;

            try {
                _vm.IsLocked = true;

                _vm.Tab.Table.Commands.SetModelValue(
                    _vm.Tuple,
                    Model,
                    fieldName,
                    value);
            }
            finally {
                _vm.IsLocked = false;
            }

            OnPropertyChanged(fieldName);
        }
    }
}