CREATE PROCEDURE [dbo].[API_GetPatientNotesData]          
    @ReferralID INT          
AS                                             
BEGIN         
          
 SELECT           
  R.[ReferralID],      
  CN.[CommonNoteID],      
        ISNULL(CN.[Note], 'NA') AS [Note],         
  ISNULL(CN.[CreatedDate], 'NA') AS [CreatedDate],             
  ISNULL(E.[FirstName] + ' ' + E.[LastName], 'NA') AS NotesAddedBy    
    FROM           
        [dbo].[CommonNotes] CN          
 LEFT JOIN [dbo].[Referrals] R      
  ON R.[ReferralID] = CN.[ReferralID]      
 INNER JOIN Employees E       
  ON E.[EmployeeID] = CN.[CreatedBy]       
    WHERE          
        R.[ReferralID] = @ReferralID          
        AND R.[IsDeleted] = 0         
        
END 