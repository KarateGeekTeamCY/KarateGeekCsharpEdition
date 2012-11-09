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


create view game_participants_total_det as
select event_id, tournament_id, game_id, team_id,
	athletes.id as athlete_id, last_name, first_name
from events join tournaments 
		on events.id = tournaments.event_id
	join games 
		on tournaments.id = games.tournament_id
	join game_participations
		on game_participations.game_id = games.id
	join athletes
		on game_participations.athlete_id = athletes.id
	join persons 
		on athletes.id = persons.id;
		

























