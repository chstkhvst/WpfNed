using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNed.Model;
using WpfNed.EF;
using WpfNed.Services;
using System.Collections.ObjectModel;
using WpfNed.DTO;

namespace WpfNed.ViewModel
{
    using RealEstateObject = WpfNed.EF.REObject;
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        TableModel tb = new TableModel();
        private IWindowService _windowService;
        public ICommand DeleteObjCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand EditObjCommand { get; }
        public ICommand UpdateObjCommand { get; }
        public EmployeeViewModel()
        {
            _windowService = new WindowService();
            AddObjVM.ObjectsUpdated += RefreshObjects;
            DeleteObjCommand = new RelayCommand(DeleteSelectedObject);
            AddObjCommand = new RelayCommand(OpenAddObj);
            EditObjCommand = new RelayCommand(OpenEditObj);
            //EditObjCommand = new RelayCommand(UpdateObj);
        }

        // КРАД ДЛЯ ОБЪЕКТА
        #region CRUD FOR OBJECTS

        private ObservableCollection<RealEstateObject> _objects;
        public ObservableCollection<RealEstateObject> Objects
        {
            get
            {
                if (_objects == null)
                {
                    _objects = new ObservableCollection<RealEstateObject>(tb.GetObjects());
                    OnPropertyChanged(nameof(Objects));
                }
                return _objects;
            }
        }
        private RealEstateObject _selectedObject;
        public RealEstateObject SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                OnPropertyChanged(nameof(SelectedObject));
            }
        }
        public void OpenAddObj()
        {
            _windowService.ShowWindow("AddObj");
        }
        public void OpenEditObj()
        {
            if (SelectedObject != null)
            {
                _windowService.ShowWindow("EditObj"); 
            }
        }
        public void RefreshObjects()
        {
            ObservableCollection<RealEstateObject> objects = new ObservableCollection<RealEstateObject>(tb.GetObjects());
            OnPropertyChanged(nameof(Objects));
            Objects.Clear();
            foreach (var obj in objects)
            {
                Objects.Add(obj); 
            }
        }
        private void DeleteSelectedObject()
        {
            //if (Objects != null)
            //{
            //    tb.DeleteObject(SelectedObject);
            //    Objects.Remove(SelectedObject);
            //}
        }
        #endregion
        private List<Contract> _contracts;
        public List<Contract> Contracts
        {
            get
            {
                if (_contracts == null)
                {
                    _contracts = tb.GetContracts();
                    OnPropertyChanged(nameof(Contracts));
                }
                return _contracts;
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
        }
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
        }
        private List<User> _users;
        public List<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = tb.GetUsers();
                    OnPropertyChanged(nameof(Users));
                }
                return _users;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
