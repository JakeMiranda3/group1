-- Write your stored procedures here

-- Colby's Stored Procedure

DELIMITER $
drop procedure if exists setAbnormality$
CREATE PROCEDURE setAbnormality(IN p_appointment_id INT, IN p_test_code INT, IN p_test_result varchar(1024))
proc: BEGIN
    DECLARE v_low    DECIMAL(5,2);
    DECLARE v_high   DECIMAL(5,2);
    DECLARE v_result DECIMAL(5,2);

    SELECT low_value, high_value
      INTO v_low, v_high
      FROM test
     WHERE test_code = p_test_code;

    IF v_low is NULL THEN
        LEAVE proc;
    END IF;
    
    SET v_result = CAST(p_test_result AS DECIMAL(5,2));

    IF v_result < v_low OR v_result > v_high THEN
        UPDATE test_order
           SET is_abnormal = TRUE
         WHERE appointment_id = p_appointment_id
           AND test_code = p_test_code;
    ELSE
        UPDATE test_order
           SET is_abnormal = FALSE
         WHERE appointment_id = p_appointment_id
           AND test_code = p_test_code;
    END IF;
END$

-- Mathew Stored Procedure
DELIMITER $
DROP PROCEDURE IF EXISTS get_most_performed_tests_during_dates$

CREATE PROCEDURE get_most_performed_tests_during_dates(IN start_date DATE, IN end_date DATE)
BEGIN 
  DECLARE total_test INT;

  SELECT COUNT(*) INTO total_test
  FROM test_order
  WHERE performed_datetime BETWEEN start_date AND end_date;

  SELECT `to`.test_code AS TestCode, `name` AS `Name`, COUNT(*) AS `SpecificTestCount`, total_test AS TotalTests,
  ROUND((COUNT(*) / total_test) * 100, 2) AS Percent, 
  SUM(
    CASE
      WHEN is_abnormal = FALSE THEN 1
      ELSE 0
    END
  ) AS NormalCount,
  SUM(
    CASE 
      WHEN is_abnormal = TRUE THEN 1
      ELSE 0
    END
  ) AS AbnormalCount
  FROM test_order AS `to` join test AS t ON `to`.test_code = t.test_code
  WHERE performed_datetime BETWEEN start_date AND end_date
  GROUP BY `to`.test_code, `name`
  HAVING SpecificTestCount >= 2
  ORDER BY SpecificTestCount DESC, `to`.test_code DESC;
END$

DELIMITER ;

call get_most_performed_tests_during_dates("1978-01-01", "2100-12-12"); -- Grabs every performed test to test procedure


-- William's Stored Procedure
DELIMITER $
DROP PROCEDURE IF EXISTS GetTestOrdersByAppointmentId;
CREATE PROCEDURE GetTestOrdersByAppointmentId(IN p_appointment_id INT)
BEGIN
    SELECT o.appointment_id, o.test_code, o.performed_datetime, o.test_result, o.is_abnormal,
           t.name, t.low_value, t.high_value, t.measurement_type, o.result_entered_datetime
    FROM test_order o
    JOIN test t ON o.test_code = t.test_code
    WHERE o.appointment_id = p_appointment_id;
END$
DELIMITER ;


-- Jake Stored Procedure
DROP PROCEDURE IF EXISTS SearchPatients$
DELIMITER $
CREATE PROCEDURE SearchPatients(p_firstName VARCHAR(100),p_lastName VARCHAR(100),p_dateOfBirth DATE
)
BEGIN
    SELECT patient.*
    FROM patient
    INNER JOIN person
        ON patient.person_id = person.person_id
    WHERE (p_firstName IS NULL
        OR person.first_name LIKE CONCAT(p_firstName, '%'))
    AND (p_lastName IS NULL
        OR person.last_name LIKE CONCAT(p_lastName, '%'))
    AND (p_dateOfBirth IS NULL
        OR DATE(person.date_of_birth) = p_dateOfBirth)
    ORDER BY person.last_name, person.first_name;
END$

DELIMITER ;

