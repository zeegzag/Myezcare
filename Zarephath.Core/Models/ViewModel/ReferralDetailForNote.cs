using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ReferralDetailForNote
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string StrDxCodes { get; set; }
        public string StrFacilities { get; set; }
        public string CaseManager { get; set; }
        

        public List<DXCodeMappingList> DxCodes
        {
            get
            {
                if (!string.IsNullOrEmpty(StrDxCodes))
                {
                    string[] items = StrDxCodes.Split(',');
                    var dxCodes = new List<DXCodeMappingList>();
                    foreach (var item in items)
                    {
                        string[] data = item.Split('|');
                        var dxCode = new DXCodeMappingList();
                        dxCode.ReferralDXCodeMappingID = Convert.ToInt64(data[0]);
                        dxCode.DXCodeID = data[1].Trim();
                        dxCode.DXCodeName = data[2].Trim();
                        dxCode.StartDate = Convert.ToDateTime(data[3], CultureInfo.InvariantCulture);
                        dxCode.EndDate = string.IsNullOrEmpty(data[4].Trim()) ? (DateTime?) null : Convert.ToDateTime(data[4], CultureInfo.InvariantCulture);
                        dxCode.Precedence = Convert.ToInt32(data[5]);
                        dxCode.DxCodeType = data[6].Trim();
                        dxCode.DxCodeShortName = data[7].Trim();
                        dxCode.DXCodeWithoutDot = data[8].Trim();
                        dxCodes.Add(dxCode);
                    }
                    return dxCodes;
                }
                return new List<DXCodeMappingList>();
            }
        }

        public List<NameValueData> Facilities
        {
            get
            {
                if (!string.IsNullOrEmpty(StrFacilities))
                {
                    string[] items = StrFacilities.Split(',');
                    var facilities = new List<NameValueData>();
                    foreach (var item in items)
                    {
                        string[] data = item.Split('|');
                        var facility = new NameValueData();
                        facility.Value = Convert.ToInt64(data[0]);
                        facility.Name = data[1];
                        facilities.Add(facility);
                    }
                    return facilities.OrderBy(c=>c.Name).ToList();
                }
                return new List<NameValueData>();
            }
        }
    }

    public class ClientForGroupNote
    {
        public ClientForGroupNote()
        {
            ReferralDetailForNote = new List<ReferralDetailForNote>();
            //Facilities = new List<NameValueData>();
        }
        public List<ReferralDetailForNote> ReferralDetailForNote { get; set; }
        //public List<NameValueData> Facilities { get; set; }
    }

    public class SaveGroupNoteModel
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string Phone1 { get; set; }
        public string Address { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public Note Note { get; set; }
        public PosDropdownModel SelectedServiceCodeForPayor { get; set; }
        public List<DXCodeMappingList> SelectedDxCodes { get; set; }

        [Ignore]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "DxCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int GroupNoteDxCodeCount { get { return SelectedDxCodes != null ? SelectedDxCodes.Count : 0; } }
    }



    public class ValidateServiceCodeModel
    {
        public DateTime ServiceDate { get; set; }
        public string ServiceCodeID { get; set; }
        public string PosID { get; set; }
        public string PayorCsv { get; set; }
    }

    public class InvalidPayorsList
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
    }
}
