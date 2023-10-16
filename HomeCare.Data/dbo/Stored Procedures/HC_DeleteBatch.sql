--EXEC HC_DeleteBatch @SortExpression = 'BatchID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '99', @IsSentStatus = '-1', @ClientName = '', @ListOfIdsInCSV = '412', @IsShowList = 'True'    
CREATE PROCEDURE [dbo].[HC_DeleteBatch] @BatchTypeID    BIGINT=0,                      
                                       @PayorID        BIGINT=0,                      
                                       @StartDate      DATE=NULL,                      
                                       @EndDate        DATE=NULL,                      
                                       @Comment        NVARCHAR(max)=NULL,                      
                                       @IsSentStatus   INT=-1,                      
                                       @ClientName VARCHAR(MAX)=null,                
                                       @SORTEXPRESSION NVARCHAR(100),                      
                                       @SORTTYPE       NVARCHAR(10),                      
                                       @FROMINDEX      INT,                      
                                       @PAGESIZE       INT,                      
                                       @ListOfIdsInCSV VARCHAR(max)=NULL,                      
                                       @IsShowList     BIT                      
--@IND   INT=0,                      
--@EIND INT =0,                      
--@IsSent bit=0,                      
--@TempBatchID bigint=0                      
AS                      
  BEGIN                      
      IF( Len(@ListOfIdsInCSV) > 0 )                      
        BEGIN                      
                                 
            DECLARE @IsSent BIT = CONVERT(BIGINT, @ListOfIdsInCSV);                      
            DECLARE @TempBatchID BIGINT = CONVERT(BIGINT, @ListOfIdsInCSV);                      
                      
            SELECT @IsSent = issent, @BatchTypeID = BatchTypeID                      
            FROM   batches                      
            WHERE  batchid = @TempBatchID                      
                      
            DECLARE @NOTES TABLE                      
              (                      
                 noteid BIGINT                      
              )                      
                      
            INSERT INTO @notes (noteid) SELECT noteid FROM   batchnotes  WHERE  batchid IN (SELECT Cast(val AS BIGINT)  FROM   Getcsvtable(@ListOfIdsInCSV))                      
                      
            SELECT noteid                      
            FROM   batchnotes                      
            WHERE  batchid IN (SELECT Cast(val AS BIGINT)                      
                               FROM   Getcsvtable(@ListOfIdsInCSV))                      
                      
            IF( @IsSent = 0 )                      
              BEGIN                      
                  BEGIN try             
                      BEGIN TRAN                      
                      
                     
                      DELETE batchapprovedservicecodes                      
          WHERE  batchid IN (SELECT Cast(val AS BIGINT)                      
                   FROM   Getcsvtable(@ListOfIdsInCSV))                      
                      
                     DELETE batchapprovedfacility                      
                      WHERE  batchid IN (SELECT Cast(val AS BIGINT)                      
                                         FROM   Getcsvtable(@ListOfIdsInCSV))                      
                      
                      DELETE batchnotes                      
                      WHERE  batchid IN (SELECT Cast(val AS BIGINT)                      
       FROM   Getcsvtable(@ListOfIdsInCSV))                      
                      
       DELETE batches                      
                      WHERE  batchid IN (SELECT Cast(val AS BIGINT)                      
                                         FROM   Getcsvtable(@ListOfIdsInCSV))         
                      
     IF(@BatchTypeID=1)                
     BEGIN                
                      --DELETE FROM signaturelogs                      
                      --WHERE  noteid IN (SELECT noteid                      
                      --                  FROM   @Notes)                      
                      
                      --DELETE FROM notes                      
                      --WHERE  noteid IN (SELECT noteid                      
                      --                  FROM   @Notes)                      
          
          
       DELETE N          
      FROM signaturelogs N          
      INNER JOIN @Notes TN ON TN.noteid = N.NoteID          
      LEFT JOIN  BatchNotes BN ON BN.NoteID = N.NoteID          
          WHERE BN.NoteID IS NULL          
        
        
     DELETE N          
      FROM NoteDXCodeMappings N          
      INNER JOIN @Notes TN ON TN.noteid = N.NoteID          
     -- LEFT JOIN  BatchNotes BN ON BN.NoteID = N.NoteID          
          --WHERE BN.NoteID IS NULL          
                    
      DELETE N          
      FROM Notes N          
      INNER JOIN @Notes TN ON TN.noteid = N.NoteID          
      LEFT JOIN  BatchNotes BN ON BN.NoteID = N.NoteID          
          WHERE BN.NoteID IS NULL          
     END                
      ELSE    
   BEGIN    
     
   DELETE N          
      FROM NoteDXCodeMappings N          
      WHERE N.BatchID IN  (SELECT Cast(val AS BIGINT)  FROM   Getcsvtable(@ListOfIdsInCSV))     
     
     
   END    
                   COMMIT                        
                  END try                      
                      
                  BEGIN catch                      
                      --SELECT -1                  
                      SELECT ERROR_NUMBER() AS ErrorNumber  ,ERROR_MESSAGE() AS ErrorMessage;                      
                      
                      ROLLBACK                      
                  END catch                      
              END                      
     END                      
            IF( @IsShowList = 1 )                      
              BEGIN                      
                  EXEC Hc_getbatchlist                      
                    @BatchTypeID,                      
                    @PayorID,                      
                    @StartDate,                      
                    @EndDate,                      
                    @Comment,                      
                    @IsSentStatus,              
     @ClientName,              
                    @SORTEXPRESSION,                      
                    @SORTTYPE,                      
                    @FROMINDEX,                      
                    @PAGESIZE                      
              END                      
                              
  END 