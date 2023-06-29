using Registor.SQL_data;
using System.Collections.ObjectModel;

namespace Registor.Model
{
    public class DataGridItem
    {

        public string Название { get; set; }
        public Сотрудники сотрудник { get; set; }
        public bool IsChecked { get; set; }
    }
}
