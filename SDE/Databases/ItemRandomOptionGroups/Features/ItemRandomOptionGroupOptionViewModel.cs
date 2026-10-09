using SDE.Databases.Generic.Features;
using SDE.Databases.ItemRandomOptions.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroupOptionViewModel : BaseModelView<ItemRandomOptionGroupOption>
    {
        private readonly ItemRandomOptionGroupViewModel _vm;

        public ItemRandomOptionGroupOptionViewModel(ItemRandomOptionGroupViewModel viewModel, ItemRandomOptionGroupOption model)
        {
            _vm = viewModel;
            Model = model;
        }

        public string Option
        {
            get => Model?.Option ?? "";

            set
            {
                ExecuteCommand(value);

                OnPropertyChanged(nameof(Description));
            }
        }

        public string MinValue
        {
            get => Model?.MinValue?.ToString() ?? "";

            set => ExecuteNullableInt(value, nameof(ItemRandomOptionGroupOption.MinValue));
        }

        public string MaxValue
        {
            get => Model?.MaxValue?.ToString() ?? "";

            set => ExecuteNullableInt(value, nameof(ItemRandomOptionGroupOption.MaxValue));
        }

        public string Param
        {
            get => Model?.Param?.ToString() ?? "";

            set => ExecuteNullableInt(value, nameof(ItemRandomOptionGroupOption.Param));
        }

        public string Chance
        {
            get => Model?.Chance?.ToString() ?? "";

            set => ExecuteNullableInt(value, nameof(ItemRandomOptionGroupOption.Chance));
        }

        public string Description
        {
            get
            {
                if (Model == null)
                    return "";

                return ItemRandomOptionDescriptions.Get(Model.Option);
            }
        }

        public List<string> OptionNames
        {
            get => _vm.OptionNames;
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

        private void ExecuteNullableInt(string value, string fieldName)
        {
            if (Model == null)
                return;

            if (String.IsNullOrWhiteSpace(value))
            {
                ExecuteCommand<int?>(null, fieldName);

                return;
            }

            if (Int32.TryParse(value, out int result))
            {
                ExecuteCommand<int?>(result, fieldName);
            }
        }
    }
}