--Exec DeleteEtsMaster @ListOfIdsInCsv =1, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100', @IsShowList=1,@loggedInID=1  
CREATE PROCEDURE [dbo].[DeleteEtsMaster]          
 @EmployeeID BIGINT = 0,          
 @StartDate DATE = NULL,          
 @EndDate DATE = NULL,          
 @IsDeleted int=-1,          
 @SortExpression NVARCHAR(100),            
 @SortType NVARCHAR(10),          
 @FromIndex INT,          
 @PageSize INT,          
 @ListOfIdsInCsv varchar(300),          
 @IsShowList bit,          
 @loggedInID BIGINT          
AS          
BEGIN          
          
 IF(LEN(@ListOfIdsInCsv)>0)          
 BEGIN            

 DECLARE @TempTable AS TABLE(
  EmployeeTimeSlotMasterID BIGINT
 )

 INSERT INTO @TempTable SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)



 UPDATE EM SET  EM.IsDeleted= 1 FROM @TempTable T
 LEFT JOIN EmployeeTimeSlotMaster EM ON EM.EmployeeTimeSlotMasterID=T.EmployeeTimeSlotMasterID
 WHERE EM.EmployeeTimeSlotMasterID>0

 
 UPDATE ED SET  ED.IsDeleted= 1 FROM @TempTable T
 LEFT JOIN EmployeeTimeSlotMaster EM ON EM.EmployeeTimeSlotMasterID=T.EmployeeTimeSlotMasterID
 INNER JOIN EmployeeTimeSlotDetails ED ON ED.EmployeeTimeSlotMasterID=EM.EmployeeTimeSlotMasterID
 WHERE EM.EmployeeTimeSlotMasterID>0


   --UPDATE EmployeeTimeSlotMaster SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END ,UpdatedBy=CAST(@loggedInID as bigint) ,UpdatedDate=GETUTCDATE() WHERE EmployeeTimeSlotMasterID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))


               
  END          
          
 IF(@IsShowList=1)          
 BEGIN          
  EXEC GetEtsMasterList @EmployeeID,@StartDate,@EndDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize          
 END          
END