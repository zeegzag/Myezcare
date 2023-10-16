using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    public class ClaimMDMessageCheckModel
    {
        public bool ServiceCallAllowed { get; set; }
        public long LastResponseID { get; set; }
    }
    public class ClaimMDResponse
    {
        public UC_Result result { get; set; }
    }

    #region LatestERA
    public class Latest_ERA_Result
    {
        public Latest_ERA_Result() {
            era = new List<Latest_ERA>();
        }
        public dynamic e_last_eraid { get; set; }
        public List<Latest_ERA> era { get; set; }
    }


    public class Latest_ERA_Result_Single
    {
        public Latest_ERA_Result_Single()
        {
            era = new Latest_ERA();
        }
        public dynamic e_last_eraid { get; set; }
        public Latest_ERA era { get; set; }
    }
    public class Latest_ERA
    {
        public string e_check_number { get; set; }
        public string e_check_type { get; set; }
        public string e_claimmd_prov_name { get; set; }
        public string e_download_time { get; set; }
        public string e_eraid { get; set; }
        public string e_paid_amount { get; set; }
        public string e_paid_date { get; set; }
        public string e_payer_name { get; set; }
        public string e_payerid { get; set; }
        public string e_prov_name { get; set; }
        public string e_prov_npi { get; set; }
        public string e_prov_taxid { get; set; }
        public string e_received_time { get; set; }
    }
    #endregion

    #region ERA_PDF
    public class ERAPDFResponse
    {
        //public UC_ERA_Result result { get; set; }
        public ERA_PDF_Result result { get; set; }
    }
    public class ERA_PDF_Result
    {
        public string e_check_number { get; set; }
        public string e_eraid { get; set; }
        public string e_paid_amount { get; set; }
        public string e_paid_date { get; set; }
        public string e_payer_name { get; set; }
        public string e_payerid { get; set; }
        public string e_prov_name { get; set; }
        public string e_prov_npi { get; set; }
        public string data { get; set; }
    }



    public class PayerListResponse
    {
        public PayerListResponse() {

            result = new PayerList_Result();
        }
        public PayerList_Result result { get; set; }
    }

    public class PayerEnrollResponse
    {
        public PayerEnrollResponse()
        {

            result = new PayerEnroll_Result();
        }
        public PayerEnroll_Result result { get; set; }
        public string LogMessage { get; set; }
    }

    public class PayerEnroll_Result
    {
        public PayerEnroll_Result() {
            error = new PayerEnroll_Error();
        }
        public string success { get; set; }
        public dynamic link { get; set; }


        public PayerEnroll_Error error { get; set; }
        


    }


    public class PayerEnroll_Error
    {
        public string error_code { get; set; }
        public string error_mesg { get; set; }

    }


    public class PayerList_Result
    {
        //public PayerList_Result()
        //{
        //    payer = new List<Latest_Payer>();
        //}
        public dynamic payer { get; set; }
    }


    public class Latest_Payer
    {

        public Latest_Payer()
        {
            //payer_alt_names = new List<Latest_AltPayer>();
        }
        public string payerid { get; set; }
        public string payer_name { get; set; }


        public string payer_state  { get; set; }
        public dynamic payer_alt_names { get; set; }
        
    }


    public class Latest_AltPayer
    {

        public Latest_AltPayer()
        {
        }
        public string alt_payerid { get; set; }
        public string alt_payer_name { get; set; }
    }





    public class ERAResponse
    {
        public ERA_Result result { get; set; }
    }


    public class NonProcessedERA
    {
        public string EraID{ get; set; }
    }

    public class ERA_Result
    {
        public string era_check_number { get; set; }
        public string era_eraid { get; set; }
        public string era_paid_amount { get; set; }
        public string era_paid_date { get; set; }
        public string era_payer_name { get; set; }
        public string era_payerid { get; set; }
        public string era_prov_name { get; set; }
        public string era_prov_npi { get; set; }
        public string data { get; set; }
    }



    public class ERA_PDF
    {
        public string data { get; set; }
    }
    #endregion

    public class ERAMDResponse
    {
        public ERAMDResponse() {
            result = new Latest_ERA_Result();
        }
        //public UC_ERA_Result result { get; set; }
        public Latest_ERA_Result result { get; set; }
    }

    public class ERAMDResponse_Single
    {
        public ERAMDResponse_Single()
        {
            result = new Latest_ERA_Result_Single();
        }
        //public UC_ERA_Result result { get; set; }
        public Latest_ERA_Result_Single result { get; set; }
    }

    public class UC_ERA_Result
    {
        public dynamic e_last_responseid { get; set; }
        public List<UC_ERA> claim { get; set; }
        public UC_ERA singleClaim { get; set; }
    }
    public class UC_ERA
    {
        public string e_batchid { get; set; }
        public string e_bill_npi { get; set; }
        public string e_bill_taxid { get; set; }
        public string e_claimid { get; set; }
        public string e_claimmd_id { get; set; }
        public string e_fdos { get; set; }
        public string e_fileid { get; set; }
        public string e_filename { get; set; }
        public string e_ins_number { get; set; }
        public string e_payerid { get; set; }
        public string e_pcn { get; set; }
        public string e_remote_claimid { get; set; }
        public string e_response_time { get; set; }
        public string e_sender_icn { get; set; }
        public string e_sender_name { get; set; }
        public string e_senderid { get; set; }
        public string e_status { get; set; }
        public string e_total_charge { get; set; }
        public dynamic messages { get; set; }
        public List<UC_Message> MessagesList { get; set; }
        public UC_Message Message { get; set; }
    }
    public class UC_ERA_Message
    {
        public string e_fields { get; set; }
        public string e_mesgid { get; set; }
        public string e_message { get; set; }
        public string e_responseid { get; set; }
        public string e_status { get; set; }
    }
    public class UC_Result
    {
        public UC_Error error { get; set; }
        public string e_messages { get; set; }

        public string e_last_responseid { get; set; }
        public dynamic claim { get; set; }
        public List<UC_Claims> UploadedClaims { get; set; }
        public UC_Claims UploadedClaim { get; set; }
    }

    public class ClaimMD_NotesResponse
    {
        public UC_NoteModel result { get; set; }
        
    }

    public class UC_NoteModel
    {


        public string e_LastNoteID { get; set; }
        public dynamic notes { get; set; }
        public List<UC_Note> UC_NoteList { get; set; }
        public UC_Note UC_Note { get; set; }
    }



    public class UC_Note
    {
        public string e_note { get; set; }
        public string e_claimmd_id { get; set; }
        public string e_claimid { get; set; }
        public string e_username { get; set; }
        public string e_date_time { get; set; }


        public string e_noteid { get; set; }
        public string e_pcn { get; set; }

    }

    public class UC_Error
    {
        public string e_error_code { get; set; }
        public string e_error_mesg { get; set; }
    }
    public class UC_Claims
    {
        public string e_batchid { get; set; }
        public string e_bill_npi { get; set; }
        public string e_bill_taxid { get; set; }
        public string e_claimid { get; set; }
        public string e_claimmd_id { get; set; }
        public string e_fdos { get; set; }
        public string e_fileid { get; set; }
        public string e_filename { get; set; }
        public string e_ins_number { get; set; }
        public string e_payerid { get; set; }
        public string e_pcn { get; set; }
        public string e_remote_claimid { get; set; }
        public string e_sender_icn { get; set; }
        public string e_sender_name { get; set; }
        public string e_senderid { get; set; }
        public string e_status { get; set; }
        public string e_total_charge { get; set; }
        public dynamic messages { get; set; }

        public List<UC_Message> MessagesList { get; set; }
        public UC_Message Message { get; set; } //{ get { return JsonConvert.DeserializeObject<UC_Claims>(messages); } }



        public List<UC_Message> E_MessagesList { get; set; }
        public UC_Message E_Message { get; set; } //{ get { return JsonConvert.DeserializeObject<UC_Claims>(messages); } }




    }
    public class UC_Message
    {
        public string e_fields { get; set; }
        public string e_mesgid { get; set; }
        public string e_message { get; set; }
        public string e_status { get; set; }
    }

    public class UC_ArchieveResult
    {
        public US_ArchieveStatusResult result { get; set; }
    }

    public class US_ArchieveStatusResult
    {
        public string claimid { get; set; }
        public string delete_status { get; set; }
        public string success { get; set; }
    }
}
