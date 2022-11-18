using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isAutoSaveUsed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFileDialogClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Filter = "Text|*.txt"
            };

            if (fileDialog.ShowDialog() is true)
            {
                using StreamReader streamReader = new(fileDialog.FileName);
                txt.Selection.Text = streamReader.ReadToEnd();
            }
            
        }

        private void btnSaveDialogClick(object sender, RoutedEventArgs e)
        {
            var saveFile = new SaveFileDialog();



            if (saveFile.ShowDialog() is true)
            {
                txt.SelectAll();
                using StreamWriter streamWriter = new(saveFile.FileName);
                streamWriter.Write(txt.Selection);
            }
        }

        private void chkAutoSaveChecked(object sender, RoutedEventArgs e)
        {
            if (_isAutoSaveUsed == false)
            {

                var result = MessageBox.Show("Tour Text Will be automatically saved on Desktop with Name AydinText.txt \nDo you accept It?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _isAutoSaveUsed = true;
                    return;
                }

                chkAutoSave.IsChecked = false;
            }
        }

        private void cpTextColorSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cpTextColor.SelectedColor is System.Windows.Media.Color color)
            {
                if (txt.Selection.IsEmpty)
                    txt.Foreground = new SolidColorBrush(cpTextColor.SelectedColor.Value);
                else
                    txt.Selection.ApplyPropertyValue(ForegroundProperty, new System.Windows.Media.SolidColorBrush(cpTextColor.SelectedColor.Value));

            }
        }

        private void cpBackColorSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cpBackColor.SelectedColor is System.Windows.Media.Color color)
                txt.Background = new SolidColorBrush(color);
        }

        private void txtTextChanged(object sender, TextChangedEventArgs e)
        {
            if (chkAutoSave.IsChecked == null || chkAutoSave.IsChecked == false)
                return;

            txt.SelectAll();

            File.WriteAllText(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\AydinText.txt", txt.Selection.Text);
        }

        private void cBoxFontStyleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txt.Selection.IsEmpty)
                txt.FontFamily = new System.Windows.Media.FontFamily(cBoxFontStyle.SelectedItem.ToString());
            else
                txt.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new System.Windows.Media.FontFamily(cBoxFontStyle.SelectedItem.ToString()));
        }

        private void cBoxFontSizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txt.Selection.IsEmpty)
                txt.FontSize = double.Parse(cBoxFontSize.SelectedItem.ToString()!);
            else
                txt.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, cBoxFontSize.SelectedItem.ToString());
        }
    }
}
