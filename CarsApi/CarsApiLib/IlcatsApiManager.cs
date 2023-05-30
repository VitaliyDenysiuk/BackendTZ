using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Text;
using CarsApiLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CarsApiLib
{
    public class IlcatsApiManager
    {
        private readonly NetworkManager networkManager;
        JsonSerializerOptions options;

        public IlcatsApiManager()
        {
            networkManager = new NetworkManager();

            options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
        }


        private ObservableCollection<BrandCar> GetBrands(string[]str)
        {
            ObservableCollection<BrandCar> brands = new ObservableCollection<BrandCar>();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == "/a")
                {
                    brands.Add(new BrandCar
                    {
                        Name = str[i - 1]
                    });
                }
            }
            for (int i = 0; i < 3; i++)
            {
                brands.RemoveAt(0);
                brands.RemoveAt(brands.Count - 1);
            }
            return brands;
        }

        public ObservableCollection<BrandCar> GetListBrands()
        {
            ObservableCollection<BrandCar> brands = null;

            try
            {
                string url = $"{IlcatsApiConfig.BaseUrl}";

                string json = networkManager.GetHtml(url);
                string[] str = json.Split('<', '>');

                brands = GetBrands(str);
            }
            catch (Exception)
            {
            }
            return brands;
        }



        private ObservableCollection<ObservableCollection<string>> GetArrModelsHtml(string[] str)
        {
            ObservableCollection<string> modelsCar;
            ObservableCollection<ObservableCollection<string>> arrModelsCar = new ObservableCollection<ObservableCollection<string>>();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Contains(@"div class=""name"""))
                {
                    modelsCar = new ObservableCollection<string>();
                    for (int j = i; j < str.Length; j++)
                    {
                        i++;
                        if (str[i].Contains(@"div class=""name"""))
                        {
                            arrModelsCar.Add(modelsCar);
                            i--;
                            break;
                        }
                        if (j==str.Length-1)
                        {
                            arrModelsCar.Add(modelsCar);
                            break;
                        }
                        modelsCar.Add(str[j]);
                        i = j;
                    }
                }
            }

            return arrModelsCar;
        }

        private InfoModelCar GetInfoModel(ObservableCollection<string> str)
        {
            InfoModelCar modelCar = new InfoModelCar();
            modelCar.ModelCars = new List<ModelCar>();
            int model = -1;

            for (int i = 0; i < str.Count; i++)
            {
                if (str[i].Contains(@"div class=""name"""))
                {
                    modelCar.Name = str[i].Substring(str[i].IndexOf(">") + 1);
                }

                if (str[i].Contains(@"a href=""/toyota/?"))
                {
                    modelCar.ModelCars.Add(new ModelCar
                    {
                        Code = str[i].Substring(str[i].IndexOf(">") + 1)
                    });
                    model++;
                }
                if (str[i].Contains(@"div class=""dateRange"""))
                {
                    string date = str[i].Substring(str[i].IndexOf(">") + 1);
                    modelCar.ModelCars[model].Date = date.Replace("&nbsp;","");
                }
                if (str[i].Contains(@"div class=""modelCode"""))
                {
                    modelCar.ModelCars[model].Equipment = str[i].Substring(str[i].IndexOf(">") + 1);
                }
            }

            return modelCar;
        }

        private ObservableCollection<InfoModelCar> GetCarModels(ObservableCollection<ObservableCollection<string>> brands)
        {
            ObservableCollection<InfoModelCar> modelCars = new ObservableCollection<InfoModelCar>();
            for (int i = 0; i < brands.Count; i++)
            {
                modelCars.Add(GetInfoModel(brands[i]));
            }
            return modelCars;
        }

        public ObservableCollection<InfoModelCar> GetModelsToyota()
        {
            ObservableCollection<InfoModelCar> modelsCar = null;
            ObservableCollection<ObservableCollection<string>> arrModelsCar = null;

            try
            {
                //string url = $"{IlcatsApiConfig.BaseUrl}{brand.ToLower()}/?function=getModels&market=EU";
                string url = $"{IlcatsApiConfig.BaseUrl}toyota/?function=getModels&market=EU";

                string html = networkManager.GetHtml(url);
                string[] str = html.Split('<');

                arrModelsCar = GetArrModelsHtml(str);

                modelsCar = GetCarModels(arrModelsCar);
            }
            catch (Exception)
            {
            }
            return modelsCar;
        }




        private ObservableCollection<ObservableCollection<string>> GetArrEquipmentsHtml(string[] str)
        {
            ObservableCollection<string> equipmentsCar;
            ObservableCollection<ObservableCollection<string>> arrEquipmentsCar = new ObservableCollection<ObservableCollection<string>>();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Contains(@"div class=""modelCode"""))
                {
                    equipmentsCar = new ObservableCollection<string>();
                    for (int j = i; j < str.Length; j++)
                    {
                        i++;
                        if (str[i].Contains(@"div class=""modelCode"""))
                        {
                            arrEquipmentsCar.Add(equipmentsCar);
                            i--;
                            break;
                        }
                        if (j == str.Length - 1)
                        {
                            arrEquipmentsCar.Add(equipmentsCar);
                            break;
                        }
                        equipmentsCar.Add(str[j]);
                        i = j;
                    }
                }
            }

            return arrEquipmentsCar;
        }

        private EquipmentModelCar GetInfoEquipment(ObservableCollection<string> str)
        {
            EquipmentModelCar modelCar = new EquipmentModelCar();

            for (int i = 0, j = 1; i < str.Count; i++)
            {
                if (str[i].Contains(@"div class=""modelCode"""))
                {
                    modelCar.Name = str[i+1].Substring(str[i+1].IndexOf(">") + 1);
                }
                if (str[i].Contains(@"div class=""dateRange"""))
                {
                    string date = str[i].Substring(str[i].IndexOf(">") + 1);
                    modelCar.Date = date.Replace("&nbsp;", "");
                }
                if (str[i].Contains(@"div class=""01"""))
                {
                    modelCar.Engine = str[i].Substring(str[i].IndexOf(">") + 1);
                }
                if (str[i].Contains(@"div class=""03"""))
                {
                    modelCar.Body = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""04"""))
                {
                    modelCar.Grade = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""05"""))
                {
                    modelCar.Transmission = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""06"""))
                {
                    modelCar.GearShift = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""07"""))
                {
                    modelCar.DriverPosition = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""08"""))
                {
                    modelCar.NumOfDoor = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
                if (str[i].Contains(@"div class=""09"""))
                {
                    modelCar.Destination1 = str[i].Substring(str[i].IndexOf(">") + 1);
                    j++;
                }
            }

            return modelCar;
        }

        private ObservableCollection<EquipmentModelCar> GetCarModelEquipments(ObservableCollection<ObservableCollection<string>> brands)
        {
            ObservableCollection<EquipmentModelCar> carModelEquipments = new ObservableCollection<EquipmentModelCar>();
            for (int i = 0; i < brands.Count; i++)
            {
                carModelEquipments.Add(GetInfoEquipment(brands[i]));
            }
            return carModelEquipments;
        }

        public ObservableCollection<EquipmentModelCar> GetEquipmentsModelToyota(ModelCar modelCar)
        {
            ObservableCollection<EquipmentModelCar> carModelEquipments = null;
            ObservableCollection<ObservableCollection<string>> arrModelEquipments = null;

            try
            {

                string dateStart = modelCar.Date.Split('-')[0];
                string dateEnd = modelCar.Date.Split('-')[1];

                dateStart = dateStart.Split('.')[1] + dateStart.Split('.')[0];
                dateEnd = dateEnd.Split('.')[1] + dateEnd.Split('.')[0];

                string url = $"{IlcatsApiConfig.BaseUrl}toyota/?function=getComplectations&market=EU&model={modelCar.Code}&startDate={dateStart}&endDate={dateEnd}";

                string html = networkManager.GetHtml(url);
                string[] str = html.Split('<');

                arrModelEquipments = GetArrEquipmentsHtml(str);

                carModelEquipments = GetCarModelEquipments(arrModelEquipments);

                for (int i = 0; i < carModelEquipments.Count; i++)
                {
                    carModelEquipments[i].ModelCar = modelCar;
                    if (i < 9)
                    {
                        carModelEquipments[i].ComponentCod = $"00{i+1}";
                    }
                    else
                    {
                        if (i < 99)
                        {
                            carModelEquipments[i].ComponentCod = $"0{i + 1}";
                        }
                        else
                        {
                            if (i < 999)
                            {
                                carModelEquipments[i].ComponentCod = $"{i + 1}";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return carModelEquipments;
        }



        private ObservableCollection<GroupEquipment> GetGroups(string[] str)
        {
            ObservableCollection<GroupEquipment> groups = new ObservableCollection<GroupEquipment>();

            for (int i = 0,j = 1; i < str.Length; i++)
            {
                if (str[i].Contains(@"div class=""name"""))
                {
                    groups.Add(new GroupEquipment
                    {
                        GroupId = j++,
                        Name = str[i+1].Split('>')[1]
                    });
                }
            }
            return groups;
        }

        public ObservableCollection<GroupEquipment> GetGroupsEquipment(EquipmentModelCar equipmentModelCar)
        {
            ObservableCollection<GroupEquipment> groups = null;

            try
            {
                string url = $"{IlcatsApiConfig.BaseUrl}toyota/?function=getGroups&language=en&market=EU&model={equipmentModelCar.ModelCar.Code}&modification={equipmentModelCar.Name}&complectation={equipmentModelCar.ComponentCod}";

                string html = networkManager.GetHtml(url);
                string[] str = html.Split('<');

                groups = GetGroups(str);

                for (int i = 0; i < groups.Count; i++)
                {
                    groups[i].EquipmentModel = equipmentModelCar;
                }
            }
            catch (Exception)
            {
            }

            return groups;
        }


        private ObservableCollection<ComplectationGroup> GetComplectations(string[] str)
        {
            ObservableCollection<ComplectationGroup> complectations = new ObservableCollection<ComplectationGroup>();

            for (int i = 0, j = 0; i < str.Length; i++)
            {
                if (str[i].Contains(@"div class=""image"""))
                {
                    complectations.Add(new ComplectationGroup
                    {
                        Image = "https:" + str[i + 2].Split('"')[1]
                    });
                }
                if (str[i].Contains(@"div class=""image"""))
                {
                    string []arr = str[i + 1].Split('=','&');
                    for (int a = 0; a < arr.Length; a++)
                    {
                        if (arr[a].Contains("subgroup"))
                        {
                            complectations[j].SubGroup = arr[a + 1];
                        }
                    }
                }
                if (str[i].Contains(@"div class=""name"""))
                {
                    complectations[j].Name = str[i + 1].Split('>')[1];
                    j++;
                }
            }
            return complectations;
        }

        public ObservableCollection<ComplectationGroup> GetComplectationsGroup(GroupEquipment group)
        {
            ObservableCollection<ComplectationGroup> complectationsGroup = null;

            try
            {
                string url = $"{IlcatsApiConfig.BaseUrl}toyota/?function=getSubGroups&market=EU&model={group.EquipmentModel.ModelCar.Code}&modification={group.EquipmentModel.Name}&complectation={group.EquipmentModel.ComponentCod}&group={group.GroupId}&language=en";

                string html = networkManager.GetHtml(url);
                string[] str = html.Split('<');

                complectationsGroup = GetComplectations(str);
                for (int i = 0; i < complectationsGroup.Count; i++)
                {
                    complectationsGroup[i].GroupEquipment = group;
                }
            }
            catch (Exception)
            {
            }
            return complectationsGroup;
        }



        private ObservableCollection<ObservableCollection<string>> GetArrPartsHtml(string[] str)
        {
            ObservableCollection<string> partsComplectation;
            ObservableCollection<ObservableCollection<string>> arrPartsComplectation = new ObservableCollection<ObservableCollection<string>>();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Contains(@"th colspan=""4"""))
                {
                    partsComplectation = new ObservableCollection<string>();
                    for (int j = i; j < str.Length; j++)
                    {
                        i++;
                        if (str[i].Contains(@"th colspan=""4"""))
                        {
                            arrPartsComplectation.Add(partsComplectation);
                            i--;
                            break;
                        }
                        if (j == str.Length - 1)
                        {
                            arrPartsComplectation.Add(partsComplectation);
                            break;
                        }
                        partsComplectation.Add(str[j]);
                        i = j;
                    }
                }
            }

            return arrPartsComplectation;
        }

        private InfoPartComplectation GetInfoPartsComplectation(ObservableCollection<string> str)
        {
            InfoPartComplectation partsComplectation = new InfoPartComplectation();
            partsComplectation.PartsComplectation = new List<PartComplectation>();

            for (int i = 0, j = 0; i < str.Count; i++)
            {
                if (str[i].Contains(@"th colspan=""4"""))
                {
                    string data = str[i].Substring(str[i].IndexOf(">") + 1);
                    string[] arr = data.Replace("nbsp;", "").Split('&');
                    partsComplectation.TreeCode = arr[0];
                    partsComplectation.Tree = arr[1];
                }
                if (str[i].Contains(@"div class=""number"""))
                {
                    string date = str[i+1].Substring(str[i+1].IndexOf(">") + 1);
                    partsComplectation.PartsComplectation.Add(new PartComplectation
                    {
                        Code = date
                    });
                }
                if (str[i].Contains(@"div class=""count"""))
                {
                    partsComplectation.PartsComplectation[j].Count = str[i].Substring(str[i].IndexOf(">") + 1);
                }
                if (str[i].Contains(@"div class=""dateRange"""))
                {
                    string date = str[i].Substring(str[i].IndexOf(">") + 1);
                    partsComplectation.PartsComplectation[j].Date = date.Replace("&nbsp;", "");
                }
                if (str[i].Contains(@"div class=""usage"""))
                {
                    partsComplectation.PartsComplectation[j].Info = str[i].Substring(str[i].IndexOf(">") + 1);
                    if (str[i+1].Contains("br/"))
                    {
                        partsComplectation.PartsComplectation[j].Info += ", " + str[i+1].Substring(str[i+1].IndexOf(">") + 1);
                    }
                    j++;
                }

            }

            return partsComplectation;
        }

        private ObservableCollection<InfoPartComplectation> GetParts(ObservableCollection<ObservableCollection<string>> brands)
        {
            ObservableCollection<InfoPartComplectation> partsComplectation = new ObservableCollection<InfoPartComplectation>();
            for (int i = 0; i < brands.Count; i++)
            {
                partsComplectation.Add(GetInfoPartsComplectation(brands[i]));
            }
            return partsComplectation;
        }

        public ObservableCollection<InfoPartComplectation> GetPartsComplectation(ComplectationGroup complectationGroup)
        {

            ObservableCollection<ObservableCollection<string>> arrPartsComplectation = null;
            ObservableCollection<InfoPartComplectation> partsComplectation = null;

            try
            {
                string url = $"{IlcatsApiConfig.BaseUrl}toyota/?function=getParts&market=EU" +
                    $"&model={complectationGroup.GroupEquipment.EquipmentModel.ModelCar.Code}" +
                    $"&modification={complectationGroup.GroupEquipment.EquipmentModel.Name}" +
                    $"&complectation={complectationGroup.GroupEquipment.EquipmentModel.ComponentCod}" +
                    $"&group={complectationGroup.GroupEquipment.GroupId}" +
                    $"&language=en&subgroup={complectationGroup.SubGroup}";

                string html = networkManager.GetHtml(url);
                string[] str = html.Split('<');

                arrPartsComplectation = GetArrPartsHtml(str);

                partsComplectation = GetParts(arrPartsComplectation);
                for (int i = 0; i < partsComplectation.Count; i++)
                {
                    partsComplectation[i].ComplectationGroup = complectationGroup;
                }

            }
            catch (Exception)
            {
            }

            return partsComplectation;
        }

    }
}
