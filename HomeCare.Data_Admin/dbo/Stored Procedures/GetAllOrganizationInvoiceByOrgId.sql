

--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: Get All Organization InvoicE By OrganizationId FOR MAIN WEB APPLICATION

CREATE PROCEDURE [dbo].[GetAllOrganizationInvoiceByOrgId]
@InvoiceDate DATE = NULL,
@DueDate DATE = NULL,
@OrganizationId BIGINT,
@InvoiceAmount NVARCHAR(50),            
@PaidAmount NVARCHAR(50),     
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
	Replace(Convert(Varchar(15),INC.InvoiceDate,103),'-','/') as InvoiceDate, 
	INC.PlanName ,
	Replace(Convert(Varchar(15),INC.DueDate,103),'-','/') as DueDate, 
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
	INC.InvoiceAmount 
   FROM Invoice INC  
   INNER JOIN Organizations ORG
   ON ORG.OrganizationID=INC.OrganizationId
   WHERE
   INC.OrganizationId =@OrganizationId
   --AND
   --((@InvoiceStatus IS NULL  OR LEN(@InvoiceStatus)=0 OR @InvoiceStatus=0) OR INC.InvoiceStatus = @InvoiceStatus)  
   --AND ((@InvoiceDate IS NULL OR LEN(@InvoiceDate)=0) OR CONVERT(DATE,INC.InvoiceDate) = @InvoiceDate)  
   --AND ((@DueDate IS NULL OR LEN(@DueDate)=0) OR CONVERT(DATE,INC.DueDate) = @DueDate)  
   --AND ((@InvoiceAmount IS NULL OR LEN(@InvoiceAmount)=0) OR IsNull(INC.InvoiceAmount,0) =CONVERT(DECIMAL(18,2), @InvoiceAmount))  
   --AND ((@PaidAmount IS NULL OR LEN(@PaidAmount)=0) OR ISNULL(INC.PaidAmount,0) =CONVERT(DECIMAL(18,2), @PaidAmount))
   --AND INC.Status=@Status
  ) AS T1          
 )                          
                           
 SELECT * FROM CTEInvoiceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   	
 END


 --select * from invoice