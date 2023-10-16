using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.ViewModel;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Net;
using System.Text;
using Zarephath.Core.Models;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Zarephath.Core.Helpers
{
    public class MailChimpHelper
    {
        public void AddEmployeeToMailChimp(HC_AddEmployeeModel addEmployeeModel, string CompanyName, string DomainName)
        {
            var employee = addEmployeeModel.Employee;
            var mailChimpMember = new MailChimpModel();
            mailChimpMember.email_address = employee.Email;
            mailChimpMember.status = "subscribed";
            mailChimpMember.merge_fields = new MergeFields();
            mailChimpMember.merge_fields.FNAME = employee.FirstName;
            mailChimpMember.merge_fields.LNAME = employee.LastName;
            mailChimpMember.merge_fields.ADDRESS = employee.Address;
            mailChimpMember.merge_fields.PHONE = employee.PhoneHome==null? string.Empty: employee.PhoneHome;
            mailChimpMember.merge_fields.MMERGE9 = string.IsNullOrEmpty(employee.str_Role)? string.Empty : employee.str_Role;
            mailChimpMember.merge_fields.MMERGE5 = employee.EmployeeUniqueID;
            mailChimpMember.merge_fields.MMERGE6 = CompanyName;
            mailChimpMember.merge_fields.MMERGE7 = DomainName;

            var requestJson = JsonConvert.SerializeObject(mailChimpMember);
            var endpoint = $"{ConfigSettings.MailChimpApiURL}/lists/{ConfigSettings.MailChimpListId}/members";
            byte[] dataStream = Encoding.ASCII.GetBytes(requestJson);
            var responsetext = string.Empty;
            WebRequest request = HttpWebRequest.Create(endpoint);
            WebResponse response = null;
            try
            {
                request.ContentType = "application/json";
                string auth = "anystring" + ":" + ConfigSettings.MailChimpApiKey;
                auth = Convert.ToBase64String(Encoding.Default.GetBytes(auth));
                request.Headers["Authorization"] = "Basic " + auth;

                request.Method = "POST";
                request.ContentLength = dataStream.Length;
                Stream newstream = request.GetRequestStream();

                newstream.Write(dataStream, 0, dataStream.Length);
                newstream.Close();

                response = request.GetResponse();

                // get the result
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    JsonSerializer json = new JsonSerializer();
                    JObject content = JObject.Parse(reader.ReadToEnd());

                    responsetext = reader.ReadToEnd();
                }

                response.Close();
            }

            catch (WebException ex)
            {
               //By-pass the exception
            }

        }
        
    }
}
