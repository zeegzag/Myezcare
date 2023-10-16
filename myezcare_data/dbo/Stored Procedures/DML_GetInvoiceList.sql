

--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: Get Invoice List FOR MAIN WEB APLICATION

CREATE PROCEDURE [dbo].[GetInvoiceList] --@CompanyName=null,@InvoiceDate =null,@DueDate =null,@InvoiceStatus="",@IsAll=1,@SortExpression="CompanyName", @SortType="ASC",  @FromIndex="1", @PageSize="20" 
	-- Add the parameters for the stored procedure here
@CompanyName NVARCHAR(200)=null,      
@InvoiceDate DATE=null,      
@DueDate DATE=null,      
@InvoiceStatus NVARCHAR(50),      
--@IsDeleted BIGINT = -1,     
@SortExpression NVARCHAR(100),                              
@SortType NVARCHAR(10),                            
@FromIndex INT,                            
@PageSize INT,
@IsAll bit,
@InvoiceAmount NVARCHAR(20)
--@PaidAmount DECIMAL(18,2)=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	print @IsAll
	SET NOCOUNT off;
	IF @IsAll=1 
	BEGIN
		print @IsAll
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
	INC.InvoiceAmount,
	(SELECT DisplayName FROM dbo.Organizations ORG WITH(NOLOCK) WHERE ORG.OrganizationId=INC.OrganizationId) AS OrganizationName
  FROM  dbo.Invoice INC  
  INNER JOIN dbo.Organizations ORG 
  ON  ORG.OrganizationId=INC.OrganizationId
  WHERE 
  ((@CompanyName IS NULL OR LEN(@CompanyName)=0) OR ORG.DisplayName LIKE '%' + @CompanyName + '%')
  AND  
  ((@InvoiceDate is null OR CONVERT(DATE,INC.InvoiceDate) >= @InvoiceDate) and (@InvoiceDate is null OR CONVERT(DATE,INC.InvoiceDate) <= @InvoiceDate))
  AND  
  ((@DueDate IS NULL OR LEN(@DueDate)=0) OR Convert(DATE,INC.DueDate) >= @DueDate )
  AND  
  (@InvoiceAmount IS NULL OR LEN(@InvoiceAmount)=0) OR (CONVERT(NVARCHAR(30), INC.InvoiceAmount) >= @InvoiceAmount)
  --AND  
  --(@PaidAmount IS NULL OR LEN(@PaidAmount)=0) OR (INC.PaidAmount) >= @PaidAmount ) 

)         
  
                         
 SELECT top 2 * FROM CTEInvoiceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
	print @IsAll
	END
	ELSE
	BEGIN
	print @IsAll
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
	INC.InvoiceAmount,
	(SELECT DisplayName FROM dbo.Organizations ORG WITH(NOLOCK) WHERE ORG.OrganizationId=INC.OrganizationId) AS OrganizationName
  FROM  dbo.Invoice INC  
  INNER JOIN dbo.Organizations ORG 
  ON  ORG.OrganizationId=INC.OrganizationId
  WHERE 
  ((@CompanyName IS NULL OR LEN(@CompanyName)=0) OR ORG.DisplayName LIKE '%' + @CompanyName + '%')
  AND  
  ((@InvoiceDate is null OR CONVERT(DATE,INC.InvoiceDate) >= @InvoiceDate) and (@InvoiceDate is null OR CONVERT(DATE,INC.InvoiceDate) <= @InvoiceDate))
  AND  
  ((@DueDate IS NULL OR LEN(@DueDate)=0) OR Convert(DATE,INC.DueDate) >= @DueDate )
  AND  
  (@InvoiceAmount IS NULL OR LEN(@InvoiceAmount)=0) OR (CONVERT(NVARCHAR(30), INC.InvoiceAmount) >= @InvoiceAmount)
)         
  

  --ALTER TABLE Invoice ADD InvoiceAmount Decimal(18,3);
                         
 SELECT top 4 * FROM CTEInvoiceList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)   
	print @IsAll
	END
END
