insert into countries (code , name) values('CY', 'Cyprus');
insert into countries (code , name) values('GR', 'Greece');

insert into cities (name, country_code) values ('Limassol', 'CY');
insert into cities (name, country_code) values ('Nicosia', 'CY');
insert into cities (name, country_code) values ('Paphos', 'CY');
insert into cities (name, country_code) values ('Larnaca', 'CY');
insert into cities (name, country_code) values ('Amochostos', 'CY');
insert into cities (name, country_code) values ('Thessaloniki', 'GR');


  
insert into addresses (id , street, "number", city_id , postal_code , country_code ) values ('0' , 'tepak' , '1' , '1' , '3025' , 'CY'); 


insert into clubs (name, address_id, country_code) values ('power fight club', 0, 'CY');


insert into persons(id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id) 
values ('0' , 'administrator' , 'xampis' , 'administrator', '02-10-1990' , 'male', '99123144' , null , 'email@gmail.com' , '0');

<<<<<<< HEAD
=======
insert into users( id , username , password, person_management, event_management , lottery , game_support , reports , settings) values ( '0', 'admin' , '3039283064aa2a9ca939b1fe23954698' , '1' , '1' , '1' , '1' , '1' , '1');


>>>>>>> a7c942ecf4612043d3147398b5e0deda134c4500
insert into athletes (id, rank, club_id ) values ('0', 'black', '1'  );



select * from cities;


select * from addresses;


select * from persons;


select * from clubs;