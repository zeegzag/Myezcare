--UpdatedBy: Akhilesh kamal    
--UpdatedDate: 12 JUNE 2020    
-- Decsription: Save RegularHours,RegularPayhourly,OvertimePayHourly in employees table    
-- exec SaveRegularHours 3,1.5,1.5,1.5,2.9  
CREATE PROC SaveRegularHours
  @EmployeeID int = NULL,
  @RegularHours float = NULL,
  @RegularHourType int = NULL,
  @RegularPayHours float = NULL,
  @OvertimePayHours float = NULL
--@OvertimeHours float =null     
AS
BEGIN

  IF (@EmployeeID > 0)
  BEGIN
    UPDATE Employees
    SET
      RegularHours = @RegularHours,
      RegularPayHours = @RegularPayHours,
      OvertimePayHours = @OvertimePayHours,
      RegularHourType = @RegularHourType
    WHERE
      EmployeeID = @EmployeeID;

    SELECT
      1;

    RETURN;
  END
  ELSE
  BEGIN

    SELECT
      0;
  END
END