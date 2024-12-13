using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNed.EF;
using WpfNed.Model;
using WpfNed.DTO;
using WpfNed.Services;
using PdfSharp.Pdf;
using System.Data.SqlTypes;
using System.Windows;

namespace WpfNed.ViewModel
{
    using RealEstateObject = WpfNed.EF.REObject;
    internal class MWViewModel : INotifyPropertyChanged
    {
        IWindowService _windowService;
        public ICommand AddReservationInDBCommand { get; }
        TableModel tb = new TableModel();
        ReservationModel rm = new ReservationModel();
        public MWViewModel()
        {
            LoadData();
            _windowService = new WindowService();
            ApplyFiltersCommand = new RelayCommand(ApplyFilters);
            AddReservationInDBCommand = new RelayCommand<Window>(AddRes);
            UpdateFilteredObjects();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<REObjectDTO> _objects;
        public List<REObjectDTO> Objects
        {
            get
            {
                if (_objects == null)
                {
                    _objects = tb.GetObjectsDTO();
                    //LinkImagesToObjects();
                    OnPropertyChanged(nameof(Objects));
                }
                return _objects;
            }
        }
        private List<REObjectDTO> _filteredObjects;
        public List<REObjectDTO> FilteredObjects
        {
            get => _filteredObjects;
            set
            {
                _filteredObjects = value;
                OnPropertyChanged(nameof(FilteredObjects));
            }
        }
        public void UpdateFilteredObjects()
        {
            if (_objects == null)
                return;
            FilteredObjects = _objects.Where(obj => obj.StatusId == 1).ToList();
        }
        public void RefreshObjects()
        {
            _objects = tb.GetObjectsDTO();
            //_images = tb.GetObjectImages();
            FilteredObjects = _objects;
            OnPropertyChanged(nameof(Objects));
        }
        //private void LinkImagesToObjects()
        //{
        //    if (_images == null) _images = tb.GetObjectImages();
        //    foreach (var obj in _objects)
        //    {
        //        obj.MainImage = _images.FirstOrDefault(img => img.ObjectId == obj.Id);
        //    }
        //}
        public ObjectType SelectedObjectType { get; set; }
        public int SelectedRoomCount { get; set; }
        public DealType SelectedDealType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public ICommand ApplyFiltersCommand { get; private set; }

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
        private List<int> _roomCounts;
        public List<int> RoomCounts
        {
            get
            {
                if (_roomCounts == null)
                {
                    _roomCounts = new List<int> { 1, 2, 3, 4, 5 };
                    OnPropertyChanged(nameof(RoomCounts));
                }
                return _roomCounts;
            }
        }
        private void ApplyFilters()
        {
            //FilteredObjects = Objects.Where(obj =>
            //    (SelectedObjectType == null || obj.ObjectType.Id == SelectedObjectType.Id) &&
            //    (SelectedRoomCount == 0 || obj.Rooms == SelectedRoomCount) &&
            //    (SelectedDealType == null || obj.DealType.Id == SelectedDealType.Id) &&
            //    (MinPrice == null || obj.Price >= MinPrice) &&
            //    (MaxPrice == null || obj.Price <= MaxPrice) &&
            //    (obj.StatusId == 1)
            //).ToList();
            FilteredObjects = tb.ApplyFiltres(SelectedObjectType?.Id, SelectedRoomCount, SelectedDealType?.Id, MinPrice, MaxPrice);
        }
        public void LoadData()
        {
            ObjectTypes = tb.GetObjectTypes();
            DealTypes = tb.GetDealTypes();
            RefreshObjects();
        }

        #region BRONIROVANIE
        public string FullName { get; set; }
        public string Passport { get; set; }
        public string Phone { get; set; }
        public DateTime? StartDate { get; set; }
        private ICommand _OpenAdCommand;

        public ICommand OpenAdCommand
        {
            get
            {
                if (_OpenAdCommand == null)
                {
                    _OpenAdCommand = new RelayCommand<REObjectDTO>(OpenBookDetails);
                }
                return _OpenAdCommand;
            }
        }
        private ReservationDTO _selectedReservation;
        public ReservationDTO SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }
        private REObjectDTO selObj;
        private void OpenBookDetails(REObjectDTO selectedObject)
        {
            if (selectedObject == null)
                return;
            selObj = selectedObject;
            SelectedReservation = new ReservationDTO();
            _windowService.ShowWindow("OpenAd", this);
        }
        public void AddRes(Window w)
        {
            int usId = rm.FindUser(FullName);
            if (usId == 0)
            {
                MessageBox.Show("Невозможно создать бронь: пользователь не зарегистрирован в системе!", "Ошибка!");
            }
            else if (string.IsNullOrEmpty(Phone) || Phone.Length != 11)
                MessageBox.Show("Пожалуйста, введите номер телефона корректно.", "Ошибка!");
            else if (string.IsNullOrEmpty(Passport) || Passport.Length != 10)
                MessageBox.Show("Пожалуйста, введите номер паспорта корректно.", "Ошибка!");
            else if (StartDate == null)
                MessageBox.Show("Пожалуйста, введите желаемую дату заезда.", "Ошибка!");
            else
            {
                SelectedReservation.ObjectId = selObj.Id;
                SelectedReservation.UserId = usId;
                SelectedReservation.StartDate = StartDate;
                rm.AddRes(SelectedReservation);
                w.Close();
                MessageBox.Show("Ваша заявка принята!", "Успешно!");
                FilteredObjects = tb.ApplyFiltres(SelectedObjectType?.Id, SelectedRoomCount, SelectedDealType?.Id, MinPrice, MaxPrice);
            }
            
        }
        #endregion


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
