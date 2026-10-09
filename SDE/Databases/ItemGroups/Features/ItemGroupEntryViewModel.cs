using SDE.Databases.Generic.Features;
using SDE.Databases.ItemRandomOptionGroups.Features;
using SDE.Editor.Database;
using SDE.View;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string Item
        {
            get => Model?.Item;
            set
            {
                ExecuteCommand(value);
                OnPropertyChanged(nameof(DisplayItemName));
            }
        }

        public string DisplayItemName
        {
            get
            {
                if (Model == null ||
                    string.IsNullOrEmpty(Model.Item))
                    return "";

                string id =
                    CachedDbs.AegisNameItem
                        .ToStringId(Model.Item);

                return DbUtilities.ItemId2Name(id);
            }
        }

        public string Rate
        {
            get
            {
                if (Model == null || !Model.Rate.HasValue)
                    return "";

                return Model.Rate.Value.ToString();
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
                    nameof(ItemGroupEntry.Rate));
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
                    nameof(ItemGroupEntry.Amount));
            }
        }

        public string Duration
        {
            get
            {
                if (Model == null || !Model.Duration.HasValue)
                    return "";

                return Model.Duration.Value.ToString();
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
                    nameof(ItemGroupEntry.Duration));
            }
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

        public List<string> RandomOptionGroups
        {
            get
            {
                List<string> groups =
                    new List<string>();

                // 빈 값 선택용
                groups.Add("");

                if (SdeEditor.Project == null)
                    return groups;

                var db =
                    SdeEditor.Project.GetMergedTable(
                        DataSources.ItemRandomOptionGroup);

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