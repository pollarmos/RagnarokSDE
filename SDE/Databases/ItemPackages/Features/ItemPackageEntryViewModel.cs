using SDE.Databases.Generic.Features;
using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Editor.Database;
using SDE.View;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<string> RandomOptionGroups
        {
            get
            {
                List<string> groups = new List<string>();

                // RandomOptionGroup 제거용 빈 값
                groups.Add("");

                if (SdeEditor.Project == null)
                    return groups;

                var db = SdeEditor.Project.GetMergedTable(DataSources.ItemRandomOptionGroup);

                if (db == null)
                    return groups;

                groups.AddRange(
                    db.FastItems
                        .OrderBy(p => p.GetKey<int>())
                        .Select(p =>
                            p.GetModel<ItemRandomOptionGroup>()?.Group)
                        .Where(p =>
                            !String.IsNullOrEmpty(p))
                        .Distinct(
                            StringComparer.OrdinalIgnoreCase));

                return groups;
            }
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