
-- exec GetAnnouncement
--CreatedBy: Akhilesh kamal
--CreatedDate: 22 Apr 2020
--Description: for getting Announcement from Admin database

CREATE PROCEDURE [dbo].[GetAnnouncement]    
  
AS  
BEGIN  
Select ReleaseNoteID,Title,Description,StartDate,EndDate,IsDeleted,    
  dbo.RemoveHTML(Description) AS DescriptionWithOutCode    
  FROM  ReleaseNotes where IsActive=1
END