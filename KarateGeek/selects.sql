select * from persons;

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




select * from tournament_participations where tournament_id = 7;

select * from team_tournament_participations where tournament_id = 2;



select * from game_participations join games on games.id = game_participations.game_id ;



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



select * from game_score ;


select gp.team_id, SUM (mean_score) FROM game_participations gp join games g on gp.game_id = g.id join game_score gs on gs.game_id = g.id where tournament_id = '3' AND  phase = '1' AND  SUM(mean_score) = '150' GROUP BY gp.team_id ORDER BY mean_score DESC  ;

































SELECT gp.team_id, SUM(mean_score) 
FROM game_participations gp 
JOIN games g ON gp.game_id = g.id 
JOIN game_score gs ON gs.game_id = g.id 

WHERE tournament_id = '3' AND  phase = '1' AND '150' = 
	(SELECT SUM(mean_score) 
		FROM game_score WHERE game_id IN 
			(SELECT ig.id FROM game_participations igp 
				JOIN games ig ON igp.game_id = ig.id 
				JOIN game_score igs ON igs.game_id = ig.id 
				WHERE ig.tournament_id = g.tournament_id AND ig.phase = g.phase AND igp.team_id = gp.team_id )) 
GROUP BY gp.team_id ORDER BY SUM(mean_score) DESC  ;





















SELECT SUM(mean_score) 
	FROM game_score WHERE game_id IN 
		(SELECT game_id FROM games g 
			JOIN tournaments t ON t.id = g.tournament_id 
			WHERE t.id = '3' AND g.phase = '1' );





















