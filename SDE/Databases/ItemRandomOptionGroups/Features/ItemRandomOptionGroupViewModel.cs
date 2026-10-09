using SDE.Databases.ItemRandomOptions.Features;
using SDE.View;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using Database.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TokeiLibrary.WPF;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public class ItemRandomOptionGroupViewModel : BaseModelView<ItemRandomOptionGroup>
    {
        public RangeObservableCollection<ItemRandomOptionGroupSlotViewModel> Slots { get; } = new RangeObservableCollection<ItemRandomOptionGroupSlotViewModel>();

        public RangeObservableCollection<ItemRandomOptionGroupOptionViewModel> RandomOptions { get; } = new RangeObservableCollection<ItemRandomOptionGroupOptionViewModel>();

        private ItemRandomOptionGroupSlotViewModel _selectedSlot;

        private ItemRandomOptionGroupOptionViewModel _selectedRandomOption;

        public bool IsLocked { get; set; }

        public ItemRandomOptionGroupViewModel(DbTab tab)
        {
            Tab = tab;
        }

        public void SetModel(ReadableTuple tuple, ItemRandomOptionGroup model)
        {
            if (IsLocked)
                return;

            Tuple = tuple;
            Model = model;

            _selectedSlot = null;
            _selectedRandomOption = null;

            OnSlotsListUpdated();
            OnRandomOptionsListUpdated();

            OnPropertyChanged("");
        }

        public string Id
        {
            get
            {
                if (Tuple == null)
                    return "";

                return Tuple.GetKey<int>().ToString();
            }
        }

        public string Group
        {
            get => Model?.Group ?? "";
            set => ExecuteCommand(value);
        }

        public string MaxRandom
        {
            get => Model?.MaxRandom?.ToString() ?? "";

            set
            {
                if (Model == null)
                    return;

                if (String.IsNullOrWhiteSpace(value))
                {
                    ExecuteCommand<int?>(null, nameof(ItemRandomOptionGroup.MaxRandom));
                    return;
                }

                if (Int32.TryParse(value, out int result))
                {
                    ExecuteCommand<int?>(result, nameof(ItemRandomOptionGroup.MaxRandom));
                }
            }
        }

        public bool IsSlotSelected
        {
            get => _selectedSlot != null;
        }

        public List<string> OptionNames
        {
            get
            {
                List<string> options = new List<string>();

                // 빈 값 허용
                options.Add("");

                if (SdeEditor.Project == null)
                    return options;

                var db = SdeEditor.Project.GetMergedTable(DataSources.ItemRandomOption);

                if (db == null)
                    return options;

                options.AddRange(
                    db.FastItems
                        .OrderBy(p => p.GetKey<int>())
                        .Select(p =>
                            p.GetModel<ItemRandomOption>()?.Option)
                        .Where(p =>
                            !String.IsNullOrEmpty(p))
                        .Distinct(
                            StringComparer.OrdinalIgnoreCase));

                return options;
            }
        }

        public ItemRandomOptionGroupSlotViewModel SelectedSlot
        {
            get => _selectedSlot;

            set
            {
                if (_selectedSlot == value)
                    return;

                _selectedSlot = value;

                OnPropertyChanged(nameof(SelectedSlot));
                OnPropertyChanged(nameof(IsSlotSelected));
            }
        }

        public bool IsRandomOptionSelected
        {
            get => _selectedRandomOption != null;
        }

        public ItemRandomOptionGroupOptionViewModel SelectedRandomOption
        {
            get => _selectedRandomOption;

            set
            {
                if (_selectedRandomOption == value)
                    return;

                _selectedRandomOption = value;

                OnPropertyChanged(nameof(SelectedRandomOption));
                OnPropertyChanged(nameof(IsRandomOptionSelected));
            }
        }

        public void ExecuteCommand<T>(T value, [CallerMemberName] string fieldName = "")
        {
            if (Model == null || Tuple == null)
                return;

            try
            {
                IsLocked = true;

                Tab.Table.Commands.SetModelValue(Tuple, Model, fieldName, value);
            }
            finally
            {
                IsLocked = false;
            }

            OnPropertyChanged(fieldName);
        }

        public void OnSlotsListUpdated()
        {
            if (Model == null)
            {
                Slots.ClearAndAddRange(new List<ItemRandomOptionGroupSlotViewModel>());

                SelectedSlot = null;

                return;
            }

            Slots.ClearAndAddRange(Model.Slots.Select(p => new ItemRandomOptionGroupSlotViewModel(this, p)));

            SelectedSlot = Slots.FirstOrDefault();
        }

        public void OnRandomOptionsListUpdated()
        {
            if (Model == null)
            {
                RandomOptions.ClearAndAddRange(new List<ItemRandomOptionGroupOptionViewModel>());

                SelectedRandomOption = null;

                return;
            }

            RandomOptions.ClearAndAddRange(Model.Random.Select(p => new ItemRandomOptionGroupOptionViewModel(this, p)));

            SelectedRandomOption = RandomOptions.FirstOrDefault();
        }

        public void ChangeSlots(List<ItemRandomOptionGroupSlot> slots, ListCommandMode mode)
        {
            if (slots == null || slots.Count == 0 || Model == null || Tuple == null)
                return;

            try
            {
                IsLocked = true;

                Tab.Table.Commands.SetModelListValue(Tuple, () => Model.Slots, slots, mode);

                _selectedSlot = null;

                OnSlotsListUpdated();
            }
            finally
            {
                IsLocked = false;
            }

            OnPropertyChanged(nameof(SelectedSlot));
            OnPropertyChanged(nameof(IsSlotSelected));
        }

        public void ChangeRandomOptions(List<ItemRandomOptionGroupOption> options, ListCommandMode mode)
        {
            if (options == null || options.Count == 0 || Model == null || Tuple == null)
                return;

            try
            {
                IsLocked = true;

                Tab.Table.Commands.SetModelListValue(Tuple, () => Model.Random, options, mode);

                _selectedRandomOption = null;

                OnRandomOptionsListUpdated();
            }
            finally
            {
                IsLocked = false;
            }

            OnPropertyChanged(nameof(SelectedRandomOption));
            OnPropertyChanged(nameof(IsRandomOptionSelected));
        }
    }
}