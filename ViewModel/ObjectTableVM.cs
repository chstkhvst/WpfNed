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

        private List<REObjectDTO> _objects;
        public List<REObjectDTO> Objects
        {
            get
            {
                if (_objects == null)
                {
                    _objects = tb.GetObjectsDTO();
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
        private REObjectDTO _selectedObject;
        public REObjectDTO SelectedObject
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
            SelectedObject = new REObjectDTO();
            _windowService.ShowWindow("AddObj", this);
        }
        public void OpenEditObj()
        {
            if (SelectedObject != null)
            {
                SelectedObject = new REObjectDTO(SelectedObject);
                _windowService.ShowWindow("EditObj", this);
            }
        }
        public void RefreshObjects()
        {
            Objects.Clear();
            Objects = tb.GetObjectsDTO();
            OnPropertyChanged("Objects");
            //Objects = new ObservableCollection<RealEstateObject>(tb.GetObjects());
        }
        private void DeleteSelectedObject()
        {
            if (Objects != null)
            {
                tbObj.DeleteObject(SelectedObject);
                Objects.Remove(SelectedObject);
                RefreshObjects();
            }
        }
        public void AddObject()
        {

            tbObj.AddObj(SelectedObject);
            RefreshObjects();
        }
        public void UpdateObject()
        {
            tbObj.UpdObj(SelectedObject);
            RefreshObjects();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
