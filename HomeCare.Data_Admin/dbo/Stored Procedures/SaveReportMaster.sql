  
--CreatedBy: Chirag Jagad  
--CreatedDate:02 June 2020  
--Description: For Save Report Master data  
CREATE PROCEDURE [dbo].[SaveReportMaster]            
 @ReportID BIGINT,      
 @ReportName NVARCHAR(MAX),       
 @SqlString NVARCHAR(MAX),  
 @ReportDescription NVARCHAR(MAX),  
 @DataSet NVARCHAR(MAX),  
 @RDL_FileName NVARCHAR(MAX)   
AS                              
BEGIN                              
                     
DECLARE @PID BIGINT  
 SET @PID =  (SELECT  MAX(ReportID) FROM ReportMaster ) +1  
 PRINT @PID  
END  
IF(@ReportID=0)                             
 BEGIN       
 SET IDENTITY_INSERT ReportMaster ON                         
 INSERT INTO ReportMaster                              
 (ReportID,ReportName,SqlString,ReportDescription,DataSet,RDL_FileName, IsDeleted, IsActive)      
 VALUES (@PID,@ReportName,@SqlString,@ReportDescription,@DataSet,@RDL_FileName, 0, 1);      
 SET IDENTITY_INSERT ReportMaster OFF    
  SELECT 1; RETURN;  
 END         
 ELSE                              
 BEGIN                              
 UPDATE ReportMaster                                
 SET       
 ReportName=@ReportName,      
 SqlString=@SqlString,      
 ReportDescription=@ReportDescription,      
 DataSet=@DataSet,      
 RDL_FileName=@RDL_FileName,  
 IsDeleted = 0,  
 IsActive = 1  
  WHERE ReportID = @ReportID;    
    
  SELECT 1; RETURN;                            
 END