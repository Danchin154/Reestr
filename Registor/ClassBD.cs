
using Registor.SQL_data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace Registor
{
    public class ClassBD : DbContext, INotifyPropertyChanged
    {
        private ReestrPravDostupaISEntities db;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Должности> Должности { get; set; }
        public ObservableCollection<Сотрудники> Сотрудники { get; set; }
        public ObservableCollection<СПА> СПА { get; set; }
        public ObservableCollection<Уволенные> Уволенные { get; set; }


        public ReestrPravDostupaISEntities DBEntitys
        {
            get
            {
                return db;
            }
            set
            {
                db = value;
                OnPropertyChanged(nameof(DBEntitys));
            }
        }

        public ClassBD()
        {
            db = new ReestrPravDostupaISEntities();
            Должности = new ObservableCollection<Должности>(DBEntitys.Должности.ToList());
            Сотрудники = new ObservableCollection<Сотрудники>(DBEntitys.Сотрудники.ToList());
            СПА = new ObservableCollection<СПА>(DBEntitys.СПА.ToList());
            Уволенные = new ObservableCollection<Уволенные>(DBEntitys.Уволенные.ToList());
           
            
        }

        private void GetData()
        {
            foreach (var item in db.Сотрудники)
            {
                Сотрудники.Add(item);
            }
            foreach (var item in db.СПА)
            {
                СПА.Add(item);
            }
            foreach (var item in db.Уволенные)
            {
                Уволенные.Add(item);
            }
        }
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

       public void Save()
        {
            db.SaveChanges();
        }
    }
}

