CREATE PROCEDURE [dbo].[SaveEBForms]
  @FormId nvarchar(max),
  @Name nvarchar(max),
  @FormLongName nvarchar(max),
  @NameForUrl nvarchar(max),
  @Version nvarchar(max),
  @IsActive bit,
  @EBCategoryIDs nvarchar(max),
  @EbMarketIDs nvarchar(max),
  @OrganizationIDs nvarchar(max),
  @FormPrice decimal(10, 2),
  @InternalFormPath nvarchar(max),
  @NewPdfURI nvarchar(max),
  @IsInternalForm bit,
  @IsDeleted bit,
  @IsINSUp nvarchar(max),
  @IsOrbeonForm bit,
  @EBFormID nvarchar(max)
AS

BEGIN

  --if exists (select [Name] from EBForms where [Name] = @Name and version= @version and EBCategoryID=@EBCategoryID and EbMarketIDs=@EbMarketIDs)  
  --Begin  
  -- Select '-1'  
  --end  
  IF (@EBFormID <> '')
  BEGIN

    UPDATE [dbo].[EBForms]
    SET
      [FormId] = @FormId,
      [Name] = @Name,
      [FormLongName] = @FormLongName,
      [NameForUrl] = @NameForUrl,
      [Version] = @Version,
      [IsActive] = @IsActive,
      [NewPdfURI] = @NewPdfURI,
      [EBCategoryID] = @EBCategoryIDs,
      [EbMarketIDs] = @EbMarketIDs,
      [OrganizationIDs] = @OrganizationIDs,
      [FormPrice] = @FormPrice,
      [UpdatedDate] = GETDATE(),
      [IsInternalForm] = @IsInternalForm,
      [InternalFormPath] = @InternalFormPath,
      [IsOrbeonForm] = @IsOrbeonForm,
      [IsDeleted] = @IsDeleted
    WHERE
      EBFormID = @EBFormID

    SELECT
      2



  END
  ELSE
  BEGIN

    IF EXISTS (SELECT
        [Name]
      FROM EBForms
      WHERE [Name] = @Name
      AND version = @version)
    BEGIN
      SELECT
        '-1'
    END
    ELSE
    BEGIN

      DECLARE @guidValue varchar(36)
      SET @guidValue = NEWID()
      DECLARE @Id bigint
      SELECT
        @Id = MAX(CONVERT(bigint, Id))
      FROM [dbo].[EBForms]
      SET @Id = @Id + 1
      SET @FormId = @Id
      INSERT INTO [dbo].[EBForms]
      (
        [EBFormID],
        [FromUniqueID],
        [Id],
        [FormId],
        [Name],
        [FormLongName],
        [NameForUrl],
        [Version],
        [IsActive],
        [HasHtml],
        [NewHtmlURI],
        [HasPDF],
        [NewPdfURI],
        [EBCategoryID],
        [EbMarketIDs],
        [OrganizationIDs],
        [FormPrice],
        [CreatedDate],
        [UpdatedDate],
        [UpdatedBy],
        [IsDeleted],
        [IsInternalForm],
        [InternalFormPath],
        [IsOrbeonForm]
      )
      VALUES
      (
        @guidValue,
        @guidValue,
        @Id,
        @FormId,
        @Name,
        @FormLongName,
        @NameForUrl,
        @Version,
        @IsActive,
        1,
        NULL,
        1,
        @NewPdfURI,
        @EBCategoryIDs,
        @EbMarketIDs,
        @OrganizationIDs,
        @FormPrice,
        GETDATE(),
        GETDATE(),
        NULL,
        0,
        @IsInternalForm,
        @InternalFormPath,
        @IsOrbeonForm
      )

      SELECT
        1
    END
  END


END