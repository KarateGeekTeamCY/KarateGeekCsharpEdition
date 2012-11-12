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


CREATE or REPLACE  VIEW athletes_total_details AS
select athletes.id, first_name, last_name, fathers_name, sex, date_of_birth, persons.phone, secondary_phone, persons.email,
		rank, clubs.name as club_name, street, number, cities.name as city, countries.name as country, athlete_first_places_ind.count as first_places

 from  persons join athletes 
	on athletes.id = persons.id 
		join addresses 
			on persons.address_id = addresses.id 
				join clubs 
					on athletes.club_id = clubs.id
						join cities 
							on cities.id = addresses.city_id
								join countries
									on countries.code = cities.country_code
										join athlete_first_places_ind
											on athlete_first_places_ind.athlete_id = athletes.id;













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



drop view athlete_view;
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

    











