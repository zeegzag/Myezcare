
-- =============================================
-- Author:		<Author,,Baliwinder>
-- Create date: <Create Date,17-04-2020,>
-- Description:	<Description,,>
-- =============================================[dbo].[GetALLInvoiceList]
ALTER PROCEDURE [dbo].[GetAllOrganizationInvoiceByOrgId]
@InvoiceDate  NVARCHAR(30) = NULL,
@DueDate  NVARCHAR(30) = NULL,
@OrganizationId BIGINT=NULL,
@InvoiceAmount NVARCHAR(50)=NULL,            
@PaidAmount NVARCHAR(50)=NULL,     
@InvoiceStatus INT = NULL,  
@SortExpression NVARCHAR(100),  
@SortType NVARCHAR(10),  
@FromIndex INT,  
@PageSize INT , 
@Status BIT  
AS                    
BEGIN    
 ;WITH CTEInvoiceList AS                          
 (                           
  SELECT *,COUNT(T1.InvoiceNumber) OVER() AS Count FROM                           
  (                          
   SELECT ROW_NUMBER() OVER (ORDER BY                 
                          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate END END ASC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate END END DESC,  
	CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'DueDate' THEN DueDate END END ASC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'DueDate' THEN DueDate END END DESC, 
  
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'InvoiceAmount' THEN InvoiceAmount END END ASC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'InvoiceAmount' THEN InvoiceAmount END END DESC,  
  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN PaidAmount END END ASC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN PaidAmount END END DESC,  
  
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus END END ASC,  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus END END DESC 
  
 
                          
  ) AS Row ,
    INC.MonthId,
	INC.OrganizationId,
	INC.InvoiceNumber,
	ORG.displayName as OrganizationName,
	--INC.InvoiceDate,
	Replace(Convert(Varchar(15),INC.InvoiceDate,106),'-',' ') as InvoiceDate, 
	INC.PlanName ,
	Replace(Convert(Varchar(15),INC.DueDate,106),'-',' ') as DueDate, 
	--INC.DueDate ,
	INC.ActivePatientQuantity,
	INC.ActivePatientUnit ,
	INC.ActivePatientAmount ,
	INC.NumberOfTimeSheetQuantity ,
	INC.NumberOfTimeSheetUnit ,
	INC.NumberOfTimeSheetAmount ,
	INC.IVRQuantity ,
	INC.IVRUnit ,
	INC.IVRAmount,
	INC.MessageQuantity ,
	INC.MessageUnit,
	INC.MessageAmount,
	INC.ClaimsQuantity,
	INC.ClaimsUnit	,
	INC.ClaimsAmount,
	INC.FormsQuantity,
	INC.FormsUnit,
	INC.FormsAmount	,
	INC.InvoiceStatus,
	INC.Status,
	INC.CreatedDate,
	INC.PaidAmount,
	INC.InvoiceAmount,
	INC.FilePath,
	INC.OrginalFileName,
	INC.IsPaid
   FROM Invoice INC  
   INNER JOIN Organizations ORG
   ON ORG.OrganizationID=INC.OrganizationId
   WHERE
   INC.OrganizationId =@OrganizationId
		AND  
		((@InvoiceDate is null
		OR @InvoiceDate ='' OR CONVERT(DATE,INC.InvoiceDate) =CONVERT(DATE, @InvoiceDate))) 
		AND 
		((@DueDate is null OR @DueDate ='' OR CONVERT(DATE,INC.DueDate) = CONVERT(DATE, @DueDate)))
		AND( 
		(@InvoiceAmount IS NULL OR @InvoiceAmount='' OR LEN(@InvoiceAmount)=0) OR (CONVERT(NVARCHAR(30), INC.InvoiceAmount) = @InvoiceAmount)
		)
		AND  
			(@InvoiceStatus IS NULL OR @InvoiceStatus ='') OR (INC.IsPaid = @InvoiceStatus) 
		AND  
			(@PaidAmount IS NULL OR @PaidAmount ='') OR (CONVERT(NVARCHAR(30),INC.PaidAmount) = @PaidAmount) 
		AND  
			INC.Status = @InvoiceStatus
  ) AS T1          
 )                          
                           
 SELECT * FROM CTEInvoiceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   	
 END
