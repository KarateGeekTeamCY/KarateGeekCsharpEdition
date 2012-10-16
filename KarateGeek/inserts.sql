insert into countries (code , name) values('CY', 'Cyprus');
insert into countries (code , name) values('GR', 'Greece');

insert into cities (name, country_code) values ('Limassol', 'CY');
insert into cities (name, country_code) values ('Nicosia', 'CY');
insert into cities (name, country_code) values ('Paphos', 'CY');
insert into cities (name, country_code) values ('Larnaca', 'CY');
insert into cities (name, country_code) values ('Amochostos', 'CY');
insert into cities (name, country_code) values ('Thessaloniki', 'GR');

  
insert into addresses (id , street, "number", city_id , postal_code , country_code ) values ('1' , 'kapou' , '3' , 1 , '3025' , 	'CY'); 


insert into clubs (name, address_id, country_code) values ('power fight club', 1, 'CY');


insert into persons(id , first_name, middle_name, last_name, date_of_birth, phone, secondary_phone, email, address_id) 
values ('1' , 'dimitris' , null , 'apostolou', '02-10-1990' , '99123144' , null , 'email@gmail.com' , '1');

