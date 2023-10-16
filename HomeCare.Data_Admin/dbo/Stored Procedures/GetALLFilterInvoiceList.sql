
CREATE PROCEDURE [dbo].[GetALLFilterInvoiceList] --@IsPaid=null,@CompanyName=null,@InvoiceDate =null,@DueDate =null,@InvoiceAmount=null,@PaidAmount=null,@InvoiceStatus="1",@IsAll=null,@SortExpression=null, @SortType="ASC",  @FromIndex="1", @PageSize="20" 
	-- Add the parameters for the stored procedure here
@CompanyName NVARCHAR(200)=NULL,      
@InvoiceDate DATE=NULL,      
@DueDate DATE=NULL,      
@InvoiceStatus NVARCHAR(50)=NULL,      
@SortExpression NVARCHAR(100)=NULL,                              
@SortType NVARCHAR(10)=NULL,                            
@FromIndex INT=NULL,                            
@PageSize INT=NULL,
@IsAll bit=0,
@InvoiceAmount NVARCHAR(20)=NULL,
@PaidAmount NVARCHAR(20)=NULL,
@IsPaid bit=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	;WITH CTEInvoiceList AS                                
 (                                 
  SELECT ROW_NUMBER() OVER (ORDER BY                      
       
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate              
    WHEN @SortExpression = 'DueDate' THEN DueDate
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
    WHEN @SortExpression = 'InvoiceDate' THEN InvoiceDate              
    WHEN @SortExpression = 'DueDate' THEN DueDate 
   END                                
  END DESC,
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'DisplayName' THEN CompanyName                    
    --WHEN @SortExpression = 'CompanyName' THEN CompanyName
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'DisplayName' THEN CompanyName                    
    --WHEN @SortExpression = 'CompanyName' THEN CompanyName
   END                                
  END DESC,
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'InvoiceAmount' THEN InvoiceAmount              
    WHEN @SortExpression = 'PaidAmount' THEN PaidAmount
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'InvoiceAmount' THEN InvoiceAmount              
    WHEN @SortExpression = 'PaidAmount' THEN PaidAmount
   END                                
  END DESC,
  CASE WHEN @SortType = 'ASC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus
   END                                 
  END ASC,                                
  CASE WHEN @SortType = 'DESC' THEN                                
   CASE                                       
	WHEN @SortExpression = 'InvoiceStatus' THEN InvoiceStatus
   END                                
  END DESC
  )AS Row, 
	INC.MonthId,
	INC.OrganizationId,
	INC.InvoiceNumber,
	--INC.InvoiceDate,
	Replace(Convert(Varchar(15),INC.InvoiceDate,106),'-','-') as InvoiceDate, 
	--INC.InvoiceDate as InvoiceDate1, 
	INC.PlanName ,
	--INC.DueDate as DueDate1, 
	Replace(Convert(Varchar(15),INC.DueDate,106),'-','-') as DueDate, 
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
	INC.Filepath,
	INC.OrginalFileName,
	(SELECT DisplayName FROM dbo.Organizations ORG WITH(NOLOCK) WHERE ORG.OrganizationId=INC.OrganizationId) AS OrganizationName
  FROM  dbo.Invoice INC  
  INNER JOIN dbo.Organizations ORG 
  ON  ORG.OrganizationId=INC.OrganizationId 
  WHERE 
  ((@CompanyName IS NULL OR LEN(@CompanyName)=0) OR ORG.DisplayName LIKE '%' + RTRIM(LTRIM(@CompanyName)) + '%')
  AND  
   ((@InvoiceDate is null
   OR @InvoiceDate ='' OR CONVERT(DATE,INC.InvoiceDate) =CONVERT(DATE, @InvoiceDate))) 
  AND ((@DueDate is null OR @DueDate ='' OR CONVERT(DATE,INC.DueDate) = CONVERT(DATE, @DueDate)))
  AND( 
	(@InvoiceAmount IS NULL OR LEN(@InvoiceAmount)=0) OR (CONVERT(NVARCHAR(30), INC.InvoiceAmount) = @InvoiceAmount)
  )
  AND  
	(@InvoiceStatus IS NULL OR LEN(@InvoiceStatus)=0) OR (INC.InvoiceStatus = @InvoiceStatus) 
  AND  
	(@PaidAmount IS NULL OR  @PaidAmount='') OR ((CONVERT(NVARCHAR(30), INC.PaidAmount)) = @PaidAmount ) 
  AND  
	(INC.IsPaid=@IsPaid)  OR (INC.IsPaid=@IsPaid)
)         
  
                         
 SELECT  * FROM CTEInvoiceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)  
END