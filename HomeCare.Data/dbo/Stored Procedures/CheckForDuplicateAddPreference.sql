CREATE Procedure [dbo].[CheckForDuplicateAddPreference]          
@PreferenceID BIGINT,        
@PreferenceName NVARCHAR(1000)  
as            
select CountValue=Count(*) from Preferences        
where PreferenceName=@PreferenceName AND PreferenceID != @PreferenceID