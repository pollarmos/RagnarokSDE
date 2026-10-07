using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using System.Runtime.CompilerServices;

namespace SDE.Databases.ItemPackages.Features
{
    public class ItemPackageEntryViewModel :
        BaseModelView<ItemPackageEntry>
    {
        private readonly ItemPackageViewModel _vm;

        public ItemPackageEntryViewModel(
            ItemPackageViewModel viewModel,
            ItemPackageEntry model)
        {
            Model = model;
            _vm = viewModel;
        }

        public string Item
        {
            get
            {
                if (Model == null ||
                    string.IsNullOrEmpty(Model.Item))
                    return "";

                return DbUtilities.ItemId2AegisName(Model.Item);
            }
            set
            {
                string item = "";

                if (!string.IsNullOrWhiteSpace(value))
                {
                    item = CachedDbs.AegisNameItem
                        .ToStringId(value);
                }

                ExecuteCommand(item, nameof(Item));

                OnPropertyChanged(nameof(DisplayItemId));
                OnPropertyChanged(nameof(DisplayItemName));
            }
        }

        public string DisplayItemId
        {
            get => Model?.Item ?? "";
        }

        public string DisplayItemName
        {
            get
            {
                if (Model == null ||
                    string.IsNullOrEmpty(Model.Item))
                    return "";

                return DbUtilities.ItemId2Name(Model.Item);
            }
        }

        public string Amount
        {
            get
            {
                if (Model == null || !Model.Amount.HasValue)
                    return "";

                return Model.Amount.Value.ToString();
            }
            set
            {
                int? newValue = null;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!int.TryParse(value, out int parsed))
                        return;

                    newValue = parsed;
                }

                ExecuteCommand(
                    newValue,
                    nameof(ItemPackageEntry.Amount));
            }
        }

        public string RentalHours
        {
            get
            {
                if (Model == null || !Model.RentalHours.HasValue)
                    return "";

                return Model.RentalHours.Value.ToString();
            }
            set
            {
                int? newValue = null;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!int.TryParse(value, out int parsed))
                        return;

                    newValue = parsed;
                }

                ExecuteCommand(
                    newValue,
                    nameof(ItemPackageEntry.RentalHours));
            }
        }

        public string Refine
        {
            get
            {
                if (Model == null || !Model.Refine.HasValue)
                    return "";

                return Model.Refine.Value.ToString();
            }
            set
            {
                int? newValue = null;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (!int.TryParse(value, out int parsed))
                        return;

                    newValue = parsed;
                }

                ExecuteCommand(
                    newValue,
                    nameof(ItemPackageEntry.Refine));
            }
        }

        public string Grade
        {
            get => Model?.Grade;
            set => ExecuteCommand(value);
        }

        public string RandomOptionGroup
        {
            get => Model?.RandomOptionGroup;
            set => ExecuteCommand(value);
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
    }
}