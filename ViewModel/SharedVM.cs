using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;
using WpfNed.Model;

namespace WpfNed.ViewModel
{
    using RealEstateObject = WpfNed.EF.REObject;
    public class SharedVM : INotifyPropertyChanged
    {
        public ObjectTableVM ObjectTableVM { get; set; }
        public UserVM UserVM { get; set; }
        public ContractVM ContractVM { get; set; }
        public ProfitReportVM ProfitReportVM { get; set; }

        TableModel tb = new TableModel();

        public SharedVM()
        {
            ObjectTableVM = new ObjectTableVM();
            UserVM = new UserVM();
            ContractVM = new ContractVM();
            ProfitReportVM = new ProfitReportVM();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region LISTS
        private List<Reservation> _reservations;
        public List<Reservation> Reservations
        {
            get
            {
                if (_reservations == null)
                {
                    _reservations = tb.GetReservations();
                    OnPropertyChanged(nameof(Reservations));
                }
                return _reservations;
            }
            set
            {
                if (_reservations != value)
                {
                    _reservations = value;
                    OnPropertyChanged(nameof(Reservations));
                }
            }
        }
        private List<ObjectType> _objectTypes;
        public List<ObjectType> ObjectTypes
        {
            get
            {
                if (_objectTypes == null)
                {
                    _objectTypes = tb.GetObjectTypes();
                    OnPropertyChanged(nameof(ObjectTypes));
                }
                return _objectTypes;
            }
            set
            {
                if (_objectTypes != value)
                {
                    _objectTypes = value;
                    OnPropertyChanged(nameof(ObjectTypes));
                }
            }
        }
        private List<DealType> _dealTypes;
        public List<DealType> DealTypes
        {
            get
            {
                if (_dealTypes == null)
                {
                    _dealTypes = tb.GetDealTypes();
                    OnPropertyChanged(nameof(DealTypes));
                }
                return _dealTypes;
            }
            set
            {
                if (_dealTypes != value)
                {
                    _dealTypes = value;
                    OnPropertyChanged(nameof(DealTypes));
                }
            }
        }
        private List<ResStatus> _resStatuses;
        public List<ResStatus> ResStatuses
        {
            get
            {
                if (_resStatuses == null)
                {
                    _resStatuses = tb.GetResStatuses();
                    OnPropertyChanged(nameof(ResStatuses));
                }
                return _resStatuses;
            }
        }
        private List<Status> _statuses;
        public List<Status> Statuses
        {
            get
            {
                if (_statuses == null)
                {
                    _statuses = tb.GetStatuses();
                    OnPropertyChanged(nameof(Statuses));
                }
                return _statuses;
            }
        }
        #endregion
    }
}
