using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfNed.EF;
using WpfNed.Model;

namespace WpfNed.ViewModel
{
    using RealEstateObject = WpfNed.EF.REObject;
    internal class MWViewModel : INotifyPropertyChanged
    {
        TableModel tb = new TableModel();
        public event PropertyChangedEventHandler PropertyChanged;

        private List<RealEstateObject> _objects;
        public List<RealEstateObject> Objects
        {
            get
            {
                if (_objects == null)
                {
                    _objects = tb.GetObjects();
                    OnPropertyChanged(nameof(Objects));
                }
                return _objects;
            }
        }
        private List<RealEstateObject> _filteredObjects;
        public List<RealEstateObject> FilteredObjects
        {
            get => _filteredObjects;
            set
            {
                _filteredObjects = value;
                OnPropertyChanged(nameof(FilteredObjects));
            }
        }
        public void RefreshObjects()
        {
            _objects = tb.GetObjects();
            FilteredObjects = _objects;
            OnPropertyChanged(nameof(Objects));
        }
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
            FilteredObjects = Objects.Where(obj =>
                (SelectedObjectType == null || obj.ObjectType.Id == SelectedObjectType.Id) &&
                (SelectedRoomCount == 0 || obj.Rooms == SelectedRoomCount) &&
                (SelectedDealType == null || obj.DealType.Id == SelectedDealType.Id) &&
                (MinPrice == null || obj.Price >= MinPrice) &&
                (MaxPrice == null || obj.Price <= MaxPrice)
            ).ToList();
        }
        public MWViewModel()
        {
            LoadData();
            ApplyFiltersCommand = new RelayCommand(ApplyFilters);
        }
        public void LoadData()
        {
            ObjectTypes = tb.GetObjectTypes();
            DealTypes = tb.GetDealTypes();
            RefreshObjects();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
