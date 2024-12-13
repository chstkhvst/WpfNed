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
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

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
        public ICommand SelectImageCommand { get; }

        public ObjectTableVM()
        {
            tbObj = new REObjModel();
            AddObjInDBCommand = new RelayCommand<Window>(AddObject);
            UpdObjInDBCommand = new RelayCommand<Window>(UpdateObject);
            _windowService = new WindowService();
            DeleteObjCommand = new RelayCommand(DeleteSelectedObject);
            AddObjCommand = new RelayCommand(OpenAddObj);
            EditObjCommand = new RelayCommand(OpenEditObj);
            RefreshObjCommand = new RelayCommand(RefreshObjects);
            SelectImageCommand = new RelayCommand(SelectImage);
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

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
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

        private ObjectImage _img = new ObjectImage(); 
        public ObjectImage Img
        {
            get => _img;
            set
            {
                _img = value;
                OnPropertyChanged(nameof(Img));
            }
        }

        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                var bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                SelectedImage = bitmap;

                // Сохранение изображения в формате byte[]
                var imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                Img.ObjImage = imageBytes; 
            }
        }


        public void AddObject(Window w)
        {
            if (SelectedObject.Square > 500 || SelectedObject.Square == 0)
                MessageBox.Show("Пожалуйста, введите реальное значение площади.", "Ошибка!");
            else if (SelectedObject.Price == 0 || SelectedObject.Price < 5000 || SelectedObject.Price > 50000000)
                MessageBox.Show("Пожалуйста, введите реальную стоимость.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedObject.Street) || SelectedObject.Street.Length > 35 || !Regex.IsMatch(SelectedObject.Street, @"^[а-яА-Яa-zA-Z\s]+$"))
                MessageBox.Show("Пожалуйста, введите корректно улицу.", "Ошибка!");
            else if (SelectedObject.Building == 0 || SelectedObject.Building > 199)
                MessageBox.Show("Пожалуйста, введите корректно номер здания.", "Ошибка!");
            else if (SelectedObject.Number != null && (SelectedObject.Building == 0 || SelectedObject.Number > 600))
                MessageBox.Show("Пожалуйста, введите корректный номер квартиры.", "Ошибка!");
            else if (SelectedObject.Rooms == 0)
                MessageBox.Show("Пожалуйста, выберите количество комнат.", "Ошибка!");
            else if (SelectedObject.Floors == 0)
                MessageBox.Show("Пожалуйста, выберите количество этажей.", "Ошибка!");
            else if (SelectedObject.DealTypeId == 0)
                MessageBox.Show("Пожалуйста, выберите тип сделки.", "Ошибка!");
            else if (SelectedObject.TypeId == 0)
                MessageBox.Show("Пожалуйста, выберите тип объекта.", "Ошибка!");
            else if (SelectedObject.OwnerId == 0)
                MessageBox.Show("Пожалуйста, выберите владельца.", "Ошибка!");
            else if (Img == null)
                MessageBox.Show("Пожалуйста, добавьте изображение.", "Ошибка!");
            else
            {
                tbObj.AddObj(SelectedObject, Img);
                RefreshObjects();
                w.Close();
            }
        }
        public void UpdateObject(Window w)
        {
            if (SelectedObject.Square > 500 || SelectedObject.Square == 0)
                MessageBox.Show("Пожалуйста, введите реальное значение площади.", "Ошибка!");
            else if (SelectedObject.Price == 0 || SelectedObject.Price < 5000 || SelectedObject.Price > 50000000)
                MessageBox.Show("Пожалуйста, введите реальную стоимость.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedObject.Street) || SelectedObject.Street.Length > 35 || !Regex.IsMatch(SelectedObject.Street, @"^[а-яА-Яa-zA-Z\s]+$"))
                MessageBox.Show("Пожалуйста, введите корректно улицу.", "Ошибка!");
            else if (SelectedObject.Building == 0 || SelectedObject.Building > 199)
                MessageBox.Show("Пожалуйста, введите корректно номер здания.", "Ошибка!");
            else if (SelectedObject.Number != null && (SelectedObject.Building == 0 || SelectedObject.Number > 600))
                MessageBox.Show("Пожалуйста, введите корректный номер квартиры.", "Ошибка!");
            else if (SelectedObject.Rooms == 0)
                MessageBox.Show("Пожалуйста, выберите количество комнат.", "Ошибка!");
            else if (SelectedObject.Floors == 0)
                MessageBox.Show("Пожалуйста, выберите количество этажей.", "Ошибка!");
            else if (SelectedObject.DealTypeId == 0)
                MessageBox.Show("Пожалуйста, выберите тип сделки.", "Ошибка!");
            else if (SelectedObject.TypeId == 0)
                MessageBox.Show("Пожалуйста, выберите тип объекта.", "Ошибка!");
            else if (SelectedObject.OwnerId == 0)
                MessageBox.Show("Пожалуйста, выберите владельца.", "Ошибка!");
            else
            {
                tbObj.UpdObj(SelectedObject);
                RefreshObjects();
                w.Close();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
