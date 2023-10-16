  
--exec [dbo].[SaveMapReport] 1,1,0,'2,3,5',1  
    
CREATE PROCEDURE [dbo].[SaveMapReport]                         
@EmployeeID BIGINT,                        
@RoleID BIGINT,     
@IsSetToTrue BIT,    
@ListOfReportIdInCsv nVARCHAR(max),                        
@SystemID VARCHAR(50)                        
AS                        
BEGIN                            
 DECLARE @CurrentDate DATETIME = (SELECT GETDATE());                        
 DECLARE @PermissionID nvarchar(100);       
 --Delete from ReportPermissionMapping where  RoleID=@RoleID;  
   
  UPDATE ReportPermissionMapping set UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=1, SystemID=@SystemID                         
  where RoleID=@RoleID  
  
 IF(@IsSetToTrue=1)                        
 BEGIN    
   
  UPDATE ReportPermissionMapping SET UpdatedBy=@EmployeeID, UpdatedDate=@CurrentDate, IsDeleted=0, SystemID=@SystemID                        
  WHERE RoleID=@RoleID and ReportID in (SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfReportIdInCsv))    
    
 END                         
            
 Insert into ReportPermissionMapping                        
  (RoleID,ReportID,                        
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,                        
  SystemID,IsDeleted)                        
 (select                         
  @RoleID RoleID,T.Val ReportID,                        
  GETUTCDATE() CreatedDate,@EmployeeID CreatedBy,GETUTCDATE() UpdatedDate,@EmployeeID UpdatedBy,                      
  @SystemID SystemID,(CASE when @IsSetToTrue=1 then 0 else 1 END) IsDeleted                         
  from                         
  (SELECT CAST(Val AS BIGINT) as Val FROM GetCSVTable(@ListOfReportIdInCsv)) as T                          
  where T.Val not in  (select ReportID from ReportPermissionMapping where RoleID=@RoleID)                        
 )                        
     Select 'Success'                                                     
END 