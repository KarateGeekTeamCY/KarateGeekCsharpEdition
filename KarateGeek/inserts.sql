insert into countries (code , name) values('CY', 'Cyprus');
insert into countries (code , name) values('GR', 'Greece');

insert into cities (name) values ('Limassol');
insert into cities (name) values ('Nicosia');
insert into cities (name) values ('Paphos');
insert into cities (name) values ('Larnaca');
insert into cities (name) values ('Amochostos');

insert into clubs (name, address_id, country_code) values ('power fight club', 1, 'CY');
  
insert into addresses (id , street, "number", city , postal_code , country_code ) values ('1' , 'kapou' , '3' , 1 , '3025' , 	'CY'); 

insert into persons(id , first_name, middle_name, last_name, date_of_birth, phone, secondary_phone, email, address_id) 
values ('1' , 'dimitris' , null , 'apostolou', '02-10-1990' , '99123144' , null , 'email@gmail.com' , '1');


