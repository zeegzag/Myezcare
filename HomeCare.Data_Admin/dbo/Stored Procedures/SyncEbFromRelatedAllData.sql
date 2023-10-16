CREATE PROCEDURE [dbo].[SyncEbFromRelatedAllData]  
@UDT_EBMarketTable   UDT_EBMarketAndCategoryTable  READONLY,    
@UDT_EBCategoryTable UDT_EBMarketAndCategoryTable READONLY,    
@UDT_EBFromTable    UDT_EBFromTable READONLY
AS        
BEGIN    
   
  
 -- MARKET VALUES ADD/UPDATE/DELETE  START  
  
 UPDATE EM SET  
 EM.Name=UM.Name, EM.UpdatedDate=GETUTCDATE()  
 FROM @UDT_EBMarketTable UM  
 LEFT JOIN EbMarkets EM ON EM.EBMarketID=UM.Id  
 WHERE EM.EBMarketID IS NOT NULL  
   
 INSERT INTO EbMarkets(EBMarketID,Id,Name,CreatedDate,UpdatedDate)  
 SELECT UM.Id,UM.Id,UM.Name,GETUTCDATE(), GETUTCDATE()  
 FROM @UDT_EBMarketTable UM  
 LEFT JOIN EbMarkets EM ON EM.EBMarketID=UM.Id  
 WHERE EM.EBMarketID IS NULL  

 DELETE EM
 FROM EbMarkets EM  
 LEFT JOIN @UDT_EBMarketTable UM ON EM.EBMarketID=UM.Id  
 WHERE UM.Id IS NULL 
  
 -- MARKET VALUES ADD/UPDATE/DELETE  END  
  
  
  
  
 -- Category VALUES ADD/UPDATE/DELETE  START  
  
 UPDATE EC SET  
 EC.Name=UM.Name, EC.UpdatedDate=GETUTCDATE()  
 FROM @UDT_EBCategoryTable UM  
 LEFT JOIN EbCategories EC ON EC.EBCategoryID=UM.Id  
 WHERE EC.EBCategoryID IS NOT NULL  
   
 INSERT INTO EbCategories(EBCategoryID,Id,Name,CreatedDate,UpdatedDate)  
 SELECT UM.Id,UM.Id,UM.Name,GETUTCDATE(), GETUTCDATE()  
 FROM @UDT_EBCategoryTable UM  
 LEFT JOIN EbCategories EC ON EC.EBCategoryID=UM.Id  
 WHERE EC.EBCategoryID IS NULL  
  
 DELETE EC
 FROM EbCategories EC  
 LEFT JOIN @UDT_EBCategoryTable UM ON EC.EBCategoryID=UM.Id  
 WHERE UM.Id IS NULL 

 -- Category VALUES ADD/UPDATE/DELETE  END  
  
    
  
 -- Forms VALUES ADD/UPDATE/DELETE  START  
  
 UPDATE EF SET  
 EF.Id=UF.Id,  
 EF.FormId=UF.FormId,  
 EF.Name=UF.Name,  
 EF.FormLongName=UF.FormLongName,  
 EF.NameForUrl=UF.NameForUrl,  
 EF.Version=UF.Version,
 EF.IsActive=UF.IsActive,  
 EF.HasHtml=UF.HasHtml,  
 EF.NewHtmlURI=UF.NewHtmlURI,  
 EF.HasPDF=UF.HasPDF,  
 EF.NewPdfURI=UF.NewPdfURI,  
 EF.EBCategoryID=UF.EBCategoryID,  
 EF.UpdatedDate=GETUTCDATE()  
 FROM @UDT_EBFromTable UF  
 LEFT JOIN EbForms EF ON EF.FromUniqueID=UF.FromUniqueID  
 WHERE EF.FromUniqueID IS NOT NULL  
  
 INSERT INTO EbForms(EBFormID,FromUniqueID,Id,FormId,Name,FormLongName,NameForUrl,Version,IsActive,HasHtml,NewHtmlURI,HasPDF,NewPdfURI,EBCategoryID,EbMarketIDs,CreatedDate,UpdatedDate)
 SELECT  UF.FromUniqueID,UF.FromUniqueID,UF.Id,UF.FormId,UF.Name,UF.FormLongName,UF.NameForUrl,UF.Version,UF.IsActive,UF.HasHtml,UF.NewHtmlURI,UF.HasPDF,UF.NewPdfURI,UF.EBCategoryID,
 UF.EbMarketIDs,  
 GETUTCDATE(),GETUTCDATE()  
 FROM @UDT_EBFromTable UF  
 LEFT JOIN EbForms EF ON EF.FromUniqueID=UF.FromUniqueID  
 WHERE EF.FromUniqueID IS NULL  
  
 DELETE EF
 FROM EbForms EF 
 LEFT JOIN  @UDT_EBFromTable UF   ON EF.FromUniqueID=UF.FromUniqueID  
 WHERE UF.FromUniqueID IS NULL

-- Forms VALUES ADD/UPDATE/DELETE  END   Version




-- Forms MARKET VALUES ADD/UPDATE  START

DECLARE @TempFormMarketTable TABLE (EBFormID NVARCHAR(MAX),EbMarketID NVARCHAR(MAX))

INSERT INTO @TempFormMarketTable (EBFormID,EbMarketID)
SELECT A.EBFormID,EbMarketID= Split.a.value('.', 'VARCHAR(100)')
FROM  (SELECT EBFormID, CAST ('<M>' + REPLACE(EbMarketIDs, ',', '</M><M>') + '</M>' AS XML) AS EbMarketID  
FROM  EbForms) AS A CROSS APPLY EbMarketID.nodes ('/M') AS Split(a);

 
INSERT INTO EBFormMarkets(EBFormID,EBMarketID)
SELECT T.EBFormID,T.EBMarketID FROM @TempFormMarketTable T
LEFT JOIN EBFormMarkets EFM ON EFM.EBFormID=T.EBFormID AND  EFM.EBMarketID = T.EBMarketID
WHERE EFM.EBFormMarketID IS NULL  -- INSERT
  
  
 DELETE EFM
 FROM EBFormMarkets EFM  
 LEFT JOIN @TempFormMarketTable T  ON EFM.EBFormID=T.EBFormID AND  EFM.EBMarketID = T.EBMarketID
 WHERE T.EBFormID IS NULL 


 -- Forms MARKET VALUES ADD/UPDATE  END
  
  
  
 SELECT 1 AS TransactionResultId   
END