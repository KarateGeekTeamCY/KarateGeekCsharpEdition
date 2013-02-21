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

--drop view athletes_total_details;

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





CREATE or REPLACE  VIEW judges_total_details AS
SELECT judges.id, first_name, last_name, sex, date_of_birth, persons.phone, persons.email,
	rank, street, addresses.number, cities.name as city, countries.name as country

FROM persons join judges 
	on judges.id = persons.id 
JOIN addresses 
	on persons.address_id = addresses.id 
join cities 
	on cities.id = addresses.city_id
join countries
	on countries.code = cities.country_code;





CREATE or REPLACE  VIEW clubs_total_details AS
SELECT clubs.name, clubs.phone, clubs.email, persons.first_name, persons.last_name, 
	street, addresses.number, cities.name as city, countries.name as country
	
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







CREATE or REPLACE  VIEW events_total_details AS
SELECT events.name AS event, events.date,official,locations.name AS location, locations.phone, 
	street, addresses.number, cities.name as city, countries.name as country, 
	tournaments.id, tournaments.name AS tournament, tournaments.sex, tournaments.age_from, tournaments.age_to, 
	tournaments.level_from,tournaments.level_to, tournaments.game_type, tournaments.scoring_type
	
FROM events 
JOIN locations ON events.location_id = locations.id
LEFT JOIN addresses ON locations.id = addresses.id
LEFT JOIN tournaments ON events.id = tournaments.event_id
LEFT JOIN cities ON addresses.city_id = cities.id
LEFT JOIN countries ON countries.code = cities.country_code;


CREATE or REPLACE  VIEW tournaments_total_details AS
SELECT events.name AS event, events.date,locations.name AS location,  
	tournaments.id, tournaments.name AS tournament, tournaments.sex, tournaments.age_from, tournaments.age_to, 
	tournaments.level_from,tournaments.level_to, tournaments.game_type, tournaments.scoring_type,
	team_tournament_participations.ranking AS rankingTeam, 
	tournament_participations.athlete_id, tournament_participations.ranking AS ranking,
	persons.last_name, persons.first_name
	
FROM tournaments JOIN events
	ON tournaments.event_id = events.id
LEFT JOIN tournament_participations
	ON tournaments.id = tournament_participations.tournament_id
LEFT JOIN team_tournament_participations
	ON tournaments.id = team_tournament_participations.tournament_id
LEFT JOIN persons
	ON tournament_participations.athlete_id = persons.id
LEFT JOIN locations
	ON events.location_id = locations.id;


	 
--
-- mikes t's views
--


CREATE OR REPLACE VIEW game_participants_total_det AS
SELECT event_id, tournament_id, game_id, team_id,
	athletes.id AS athlete_id, last_name, first_name
FROM events 
	JOIN tournaments 
		ON events.id = tournaments.event_id
	JOIN games 
		ON tournaments.id = games.tournament_id
	JOIN game_participations
		ON game_participations.game_id = games.id
	JOIN athletes
		ON game_participations.athlete_id = athletes.id
	JOIN persons 
		ON athletes.id = persons.id;
		







CREATE OR REPLACE VIEW total_point_system AS

SELECT 
games.id as game_id, 
game_participations.athlete_id as athlete_id, 
SUM(technical_point)

FROM games 
JOIN game_participations
    ON games.id = game_participations.game_id
    
JOIN game_points
    on games.id = game_points.game_id
    
LEFT JOIN team_tournament_participations
    on team_tournament_participations.id = game_participations.team_id

GROUP BY games.id, game_participations.athlete_id;






CREATE OR REPLACE VIEW total_score_system AS

SELECT 
tournaments.id as tournament_id, 
games.id as game_id, 
games.phase as phase, 
games.position as position, 
game_participations.athlete_id as athlete_id, 
mean_score 

FROM games 

JOIN tournaments
    on tournaments.id = games.tournament_id
    
JOIN game_participations
    ON games.id = game_participations.game_id
    
JOIN game_score
    on games.id = game_score.game_id
    
LEFT JOIN team_tournament_participations
    on team_tournament_participations.id = game_participations.team_id
    
    ORDER BY mean_score DESC;



--drop view athlete_view;
create or replace view athlete_view as 
select 
persons.id as id, 
first_name,
last_name,
fathers_name,
sex,
extract(year from age(date_of_birth)) as age,
phone, 
secondary_phone,
email,
address_id, 
addresses.street as street_name, 
cities.name as city, 
countries.name as country, 
addresses.number as address_num,  
rank from persons 
join athletes on persons.id = athletes.id 
join addresses on persons.address_id = addresses.id 
join countries on addresses.country_code = countries.code 
join cities on cities.id = addresses.city_id;    

    

--mixalis an graph view

create or replace view graph_view as 
select graph, name from lottery_graph join tournaments on lottery_graph.id = tournaments.id;










