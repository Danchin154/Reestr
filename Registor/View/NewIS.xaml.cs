using Registor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Registor.View
{
    public partial class NewIS : Window
    {
        string filePath = "columns.txt";
        private DataGrid MyDataGrid;

        public event PropertyChangedEventHandler PropertyChanged;

        public NewIS(DataGrid myDataGrid)
        {
            InitializeComponent();

            DataContext = new ViewModel();
            this.MyDataGrid = myDataGrid;
            
        }

        public NewIS()
        {

        }

        private void Closen_Click(object sender, RoutedEventArgs e)
        {
            NewIS.GetWindow(this).Close();
        }

        public void NewInfoSystem(object sender, RoutedEventArgs e)
        {
            string columnName = NameIS.Text;
            DataGridCheckBoxColumn column = new DataGridCheckBoxColumn();
            column.Header = columnName;
            column.Binding = new Binding(columnName);
            column.CellStyle = new Style(typeof(DataGridCell))
            {
                Setters = {
                    new Setter(CheckBox.IsCheckedProperty, new Binding(columnName)
            {
                        Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    })
                }
            };
            if (MyDataGrid != null)
            {
                MyDataGrid.Columns.Add(column);
                File.AppendAllText(filePath, columnName + Environment.NewLine);
            }

            foreach (var item in MyDataGrid.Items)
            {
                var propertyInfo = item.GetType().GetProperty("IsSelected");
                if (propertyInfo != null && propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(item, false);
                }
            }

        }
        public void LoadSavedColumns()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string columnName = line.Trim();
                    DataGridCheckBoxColumn column = new DataGridCheckBoxColumn();
                    column.Header = columnName;
                    column.Binding = new Binding(columnName);
                    column.CellStyle = new Style(typeof(DataGridCell))
                    {
                        Setters = {
                    new Setter(CheckBox.IsCheckedProperty, new Binding(columnName)
            {
                        Mode = BindingMode.TwoWay, UpdateSourceTrigger  = UpdateSourceTrigger.PropertyChanged})
                }
                    };
                    MyDataGrid.Columns.Add(column);
                    ((ViewModel)DataContext).ColumnNames.Add(columnName);
                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.UpdateColumns();
            }
        }
    }
}