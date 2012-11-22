drop view athlete_view;

create or replace view athlete_view as 
select persons.id as persons_id, 
first_name,last_name,
fathers_name,
sex,
extract(year from age(date_of_birth)) as age,
phone, 
secondary_phone,
email,
address_id, 
addresses.street as stret_name, 
cities.name as city, 
countries.name as country, 
addresses.number as address_num,  
rank from persons 
join athletes on persons.id = athletes.id 
join addresses on persons.address_id = addresses.id 
join countries on addresses.country_code = countries.code 
join cities on cities.id = addresses.city_id;    

select * from athlete_view;                                   