using System.Collections.Generic;

namespace OopFactory.Edi835Parser.Models
{

    public class Edi271ResponseModel
    {
        public Edi271ResponseModel()
        {
            Edi271ModelList = new List<Edi271Model>();
        }

        public List<Edi271Model> Edi271ModelList { get; set; }
        public string GeneratedFileRelativePath { get; set; }
        public string GeneratedFileAbsolutePath { get; set; }
    }



    public class Edi271Model
    {
        public string FileName { get; set; }
        
        public string NM103_SourceName { get; set; }
        public string NM109_SourceID { get; set; }
        public string Source_ResponseCode { get; set; }
        public string Source_RejectReasonCode { get; set; }
        public string Source_FollowUpActionCode { get; set; }



        public string PER04_SourceContactInfo { get; set; }
        public string NM103_ReceiverName { get; set; }
        public string NM109_ReceiverID { get; set; }
        public string Receiver_ResponseCode { get; set; }
        public string Receiver_RejectReasonCode { get; set; }
        public string Receiver_FollowUpActionCode { get; set; }



        public string NM103_LastName { get; set; }
        public string NM104_FirstName { get; set; }
        public string NM109_AHCCCSID { get; set; }
        public string N301_Address { get; set; }
        public string N401_City { get; set; }
        public string N402_State { get; set; }
        public string N403_ZipCode { get; set; }
        public string DMG02_DOB { get; set; }
        public string DMG03_Gender { get; set; }
        public string Client_ResponseCode { get; set; }
        public string Client_RejectReasonCode { get; set; }
        public string Client_FollowUpActionCode { get; set; }


        public string Eligibile { get; set; }
        public string EligibilityGroup { get; set; }
        public string EligibilityDate { get; set; }
    }

   
}
