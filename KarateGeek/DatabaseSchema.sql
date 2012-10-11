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
BEGIN;


drop schema IF EXISTS sch1 CASCADE;      -- WARNING: This also deletes all tables!


-- table creation:


CREATE TABLE persons (
    id              SERIAL,  				-- "SERIAL" as a data type means an auto-incr. integer
    first_name      varchar(50)     NOT NULL,
    middle_name     varchar(50)     NOT NULL,
    last_name       varchar(50)     NOT NULL,
    date_of_birth   date,
    phone           char(15)        NOT NULL,    	-- E.164 standard
    secondary_phone char(15),
    email           varchar(50),
    address_id      integer         NOT NULL REFERENCES addresses( id ),
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
	-- event_id of the ERD is useless and was omitted
);


CREATE TABLE countries (
	code            char(2)         PRIMARY KEY,
	name            varchar(80)     NOT NULL
);

----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
--dikes mou allages
CREATE TABLE event (
	id              integer			NOT NULL,
	name            character varying(80)     NOT NULL,
	official        boolean         NOT NULL DEFAULT true, 
	day             timestamp       NOT NULL, 
	description     character varying(255),
	location        integer         ,
	CONSTRAINT event_pkey PRIMARY KEY (id),
	CONSTRAINT event_location_fkey FOREIGN KEY (location)
      REFERENCES locations (id)
);


CREATE TABLE tournament ( 
	id              integer,
	level           character varying(50)   	NOT NULL, 
	category        character varying(50),            
    event_id        integer,
	CONSTRAINT tournament_pkey PRIMARY KEY (id),
	CONSTRAINT tournament_event_id_fkey FOREIGN KEY (event_id)
      REFERENCES events (id)
);

CREATE TABLE game_type ( 
	id                integer,
	name      		  character varying(50)   	NOT NULL, 
	description       character varying(50) 	NOT NULL,              
	CONSTRAINT game_type_pkey PRIMARY KEY (id)
);

CREATE TABLE game(
	id                integer,
	phase      		  character varying(10)   	NOT NULL, 
	position          character varying(10) 	NOT NULL,              
	CONSTRAINT game_pkey PRIMARY KEY (id)
);

CREATE TABLE game_participation(
	athlete_person_id integer,
	game_id integer,
	CONSTRAINT game_participation_athlete_person_id_fkey FOREIGN KEY (athlete_person_id)
      REFERENCES  athletes (id),
	CONSTRAINT game_participation_game_id_fkey FOREIGN KEY (game_id)
      REFERENCES  game (id)  
);


-- commit transaction (will destroy all existing tables and data):
--COMMIT;
