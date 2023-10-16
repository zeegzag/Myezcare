--
--
/*
CREATED BY :PALLAV SAXENA
CREATE DATE: 10/25/2019
PURPOSE: CLONE THE VISIT TASK FROM THE OTHER TASK IN ANY CARETYPE . THIS SP WILL BE USED TO COPY TASK FROM ONE CARETYPE TO THE OTHER USING ONE CLICK.

PARAMETERS: 
		@SRCCARETYPENAME - NAME OF THE CARETYPE FROM WHICH THE TASK ARE REQUIRED TO COPIED. NAME IS USED INSTEAD OF ID TO NOT BIND THE ID VALUES FROM  DIFFERENT DBS.
		@TARGETCARETYPENAME - NAME OF THE CARE TYPE TO WHICH THE TASK ARE REQUIRED TO BE COPIED.
		@ORGID - PASS THE ORGANIZATION ID WHICH IS INSERTED INTO THE FIELD SYSTEMID.
EXAMPLE: EXECUTE CLONETASKS 'Personal Care','Consumer Directed',null,1,55
*/


CREATE procedure [dbo].[CloneTasks](
@SrcCaretypeName varchar(50), 
@TargetCareTypeName varchar(50),
@CreateDate DateTime,
@CreatedBy int=1,
@OrgID varchar(50)=1,
@ServiceCode varchar(100)=null
)
as
BEGIN

IF @CREATEDATE IS NULL BEGIN SET @CREATEDATE=GETDATE() END



BEGIN TRY
Begin Tran
IF ((SELECT COUNT(*) FROM DDMASTER WHERE TITLE=@TargetCareTypeName)>0  AND (SELECT COUNT(*) FROM DDMASTER WHERE TITLE=@SrcCaretypeName)>0 )
BEGIN
Insert into visitTasks([VisitTaskType], [VisitTaskDetail], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID], [ServiceCodeID], [IsRequired], [MinimumTimeRequired], [IsDefault], [VisitTaskCategoryID], [VisitTaskSubCategoryID], [SendAlert], [VisitType], [CareType], [Frequency])
select [VisitTaskType], [VisitTaskDetail], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [SystemID],case @ServiceCode when Null then [ServiceCodeID] else @ServiceCode end, [IsRequired], [MinimumTimeRequired], [IsDefault], [VisitTaskCategoryID], [VisitTaskSubCategoryID], [SendAlert], [VisitType], 
	(SELECT DDMaster.DDMasterID FROM DDMaster INNER JOIN lu_DDMasterTypes ON DDMaster.ItemType = lu_DDMasterTypes.DDMasterTypeID WHERE (DDMaster.Title = @TargetCareTypeName) AND DDMaster.IsDeleted = 0 AND NAME='Care Type'), [Frequency]
 from visittasks where VisitTasktype='Task' and caretype=(SELECT DDMaster.DDMasterID FROM DDMaster INNER JOIN lu_DDMasterTypes ON DDMaster.ItemType = lu_DDMasterTypes.DDMasterTypeID WHERE (DDMaster.Title = @SrcCaretypeName) AND (DDMaster.IsDeleted = 0 AND NAME='Care Type' )) and isDeleted=0
 select cast(@@trancount AS VARCHAR(50))+' Task Cloned'
   Commit;

 END
 ELSE
 BEGIN
 Rollback;
 SELECT 'Either Source or Target Caretype not found' 
 END
 
 END TRY

 BEGIN CATCH
 RollBack;
        SELECT  'Error Occured -'+
            --ERROR_NUMBER() AS ErrorNumber  
            --,ERROR_SEVERITY() AS ErrorSeverity  
            --,ERROR_STATE() AS ErrorState  
            ERROR_PROCEDURE() AS ErrorProcedure  
            ,ERROR_LINE() AS ErrorLine  
            ,ERROR_MESSAGE() AS ErrorMessage;  
    END CATCH
END