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
drop table cities cascade;
drop table addresses cascade;
drop table locations cascade;
drop table clubs cascade;
drop table persons cascade;
drop table athletes cascade;
drop table judges cascade;
drop table users cascade;
drop table game_types cascade;
drop table events cascade;
drop table tournaments cascade;
drop table games cascade;
drop table team_tournament_participations cascade;
drop table tournament_participations cascade;
drop table game_participation cascade;
drop table technical_points cascade;
drop table game_score cascade;


-- table creation:


CREATE TABLE countries (
	code            char(2),
	name            varchar(80)     NOT NULL,
	primary key (code)
);


create table cities (
  id    	BIGSERIAL,
  name  	varchar(80)	not null,
  country_code	varchar(2)	references countries(code),
  primary key(id)
);



CREATE TABLE addresses (
	id              BIGSERIAL,
	street          varchar(50)     NOT NULL,
	number          varchar(12)     NOT NULL,
	city_id         integer 		references cities(id),
	postal_code     varchar(12)     NOT NULL,
	country_code    char(2)         DEFAULT 'CY' REFERENCES countries(code),
	primary key(id)
);



CREATE TABLE locations (
	id              BIGSERIAL,
	name            varchar(80),
	phone           char(15),    		-- E.164 standard
	email	        varchar(50),
	address_id 		integer 		REFERENCES addresses(id),
	primary key(id)
);



create table clubs (
	id	BIGSERIAL,
	name	varchar(50) 	not null,
	phone	char(50),
	email	varchar(50),
	logo 	bytea,
	address_id      integer		REFERENCES addresses( id ),
	country_code    char(2)         DEFAULT 'CY'	REFERENCES countries(code),
	primary key (id)
);



CREATE TABLE persons (
	id              BIGSERIAL,  				-- "BIGSERIAL" as a data type means an auto-incr. integer
	first_name      varchar(50)     	NOT NULL,
	last_name       varchar(50),
	fathers_name    varchar(50)     	NOT NULL,
	sex 		varchar(10)		NOT NULL,
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
	class           character(1)    NOT NULL,
	primary key(id)
);



create table users (
	id              	int REFERENCES persons(id) ON DELETE NO ACTION,
	username 		varchar(255) not null,
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

create table game_types (
	id 		BIGSERIAL,
	name		varchar(50),
	description	varchar(255),
	primary key(id)
);


create table events (
	id              BIGSERIAL,
	name            varchar(80)     NOT NULL,
	date            date,
	official        boolean      	DEFAULT false, 
	location_id     integer  		references locations(id),
	primary key (id)
);


CREATE TABLE tournaments ( 
	id              BIGSERIAL,
	name 		varchar(80) 	NOT NULL,
	sex		varchar(10)	NOT NULL,
	age_from	integer		NOT NULL,
	age_to		integer 	NOT NULL,
	level_from      character varying(50)   	NOT NULL, 
	level_to        character varying(50)   	NOT NULL, 
	game_type       character varying(50)		NOT NULL,  
	game		character varying(50)		NOT NULL,          
	event_id        integer references events(id),
	primary key(id)
	
);




CREATE TABLE games (
	id            	BIGSERIAL,
	phase      	varchar(10)   	NOT NULL, 
	position       	character varying(10) 	NOT NULL,    
	tournament_id 	integer references tournaments(id),
	game_type_id 	integer references game_types(id),  
	primary key(id)
);



create table team_tournament_participations (
	id 		BIGSERIAL,
	descreption	varchar(255),
	club_id		integer		references clubs(id),
	tournament_id 	integer		references tournaments(id),
	primary key (id)
);




CREATE TABLE tournament_participations (
	athlete_id	integer		references athletes(id),
	tournament_id	integer 	references tournaments(id), 
	rank_at_time 	varchar(50),
	position	integer,
	team_participation_id	integer	references team_tournament_participations(id),
	PRIMARY KEY (athlete_id, tournament_id)
);


create table game_participation (
	athlete_id 	integer references athletes (id),
	game_id 	integer references games (id),
	primary key (athlete_id, game_id)
);



create table technical_points(
	id 		BIGSERIAL,
	name 		varchar(25),
	points 		integer,
	description	varchar(255),
	primary key(id)
);



CREATE TABLE game_score(

	game_id BIGSERIAL,
	athlete_id 	integer references athletes(id),
	
	technical_points_id	integer 	references technical_points(id),
	
	head_score	integer,
	score1 		integer,
	score2 		integer,
	score3 		integer,
	score4 		integer,
	
	head_judge_id	integer 	references judges(id),
	judge1		integer		references judges(id),
	judge2	 	integer  	references judges(id),
	judge3 		integer  	references judges(id),
	judge4 		integer  	references judges(id),
	
	PRIMARY KEY (game_id, athlete_id)

	);





















