using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNed.DTO;
using WpfNed.EF;
using WpfNed.Model;
using WpfNed.Services;
using System.IO;


namespace WpfNed.ViewModel
{
    public class ContractVM : INotifyPropertyChanged
    {

        TableModel tb = new TableModel();
        ContractModel cm = new ContractModel();
        private IWindowService _windowService;
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand EditObjCommand { get; }
        public ICommand UpdateObjCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand UpdObjInDBCommand { get; }
        public ContractVM()
        {
            //AddObjInDBCommand = new RelayCommand(AddObject);
            //UpdObjInDBCommand = new RelayCommand(UpdateObject);
            _windowService = new WindowService();
            DeleteObjCommand = new RelayCommand(DeleteSelectedObject);
            AddObjCommand = new RelayCommand(OpenAddObj);
            //EditObjCommand = new RelayCommand(OpenEditObj);
            RefreshObjCommand = new RelayCommand(RefreshObjects);
        }

        #region ТЕСТ ТЕСТ ТЕСТ ТЕСТ ТЕСТ ТЕСТ 
        private int _selectedReservationId;
        private int _selectedUserId;

        public int SelectedReservationId
        {
            get => _selectedReservationId;
            set
            {
                _selectedReservationId = value;
                OnPropertyChanged();
            }
        }

        public int SelectedUserId
        {
            get => _selectedUserId;
            set
            {
                _selectedUserId = value;
                OnPropertyChanged();
            }
        }

    #endregion

    #region ADDITIONAL LISTS
    private List<UserDTO> _users;
        public List<UserDTO> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = tb.GetUsersDTO().Where(u => u.RoleId == 1).ToList();
                    OnPropertyChanged(nameof(Users));
                }
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
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
            set
            {
                if (_reservations != value)
                {
                    _reservations = value;
                    OnPropertyChanged(nameof(Reservations));
                }
            }
        }
        #endregion
        private List<ContractDTO> _contracts;
        public List<ContractDTO> Contracts
        {
            get
            {
                if (_contracts == null)
                {
                    _contracts = tb.GetContractsDTO();
                    OnPropertyChanged(nameof(Contracts));
                }
                return _contracts;
            }
            set
            {
                if (_contracts != value)
                {
                    _contracts = value;
                    OnPropertyChanged(nameof(Contracts));
                }
            }
        }
        private ContractDTO _selectedContract;
        public ContractDTO SelectedContract
        {
            get => _selectedContract;
            set
            {
                _selectedContract = value;
                OnPropertyChanged(nameof(SelectedContract));
            }
        }

        public void OpenAddObj()
        {
            if (SelectedReservationId > 0 && SelectedUserId > 0)
            {
                var newContract = new ContractDTO
                {
                    SignDate = DateTime.Now,
                    ReservationId = SelectedReservationId,
                    UserId = SelectedUserId,
                    Total = CalculateTotal(SelectedReservationId),
                };
                cm.AddObj(newContract);
                RefreshObjects();
            }
        }

        private int CalculateTotal(int reservationId)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
            return reservation?.Object.Price ?? 0;
        }
        public void RefreshObjects()
        {
            Contracts.Clear();
            Contracts = tb.GetContractsDTO();
            OnPropertyChanged("Contracts");
            //Objects = new ObservableCollection<RealEstateObject>(tb.GetObjects());
        }
        private void DeleteSelectedObject()
        {
            if (Contracts != null)
            {
                cm.DeleteObject(SelectedContract);
                Contracts.Remove(SelectedContract);
                RefreshObjects();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
