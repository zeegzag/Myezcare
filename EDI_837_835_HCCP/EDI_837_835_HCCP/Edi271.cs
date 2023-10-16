using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using OopFactory.Edi835Parser.Models;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;
using OopFactory.X12.Validation;
using OopFactory.Edi835Parser.Models;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;
using TypedLoopNM1 = OopFactory.X12.Parsing.Model.Typed.TypedLoopNM1;

namespace EDI_837_835_HCCP
{
    public class Edi271
    {
        #region CORE METHODS


        public Edi271ResponseModel GenerateEdi271Model(string absolutePathForEdi271File, string absolutePathForGenerateReadableFile, string relativePathForGenerateReadableFile)
        {
            Edi271ResponseModel edi271ResponseModel = new Edi271ResponseModel()
                {
                    GeneratedFileAbsolutePath = absolutePathForGenerateReadableFile,
                    GeneratedFileRelativePath = relativePathForGenerateReadableFile
                };

            #region Parse 271 File

            Stream transformStream = null;
            Stream inputStream = new FileStream(absolutePathForEdi271File, FileMode.Open, FileAccess.Read);
            transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EDI_837_835_HCCP.Transformations.X12-271-XML-to-CSV.xslt");


            X12Parser parser = new X12Parser();
            Interchange interchange = parser.Parse(inputStream);
            interchange.SerializeToX12(true);
            string xml = interchange.Serialize();

            var transform = new XslCompiledTransform();
            transform.Load(XmlReader.Create(transformStream));
            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("filename", "", new FileInfo(absolutePathForEdi271File).Name);

            arguments.AddExtensionObject(CustomXsltExtensionFunctions.Namespace, new CustomXsltExtensionFunctions());

            MemoryStream mstream = new MemoryStream();
            transform.Transform(XmlReader.Create(new StringReader(xml)), arguments, mstream);
            mstream.Flush();
            mstream.Position = 0;
            string content = new StreamReader(mstream).ReadToEnd();
            {
                FileInfo outputFileInfo = new FileInfo(absolutePathForGenerateReadableFile);
                if (!Directory.Exists(outputFileInfo.DirectoryName))
                    Directory.CreateDirectory(outputFileInfo.DirectoryName);



                string filename = String.Format("{0}\\{1}{2}", outputFileInfo.DirectoryName, outputFileInfo.Name.Replace(outputFileInfo.Extension, ""), outputFileInfo.Extension);
                using (Stream outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(outputStream))
                    {
                        writer.Write(content);
                        writer.Close();
                    }
                    outputStream.Close();
                }


                using (CsvReader reader = new CsvReader(filename))
                {
                    int index = 0;
                    foreach (string[] vals in reader.RowEnumerator)
                    {
                        //Skip first 1 row
                        if (index >= 1)
                        {
                            var i = 0;
                            Edi271Model model = new Edi271Model();

                            //model.FileName = vals[i++];
                            model.NM103_SourceName = vals[i++];
                            model.PER04_SourceContactInfo = vals[i++];
                            model.Source_ResponseCode = vals[i++];
                            model.Source_RejectReasonCode = GetRejectResponseFromCode(vals[i++]);
                            model.Source_FollowUpActionCode = vals[i++];

                            model.NM103_ReceiverName = vals[i++];
                            model.Receiver_ResponseCode = vals[i++];
                            model.Receiver_RejectReasonCode = GetRejectResponseFromCode(vals[i++]);
                            model.Receiver_FollowUpActionCode = vals[i++];

                            model.NM103_LastName = vals[i++];
                            model.NM104_FirstName = vals[i++];
                            model.NM109_AHCCCSID = vals[i++];
                            model.DMG03_Gender = vals[i++];
                            model.DMG02_DOB = vals[i++];
                            model.N301_Address = vals[i++];
                            model.N402_State = vals[i++];
                            model.N403_ZipCode = vals[i++];
                            model.Client_ResponseCode = vals[i++];
                            model.Client_RejectReasonCode = GetRejectResponseFromCode(vals[i++]);
                            model.Client_FollowUpActionCode = vals[i++];


                            model.Eligibile = vals[i++];
                            model.EligibilityGroup = vals[i++];
                            model.EligibilityDate = vals[i++];




                            edi271ResponseModel.Edi271ModelList.Add(model);
                        }
                        index++;
                    }
                }
            }


            #endregion Parse 271 File

            return edi271ResponseModel;
        }


        private string GetRejectResponseFromCode(string value)
        {
            value = value.Trim().ToLower();
            switch (value)
            {
                //Source Response Code
                case "4": return "Number of requests exceeded";
                case "04": return "Number of requests exceeded";

                //Receiver Response Code
                case "41": return "Submitter falls below success rate threshold.";
                case "43": return "Invalid or missing NPI or not found.";
                case "50": return "Provider is Terminated/Pended status or excluded.";
                case "51": return "NPI/Provider ID not found";

                //CLIENT RESPONSE CODE
                case "15": return "Did not supply minimum search criteria for subscriber. No elig/enroll data is returned.";
                case "57": return "Invalid Begin/End date used in request. No elig/enroll data is returned.";
                case "60": return "DOB is after DOS";
                case "61": return "DOD precedes DOS";
                case "62": return "DOS is more than 12 months in past";
                case "63": return "Begin DOS is in future or if end DOS is more than 60 days in future";
                case "72": return "Multiple matches found or no match on SSN or Medicare Claim ID; AHCCCS ID is different (data will still be returned); Prisoner ID used in search or a subscriber was found and has a Prisoner ID (No data will be returned)";
                default: return value;
            }
        }

        #endregion


    }


    public class CustomXsltExtensionFunctions
    {
        public const string Namespace = "http://XsltSampleSite.XsltFunctions/1.0";



        public string GetRejectResponseFromCode(string value)
        {
            value = value.Trim();
            switch (value)
            {
                //Source Response Code
                case "4": return "4: Number of requests exceeded";
                case "04": return "04: Number of requests exceeded";

                //Receiver Response Code
                case "41": return "41: Authorization/Access Restrictions.";
                case "43": return "43: Invalid or missing NPI or not found.";
                case "50": return "50: Provider is Terminated/Pended status or excluded.";
                case "51": return "51: NPI/Provider ID not found";

                //CLIENT RESPONSE CODE
                case "15": return "15: Required application data missing.";
                case "42": return "42: Unable to Respond at Current Time";
                //case "43": return "43: Invalid/Missing Provider Identification";
                case "45": return "45: Invalid/Missing Provider Specialty";
                case "47": return "47: Invalid/Missing Provider State";
                case "48": return "48: Invalid/Missing Referring Provider Identification Number";
                case "49": return "49: Provider is Not Primary Care Physician";
                //case "51": return "51: Provider Not on File";
                case "52": return "52: Service Dates Not Within Provider Plan Enrollment";
                case "56": return "56: Inappropriate Date";
                case "57": return "57: Invalid/Missing Date(s) of Service.";
                case "58": return "58: Invalid/Missing Date-of-Birth";
                case "60": return "60: DOB is after DOS,Date of Birth Follows Date(s) of Service";
                case "61": return "61: DOD precedes DOS";
                case "62": return "62: DOS is more than 12 months in past";
                case "63": return "63: Date of Service in Future";

                case "64": return "64: Invalid/Missing Patient ID";
                case "65": return "63: Invalid/Missing Patient Name";
                case "66": return "66: Invalid/Missing Patient Gender Code";
                case "67": return "67: Patient Not Found";
                case "68": return "68: Duplicate Patient ID Number";
                case "71": return "71: Patient Birth Date Does Not Match That for the Patient on the Database";
                case "72": return "72: Invalid/Missing Subscriber/Insured ID";

                case "73": return "73: Invalid/Missing Subscriber/Insured Name";
                case "74": return "74: Invalid/Missing Subscriber/Insured Gender Code";
                case "75": return "75: Subscriber/Insured Not Found";
                case "76": return "76: Duplicate Subscriber/Insured ID Number";
                case "77": return "77: Subscriber Found, Patient Not Found";
                case "78": return "78: Subscriber/Insured Not in Group/Plan Identified";
                default: return value;
            }
        }
        public string GetFollowUpActionCode(string value)
        {
            value = value.Trim();
            switch (value)
            {
                //Source Response Code
                case "C": return "C: Please Correct and Resubmit";
                case "N": return "N: Resubmission Not Allowed";
                case "R": return "R: Resubmission Allowed";
                case "S": return "S: Do Not Resubmit; Inquiry Initiated to a Third Party";
                case "W": return "W: Please Wait 30 Days and Resubmit";
                case "X": return "X: Please Wait 10 Days and Resubmit";
                case "Y": return "Y: Do Not Resubmit; We Will Hold Your Request and Respond Again Shortly";

                default: return value;
            }
        }
        public string GetEligibilityStatus(string value)
        {
            value = value.Trim();
            switch (value)
            {
                //Source Response Code

                case "1": return "Active Coverage";
                case "2": return "Active - Full Risk Capitation";
                case "3": return "Active - Services Capitated";
                case "4": return "Active - Services Capitated to Primary Care Physician";
                case "5": return "Active - Pending Investigation";
                case "6": return "Inactive";
                case "7": return "Inactive - Pending Eligibility Update";
                case "8": return "Inactive - Pending Investigation";
                case "A": return "Co-Insurance";
                case "B": return "Co-Payment";
                case "C": return "Deductible";
                case "D": return "Benefit Description";
                case "E": return "Exclusions";
                case "F": return "Limitations";
                case "G": return "Out of Pocket (Stop Loss)";
                case "H": return "Unlimited";
                case "I": return "Non-Covered";
                case "J": return "Cost Containment";
                case "K": return "Reserve";
                case "L": return "Primary Care Provider";
                case "M": return "Pre-existing Condition";
                case "N": return "Services Restricted to Following Provider";
                case "O": return "Not Deemed a Medical Necessity";
                case "P": return "Benefit Disclaimer";
                case "Q": return "Second Surgical Opinion Required";
                case "R": return "Other or Additional Payor";
                case "S": return "Prior Year(s) History";
                case "T": return "Card(s) Reported Lost/Stolen";
                case "U": return "Contact Following Entity for Eligibility or Benefit Information";
                case "V": return "Cannot Process";
                case "W": return "Other Source of Data";
                case "X": return "Health Care Facility";
                case "Y": return "Spend Down";
                case "CB": return "Coverage Basis";
                case "MC": return "Managed Care Coordinator";
                default: return "Active Coverage";
            }
        }


        public string GetCoverageLevelFromCode(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "CHD": return "Children Only";
                case "DEP": return "Dependents Only";
                case "ECH": return "Employee and Children";
                case "EMP": return "Employee Only";
                case "ESP": return "Employee and Spouse";
                case "FAM": return "Family";
                case "IND": return "Individual";
                case "SPC": return "Spouse and Children";
                case "SPO": return "Spouse Only";
                default: return value;
            }
        }
        public string GetInsuranceTypeFromCode(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "D": return "Disability";
                case "12": return "Medicare Secondary Working Aged Beneficiary or Spouse with Employer Group Health Plan";
                case "13": return "Medicare Secondary End-Stage Renal Disease Beneficiary in the Mandated Coordination Period with an Employer's Group Health Plan";
                case "14": return "Medicare Secondary, No-fault Insurance including Auto is Primary";
                case "15": return "Medicare Secondary Worker's Compensation";
                case "16": return "Medicare Secondary Public Health Service (PHS)or Other Federal Agency";
                case "41": return "Medicare Secondary Black Lung";
                case "42": return "Medicare Secondary Veteran's Administration";
                case "43": return "Medicare Secondary Disabled Beneficiary Under Age 65 with Large Group Health Plan (LGHP)";
                case "47": return "Medicare Secondary, Other Liability Insurance is Primary";
                case "AP": return "Auto Insurance Policy";
                case "C1": return "Commercial";
                case "CO": return "Consolidated Omnibus Budget Reconciliation Act (COBRA)";
                case "CP": return "Medicare Conditionally Primary";
                case "DB": return "Disability Benefits";
                case "EP": return "Exclusive Provider Organization";
                case "FF": return "Family or Friends";
                case "GP": return "Group Policy";
                case "HM": return "Health Maintenance Organization (HMO)";
                case "HN": return "Health Maintenance Organization (HMO) - Medicare Risk";
                case "HS": return "Special Low Income Medicare Beneficiary";
                case "IN": return "Indemnity";
                case "IP": return "Individual Policy";
                case "LC": return "Long Term Care";
                case "LD": return "Long Term Policy";
                case "LI": return "Life Insurance";
                case "LT": return "Litigation";
                case "MA": return "Medicare Part A";
                case "MB": return "Medicare Part B";
                case "MC": return "Medicaid";
                case "MH": return "Medigap Part A";
                case "MI": return "Medigap Part B";
                case "MP": return "Medicare Primary";
                case "OT": return "Other";
                case "PE": return "Property Insurance - Personal";
                case "PL": return "Personal";
                case "PP": return "Personal Payment (Cash - No Insurance)";
                case "PR": return "Preferred Provider Organization (PPO)";
                case "PS": return "Point of Service (POS)";
                case "QM": return "Qualified Medicare Beneficiary";
                case "RP": return "Property Insurance - Real";
                case "SP": return "Supplemental Policy";
                case "TF": return "Tax Equity Fiscal Responsibility Act (TEFRA)";
                case "WC": return "Workers Compensation";
                case "WU": return "Wrap Up Policy";
                default: return value;
            }
        }


        public string GetServiceCodeTypeFromCode(string value)
        {
            value = value.Trim();
            switch (value)
            {
                case "1": return "Medical Care";
                case "2": return "Surgical";
                case "3": return "Consultation";
                case "4": return "Diagnostic X-Ray";
                case "5": return "Diagnostic Lab ";
                case "6": return "Radiation Therapy ";
                case "7": return "Anesthesia ";
                case "8": return "Surgical Assistance";
                case "9": return "Other Medical";
                case "10": return "Blood Charges";
                case "11": return "Used Durable Medical Equipment";
                case "12": return "Durable Medical Equipment Purchase";
                case "13": return "Ambulatory Service Center Facility";
                case "14": return "Renal Supplies in the Home";
                case "15": return "Alternate Method Dialysis";
                case "16": return "Chronic Renal Disease (CRD) Equipment";
                case "17": return "Pre-Admission Testing";
                case "18": return "Durable Medical Equipment Rental";
                case "19": return "Pneumonia Vaccine";
                case "20": return "Second Surgical Opinion";
                case "21": return "Third Surgical Opinion";
                case "22": return "Social Work";
                case "23": return "Diagnostic Dental";
                case "24": return "Periodontics";
                case "25": return "Restorative";
                case "26": return "Endodontics";
                case "27": return "Maxillofacial Prosthetics";
                case "28": return "Adjunctive Dental Services";
                case "30": return "Health Benefit Plan Coverage";
                case "32": return "Plan Waiting Period";
                case "33": return "Chiropractic";
                case "34": return "Chiropractic Office Visits";
                case "35": return "Dental Care";
                case "36": return "Dental Crowns";
                case "37": return "Dental Accident";
                case "38": return "Orthodontics";
                case "39": return "Prosthodontics";
                case "40": return "Oral Surgery";
                case "41": return "Routine (Preventive) Dental";
                case "42": return "Home Health Care";
                case "43": return "Home Health Prescriptions";
                case "44": return "Home Health Visits";
                case "45": return "Hospice";
                case "46": return "Respite Care";
                case "47": return "Hospital";
                case "48": return "Hospital-Inpatient";
                case "49": return "Hospital-Room and Board";
                case "50": return "Hospital-Outpatient";
                case "51": return "Hospital-Emergency Accident";
                case "52": return "Hospital-Emergency Medical";
                case "53": return "Hospital-Ambulatory Surgical";
                case "54": return "Long Term Care";
                case "55": return "Major Medical";
                case "56": return "Medically Related Transportation";
                case "57": return "Air Transportation";
                case "58": return "Cabulance";
                case "59": return "Licensed Ambulance";
                case "60": return "General Benefits";
                case "61": return "In-vitro Fertilization";
                case "62": return "MRI/CAT Scan";
                case "63": return "Donor Procedures";
                case "64": return "Acupuncture";
                case "65": return "Newborn Care";
                case "66": return "Pathology";
                case "67": return "Smoking Cessation";
                case "68": return "Well Baby Care";
                case "69": return "Maternity";
                case "70": return "Transplants";
                case "71": return "Audiology Exam";
                case "72": return "Inhalation Therapy";
                case "73": return "Diagnostic Medical";
                case "74": return "Private Duty Nursing";
                case "75": return "Prosthetic Device";
                case "76": return "Dialysis";
                case "77": return "Otological Exam";
                case "78": return "Chemotherapy";
                case "79": return "Allergy Testing";
                case "80": return "Immunizations";
                case "81": return "Routine Physical";
                case "82": return "Family Planning";
                case "83": return "Infertility";
                case "84": return "Abortion";
                case "85": return "AIDS";
                case "86": return "Emergency Services";
                case "87": return "Cancer";
                case "88": return "Pharmacy";
                case "89": return "Free Standing Prescription Drug";
                case "90": return "Mail Order Prescription Drug";
                case "91": return "Brand Name Prescription Drug";
                case "92": return "Generic Prescription Drug";
                case "93": return "Podiatry";
                case "94": return "Podiatry - Office Visits";
                case "95": return "Podiatry - Nursing Home Visits";
                case "96": return "Professional (Physician)";
                case "97": return "Anesthesiologist";
                case "98": return "Professional (Physician) Visit - Office";
                case "99": return "Professional (Physician) Visit - Inpatient";
                case "A0": return "Professional (Physician) Visit - Outpatient";
                case "A1": return "Professional (Physician) Visit - Nursing Home";
                case "A2": return "Professional (Physician) Visit - Skilled Nursing Facility";
                case "A3": return "Professional (Physician) Visit - Home";
                case "A4": return "Psychiatric";
                case "A5": return "Psychiatric - Room and Board";
                case "A6": return "Psychotherapy";
                case "A7": return "Psychiatric - Inpatient";
                case "A8": return "Psychiatric - Outpatient";
                case "A9": return "Rehabilitation";
                case "AA": return "Rehabilitation - Room and Board";
                case "AB": return "Rehabilitation - Inpatient";
                case "AC": return "Rehabilitation - Outpatient";
                case "AD": return "Occupational Therapy";
                case "AE": return "Physical Medicine";
                case "AF": return "Speech Therapy";
                case "AG": return "Skilled Nursing Care";
                case "AH": return "Skilled Nursing Care - Room and Board";
                case "AI": return "Substance Abuse";
                case "AJ": return "Alcoholism";
                case "AK": return "Drug Addiction";
                case "AL": return "Vision (Optometry)";
                case "AM": return "Frames";
                case "AN": return "Routine Exam";
                case "AO": return "Lenses";
                case "AQ": return "Nonmedically Necessary Physical";
                case "AR": return "Experimental Drug Therapy";
                case "B1": return "Burn Care";
                case "B2": return "Brand Name Prescription Drug - Formulary";
                case "B3": return "Brand Name Prescription Drug - Non-Formulary";
                case "BA": return "Independent Medical Evaluation";
                case "BB": return "Partial Hospitalization (Psychiatric)";
                case "BC": return "Day Care (Psychiatric)";
                case "BD": return "Cognitive Therapy";
                case "BE": return "Massage Therapy";
                case "BF": return "Pulmonary Rehabilitation";
                case "BG": return "Cardiac Rehabilitation";
                case "BH": return "Pediatric";
                case "BI": return "Nursery";
                case "BJ": return "Skin";
                case "BK": return "Orthopedic";
                case "BL": return "Cardiac";
                case "BM": return "Lymphatic";
                case "BN": return "Gastrointestinal";
                case "BP": return "Endocrine";
                case "BQ": return "Neurology";
                case "BR": return "Eye";
                case "BS": return "Invasive Procedures";
                case "BT": return "Gynecological";
                case "BU": return "Obstetrical";
                case "BV": return "Obstetrical/Gynecological";
                case "BW": return "Mail Order Prescription Drug: Brand Name";
                case "BX": return "Mail Order Prescription Drug: Generic";
                case "BY": return "Physician Visit - Office: Sick";
                case "BZ": return "Physician Visit - Office: Well";
                case "C1": return "Coronary Care";
                case "CA": return "Private Duty Nursing - Inpatient";
                case "CB": return "Private Duty Nursing - Home";
                case "CC": return "Surgical Benefits - Professional (Physician)";
                case "CD": return "Surgical Benefits - Facility";
                case "CE": return "Mental Health Provider - Inpatient";
                case "CF": return "Mental Health Provider - Outpatient";
                case "CG": return "Mental Health Facility - Inpatient";
                case "CH": return "Mental Health Facility - Outpatient";
                case "CI": return "Substance Abuse Facility - Inpatient";
                case "CJ": return "Substance Abuse Facility - Outpatient";
                case "CK": return "Screening X-ray";
                case "CL": return "Screening laboratory";
                case "CM": return "Mammogram, High Risk Patient";
                case "CN": return "Mammogram, Low Risk Patient";
                case "CO": return "Flu Vaccination";
                case "CP": return "Eyewear and Eyewear Accessories";
                case "CQ": return "Case Management";
                case "DG": return "Dermatology";
                case "DM": return "Durable Medical Equipment";
                case "DS": return "Diabetic Supplies";
                case "GF": return "Generic Prescription Drug - Formulary";
                case "GN": return "Generic Prescription Drug - Non-Formulary";
                case "GY": return "Allergy";
                case "IC": return "Intensive Care";
                case "MH": return "Mental Health";
                case "NI": return "Neonatal Intensive Care";
                case "ON": return "Oncology";
                case "PT": return "Physical Therapy";
                case "PU": return "Pulmonary";
                case "RN": return "Renal";
                case "RT": return "Residential Psychiatric Treatment";
                case "TC": return "Transitional Care";
                case "TN": return "Transitional Nursery Care";
                case "UC": return "Urgent Care";
                default: return value;
            }
        }

    }
}
