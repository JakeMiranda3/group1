create table person(
	person_id int auto_increment,
    last_name varchar(20) not null, 
    first_name varchar(20) not null, 
    date_of_birth dateTime not null, 
	contact_phone_number char(12), 
    address varchar(20), 
    zip varchar(9), 
    city varchar(20), 
    state varchar(20),
    primary key(person_id)
);

create table administrator(
	person_id int, 
    administrator_id int auto_increment not null, 
    
    foreign key administrator_fk_person
    (person_id) references person(person_id), 
    
    primary key(person_id), 
    
    unique uq_administrator_administrator_id (administrator_id)
);

create table nurse(
	person_id int, 
    nurse_id int auto_increment not null, 
    
    foreign key nurse_fk_person
    (person_id) references person(person_id), 
    
    primary key (person_id),
    unique uq_nurse_nurse_id (nurse_id)
);

create table doctor(
	person_id int, 
    doctor_id int auto_increment not null,
    
    primary key(person_id), 
    
    foreign key doctor_fk_person 
    (person_id) references person(person_id), 
    
    unique uq_doctor_doctor_id (doctor_id)
);

create table doctor_speciality(
	person_id int, 
    speciality_name varchar(20), 
    
    primary key(person_id, speciality_name),
    
    foreign key doctor_speciality_fk_person 
    (person_id) references person(person_id)
    
    
);

