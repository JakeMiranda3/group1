-- Write your stored procedures here

-- Colby's Stored Procedure

DELIMITER $
drop procedure if exists setAbnormality;
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

DELIMITER ;


-- William's Stored Procedure
DELIMITER $
DROP PROCEDURE IF EXISTS GetTestOrdersByAppointmentId;
CREATE PROCEDURE GetTestOrdersByAppointmentId(IN p_appointment_id INT)
BEGIN
	SELECT o.appointment_id, o.test_code, o.performed_datetime, o.test_result, o.is_abnormal,
		   t.name, t.low_value, t.high_value, t.measurement_type
	FROM test_order o
	JOIN test t ON o.test_code = t.test_code
	WHERE o.appointment_id = p_appointment_id;
END$
DELIMITER ;


