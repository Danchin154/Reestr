using Registor.Model;
using Registor.SQL_data;
using Registor.View;
using Registor.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Registor
{

    public partial class MainWindow : Window
    {

        private ViewModel vm;
        public NewIS iss;
        public MainWindow()
        {
            MyDataGrid = new DataGrid(); // сначала создаем MyDataGrid
            InitializeComponent();
            vm = new ViewModel();
            iss = new NewIS(MyDataGrid);
            iss.LoadSavedColumns();
        }

        private void OpenWindow_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid != null)
            {
                NewIS newis = new NewIS(MyDataGrid);
                newis.Show();
            }
        }

        private void DeleteIS_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid.Columns.Count > 0 && MyDataGrid.SelectedCells.Count > 0)
            {
                // Сначала проверяем, что столбцов больше нуля
                DataGridColumn selectedColumn = MyDataGrid.SelectedCells[0].Column;
                if (selectedColumn is DataGridCheckBoxColumn)
                {
                    int columnIndex = MyDataGrid.Columns.IndexOf(selectedColumn);
                    MyDataGrid.Columns.RemoveAt(columnIndex);

                    // Обновляем файл columns.txt, чтобы удалить выбранный столбец
                    string[] lines = File.ReadAllLines("columns.txt");
                    if (columnIndex >= 0 && columnIndex < lines.Length)
                    {
                        List<string> columnsList = new List<string>(lines);
                        columnsList.RemoveAt(columnIndex);
                        File.WriteAllLines("columns.txt", columnsList.ToArray());
                    }

                    // Обновляем список столбцов в окне NewIS
                    var newis = Application.Current.Windows.OfType<NewIS>().FirstOrDefault();
                    newis.LoadSavedColumns();

                    // Обновляем список столбцов 
                    UpdateColumns();
                }
            }
        }

        public void UpdateColumns()
        {
            // Очищаем список столбцов
            MyDataGrid.Columns.Clear();

            // Загружаем список столбцов из файла columns.txt
            string[] lines = File.ReadAllLines("columns.txt");
            DataGridCheckBoxColumn checkBoxColumn = null;
            foreach (string line in lines)
            {
                if (!String.IsNullOrEmpty(line))
                {
                    if (checkBoxColumn == null)
                    {
                        // Если объект DataGridCheckBoxColumn еще не создан, то создаем его
                        checkBoxColumn = new DataGridCheckBoxColumn();
                    }
                    checkBoxColumn.Header = line;
                    checkBoxColumn.Binding = new Binding(line);
                    MyDataGrid.Columns.Add(checkBoxColumn);
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = Search.Text;
            if (MyDataGrid != null && MyDataGrid.ItemsSource != null)
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(MyDataGrid.ItemsSource);
                if (!string.IsNullOrEmpty(searchValue))
                {
                    collectionView.Filter = item =>
                    {
                        var person = item as Сотрудники;
                        return (person != null &&
                                !string.IsNullOrEmpty(person.ФИО) &&
                                !string.IsNullOrEmpty(searchValue) &&
                                person.ФИО.ToLower().Contains(searchValue.ToLower()));
                    };
                }
                else
                {
                    collectionView.Filter = null;
                }
            }
        }

        private void MyDataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DeleteZ deletez = new DeleteZ();
            deletez.Show();
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DataContext is ViewModel viewModel)
            {
                viewModel.Save();
            }
        }

    }
}