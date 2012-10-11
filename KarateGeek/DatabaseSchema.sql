--
--                        karate geek
--
--
--     Copyright (C) 2012 the KarateGeek Team
--
--     This program is free software: you can redistribute it and/or modify
--     it under the terms of the GNU General Public License as published by
--     the Free Software Foundation, either version 3 of the License, or
--     (at your option) any later version.
--
--     This program is distributed in the hope that it will be useful,
--     but WITHOUT ANY WARRANTY; without even the implied warranty of
--     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
--     GNU General Public License for more details.
--
--     You should have received a copy of the GNU General Public License
--     along with this program.  If not, see <http://www.gnu.org/licenses/>.
--
--

------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------

-- NOTES:
--
-- * Using the following CONVENTIONS:
--
-- **  Using "id" for PK, "person_id" for FK!
-- **  SQL KEYWORDS (but NOT data types) :   UPPERCASE
-- **  other identifiers :                   lowercase
-- **  PRIMARY KEY (unless composite) and REFERENCES as column constraints
-- **  CHECK as column or table constraint
-- **  SQL standard: "Omitting the column list in the REFERENCES clause implies that the foreign
--                    key shall reference the primary key of the referenced table."
-- **  Using the "char" type only when the field length is absolutely fixed, eg. country codes
--       (since there is no performance advantage, it's a matter of style)
--
-- * From the PostgreSQL documentation: "While 'character(n)' has performance advantages in some
--    other database systems, there is no such advantage in PostgreSQL. [...] In most situations
--    'text' or 'character varying' should be used instead."
--
-- * Using 96 columns for this script, unless impossible
--
-- * Standardising varchar lengths (maybe useful for GUI design) to: 12, 50, 80, 255
--
-- * I still don't know whether the "schema_name." table prefix is necessary (seems it isn't)
--
--

------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------

-- begin transaction:

-- table drops



drop table countries cascade;
drop table addresses cascade;
drop table locations cascade;
drop table clubs cascade;
drop table persons cascade;
drop table athletes cascade;
drop table judges cascade;
drop table users cascade;



-- table creation:


CREATE TABLE countries (
	code            char(2)         PRIMARY KEY,
	name            varchar(80)     NOT NULL
);



CREATE TABLE addresses (
	id              SERIAL,
	street          varchar(50)     NOT NULL,
	number          varchar(12)     NOT NULL,
	city            varchar(50)     NOT NULL,
	postal_code     varchar(12)     NOT NULL,
	country_code    char(2)         DEFAULT 'CY' REFERENCES countries(code),
	primary key(id)
);



CREATE TABLE locations (
	id              SERIAL       REFERENCES addresses(id),
	name            varchar(80),
	phone           char(15),    		-- E.164 standard
	description     varchar(255),
	primary key(id)
);



create table clubs (
	id	SERIAL,
	name	varchar(50) 	not null,
	phone	char(50),
	email	varchar(50),
	logo 	bytea,
	address_id      integer		REFERENCES addresses( id ),
	country_code    char(2)         DEFAULT 'CY'	REFERENCES countries(code),
	primary key (id)
);



CREATE TABLE persons (
    id              SERIAL,  				-- "SERIAL" as a data type means an auto-incr. integer
    first_name      varchar(50)     	NOT NULL,
    middle_name     varchar(50),
    last_name       varchar(50)     	NOT NULL,
    date_of_birth   date,
    phone           char(15)        	NOT NULL,    	-- E.164 standard
    secondary_phone char(15),
    email           varchar(50),
    address_id      integer      	REFERENCES addresses( id ),
    primary key(id)
);




CREATE TABLE athletes (
	id              int REFERENCES persons(id) ON DELETE NO ACTION, 	-- not CASCADE!
	rank            varchar(50)     NOT NULL,  -- char or int?
	club_id         integer         NOT NULL REFERENCES clubs(id),
	primary key(id)
);



CREATE TABLE judges (
	id              int REFERENCES persons(id) ON DELETE NO ACTION,
	rank            varchar(50)     NOT NULL,  				-- char or int?
	class           character(1)    NOT NULL
);



create table users (
	id              	int REFERENCES persons(id) ON DELETE NO ACTION,
	password		varchar(255) not null,
	person_management	boolean not null default  false,
	event_management	boolean not null default  false,
	lottery			boolean not null default  false,
	game_support		boolean not null default  false,
	reports			boolean not null default  false,	
	settings		boolean not null default  false,
	primary key (id)
);







----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
--dikes mou allages



CREATE TABLE events (
	id              SERIAL,
	name            varchar(80)     NOT NULL,
	day             date       	NOT NULL,
	official        boolean         NOT NULL DEFAULT false, 
	game_type_id 	int references game_types(id),
	location        integer  	references locations(id),
	primary key (id)
);


CREATE TABLE tournaments ( 
	id              SERIAL,
	level           character varying(50)   	NOT NULL, 
	category        character varying(50),            
	event_id        integer references events(id),
	primary key(id)
	
);


CREATE TABLE game_types ( 
	id            	SERIAL,
	name      	character varying(50)  	NOT NULL, 
	description    	character varying(255) 	NOT NULL,              
	PRIMARY KEY (id)
);


CREATE TABLE games (
	id            	SERIAL,
	phase      	varchar(10)   	NOT NULL, 
	position       	character varying(10) 	NOT NULL,    
	tournament_id 	integer references tournament(id),
	game_type_id 	integer references game_types(id),  
	primary key(id)
);



CREATE TABLE tournament_participations (
	athlete_id	integer		references athletes(id),
	tournament_id	integer 	references tournaments(id), 
	rank_at_time 	varchar(50),
	position	integer,
	team_participation_id	integer	references team_tournament_participations(id),
	PRIMARY KEY (athlete_id, tournament_id)
);



create table team_tournament_participations (
	id 		serial,
	descreption	varchar(255),
	club_id		integer		references clubs(id),
	tournament_id 	integer		references tournaments(id),
	primary key (id)
);



CREATE TABLE game_score(--diorthwse ta ama mporeis giati den thimame pws ginetai akrivws auto me ta kleidai
	game_id integer,
	athlete_person_id integer,
	technical_points_id integer,
	head_score integer,
	score1 integer 	NOT NULL,
	score2 integer NOT NULL,
	score3 integer NOT NULL,
	score4 integer NOT NULL,
	head_judge_id integer,
	judge1 integer,
	judge2 integer,
	judge3 integer,
	judge4 integer,
	PRIMARY KEY (game_id,athletes_person_id),
	FOREIGN KEY (game_id) REFERENCES game(id),
	FOREIGN KEY (athlete_person_id) REFERENCES athletes(person_id),
	FOREIGN KEY (head_judge) REFERENCES judges(person_id),
	FOREIGN KEY (technical_points_id) REFERENCES athlets(person_id),--auto den kserw pou paei
	FOREIGN KEY (judge1) REFERENCES judges(person_id),
	FOREIGN KEY (judge2) REFERENCES judges(person_id),
	FOREIGN KEY (judge3) REFERENCES judges(person_id),
	FOREIGN KEY (judge4) REFERENCES judges(person_id)
);


