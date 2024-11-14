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
    public class ObjectTableVM : INotifyPropertyChanged
    {
        TableModel tb = new TableModel();
        private readonly REObjModel tbObj;
        private IWindowService _windowService;
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand EditObjCommand { get; }
        public ICommand UpdateObjCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand UpdObjInDBCommand { get; }

        // Свойства для привязки данных
        public int Rooms { get; set; }
        public int Floors { get; set; }
        public int Square { get; set; }
        public int TypeId { get; set; }
        public int DealTypeId { get; set; }
        public string Street { get; set; }
        public int Building { get; set; }
        public int? Number { get; set; }
        public int Price { get; set; }
        public int OwnerId { get; set; }
        public int StatusId { get; set; }
        public ObjectTableVM()
        {
            tbObj = new REObjModel();
            AddObjInDBCommand = new RelayCommand(AddObject);
            UpdObjInDBCommand = new RelayCommand(UpdateObject);
            _windowService = new WindowService();
            DeleteObjCommand = new RelayCommand(DeleteSelectedObject);
            AddObjCommand = new RelayCommand(OpenAddObj);
            EditObjCommand = new RelayCommand(OpenEditObj);
            RefreshObjCommand = new RelayCommand(RefreshObjects);
            //EditObjCommand = new RelayCommand(UpdateObj);
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

        private List<Owner> _owners;
        public List<Owner> Owners
        {
            get
            {
                if (_owners == null)
                {
                    _owners = tb.GetOwners();
                    OnPropertyChanged(nameof(Owners));
                }
                return _owners;
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
            set
            {
                if (_objects != value)
                {
                    _objects = value;
                    OnPropertyChanged(nameof(Objects));
                }
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
                if (_selectedObject != null)
                {
                    _selectedObject.Owner = tb.GetOwners().FirstOrDefault(o => o.Id == _selectedObject.OwnerId);
                    OnPropertyChanged(nameof(SelectedObject.Owner)); // Уведомляем об изменении владельца
                }
            }
        }
        private REObjectDTO _SselectedObject;
        public REObjectDTO SSelectedObject
        {
            get => _SselectedObject;
            set
            {
                _SselectedObject = value;
                OnPropertyChanged(nameof(SSelectedObject));
                //if (_SselectedObject != null)
                //{
                //    _selectedObject.Owner = tb.GetOwners().FirstOrDefault(o => o.Id == _selectedObject.OwnerId);
                //    OnPropertyChanged(nameof(SelectedObject.Owner)); // Уведомляем об изменении владельца
                //}
            }
        }
        public void OpenAddObj()
        {
            _windowService.ShowWindow("AddObj", this);
        }
        public void OpenEditObj()
        {
            if (SelectedObject != null)
            {
                _windowService.ShowWindow("EditObj", this);
            }
        }
        public void RefreshObjects()
        {
            Objects = new ObservableCollection<RealEstateObject>(tb.GetObjects());
            //OnPropertyChanged(nameof(Objects));
            //Objects.Clear();
            //foreach (var obj in objects)
            //{
            //    Objects.Add(obj);
            //}
            //var updatedObjects = tb.GetObjects();
            //Objects.Clear(); 
            //foreach (var obj in updatedObjects)
            //{
            //    Objects.Add(obj); 
            //}
            //OnPropertyChanged(nameof(Objects));
        }
        private void DeleteSelectedObject()
        {
            if (Objects != null)
            {
                tbObj.DeleteObject(SelectedObject);
                Objects.Remove(SelectedObject);
            }
        }
        public void AddObject()
        {
            var newObject = new REObjectDTO
            {
                Rooms = this.Rooms,
                Floors = this.Floors,
                Square = (int)this.Square,
                TypeId = (int)this.TypeId,
                DealTypeId = (int)this.DealTypeId,
                Street = this.Street,
                Building = (int)this.Building,
                Number = this.Number == 0 ? (int?)null : this.Number,
                Price = (int)this.Price,
                OwnerId = (int)this.OwnerId,
                StatusId = 1
            };
            tbObj.AddObj(newObject);
            RefreshObjects();
        }
        public void UpdateObject()
        {
            var newObject = new REObjectDTO
            {
                Id = SelectedObject.Id,
                Rooms = SelectedObject.Rooms,
                Floors = SelectedObject.Floors,
                Square = SelectedObject.Square,
                TypeId = SelectedObject.TypeId,
                DealTypeId = SelectedObject.DealTypeId,
                Street = SelectedObject.Street,
                Building = SelectedObject.Building,
                Number = SelectedObject.Number,
                Price = SelectedObject.Price,
                OwnerId = SelectedObject.OwnerId,
                StatusId = SelectedObject.StatusId
            };
            tbObj.UpdObj(newObject);
            RefreshObjects();
            SelectedObject = Objects.FirstOrDefault(o => o.Id == newObject.Id);
            if (SelectedObject != null)
            {
                SelectedObject.Owner = tb.GetOwners().FirstOrDefault(o => o.Id == SelectedObject.OwnerId);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
