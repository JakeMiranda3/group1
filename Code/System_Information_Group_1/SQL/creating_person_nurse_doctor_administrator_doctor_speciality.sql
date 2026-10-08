
use cs3230f26_g1;
SET FOREIGN_KEY_CHECKS = 0;

drop table if exists `person`;
drop table if exists `administrator`;
drop table if exists `nurse`;
drop table if exists `doctor`;
drop table if exists `doctor_speciality`;
drop table if exists `appointment`;
drop table if exists `patient`;
drop table if exists `visit`;


CREATE TABLE person (
    person_id INT AUTO_INCREMENT,
    last_name VARCHAR(20) NOT NULL,
    first_name VARCHAR(20) NOT NULL,
    date_of_birth DATETIME NOT NULL,
    contact_phone_number CHAR(12),
    address VARCHAR(20),
    zip VARCHAR(9),
    city VARCHAR(20),
    state VARCHAR(20),
    PRIMARY KEY (person_id)
);

CREATE TABLE administrator (
    person_id INT,
    administrator_id INT AUTO_INCREMENT NOT NULL,
    CONSTRAINT administrator_fk_person FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    PRIMARY KEY (person_id),
    UNIQUE uq_administrator_administrator_id (administrator_id)
);

CREATE TABLE nurse (
    person_id INT,
    nurse_id INT AUTO_INCREMENT NOT NULL,
    CONSTRAINT nurse_fk_person FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    PRIMARY KEY (person_id),
    UNIQUE uq_nurse_nurse_id (nurse_id)
);

CREATE TABLE doctor (
    person_id INT,
    doctor_id INT AUTO_INCREMENT NOT NULL,
    PRIMARY KEY (person_id),
    CONSTRAINT doctor_fk_person FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    UNIQUE uq_doctor_doctor_id (doctor_id)
);

CREATE TABLE doctor_speciality (
    person_id INT,
    speciality_name VARCHAR(20),
    PRIMARY KEY (person_id , speciality_name),
    CONSTRAINT doctor_speciality_fk_person FOREIGN KEY (person_id)
        REFERENCES person (person_id)
);

CREATE TABLE patient (
    person_id INT UNIQUE PRIMARY KEY,
    patient_id INT AUTO_INCREMENT,
    is_active BOOLEAN DEFAULT TRUE,
    CONSTRAINT patient_fk_person FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    UNIQUE uq_patient_patient_id (patient_id)
);

CREATE TABLE appointment (
    appointment_id INT AUTO_INCREMENT PRIMARY KEY,
    doctor_id INT NOT NULL,
    patient_id INT NOT NULL,
    reason VARCHAR(100),
    appointment_datetime DATETIME,
    UNIQUE (patient_id , appointment_datetime),
    UNIQUE (doctor_id , appointment_datetime),
    CONSTRAINT appointment_fk_doctor FOREIGN KEY (doctor_id)
        REFERENCES doctor (doctor_id),
    CONSTRAINT appointment_fk_patient FOREIGN KEY (patient_id)
        REFERENCES patient (patient_id)
);

CREATE TABLE visit (
    appointment_id INT PRIMARY KEY,
    symptoms VARCHAR(200),
    nurse_id INT,
    systolic_blood_pressure INT,
    diastolic_blood_pressure INT,
    temperature DECIMAL(4 , 1 ),
    pulse INT,
    height INT,
    weight INT,
    initial_diagnosis VARCHAR(200),
    final_diagnosis VARCHAR(200),
    CONSTRAINT visit_fk_appointment FOREIGN KEY (appointment_id)
        REFERENCES appointment (appointment_id),
    CONSTRAINT visit_fk_nurse FOREIGN KEY (nurse_id)
        REFERENCES nurse (nurse_id)
);


SET FOREIGN_KEY_CHECKS = 1;
