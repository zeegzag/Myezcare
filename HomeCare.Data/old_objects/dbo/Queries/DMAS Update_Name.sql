--CreatedBy: Ritesh Kumar
--CreatedDate: 20 feb 2020
--Description: change name Dmas_Form to "Department of Medical Assistance Services (DMAS)" in Role & permission tree

 UPDATE [dbo].[Permissions]
   SET PermissionName='Department of Medical Assistance Services (DMAS)'
 WHERE PermissionCode='DMAS_From'






