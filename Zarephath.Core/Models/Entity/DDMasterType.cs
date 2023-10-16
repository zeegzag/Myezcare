using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    // NOTE: please enum check DDType
    [TableName("lu_DDMasterTypes")]
    [PrimaryKey("DDMasterTypeID")]
    [Sort("DDMasterTypeID", "DESC")]
    public class DDMasterType
    {
        public long DDMasterTypeID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsDisplayValue { get; set; }
        public long ParentID { get; set; }
    }


    public enum Enum_DDMasterType
    {
        Care_Type=1,
        Payer_Group,
        Business_Line,
        NPI_Options,
        Revenue_Code,
        Admission_Type,
        AdmissionSource,
        Patient_Status,
        Visit_Type,
        Facility_Code,
        Patient_Frequency_Code,
        Patient_System_Status,
        Task_Frequency_Code,
        Assessment_Question_Category,
        Assessment_Question_SubCategory,
        Gender,
        Language_Preference,
        Designation,
        Document_Section,
        Equipment
    }
}
