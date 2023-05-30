using CarsApi.Infrastructure;
using CarsApiLib;
using CarsApiLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarsApi.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        private IlcatsApiManager ilcatsManager;

        private ObservableCollection<BrandCar> brands;
        public ObservableCollection<BrandCar> Brands
        {
            get
            {
                return brands;
            }
            set
            {
                brands = value;
                NotifyOfPropertyChanged();
            }
        }
        private BrandCar selectedBrand;
        public BrandCar SelectedBrand
        {
            get
            {
                return selectedBrand;
            }
            set
            {
                selectedBrand = value;
                NotifyOfPropertyChanged();
            }
        }

        private ObservableCollection<InfoModelCar> toyotaModels;
        public ObservableCollection<InfoModelCar> ToyotaModels
        {
            get
            {
                return toyotaModels;
            }
            set
            {
                toyotaModels = value;
                NotifyOfPropertyChanged();
            }
        }
        private InfoModelCar selectedToyotaInfoModel;
        public InfoModelCar SelectedToyotaInfoModel
        {
            get
            {
                return selectedToyotaInfoModel;
            }
            set
            {
                selectedToyotaInfoModel = value;
                NotifyOfPropertyChanged();
            }
        }

        private ObservableCollection<EquipmentModelCar> toyotaModelEquipments;
        public ObservableCollection<EquipmentModelCar> ToyotaModelEquipments
        {
            get
            {
                return toyotaModelEquipments;
            }
            set
            {
                toyotaModelEquipments = value;
                NotifyOfPropertyChanged();
            }
        }
        private EquipmentModelCar selectedToyotaModelEquipment;
        public EquipmentModelCar SelectedToyotaModelEquipment
        {
            get
            {
                return selectedToyotaModelEquipment;
            }
            set
            {
                selectedToyotaModelEquipment = value;
                NotifyOfPropertyChanged();
            }
        }

        private ModelCar selectedToyotaModel;
        public ModelCar SelectedToyotaModel
        {
            get
            {
                return selectedToyotaModel;
            }
            set
            {
                selectedToyotaModel = value;
                NotifyOfPropertyChanged();
            }
        }

        private ObservableCollection<GroupEquipment> groupsEquipment;
        public ObservableCollection<GroupEquipment> GroupsEquipment
        {
            get
            {
                return groupsEquipment;
            }
            set
            {
                groupsEquipment = value;
                NotifyOfPropertyChanged();
            }
        }
        private GroupEquipment selectedGroupEquipment;
        public GroupEquipment SelectedGroupEquipment
        {
            get
            {
                return selectedGroupEquipment;
            }
            set
            {
                selectedGroupEquipment = value;
                NotifyOfPropertyChanged();
            }
        }

        private ObservableCollection<ComplectationGroup> complectationsGroup;
        public ObservableCollection<ComplectationGroup> ComplectationsGroup
        {
            get
            {
                return complectationsGroup;
            }
            set
            {
                complectationsGroup = value;
                NotifyOfPropertyChanged();
            }
        }
        private ComplectationGroup selectedComplectationGroup;
        public ComplectationGroup SelectedComplectationGroup
        {
            get
            {
                return selectedComplectationGroup;
            }
            set
            {
                selectedComplectationGroup = value;
                NotifyOfPropertyChanged();
            }
        }

        private ObservableCollection<InfoPartComplectation> partsComplectation;
        public ObservableCollection<InfoPartComplectation> PartsComplectation
        {
            get
            {
                return partsComplectation;
            }
            set
            {
                partsComplectation = value;
                NotifyOfPropertyChanged();
            }
        }
        private InfoPartComplectation selectedPartComplectation;
        public InfoPartComplectation SelectedPartComplectation
        {
            get
            {
                return selectedPartComplectation;
            }
            set
            {
                selectedPartComplectation = value;
                NotifyOfPropertyChanged();
            }
        }

        private string visibilityGridBrands;
        public string VisibilityGridBrands
        {
            get
            {
                return visibilityGridBrands;
            }
            set
            {
                visibilityGridBrands = value;
                NotifyOfPropertyChanged();
            }
        }
        private string visibilityGridToyotaModes;
        public string VisibilityGridToyotaModes
        {
            get
            {
                return visibilityGridToyotaModes;
            }
            set
            {
                visibilityGridToyotaModes = value;
                NotifyOfPropertyChanged();
            }
        }
        private string visibilityGridToyotaModeEquipments;
        public string VisibilityGridToyotaModeEquipments
        {
            get
            {
                return visibilityGridToyotaModeEquipments;
            }
            set
            {
                visibilityGridToyotaModeEquipments = value;
                NotifyOfPropertyChanged();
            }
        }
        private string visibilityGridGroupsEquipment;
        public string VisibilityGridGroupsEquipment
        {
            get
            {
                return visibilityGridGroupsEquipment;
            }
            set
            {
                visibilityGridGroupsEquipment = value;
                NotifyOfPropertyChanged();
            }
        }
        private string visibilityGridComplectationsGroup;
        public string VisibilityGridComplectationsGroup
        {
            get
            {
                return visibilityGridComplectationsGroup;
            }
            set
            {
                visibilityGridComplectationsGroup = value;
                NotifyOfPropertyChanged();
            }
        }
        private string visibilityGridPartsComplectation;
        public string VisibilityGridPartsComplectation
        {
            get
            {
                return visibilityGridPartsComplectation;
            }
            set
            {
                visibilityGridPartsComplectation = value;
                NotifyOfPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ilcatsManager = new IlcatsApiManager();

            SelectedBrand = new BrandCar();
            SelectedToyotaInfoModel = new InfoModelCar();
            SelectedToyotaModelEquipment = new EquipmentModelCar();
            SelectedToyotaModel = new ModelCar();
            SelectedGroupEquipment = new GroupEquipment();

            VisibilityGridBrands = "Visible";
            VisibilityGridToyotaModes = "Hidden";
            VisibilityGridToyotaModeEquipments = "Hidden";
            VisibilityGridGroupsEquipment = "Hidden";
            VisibilityGridComplectationsGroup = "Hidden";
            VisibilityGridPartsComplectation = "Hidden";

            InitCommands();
        }

        private void InitCommands()
        {
            GetListBrandsCommand = new RelayCommand(param =>
            {
                Brands = ilcatsManager.GetListBrands();
                VisibilityGridBrands = "Visible";
                VisibilityGridToyotaModes = "Hidden";
                VisibilityGridToyotaModeEquipments = "Hidden";
                VisibilityGridGroupsEquipment = "Hidden";
                VisibilityGridComplectationsGroup = "Hidden";
                VisibilityGridPartsComplectation = "Hidden";
            });
            GetModelsToyotaCommand = new RelayCommand(param =>
            {
                ToyotaModels = ilcatsManager.GetModelsToyota();
                VisibilityGridBrands = "Hidden";
                VisibilityGridToyotaModes = "Visible";
                VisibilityGridToyotaModeEquipments = "Hidden";
                VisibilityGridGroupsEquipment = "Hidden";
                VisibilityGridComplectationsGroup = "Hidden";
                VisibilityGridPartsComplectation = "Hidden";
            });
            GetViewEquipmentsModelCommand = new RelayCommand(param =>
            {
                ToyotaModelEquipments = ilcatsManager.GetEquipmentsModelToyota(SelectedToyotaModel);
                VisibilityGridBrands = "Hidden";
                VisibilityGridToyotaModes = "Hidden";
                VisibilityGridToyotaModeEquipments = "Visible";
                VisibilityGridGroupsEquipment = "Hidden";
                VisibilityGridComplectationsGroup = "Hidden";
                VisibilityGridPartsComplectation = "Hidden";
            });
            GetGroupsEquipmentCommand = new RelayCommand(param =>
            {
                GroupsEquipment = ilcatsManager.GetGroupsEquipment(SelectedToyotaModelEquipment);
                VisibilityGridBrands = "Hidden";
                VisibilityGridToyotaModes = "Hidden";
                VisibilityGridToyotaModeEquipments = "Hidden";
                VisibilityGridGroupsEquipment = "Visible";
                VisibilityGridComplectationsGroup = "Hidden";
                VisibilityGridPartsComplectation = "Hidden";
            });
            GetComplectationsGroupCommand = new RelayCommand(param =>
            {
                ComplectationsGroup = ilcatsManager.GetComplectationsGroup(SelectedGroupEquipment);
                VisibilityGridBrands = "Hidden";
                VisibilityGridToyotaModes = "Hidden";
                VisibilityGridToyotaModeEquipments = "Hidden";
                VisibilityGridGroupsEquipment = "Hidden";
                VisibilityGridComplectationsGroup = "Visible";
                VisibilityGridPartsComplectation = "Hidden";
            });
            GetPartsComplectationCommand = new RelayCommand(param =>
            {
                PartsComplectation = ilcatsManager.GetPartsComplectation(SelectedComplectationGroup);
                VisibilityGridBrands = "Hidden";
                VisibilityGridToyotaModes = "Hidden";
                VisibilityGridToyotaModeEquipments = "Hidden";
                VisibilityGridGroupsEquipment = "Hidden";
                VisibilityGridComplectationsGroup = "Hidden";
                VisibilityGridPartsComplectation = "Visible";
            });
        }

        public ICommand GetListBrandsCommand { get; private set; } 
        public ICommand GetModelsToyotaCommand { get; private set; }
        public ICommand GetViewEquipmentsModelCommand { get; private set; }
        public ICommand GetGroupsEquipmentCommand { get; private set; }
        public ICommand GetComplectationsGroupCommand { get; private set; }
        public ICommand GetPartsComplectationCommand { get; private set; }


    }
}
