CREATE DATABASE QuantityMeasurementAppDB;

USE QuantityMeasurementAppDB;

-- Step 1: Create the table
CREATE TABLE quantity_measurements
(
    id               INT           IDENTITY(1,1) PRIMARY KEY,
    operation        NVARCHAR(50)  NOT NULL,
    first_value      FLOAT         NOT NULL DEFAULT 0,
    first_unit       NVARCHAR(50)  NULL,
    second_value     FLOAT         NOT NULL DEFAULT 0,
    second_unit      NVARCHAR(50)  NULL,
    result_value     FLOAT         NOT NULL DEFAULT 0,
    measurement_type NVARCHAR(50)  NULL,
    is_error         BIT           NOT NULL DEFAULT 0,
    error_message    NVARCHAR(500) NULL,
    created_at       DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- Step 2: sp_SaveMeasurement
-- Inserts one record into the table
CREATE PROCEDURE sp_SaveMeasurement
    @operation        NVARCHAR(50),
    @first_value      FLOAT,
    @first_unit       NVARCHAR(50),
    @second_value     FLOAT,
    @second_unit      NVARCHAR(50),
    @result_value     FLOAT,
    @measurement_type NVARCHAR(50),
    @is_error         BIT,
    @error_message    NVARCHAR(500)
AS
BEGIN
    INSERT INTO quantity_measurements
    (
        operation,
        first_value,
        first_unit,
        second_value,
        second_unit,
        result_value,
        measurement_type,
        is_error,
        error_message
    )
    VALUES
    (
        @operation,
        @first_value,
        @first_unit,
        @second_value,
        @second_unit,
        @result_value,
        @measurement_type,
        @is_error,
        @error_message
    );
END
GO

-- Step 3: sp_GetAllMeasurements
-- Returns all records newest first
CREATE PROCEDURE sp_GetAllMeasurements
AS
BEGIN
    SELECT
        id,
        operation,
        first_value,
        first_unit,
        second_value,
        second_unit,
        result_value,
        measurement_type,
        is_error,
        error_message,
        created_at
    FROM quantity_measurements
    ORDER BY created_at DESC;
END
GO

-- Step 4: sp_GetMeasurementsByOperation
-- Returns records matching the given operation name
CREATE PROCEDURE sp_GetMeasurementsByOperation
    @operation NVARCHAR(50)
AS
BEGIN
    SELECT
        id,
        operation,
        first_value,
        first_unit,
        second_value,
        second_unit,
        result_value,
        measurement_type,
        is_error,
        error_message,
        created_at
    FROM quantity_measurements
    WHERE operation = @operation
    ORDER BY created_at DESC;
END
GO

-- Step 5: sp_GetMeasurementsByType
-- Returns records matching the given measurement type
CREATE PROCEDURE sp_GetMeasurementsByType
    @measurement_type NVARCHAR(50)
AS
BEGIN
    SELECT
        id,
        operation,
        first_value,
        first_unit,
        second_value,
        second_unit,
        result_value,
        measurement_type,
        is_error,
        error_message,
        created_at
    FROM quantity_measurements
    WHERE measurement_type = @measurement_type
    ORDER BY created_at DESC;
END
GO

-- Step 6: sp_GetTotalCount
-- Returns the total number of records
CREATE PROCEDURE sp_GetTotalCount
AS
BEGIN
    SELECT COUNT(*) AS TotalCount
    FROM quantity_measurements;
END
GO

-- Step 7: sp_DeleteAllMeasurements
-- Deletes every record from the table
CREATE PROCEDURE sp_DeleteAllMeasurements
AS
BEGIN
    DELETE FROM quantity_measurements;
END
GO
