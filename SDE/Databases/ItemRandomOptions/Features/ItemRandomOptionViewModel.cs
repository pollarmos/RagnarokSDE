using SDE.Databases.Generic.Features;
using SDE.Databases.ItemRandomOptions.Properties;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Runtime.CompilerServices;
using Utilities;

namespace SDE.Databases.ItemRandomOptions.Features
{
    public class ItemRandomOptionViewModel
        : BaseModelView<ItemRandomOption>
    {
        public bool IsLocked { get; set; }

        public ItemRandomOptionViewModel(DbTab tab)
        {
            Tab = tab;
        }

        public void SetModel(
            ReadableTuple tuple,
            ItemRandomOption model)
        {
            if (IsLocked)
                return;

            Tuple = tuple;
            Model = model;

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

        public string Option
        {
            get => Model?.Option;
            set
            {
                ExecuteCommand(value);

                OnPropertyChanged(
                    nameof(Description));
            }
        }

        public string Description
        {
            get
            {
                if (Model == null)
                    return "";

                return ItemRandomOptionDescriptions.Get(
                    Model.Option);
            }
        }

        public string Script
        {
            get => Model?.Script;
            set => ExecuteCommand(value);
        }

        public void ExecuteCommand<T>(
            T value,
            [CallerMemberName] string fieldName = "")
        {
            if (Model == null || Tuple == null)
                return;

            try
            {
                IsLocked = true;

                Tab.Table.Commands.SetModelValue(
                    Tuple,
                    Model,
                    fieldName,
                    value);
            }
            finally
            {
                IsLocked = false;
            }

            OnPropertyChanged(fieldName);
        }
    }
}