using Database.Commands;
using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Utilities.Commands;

namespace SDE.Databases.ItemRandomOptionGroups.Features
{
    public partial class ItemRandomOptionGroupViewYaml : UserControl, IDatabaseView
    {
        private ItemRandomOptionGroupViewModel _viewModel;

        private DbTab _tab;

        public ItemRandomOptionGroupViewYaml()
        {
            InitializeComponent();
        }

        public void Init(DbTab tab)
        {
            _tab = tab;

            tab.UpdateAction = _updateAction;

            _viewModel =
                new ItemRandomOptionGroupViewModel(tab);

            _viewModel.SetModel(null, null);

            DataContext = _viewModel;

            WeakEventManager<BaseDatabase, EventArgs>.AddHandler(
                tab.Database,
                nameof(BaseDatabase.TableModified),
                OnTableModified);

            if (tab.SelectedItem != null)
                _updateAction(tab.SelectedItem);
        }

        private void OnTableModified(object sender, EventArgs e)
        {
            if (!(sender is BaseDatabase db))
                return;

            switch (db.Table.Commands.StackStatus)
            {
                case StackStatus.Undo:
                case StackStatus.Redo:
                case StackStatus.Restore:

                    _viewModel.OnRandomOptionsListUpdated();

                    break;
            }
        }

        private void _updateAction(ReadableTuple tuple)
        {
            if (_viewModel.IsLocked)
                return;

            if (tuple != null)
            {
                _viewModel.SetModel(tuple, tuple.GetModel<ItemRandomOptionGroup>());
            }
            else
            {
                _viewModel.SetModel(null, null);
            }
        }

        private void _buttonAddSlot_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Model == null)
                return;

            int slotId = 1;

            while (_viewModel.Model.Slots.Any(p => p.Slot == slotId))
            {
                slotId++;
            }

            ItemRandomOptionGroupSlot slot = new ItemRandomOptionGroupSlot{ Slot = slotId };

            _viewModel.ChangeSlots(new List<ItemRandomOptionGroupSlot> { slot }, ListCommandMode.Add);
        }

        private void _buttonDeleteSlot_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedSlot == null || _viewModel.Model == null)
                return;

            List<ItemRandomOptionGroupSlot> newList = _viewModel.Model.Slots.ToList();

            newList.Remove(_viewModel.SelectedSlot.Model);

            _viewModel.ChangeSlots(newList, ListCommandMode.ChangeList);
        }

        private void _buttonAddSlotOption_Click(object sender, RoutedEventArgs e)
        {
            ItemRandomOptionGroupSlotViewModel slot = _viewModel.SelectedSlot;

            if (slot == null || slot.Model == null)
                return;

            ItemRandomOptionGroupOption option = new ItemRandomOptionGroupOption { Option = "" };

            slot.ChangeOptions(new List<ItemRandomOptionGroupOption> { option }, ListCommandMode.Add);
        }

        private void _buttonDeleteSlotOption_Click(object sender, RoutedEventArgs e)
        {
            ItemRandomOptionGroupSlotViewModel slot = _viewModel.SelectedSlot;

            if (slot == null || slot.Model == null || slot.SelectedOption == null)
                return;

            List<ItemRandomOptionGroupOption> newList = slot.Model.Options.ToList();

            newList.Remove(slot.SelectedOption.Model);

            slot.ChangeOptions(newList, ListCommandMode.ChangeList);
        }

        private void _buttonAddRandomOption_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Model == null)
                return;

            ItemRandomOptionGroupOption option = new ItemRandomOptionGroupOption { Option = "" };

            _viewModel.ChangeRandomOptions(new List<ItemRandomOptionGroupOption> { option }, ListCommandMode.Add);
        }

        private void _buttonDeleteRandomOption_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRandomOption == null || _viewModel.Model == null)
                return;

            List<ItemRandomOptionGroupOption> newList = _viewModel.Model.Random.ToList();

            newList.Remove( _viewModel.SelectedRandomOption.Model);

            _viewModel.ChangeRandomOptions(newList, ListCommandMode.ChangeList);
        }
    }
}