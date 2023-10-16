using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class PhysicianModel
    {
        public PhysicianModel()
        {
            Physician = new Physician();
            StateList = new List<State>();
            PhysicianTypeList = new List<PhysicianType>();
        }
        public Physician Physician { get; set; }
        public List<State> StateList { get; set; }
        public List<PhysicianType> PhysicianTypeList { get; set; }
    }

    public class SetPhysicianListPage
    {
        public SetPhysicianListPage()
        {
            SearchPhysicianListPage = new SearchPhysicianListPage();
            DeleteFilter = new List<NameValueData>();
        }
        public SearchPhysicianListPage SearchPhysicianListPage { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class SearchPhysicianListPage
    {
        public string PhysicianName { get; set; }
        public string NPINumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string PhysicianTypeID { get; set; }
        public string PhysicianTypeName { get; set; }
    }

    public class ListPhysicianModel
    {
        public string PhysicianID { get; set; }
        public string PhysicianName { get; set; }
        public string NPINumber { get; set; }
        public string EncryptedPhysicianID { get { return Crypto.Encrypt(Convert.ToString(PhysicianID)); } }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))
                    return String.Format("{0}, {1} - {2} {3}", Address, City, ZipCode, StateCode);

                return "";
            }
        }
        public string PhysicianTypeID { get; set; }
        public string PhysicianTypeName { get; set; }
    }
    public class PhysicianType
    {
        public string PhysicianTypeID { get; set; }
        public string PhysicianTypeName { get; set; }
    }
    public class APIPhysicianModel
    {
        public long PhysicianTypeID { get; set; }
        public string Specialist { get; set; }
        public string Name { get; set; }
        public string NPI { get; set; }
        public string PracticeAddress { get; set; }

    }

}
