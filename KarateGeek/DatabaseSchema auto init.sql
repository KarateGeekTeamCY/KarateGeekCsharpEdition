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
drop table events cascade;
drop table tournaments cascade;
drop table games cascade;
drop table team_tournament_participations cascade;
drop table tournament_participations cascade;
drop table game_participation cascade;
drop table game_score cascade;


-- table creation:


CREATE TABLE countries (
	code            char(2),
	name            varchar(80)     NOT NULL,
	primary key (code)
);


create table cities (
  id    	BIGSERIAL,
  name  	varchar(80)	not null UNIQUE,
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
	id		BIGSERIAL,
	name		varchar(50) 	not null,
	phone		char(50),
	email		varchar(50),
	logo 		bytea,
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
	address_id      integer      		REFERENCES addresses( id ),
	primary key(id)
);




CREATE TABLE athletes (
	id              int REFERENCES persons(id) 	ON DELETE NO ACTION, 	-- not CASCADE!
	rank            varchar(50)     		NOT NULL,  -- char or int?
	club_id         integer         		NOT NULL REFERENCES clubs(id),
	primary key(id)
);



CREATE TABLE judges (
	id              int REFERENCES persons(id) 	ON DELETE NO ACTION,
	rank            varchar(50)     		NOT NULL,  				-- char or int?
	class           character(1)    		NOT NULL,
	primary key(id)
);



create table users (
	id              	int REFERENCES persons(id) 	ON DELETE NO ACTION,
	username 		varchar(255) 			NOT NULL,
	password		varchar(255) 			NOT NULL,
	person_management	boolean 			NOT NULL DEFAULT FALSE,
	event_management	boolean 			NOT NULL DEFAULT FALSE,
	lottery			boolean 			NOT NULL DEFAULT FALSE,
	game_support		boolean 			NOT NULL DEFAULT FALSE,
	reports			boolean 			NOT NULL DEFAULT FALSE,
	settings		boolean 			NOT NULL DEFAULT FALSE,
	primary key (id)
);



--create table game_types (
--	id 		BIGSERIAL,
--	name		varchar(50),
--	description	varchar(255),
--	primary key(id)
--);



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
	name 		varchar(80) 			NOT NULL,
	sex		varchar(10)			NOT NULL,
	age_from	integer				NOT NULL,
	age_to		integer 			NOT NULL,
	level_from      character varying(50)   	NOT NULL, 
	level_to        character varying(50)   	NOT NULL, 
	game_type       character varying(50)		NOT NULL,  
	game		character varying(50)		NOT NULL,          
	level           character varying(50)   	NOT NULL,
	category        character varying(50),
	event_id        integer 			references events(id),
	primary key(id)

);




CREATE TABLE games (
	id            	BIGSERIAL,
	phase      	varchar(10)   		NOT NULL,
	position       	character varying(10) 	NOT NULL,
	tournament_id 	integer 		references tournaments(id),
	game_type 	varchar(50)		not null,
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
	athlete_id		integer		references athletes(id),
	tournament_id		integer 	references tournaments(id),
	rank_at_time 		varchar(50),
	position		integer,
	team_participation_id	integer		references team_tournament_participations(id),
	
	PRIMARY KEY (athlete_id, tournament_id)
);


create table game_participation (
	athlete_id 	integer references athletes (id),
	game_id 	integer references games (id),
	primary key (athlete_id, game_id)
);







CREATE TABLE game_score(

	game_id BIGSERIAL,
	athlete_id 	integer references athletes(id),

	technical_point	integer,
	technical_point_desc	varchar(50),

	is_points	boolean,	-- afto tha ksexorizi to kata poso i ponti einai points diladi apo 1-10
					-- alios tha simeni oti einai flag system kai ta values tha einai 0-1
					-- opou 0 -> red
					-- 	1 -> white
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



--
--	initialiazetion
--


insert into countries (code , name) values('CY', 'Cyprus');
insert into countries (code , name) values('GR', 'Greece');

insert into cities (name, country_code) values ('Limassol', 'CY');
insert into cities (name, country_code) values ('Nicosia', 'CY');
insert into cities (name, country_code) values ('Paphos', 'CY');
insert into cities (name, country_code) values ('Larnaca', 'CY');
insert into cities (name, country_code) values ('Amochostos', 'CY');
insert into cities (name, country_code) values ('Thessaloniki', 'GR');



insert into addresses (id , street, "number", city_id , postal_code , country_code ) values ('0' , 'tepak' , '1' , '1' , '3025' , 'CY');


insert into clubs (name, address_id, country_code) values ('power fight club', 0, 'CY');


insert into persons(id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
values ('0' , 'administrator' , 'xampis' , 'administrator', '02-10-1990' , 'male', '99123144' , null , 'email@gmail.com' , '0');

insert into users( id , username , password, person_management, event_management , lottery , game_support , reports , settings) values ( '0', 'admin' , '3039283064aa2a9ca939b1fe23954698' , '1' , '1' , '1' , '1' , '1' , '1');


insert into athletes (id, rank, club_id ) values ('0', 'black', '1'  );














