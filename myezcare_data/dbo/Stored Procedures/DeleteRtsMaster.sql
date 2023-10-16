--Exec DeleteRtsMaster @ListOfIdsInCsv =1, @SortExpression = 'Name', @SortType = 'DESC', @FromIndex = '1', @PageSize = '100', @IsShowList=1,@loggedInID=1  
CREATE PROCEDURE [dbo].[DeleteRtsMaster]
 @ReferralID BIGINT = 0,          
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
  ReferralTimeSlotMasterID BIGINT
 )

 INSERT INTO @TempTable SELECT CAST(Val AS BIGINT) FROM GetCSVTable(@ListOfIdsInCsv)



 UPDATE EM SET  EM.IsDeleted= 1 FROM @TempTable T
 LEFT JOIN ReferralTimeSlotMaster EM ON EM.ReferralTimeSlotMasterID=T.ReferralTimeSlotMasterID
 WHERE EM.ReferralTimeSlotMasterID>0

 
 UPDATE ED SET  ED.IsDeleted= 1 FROM @TempTable T
 LEFT JOIN ReferralTimeSlotMaster EM ON EM.ReferralTimeSlotMasterID=T.ReferralTimeSlotMasterID
 INNER JOIN ReferralTimeSlotDetails ED ON ED.ReferralTimeSlotMasterID=EM.ReferralTimeSlotMasterID
 WHERE EM.ReferralTimeSlotMasterID>0


               
  END          
          
 IF(@IsShowList=1)          
 BEGIN          
  EXEC GetRtsMasterList @ReferralID,@StartDate,@EndDate,@IsDeleted,@SortExpression, @SortType, @FromIndex, @PageSize          
 END          
END 
