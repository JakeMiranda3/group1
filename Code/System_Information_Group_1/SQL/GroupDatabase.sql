SET FOREIGN_KEY_CHECKS = 0;
drop table if exists person;
drop table if exists administrator;
drop table if exists nurse;
drop table if exists doctor;
drop table if exists doctor_speciality;
drop table if exists patient;
drop table if exists appointment;

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
    FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    PRIMARY KEY (person_id),
    UNIQUE uq_administrator_administrator_id (administrator_id)
);

CREATE TABLE nurse (
    person_id INT,
    nurse_id INT AUTO_INCREMENT NOT NULL,
    FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    PRIMARY KEY (person_id),
    UNIQUE uq_nurse_nurse_id (nurse_id)
);

CREATE TABLE doctor (
    person_id INT,
    doctor_id INT AUTO_INCREMENT NOT NULL,
    PRIMARY KEY (person_id),
    FOREIGN KEY (person_id)
        REFERENCES person (person_id),
    UNIQUE uq_doctor_doctor_id (doctor_id)
);

CREATE TABLE doctor_speciality (
    person_id INT,
    speciality_name VARCHAR(20),
    PRIMARY KEY (person_id , speciality_name),
    FOREIGN KEY (person_id)
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
    CONSTRAINT appointment_fk_doctor FOREIGN KEY (doctor_id)
        REFERENCES doctor (doctor_id),
    CONSTRAINT appointment_fk_patient FOREIGN KEY (patient_id)
        REFERENCES patient (patient_id),
	UNIQUE uq_appointment_patient_id_appointment_datetime (patient_id , appointment_datetime),
	UNIQUE uq_appointment_doctor_id_appointment_datetime (doctor_id , appointment_datetime)
);

CREATE TABLE account(
	person_id INT, 
    account_type enum('Administrator', 'Nurse'),
    username varchar(100), 
    hashed_password varchar(100),
    primary key (person_id, account_type),
    constraint account_fk_person 
    foreign key (person_id) references person(person_id), 
    unique uq_account_username (username)
);

CREATE TABLE account(
	person_id INT, 
    account_type enum('Administrator', 'Nurse'),
    username varchar(100), 
    hashed_password varchar(100),
    primary key (person_id, account_type),
    constraint account_fk_person 
    foreign key (person_id) references person(person_id), 
    unique uq_account_username (username)
);
SET FOREIGN_KEY_CHECKS = 1;


	

