using Registor.Model;
using Registor.SQL_data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Registor.ViewModels
{

    public class ViewModel : BaseVM
    {
        private ClassBD _bdConn;
        private Сотрудники _сотрудники;
        private СПА _спа;
        private Должности _должности;
        private bool _статус;

        public ClassBD BDConn
        {
            get
            {
                return _bdConn;
            }
            set
            {
                _bdConn = value;
                OnPropertyChanged(nameof(BDConn));
            }
        }

        public static List<СПА> SPAs { get; set; }
        public static List<Должности> Positions { get; set; }
        public static List<Сотрудники> Personals { get; set; }

        private ObservableCollection<string> _columnNames = new ObservableCollection<string>();

        public ObservableCollection<string> ColumnNames
        {
            get { return _columnNames; }
            set { _columnNames = value; OnPropertyChanged("ColumnNames"); }
        }

        public Сотрудники CurrentEmployee
        {
            get
            {

                return _сотрудники;
            }
            set
            {
                _сотрудники = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }

      

        public СПА SelectedSPA
        {
           get
            {
                return _спа;
            }
            set
            {
                _спа = value;
                OnPropertyChanged(nameof(SelectedSPA));
            }
        }

        public Должности SelectedPosition
        {
         get
            {
                return _должности;
            }
           set
           {
               _должности = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }
        


            public ViewModel() 
        {
            BDConn = new ClassBD();
            SPAs = new List<СПА>(BDConn.СПА.ToList());
            Positions = new List<Должности>(BDConn.Должности.ToList());
            Personals = new List<Сотрудники>(BDConn.Сотрудники.ToList());
            var employees = BDConn.Сотрудники.ToList();
            foreach (var employee in employees)
            {
                ColumnNames.Add(employee.ФИО);
            }
        }

       
        public void Save()
        {
            BDConn.Save();
        }


    }

    
}

