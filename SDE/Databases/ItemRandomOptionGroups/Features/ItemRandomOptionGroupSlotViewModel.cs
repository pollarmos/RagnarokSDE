using SDE.Databases.Generic.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Database.Commands;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroupSlotViewModel : BaseModelView<ItemRandomOptionGroupSlot>
    {
        private readonly ItemRandomOptionGroupViewModel _vm;

        public RangeObservableCollection<ItemRandomOptionGroupOptionViewModel> Options { get; } = new RangeObservableCollection<ItemRandomOptionGroupOptionViewModel>();

        private ItemRandomOptionGroupOptionViewModel _selectedOption;

        public ItemRandomOptionGroupSlotViewModel(ItemRandomOptionGroupViewModel viewModel, ItemRandomOptionGroupSlot model)
        {
            _vm = viewModel;
            Model = model;

            OnOptionsListUpdated();
        }

        public string Slot
        {
            get => Model?.Slot.ToString() ?? "";

            set
            {
                if (Model == null)
                    return;

                if (Int32.TryParse(value, out int result))
                {
                    ExecuteCommand(result, nameof(ItemRandomOptionGroupSlot.Slot));
                }
            }
        }

        public int OptionCount
        {
            get => Model?.Options?.Count ?? 0;
        }

        public bool IsOptionSelected
        {
            get => _selectedOption != null;
        }

        public ItemRandomOptionGroupOptionViewModel SelectedOption
        {
            get => _selectedOption;

            set
            {
                if (_selectedOption == value)
                    return;

                _selectedOption = value;

                OnPropertyChanged(nameof(SelectedOption));
                OnPropertyChanged(nameof(IsOptionSelected));
            }
        }

        public void ExecuteCommand<T>(T value, [CallerMemberName] string fieldName = "")
        {
            if (Model == null || _vm.Tuple == null)
                return;

            try
            {
                _vm.IsLocked = true;

                _vm.Tab.Table.Commands.SetModelValue(_vm.Tuple, Model, fieldName, value);
            }
            finally
            {
                _vm.IsLocked = false;
            }

            OnPropertyChanged(fieldName);
        }

        public void OnOptionsListUpdated()
        {
            _selectedOption = null;

            if (Model == null)
            {
                Options.ClearAndAddRange(new List<ItemRandomOptionGroupOptionViewModel>());

                OnPropertyChanged(nameof(SelectedOption));

                return;
            }

            Options.ClearAndAddRange(Model.Options.Select(p => new ItemRandomOptionGroupOptionViewModel(_vm, p)));

            SelectedOption = Options.FirstOrDefault();
        }

        public void ChangeOptions(List<ItemRandomOptionGroupOption> options, ListCommandMode mode)
        {
            if (options == null || options.Count == 0 || Model == null || _vm.Tuple == null)
                return;

            try
            {
                _vm.IsLocked = true;

                _vm.Tab.Table.Commands.SetModelListValue(_vm.Tuple, () => Model.Options, options, mode);

                _selectedOption = null;

                OnOptionsListUpdated();
            }
            finally
            {
                _vm.IsLocked = false;
            }

            OnPropertyChanged(nameof(OptionCount));
            OnPropertyChanged(nameof(SelectedOption));
            OnPropertyChanged(nameof(IsOptionSelected));
        }
    }
}