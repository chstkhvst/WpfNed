using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNed.Model;

namespace WpfNed.ViewModel
{
    public class ResVM
    {
        public ICommand DeleteObjCommand { get; }
        ObsTableModel tb = new ObsTableModel();
        private readonly ReservationModel tbObj;
        public ResVM()
        {

        }
    }
}
