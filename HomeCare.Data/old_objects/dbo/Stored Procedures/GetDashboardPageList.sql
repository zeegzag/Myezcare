CREATE PROCEDURE [dbo].[GetDashboardPageList]            
@GetReferralRelatedData varchar(10)=null,        
@SortExpressionIM NVARCHAR(100),          
@SortExpressionCS NVARCHAR(100),                    
@SortExpressionDL NVARCHAR(100),                              
@SortType NVARCHAR(10),                  
@FromIndex INT,                  
@PageSize INT,        
@InternalMessageAssigneeID bigint,        
@AssigneeID bigint,      
@ReferralStatusIds varchar(100),
@ReferralStatusIdsForInDoc  varchar(100)   
AS        
        
BEGIN        
 IF(@GetReferralRelatedData=1)        
   BEGIN        
  exec GetDashboardInternalMessgaeList @InternalMessageAssigneeID,@AssigneeID,@SortExpressionIM ,@SortType,@FromIndex,@PageSize    
 exec GetDashboardInCompeleteSparFormandCheckList @AssigneeID,@SortExpressionCS ,@SortType,@FromIndex,@PageSize,@ReferralStatusIds    
 exec GetDashboardMissingandExpireDocumentList @AssigneeID,@SortExpressionDL ,@SortType,@FromIndex,@PageSize ,@ReferralStatusIds        
 exec GetDashboardResolvedInternalMessgaeList  @InternalMessageAssigneeID,@AssigneeID,@SortExpressionIM ,@SortType,@FromIndex,@PageSize    
 exec GetDashboardInternalMissingandExpireDocumentList  @AssigneeID,@SortExpressionDL ,@SortType,@FromIndex,@PageSize ,@ReferralStatusIdsForInDoc        
   END        
  ELSE        
    BEGIN        
     Select Null;        
     Select Null;        
     Select Null;
	 Select Null;
	 Select Null;        
    END         
END
