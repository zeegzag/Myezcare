create procedure [dbo].[GetTokenList]  
(  
  @Module nvarchar(128)  
)  
as  
begin  
        select e.TokenID,e.Tokens from EmailTemplateTokens e where e.Module=@Module   
end