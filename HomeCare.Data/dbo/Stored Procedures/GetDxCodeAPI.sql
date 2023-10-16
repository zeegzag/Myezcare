--  EXEC SaveDxCodeAPI 'teyrteyeer', 'Y07.6','', '',''
--CreatedBy:Akhilesh Kamal
--CreatedDate:10 Feb 2020
--Description: for save dx code in database from search API
CREATE PROCEDURE [dbo].[GetDxCodeAPI] 
--@DXCodeID BIGINT=0, 
@DXCodeName NVARCHAR(200),  
@DXCodeWithoutDot NVARCHAR(200),  
@DxCodeType NVARCHAR(200),  
@Description NVARCHAR(MAX),  
@DxCodeShortName NVARCHAR(200)
--@IsDeleted BIGINT
AS  
BEGIN  
 IF EXISTS (SELECT * FROM DXCodes WHERE  DXCodeName = @DXCodeName) 
  BEGIN                    
 SELECT -1 RETURN; 
END

--IF(@DXCodeID=0)                         
-- BEGIN                          
  INSERT INTO DXCodes                          
  (DXCodeName,DXCodeWithoutDot,DxCodeType,Description,IsDeleted,EffectiveFrom,EffectiveTo,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
  VALUES                          
  (@DXCodeName,@DXCodeWithoutDot,@DxCodeType,@Description,0,GETUTCDATE(),GETUTCDATE(),GETUTCDATE(),1,'','',1);        

 IF(@DXCodeName !=NULL)
 BEGIN
 select  TOP 1 DXCodeID,DXCodeName,DXCodeWithoutDot,DxCodeType,Description,DxCodeShortName
	   from DXCodes DC  
 inner join DxCodeTypes DT on DT.DxCodeTypeID=DC.DxCodeType  
 WHERE     
    DXCodeName LIKE '%'+@DXCodeName+'%' 
   AND  
   IsDeleted!=1; 
   END  
  
END
--END