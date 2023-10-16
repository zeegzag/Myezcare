using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ClaimAdjustmentTypes")]
    [PrimaryKey("ClaimAdjustmentTypeID")]
    [Sort("ClaimAdjustmentTypeID", "DESC")]
    public class ClaimAdjustmentType
    {
        public string ClaimAdjustmentTypeID { get; set; }
        public string ClaimAdjustmentTypeName { get; set; }
        public bool IsDeleted { get; set; }



        public static string ClaimAdjustmentType_IS = "IS";
        public static string ClaimAdjustmentType_Denial = "Denial";

        public static string ClaimAdjustmentType_Resend = "Resend";
        public static string ClaimAdjustmentType_Void = "Void";
        public static string ClaimAdjustmentType_Replacement = "Replacement";
        public static string ClaimAdjustmentType_WriteOff = "Write-Off";
        public static string ClaimAdjustmentType_Remove = "Remove";


        public static string ClaimAdjustmentType_PayorChange = "Payor-Change";
        public static string ClaimAdjustmentType_DataValidation = "Data-Validation";
        public static string ClaimAdjustmentType_Other = "Other";


        public static string ClaimAdjustmentLevel_Line = "line";
        public static string ClaimAdjustmentLevel_Claim = "claim";
        public static string ClaimAdjustmentLevel_Batch = "batch";


    }


}
