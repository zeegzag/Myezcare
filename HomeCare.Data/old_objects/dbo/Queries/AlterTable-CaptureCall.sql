
--CreatedBy: Jemin    
--CreatedDate: 10 June 2020    
--Description: For Save CaptureC all in CaptureCall table    
    
ALTER TABLE CaptureCall
ADD Notes nvarchar(max);

ALTER TABLE CaptureCall
ADD EmployeesIDs nvarchar(255);

ALTER TABLE CaptureCall
ADD CallType nvarchar(255);

ALTER TABLE CaptureCall
ADD RelatedWithPatient BIGINT;

ALTER TABLE CaptureCall
ADD Date DateTime;