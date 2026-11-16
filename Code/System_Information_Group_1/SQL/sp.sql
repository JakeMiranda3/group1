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

  SELECT `to`.test_code, `name`, COUNT(*) AS `specific_test_count`, total_test,
  ROUND((COUNT(*) / total_test) * 100, 2) AS percent, 
  SUM(
    CASE
      WHEN is_abnormal = FALSE THEN 1
      ELSE 0
    END
  ) AS normal_count,
  SUM(
    CASE 
      WHEN is_abnormal = TRUE THEN 1
      ELSE 0
    END
  ) AS abnormal_count
  FROM test_order AS `to` join test AS t ON `to`.test_code = t.test_code
  WHERE performed_datetime BETWEEN start_date AND end_date
  GROUP BY `to`.test_code, `name`
  HAVING specific_test_count >= 2
  ORDER BY specific_test_count DESC, `to`.test_code DESC;
END$

DELIMITER ;

call get_most_performed_tests_during_dates("1980-01-01", "2100-12-12"); -- Grabs every performed test


