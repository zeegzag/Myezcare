using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class SetVehicleModel
    {
        public SetVehicleModel()
        {
            VehicleModel = new VehicleModel();
            TransportContactNameListModel = new List<TransportContactNameListModel>();
            VehicleTypeListModel = new List<VehicleTypeListModel>();
        }
        public VehicleModel VehicleModel { get; set; }
        public List<TransportContactNameListModel> TransportContactNameListModel { get; set; }
        public List<VehicleTypeListModel> VehicleTypeListModel { get; set; }
        public List<EmployeeListModel> EmployeeList { get; set; }
    }
    public class VehicleModel : BaseEntity
    {
        public long VehicleID { get; set; }

        [Display(Name = "VIN_Number", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "VIN_NumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VIN_Number { get; set; }
        public long? SeatingCapacity { get; set; }
        public string VehicleType { get; set; }

        [Display(Name = "BrandName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "BrandNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BrandName { get; set; }

        [Display(Name = "Model", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "ModelRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Model { get; set; }
        public string Color { get; set; }

        [Display(Name = "Attendent", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "AttendentRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Attendent { get; set; }

        [Required(ErrorMessageResourceName = "TransportServiceRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ContactID { get; set; }
        public string ContactType { get; set; }
        public string TransportService { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedVehicleID { get { return VehicleID > 0 ? Crypto.Encrypt(Convert.ToString(VehicleID)) : ""; } }
        public int Row { get; set; }
        public int Count { get; set; }
        public string note { get; set; }
        [Required(ErrorMessageResourceName = "EmployeeIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmployeeID { get; set; }
    }

    public class HC_SetVehicleListModel
    {
        public HC_SetVehicleListModel()
        {
            TransportContactTypeListModel = new List<TransportContactTypeListModel>();
            SearchVehicleModel = new SearchVehicleModel();
            DeleteFilter = new List<NameValueData>();
            IsPartial = false;
        }
        public List<TransportContactTypeListModel> TransportContactTypeListModel { get; set; }
        public SearchVehicleModel SearchVehicleModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public bool IsPartial { get; set; }
    }

    public class SearchVehicleModel
    {
        public long? VehicleID { get; set; }
        public string VIN_Number { get; set; }
        public long? SeatingCapacity { get; set; }
        public string VehicleType { get; set; }
        public string BrandName { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Attendent { get; set; }
        public string ContactID { get; set; }
        public string ContactType { get; set; }
        public string TransportService { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
    }

    public class VehicleTypeListModel
    {
        public string VehicleTypeID { get; set; }
        public string VehicleTypes { get; set; }
    }

}
