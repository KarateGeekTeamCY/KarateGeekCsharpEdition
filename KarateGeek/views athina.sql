create or replace view athlete_first_places_ind as
select athlete_id, COUNT(athlete_id)
from tournament_participations where ranking = '1'
group by athlete_id;

create or replace view athlete_second_places_ind as
select athlete_id, count(athlete_id)
from tournament_participations where ranking = '2'
group by athlete_id;

create or replace view athlete_third_places_ind as
select athlete_id, count(athlete_id)
from tournament_participations where ranking = '3'
group by athlete_id;





drop view athletes_total_details;

CREATE or REPLACE  VIEW athletes_total_details AS
select athletes.id, first_name, last_name, fathers_name, sex, date_of_birth, persons.phone, secondary_phone, persons.email,
		rank, clubs.name as club_name, street, addresses.number, addresses.postal_code, cities.name as city, countries.name as country,
		athlete_first_places_ind.count as first_places, 
		athlete_second_places_ind.count as second_places, 
		athlete_third_places_ind.count as third_places

FROM  persons join athletes 
	on athletes.id = persons.id 
LEFT JOIN addresses 
	on persons.address_id = addresses.id 
LEFT JOIN clubs 
	on athletes.club_id = clubs.id
LEFT JOIN cities 
	on cities.id = addresses.city_id
LEFT JOIN countries
	on countries.code = cities.country_code
LEFT JOIN athlete_first_places_ind
	on athlete_first_places_ind.athlete_id = athletes.id
LEFT JOIN athlete_second_places_ind
	on athlete_second_places_ind.athlete_id = athletes.id
LEFT JOIN athlete_third_places_ind
	on athlete_third_places_ind.athlete_id = athletes.id;






drop view judges_total_details;

CREATE or REPLACE  VIEW judges_total_details AS
SELECT judges.id, first_name, last_name, sex, date_of_birth, persons.phone, persons.email,
	street, addresses.number, addresses.postal_code, cities.name as city, countries.name as country,
	rank

FROM persons join judges 
	on judges.id = persons.id 
JOIN addresses 
	on persons.address_id = addresses.id 
join cities 
	on cities.id = addresses.city_id
join countries
	on countries.code = cities.country_code;



drop view clubs_total_details;

CREATE or REPLACE  VIEW clubs_total_details AS
SELECT clubs.id as club_id, clubs.name, clubs.phone, clubs.email, persons.first_name, persons.last_name, 
	street, addresses.number, addresses.postal_code, cities.name as city, countries.name as country
	
FROM clubs JOIN athletes
	ON athletes.club_id = clubs.id
LEFT JOIN persons
	ON persons.id = athletes.id
LEFT JOIN addresses
	ON addresses.id = clubs.address_id
LEFT JOIN cities 
	ON cities.id = addresses.city_id
LEFT JOIN countries
	ON countries.code = cities.country_code;





drop view events_total_details ;

CREATE or REPLACE  VIEW events_total_details AS
SELECT events.name AS event,events.id as event_id, events.date,official,locations.name AS location, locations.phone, 
	tournaments.id as tournament_id, tournaments.name AS tournament, tournaments.sex, tournaments.age_from, tournaments.age_to, 
	tournaments.level_from,tournaments.level_to, tournaments.game_type, tournaments.scoring_type,
	persons.last_name, persons.first_name,
	street, addresses.number, addresses.postal_code, cities.name as city, countries.name as country
FROM events JOIN locations
	ON events.location_id = locations.id
LEFT JOIN addresses
	ON locations.id = addresses.id
LEFT JOIN cities 
	ON cities.id = addresses.city_id
LEFT JOIN countries
	ON countries.code = cities.country_code
LEFT JOIN tournaments
	ON events.id = tournaments.event_id
LEFT JOIN tournament_participations
	ON tournaments.id = tournament_participations.tournament_id
LEFT JOIN team_tournament_participations
	ON tournaments.id = team_tournament_participations.tournament_id
LEFT JOIN persons
	ON tournament_participations.athlete_id = persons.id;




drop view tournaments_total_details ;

CREATE or REPLACE  VIEW tournaments_total_details AS
SELECT  events.name AS event, events.date,locations.name AS location,  
	tournaments.id, tournaments.name AS tournament, tournaments.sex, tournaments.age_from, tournaments.age_to, 
	tournaments.level_from,tournaments.level_to, tournaments.game_type, tournaments.scoring_type,
	tournament_participations.athlete_id, tournament_participations.ranking AS ranking,
	persons.last_name, persons.first_name,
	street, addresses.number, addresses.postal_code, cities.name as city, countries.name as country
	
FROM tournaments JOIN events
	ON tournaments.event_id = events.id
LEFT JOIN tournament_participations
	ON tournaments.id = tournament_participations.tournament_id
LEFT JOIN persons
	ON tournament_participations.athlete_id = persons.id
LEFT JOIN locations
	ON events.location_id = locations.id
LEFT JOIN addresses
	ON locations.id = addresses.id
LEFT JOIN cities 
	ON cities.id = addresses.city_id
LEFT JOIN countries
	ON countries.code = cities.country_code;


	 
																							
