using SDE.Databases.Generic.Features;
using SDE.Editor.Database;
using SDE.Editor.Generic.DbTabs;
using System.Windows.Controls;

namespace SDE.Databases.ItemRandomOptions.Features
{
    public partial class ItemRandomOptionViewYaml
        : UserControl, IDatabaseView
    {
        private ItemRandomOptionViewModel _viewModel;

        public ItemRandomOptionViewYaml()
        {
            InitializeComponent();
        }

        public void Init(DbTab tab)
        {
            tab.UpdateAction = _updateAction;

            _viewModel =
                new ItemRandomOptionViewModel(tab);

            _viewModel.SetModel(null, null);

            DataContext = _viewModel;

            if (tab.SelectedItem != null)
                _updateAction(tab.SelectedItem);
        }

        private void _updateAction(
            ReadableTuple tuple)
        {
            if (_viewModel.IsLocked)
                return;

            if (tuple != null)
            {
                _viewModel.SetModel(
                    tuple,
                    tuple.GetModel<ItemRandomOption>());
            }
            else
            {
                _viewModel.SetModel(
                    null,
                    null);
            }
        }
    }
}