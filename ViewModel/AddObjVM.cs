using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfNed.DTO;
using WpfNed.EF;
using WpfNed.Model;

namespace WpfNed.ViewModel
{
    internal class AddObjVM : INotifyPropertyChanged
    {
        private readonly TableModel tb;
        private readonly REObjModel tbObj;
        public ICommand AddObjInDBCommand { get; private set; }

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
        public AddObjVM()
        {
            tb = new TableModel();
            tbObj = new REObjModel();
            AddObjInDBCommand = new RelayCommand(AddObject);
        }
        public static event Action ObjectsUpdated;
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
            tbObj.AddObj( newObject );
            ObjectsUpdated.Invoke();
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}