
--UpdatedDate: 13 june 2020
-- Decsription: Add column in Employees table RegularHours,RegularPayhourly,OvertimePayHourly
        
ALTER TABLE employees
ADD RegularHours float;

ALTER TABLE employees
ADD RegularPayhourly float;

ALTER TABLE employees
ADD OvertimePayHourly float;

