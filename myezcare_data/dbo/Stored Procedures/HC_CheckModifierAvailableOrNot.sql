CREATE PROCEDURE [dbo].[HC_CheckModifierAvailableOrNot]              
@modifier1 varchar(100)=null,    
@modifier2 varchar(100)=null,    
@modifier3 varchar(100)=null,    
@modifier4 varchar(100)=null,    
@CurrentDate datetime,    
@loggedInUserId BIGINT,                      
@SystemID VARCHAR(100)           
AS                                
BEGIN     
  
Declare @IsDeleted bit;  
DECLARE @temp TABLE (ModifierID bigint,ModifierCode varchar(Max),IsAvailable bit);    
    
--Modifier 1    
IF (@modifier1 IS NOT NULL AND LEN(@modifier1) != 0)    
BEGIN    
 IF Exists(SELECT * FROM Modifiers WHERE ModifierCode=@modifier1)    
 BEGIN     
 SELECT @IsDeleted=IsDeleted FROM Modifiers WHERE ModifierCode=@modifier1  
 IF (@IsDeleted = 'True')  
 Update Modifiers set IsDeleted=0 WHERE ModifierCode=@modifier1  
  
 INSERT INTO @temp     
 SELECT ModifierID,ModifierCode,1 FROM Modifiers WHERE ModifierCode=@modifier1;     
 END    
 ELSE    
 BEGIN     
  INSERT INTO Modifiers(ModifierCode,ModifierName,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)     
  VALUES (@modifier1,@modifier1,0,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID);    
    
  INSERT INTO @temp     
  SELECT @@IDENTITY,@modifier1,0;    
 END    
END    
    
--Modifier 2    
IF (@modifier2 IS NOT NULL AND LEN(@modifier2) != 0)    
BEGIN    
 IF Exists(SELECT * FROM Modifiers WHERE ModifierCode=@modifier2)    
 BEGIN     
 SELECT @IsDeleted=IsDeleted FROM Modifiers WHERE ModifierCode=@modifier2  
 IF (@IsDeleted = 'True')  
 Update Modifiers set IsDeleted=0 WHERE ModifierCode=@modifier2  
  
 INSERT INTO @temp     
 SELECT ModifierID,ModifierCode,1 FROM Modifiers WHERE ModifierCode=@modifier2;     
 END    
 ELSE    
 BEGIN     
  INSERT INTO Modifiers(ModifierCode,ModifierName,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)     
  VALUES (@modifier2,@modifier2,0,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID);    
    
  INSERT INTO @temp     
  SELECT @@IDENTITY,@modifier2,0;    
 END    
END    
    
--Modifier 3    
IF (@modifier3 IS NOT NULL AND LEN(@modifier3) != 0)    
BEGIN    
 IF Exists(SELECT * FROM Modifiers WHERE ModifierCode=@modifier3)    
 BEGIN     
 SELECT @IsDeleted=IsDeleted FROM Modifiers WHERE ModifierCode=@modifier3  
 IF (@IsDeleted = 'True')  
 Update Modifiers set IsDeleted=0 WHERE ModifierCode=@modifier3  
    
 INSERT INTO @temp     
 SELECT ModifierID,ModifierCode,1 FROM Modifiers WHERE ModifierCode=@modifier3;     
 END    
 ELSE    
 BEGIN     
  INSERT INTO Modifiers(ModifierCode,ModifierName,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)     
  VALUES (@modifier3,@modifier3,0,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID);    
    
  INSERT INTO @temp     
  SELECT @@IDENTITY,@modifier3,0;    
 END    
END    
    
--Modifier 4    
IF (@modifier4 IS NOT NULL AND LEN(@modifier4) != 0)    
BEGIN    
 IF Exists(SELECT * FROM Modifiers WHERE ModifierCode=@modifier4)    
 BEGIN   
 SELECT @IsDeleted=IsDeleted FROM Modifiers WHERE ModifierCode=@modifier4  
 IF (@IsDeleted = 'True')  
 Update Modifiers set IsDeleted=0 WHERE ModifierCode=@modifier4    
    
 INSERT INTO @temp     
 SELECT ModifierID,ModifierCode,1 FROM Modifiers WHERE ModifierCode=@modifier4;     
 END    
 ELSE    
 BEGIN     
  INSERT INTO Modifiers(ModifierCode,ModifierName,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)     
  VALUES (@modifier4,@modifier4,0,@CurrentDate,@loggedInUserId,@CurrentDate,@loggedInUserId,@SystemID);    
    
  INSERT INTO @temp     
  SELECT @@IDENTITY,@modifier4,0;    
 END    
END    
    
SELECT * FROM @temp    
    
--Select * from CSVtoTableWithIdentity(@modifier,',') T    
--Left JOIN Modifiers M ON M.ModifierCode = T.RESULT    
--where M.ModifierCode Is NULL    
                      
END
