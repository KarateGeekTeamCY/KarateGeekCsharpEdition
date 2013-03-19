﻿select * from persons;

select * from athletes;

select * from addresses;

select * from users;



select * from persons JOIN athletes on persons.id = athletes.id where persons.first_name like 'd%';
-- same as:
--select * from persons NATURAL JOIN athletes where persons.first_name like 'd%';


select * from clubs;

select * from tournaments;

select * from game_score;



select game_participations.team_id, 
mean_score 
from team_tournament_participations 
join game_participations 
	on team_tournament_participations.id = game_participations.team_id 
join game_score 
	on team_tournament_participations.id = game_score.team_id 

	where tournament_id = '8' ORDER BY mean_score DESC ;




select * from tournament_participations;

select * from team_tournament_participations where tournament_id = 2;



select * from game_participations join games on games.id = game_participations.game_id where phase = 0;



select * from games;




select * from game_participations natural join athletes natural join persons;

SELECT * FROM games g LEFT JOIN game_participations gp ON g.id = gp.game_id ORDER BY phase, position ;



select * from  game_flag where game_id = '1' ;


select gp.athlete_id, mean_score 

FROM game_participations gp
join games g
	on gp.game_id = g.id 
join game_score gs
	on gs.game_id = g.id 

where tournament_id = '7' AND  phase = '0' ORDER BY mean_score DESC ;



select * from game_score;






