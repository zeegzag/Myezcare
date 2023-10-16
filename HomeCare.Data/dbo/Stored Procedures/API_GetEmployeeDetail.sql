  
-- exec [API_GetEmployeeDetail] 'demo'  
                 
CREATE PROCEDURE [dbo].[API_GetEmployeeDetail]            
 @UserName nvarchar(50)                          
AS                          
BEGIN      
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()     
 DECLARE @OrganizationTwilioNumber NVARCHAR(30);      
 SELECT @OrganizationTwilioNumber=TwilioFromNo FROM OrganizationSettings      
      
 -- get mobile aggrement permission id      
 DECLARE @TermsPermissionID INT = 0      
 Select @TermsPermissionID = PermissionID from  Permissions where PermissionPlatform = 'Mobile' and PermissionCode = 'Agency_Aggrement'      
      
 If @TermsPermissionID is null Begin      
 Set @TermsPermissionID = 0      
 End      
      
      
 SELECT e.EmployeeID,EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),e.FirstName,e.LastName, e.Email, e.UserName, e.Password, e.PasswordSalt, e.PhoneWork,          
 e.PhoneHome, e.IsActive, e.IsVerify, e.RoleID,e.IsDeleted, e.Roles, e.MobileNumber , e.LoginFailedCount, e.IsFingerPrintAuth,ProfileUrl=e.ProfileImagePath,          
 e.IVRPin,e.AssociateWith,@OrganizationTwilioNumber AS OrganizationTwilioNumber      
       
 -- Sameer M. [2021-July-13]      
 -- added for mobile application usage      
      
 , IsTermsConditionMobileAccepted,IsFirstTimeLogin      
 , IsTermsConditionMobileRequired = (Select Count(*) from RolePermissionMapping where RoleID = e.RoleID AND PermissionID = @TermsPermissionID AND IsDeleted = 0 )      
      
 -- end here      
      
 FROM dbo.Employees e WHERE e.UserName=@UserName    
 declare   
@Name nvarchar(MAX),  
@Address nvarchar(MAX),  
@HireDate nvarchar(MAX),  
@Date nvarchar(MAX),  
@TermConditionMobile nvarchar(MAX)  
set @Date = cast(getdate() as varchar(max))  
   select @Name=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),@HireDate=cast(e.HireDate as varchar(max)),@Address=CONCAT(Address,' ',City,' ',StateCode,' ',ZipCode) from Employees e where UserName=@UserName  
    set @TermConditionMobile='<div>CAREGIVER - TERMS AND CONDITIONS</div>  <div>1. You are appointed for providing services such as: (a) chronic disease management program for chronically ill</div>  <div>patients; (b) round the clock pain and palliative c
are program for bed ridden, terminally ill and paraplegic patients,</div>  <div>and (c) hospital to home transition care program for post-operative patients etc., (“Services”) to the Clients of the</div>  <div>Company, most of whom are senior citizens (“C
lients”). You agree that the Services are not exhaustive, and the</div>  <div>provision of incidental services shall be deemed to form part of the Services which is included in the Fee payable</div>  <div>to You in lieu of your Services as specifically me
ntioned under the Annexure (“Fee”).</div>  <div>2. You have represented and assured that You are duly registered and licensed general duty assistant and compliant</div>  <div>with all necessary sanctions, approvals, permissions, licenses required under th
e applicable laws and which You</div>  <div>shall maintain and keep valid as long as You are engaged/retained with the Company.</div>  <div>3. Your engagement will be with effect from ………………. and shall remain effective unless terminated in writing</div>  
<div>either by the Company or You upon expiry of 15 (fifteen) days’ notice period. However, the Company reserves</div>  <div>right to terminate this engagement with immediate effect if you are found:</div>  <div>(a) negligent in the performance of respons
ibilities to the Company and/or Clients;</div>  <div>(b) to be in breach of any code of conduct or Company’s policy;</div>  <div>(c) violating any provision of this engagement;</div>  <div>(d) conducting yourself in a fraudulent or dishonest manner with t
he Company or/and Clients;</div>  <div>(e) conducting yourself in a manner which is harmful to the interests or reputation of the Company and/or</div>  <div>Clients;</div>  <div>(f) absent from work for more than 7 (seven) days without any prior notice.</
div>  <div>Upon termination of your engagement with the Company and completion of any/all termination related</div>  <div>formalities, the Company shall release your full and final payment after adjusting any outstanding dues, claims,</div>  <div>loss etc
. For sake of clarity, Company shall not release the full and final payment in case your Services are</div>  <div>terminated with immediate effect, unless the Company has duly satisfied and verified itself against the</div>  <div>actions/disputes, if any,
 of such Caregiver.</div>  <div>Date:<b>'+@Date+'</b> </div>  <div>Name:<b>'+@Name+'</b></div>  <div>Care Angel</div>  <div>Care Partner</div>  <div>Address:<b>'+@Address+'</b></div>  <div>Dear <b>'+@Name+'</b></div>  <div>We, EMOHA (hereinafter referred 
as the “Company”/ “Us”) appreciate your interest to work with us and basis our</div>  <div>discussions in this regard, the Company is pleased to appoint you on principal to principal basis as CAREGIVER</div>  <div>(hereinafter referred as “Caregiver” / “Y
ou”) for Gurugram region, on the following terms and conditions.</div>  <div>7/24/2021 SUPPORT@MYEZCARE.COM test patient 10869 N SCOTTSDALE RD SCOTTSDALE NJ, 85254 7/24/2021 test patient MYEZCARE LLC 2134, 22nd C Main Road Bengaluru HI 5601</div>  <div>4.
 Obligations, Representations and Warranties of the Caregiver:</div>  <div>(a) The Caregiver agrees to conduct herself/himself in appropriate manner as per the requirements of the</div>  <div>Company and/or Clients.</div>  <div>(b) The Caregiver will not 
use or disclose any confidential information regarding the Company and/or Clients</div>  <div>to anyone in case he/she becomes aware ofsuch information.</div>  <div>(c) The Caregiver undertakes that all information that she/he has provided to the Company 
while engaging with</div>  <div>the Company istrue, accurate, complete and correct.</div>  <div>(d) The Caregiver understands and agrees that any misrepresentation, falsification or material omission of</div>  <div>information provided to the Company may 
form one of the grounds for the Company to terminate his/her</div>  <div>services with immediate effect.</div>  <div>(e) The Caregiver authorizes the investigation, furnishing and disclosure of any/all information regarding</div>  <div>her/him and/or her/
his characterto theCompany and its affiliated entities, agents,representatives, andClients.</div>  <div>(f) The Caregiver agrees to inform the Company immediately of any material changes in Caregiver’s status,</div>  <div>which may impact the Clients and 
this includes positive and/or adverse results of any medical tests, court</div>  <div>proceedings etc.</div>  <div>(g) The Caregiver assures that there is no legal action, civil or criminal, pending against him/her.</div>  <div>(h) The Caregiver undertake
s to get himself/herself verified by the concerned local police station (as per Clients</div>  <div>address) and submit necessary documents in this regard to the Company and/or the Clients, as the case may</div>  <div>be.</div>  <div>(i) TheCaregiverunder
takes notto be absentfromwork/dutywithoutprior approval of the Clients and intimation</div>  <div>to the Company.</div>  <div>(j) The Caregiver undertakes not to work directly with any Client for a period of 2 (two) years from the date of</div>  <div>term
ination of this engagement.</div>  <div>(k) The Caregiver shall have no objection in case of use of profile and photographs of the Caregiver by the</div>  <div>Company for promoting the services of Caregiver and/or the Company.</div>  <div>5. You will be 
assigned Client(s) for rendering Services from time to time. The Company reserves its rights to make</div>  <div>any type of changes in assigning the Caregiver to Client(s) anytime, at its sole discretion. In this regard, it is</div>  <div>hereby further 
clarified and understood by you, that your engagement will be based on the hiring decision of the</div>  <div>Client(s), the Company shall not be responsible for any fallout and consequences in your Services.</div>  <div>6. The Caregiver shall perform the
 Services hereunder as independent person and in no way any employee and</div>  <div>employer relationship exists between the Company and the Caregiver or any other relationship. In the</div>  <div>performance of the Services hereunder, the Caregiver shal
l comply with all applicable laws, rules and regulation.</div>  <div>7. The Caregiver agrees to indemnify and hold the Company harmless against any and all liabilities, claims</div>  <div>(including third party claims), damage, costs or expenses, or attor
ney’s fees, which the Company may incur or be</div>  <div>required to pay by reason of any lawsuit, arbitration or other legal proceeding or claims arising out of or in</div>  <div>connection with:</div>  <div>(a) any act or omission of the Caregiver incl
uding any misrepresentation, negligence, default [clinical or nonclinical] in rendering the Services due to which Client(s) suffer any loss, harm or injury including death;</div>  <div>(b) any breach of the terms of this engagement and/or applicable laws 
and/or his/her licenses by the Caregiver;</div>  <div>(c) any actions, claims, and/or proceedings if any, initiated by your previous engagements/retainer</div>  <div>ship/employer(s) or their clients.</div>  <div>Notwithstanding anything contained herein,
 the Company shall not be liable, in any manner whatsoever, to the</div>  <div>Caregiver except making timely payment of the undisputed Fee to the Caregiver for his/her Services in accordance</div>  <div>with the terms of this engagement.</div>  <div>8. T
he waiver or failure of the Company to enforce any terms of engagement shall not be construed or operate as a</div>  <div>waiver of any future breach of such term or any other term of engagement. Any waiver of terms of this engagement</div>  <div>by the C
ompany shall be valid and effected if given in writing by the Company.</div>  <div>9. The terms of this engagement shall not be varied, amended or modified by any of the parties in any manner unless</div>  <div>such variation, amendment or modification is
 agreed to in writing and duly executed by both, the Company and</div>  <div>the Caregiver.</div>  <div>10. The Services to be performed by the Caregiver are personal in nature and therefore the obligations of Caregiver</div>  <div>will not be assigned or
 transferred by the Caregiver to any third party.</div>  <div>11. This engagement shall be governed and construed by and in accordance with the laws of India. Courts at</div>  <div>Gurugram, Haryana alone shall have jurisdiction over the disputes arising 
out of terms of this engagement.</div>  <div>Please sign the copy of the terms and conditions as a token of your acceptance to the aforementioned.</div>  <div>Thank you,</div>  <div>……………………………………………</div>  <div>Human Resources</div>  <div>EMOHA (Ignox La
bs Pvt. Ltd.)</div>  <div>(Contact details)The above mentioned terms and conditions of the engagement have been explained to me in</div>  <div>vernacular and are fully understood by me. I have had a reasonable opportunity to seek independent advice on the
</div>  <div>same and I hereby accept the same.</div>  <div>Signature and Date</div>  <div><br></div>  <div>____________________________</div>  <div>_______ 7/24/2021_____________________</div>  <div>ANNEXURE</div>  <div>Visit Type Payout per Visit Paymen
t Cycle</div>  <div>* Payment of Fee and/or any benefits or incentive, issubject to such deduction of tax as warranted under</div>  <div>the applicable local laws from time to time.</div>  <div>*You will provide your PAN details to the Company for deducti
on of tax at source @10%. Further, any/all</div>  <div>benefits or incentives that may be offered to you during your engagement with the Company, shall be</div>  <div>at Company’s sole discretion and may be revised and/or discontinued at any point of time
, without any</div>  <div>prior notice</div>  <div>* In case there is no PAN card submitted, TDS will be at 20%.</div>  '       
 SELECT AndroidMinimumVersion,AndroidCurrentVersion,IOSMinimumVersion,IOSCurrentVersion, @TermConditionMobile as TermsConditionMobile FROM OrganizationSettings              
END 