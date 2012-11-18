select * from persons;

select * from athletes;

select * from addresses;

select * from users;

select * from persons JOIN athletes on persons.id = athletes.id where persons.first_name like 'd%';
-- same as:
--select * from persons NATURAL JOIN athletes where persons.first_name like 'd%';

select * from clubs;