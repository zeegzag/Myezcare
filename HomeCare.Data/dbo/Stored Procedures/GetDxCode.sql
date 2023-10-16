--UpdatedBy: Akhilesh Kamal  
--UpdatedDate 02/march/2020  
--Description: For select one dx code  
  
--  EXEC [GetDxCode] 'a00', 'A00','', '',''    
    
CREATE PROCEDURE [dbo].[GetDxCode]     
--@DXCodeID BIGINT=0,     
@DXCodeName NVARCHAR(200),      
@DXCodeWithoutDot NVARCHAR(200),      
@DxCodeType NVARCHAR(200),      
@Description NVARCHAR(MAX),      
@DxCodeShortName NVARCHAR(200)    
--@IsDeleted BIGINT    
AS      
BEGIN      
     
    -- putted top 1
	-- kundan - 2-5-2020
 select TOP 1 DXCodeID,DXCodeName,DXCodeWithoutDot,DxCodeType,Description,DxCodeShortName    
    from DXCodes DC      
 inner join DxCodeTypes DT on DT.DxCodeTypeID=DC.DxCodeType      
 WHERE         
 --   DXCodeName LIKE '%'+@DXCodeName+'%'   
 --AND DXCodeWithoutDot  LIKE '%'+@DXCodeWithoutDot+'%'  
  DXCodeName = @DXCodeName  
 OR DXCodeWithoutDot = @DXCodeWithoutDot  
   AND      
   IsDeleted!=1;     
    
      
END    
--END    