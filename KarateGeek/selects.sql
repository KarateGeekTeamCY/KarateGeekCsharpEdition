select * from persons;

select * from athletes;

select * from addresses;

select * from users;	

select * from persons JOIN athletes on persons.id = athletes.id where persons.first_name like 'd%';

