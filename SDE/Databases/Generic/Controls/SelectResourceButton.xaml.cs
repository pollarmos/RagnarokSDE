using ErrorManager;
using GRF.Image;
using SDE.Databases.ClientItems.Features;
using SDE.Databases.Generic.Parser;
using SDE.Editor.LuaTables;
using SDE.View;
using SDE.View.Dialogs;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TokeiLibrary;
using TokeiLibrary.WPF.Styles;
using Utilities.Services;

namespace SDE.Databases.Generic.Controls {
	public enum SelectResourceSource {
		Card,
		Icon,
		Collection,
		QuestIcons,
		Npc,
	}

	/// <summary>
	/// Interaction logic for SelectResourceButton.xaml
	/// </summary>
	public partial class SelectResourceButton : UserControl {
		public SelectResourceButton() {
			InitializeComponent();
			
			_contextMenu.PlacementTarget = _button;
		}

		public static readonly DependencyProperty SourceFieldProperty =
			DependencyProperty.Register(
				nameof(SourceField),
				typeof(string),
				typeof(SelectResourceButton),
				new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string SourceField {
			get => (string)GetValue(SourceFieldProperty);
			set => SetValue(SourceFieldProperty, value);
		}

		public static readonly DependencyProperty SearchTextProperty =
			DependencyProperty.Register(
				nameof(SearchText),
				typeof(string),
				typeof(SelectResourceButton),
				new FrameworkPropertyMetadata(default(string)));

		public string SearchText {
			get => (string)GetValue(SearchTextProperty);
			set => SetValue(SearchTextProperty, value);
		}

		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register(
				nameof(Source),
				typeof(SelectResourceSource),
				typeof(SelectResourceButton),
				new PropertyMetadata(SelectResourceSource.Card));

		public SelectResourceSource Source {
			get => (SelectResourceSource)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}

		public static readonly DependencyProperty AutoCompleteProperty =
			DependencyProperty.Register(
				nameof(AutoComplete),
				typeof(Visibility),
				typeof(SelectResourceButton),
				new FrameworkPropertyMetadata(Visibility.Visible, OnAutoCompletePropertyChanged));

		public Visibility AutoComplete {
			get => (Visibility)GetValue(AutoCompleteProperty);
			set => SetValue(AutoCompleteProperty, value);
		}

		private static void OnAutoCompletePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var control = (SelectResourceButton)d;

			control._miSelect.Visibility = (Visibility)e.NewValue;
		}

		private void _select_Click(object sender, RoutedEventArgs e) {
			try {
				var tuple = SdeEditor.Instance.FindTopmostTab().SelectedItem;

				if (tuple == null)
					return;

				var viewId = DbReader.ToInt(tuple.GetModel<ClientItem>().ClassNumber);
				SourceField = LuaHelper.GetSpriteFromViewId(viewId, LuaHelper.ViewIdTypes.Headgear, tuple);
			}
			catch (Exception err) {
				ErrorHandler.HandleException(err);
			}
		}

		private void _selectFromList_Click(object sender, RoutedEventArgs e) {
			try {
				var grfPath = _getGrfPath();

				MultiGrfExplorer dialog = new MultiGrfExplorer(SdeEditor.MetaGrf, EncodingService.FromAnyToDisplayEncoding(grfPath), _getExtension(), EncodingService.FromAnyToDisplayEncoding(SourceField ?? ""));
				dialog._textBoxSearch.Text = SearchText;

				if (dialog.ShowDialog() == true) {
					switch (Source) {
						case SelectResourceSource.QuestIcons:
							SourceField = Path.GetFileName(dialog.SelectedPath.GetFullPath());
							break;
						default:
							SourceField = Path.GetFileNameWithoutExtension(dialog.SelectedPath.GetFullPath());
							break;
					}
				}
			}
			catch (Exception err) {
				ErrorHandler.HandleException(err);
			}
		}

		private void _button_Click(object sender, RoutedEventArgs e) {
			if (AutoComplete != Visibility.Visible) {
                try {
                    var preview = _createCardPreview();
                    if (preview != null) {
                        preview.ShowDialog();
                        return;
                    }

                    _selectFromList_Click(null, null);
                }
                catch (Exception err) {
                    ErrorHandler.HandleException(err);
                }
				return;
			}

			_button.ContextMenu.IsOpen = true;
		}

        private Window _createCardPreview() {
            if (Source != SelectResourceSource.Card || String.IsNullOrWhiteSpace(SourceField))
                return null;

            var resources = SdeEditor.MetaGrf;
            var directory = EncodingService.FromAnyToDisplayEncoding(_getGrfPath());
            var filename = EncodingService.FromAnyToDisplayEncoding(SourceField) + ".bmp";
            var entry = resources.GetEntry(Path.Combine(directory, filename));

            if (entry == null) {
                var path = resources.FilesInDirectory(directory).FirstOrDefault(p =>
                    String.Equals(Path.GetFileName(p), filename, StringComparison.OrdinalIgnoreCase));
                if (path == null)
                    return null;

                entry = resources.GetEntry(path);
            }

            if (entry == null)
                return null;

            var image = ImageProvider.GetImage(entry).Cast<BitmapSource>();
            return new TkWindow(entry.DisplayRelativePath, "cde.ico", SizeToContent.WidthAndHeight, ResizeMode.CanResize) {
                Owner = WpfUtilities.TopWindow,
                MaxWidth = SystemParameters.WorkArea.Width * 0.9,
                MaxHeight = SystemParameters.WorkArea.Height * 0.9,
                Content = new ScrollViewer {
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = new Image { Source = image, Stretch = Stretch.None }
                }
            };
        }

		private string _getExtension() {
			switch (Source) {
				case SelectResourceSource.Npc:
					return ".spr";
				default:
					return ".bmp";
			}
		}

		private string _getGrfPath() {
			switch (Source) {
				case SelectResourceSource.Icon:
					return @"data\texture\À¯ÀúÀÎÅÍÆäÀÌ½º\item";
				case SelectResourceSource.Card:
					return @"data\texture\À¯ÀúÀÎÅÍÆäÀÌ½º\cardbmp";
				case SelectResourceSource.Collection:
					return @"data\texture\À¯ÀúÀÎÅÍÆäÀÌ½º\collection";
				case SelectResourceSource.QuestIcons:
					return @"data\texture\À¯ÀúÀÎÅÍÆäÀÌ½º\renew_questui";
				case SelectResourceSource.Npc:
					return @"data\sprite\npc";
				default:
					throw new ArgumentOutOfRangeException(nameof(Source));
			}
		}
	}
}
