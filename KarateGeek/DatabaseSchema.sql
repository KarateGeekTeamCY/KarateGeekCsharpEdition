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


create schema sch1;


-- table creation:


CREATE TABLE sch1.persons (
	id              SERIAL PRIMARY KEY,  -- "SERIAL" as a data type means an auto-incr. integer
    first_name      varchar(50)     NOT NULL,
    middle_name     varchar(50)     NOT NULL,
    last_name       varchar(50)     NOT NULL,
    date_of_birth   date,
    phone           char(15)        NOT NULL,    -- E.164 standard
    secondary_phone char(15),
    email           varchar(50),
    address_id      integer         NOT NULL REFERENCES sch1.addresses,

    CHECK (email ~* '^[_a-zA-Z0-9-+]+(.[_a-zA-Z0-9-+]+)*@[a-zA-Z0-9-]+(.[a-zA-Z0-9-]+)*.([a-zA-Z]{2,6})$')
                                         -- regex probably not good enough!
);


CREATE TABLE sch1.athletes (
	id              int REFERENCES sch1.persons ON DELETE NO ACTION, -- not CASCADE!
	rank            varchar(50)     NOT NULL,  -- char or int?
	club_id         integer         NOT NULL REFERENCES sch1.clubs
);


CREATE TABLE sch1.judges (
	id              int REFERENCES sch1.persons ON DELETE NO ACTION,
	rank            varchar(50)     NOT NULL,  -- char or int?
    class           character(1)    NOT NULL
);


CREATE TABLE sch1.addresses (
	id              SERIAL PRIMARY KEY,
	street          varchar(50)     NOT NULL,
	number          varchar(12)     NOT NULL,
	city            varchar(50)     NOT NULL,
	postal_code     varchar(12)     NOT NULL,
	country_code    char(2)         DEFAULT 'CY' REFERENCES sch1.countries
);


CREATE TABLE sch1.locations (
	id              integer          REFERENCES sch1.addresses,
	name            varchar(80),
	phone           char(15),    -- E.164 standard
	description     varchar(255)
	-- event_id of the ERD is useless and was omitted
);


CREATE TABLE sch1.countries (
	code            char(2)         PRIMARY KEY,
	name            varchar(80)     NOT NULL
);


CREATE TABLE sch1.events (
	id              SERIAL          PRIMARY KEY,
	name            varchar(80)     NOT NULL,
	official        boolean         NOT NULL DEFAULT true, -- default true according to specs
	day             timestamp       NOT NULL -- has both date and time info, μsec precision
	description     varchar(255)
	location        integer         REFERENCES sch1.locations
);


CREATE TABLE sch1.tournaments ( -- we need to discuss this one!
	id              SERIAL PRIMARY KEY,
	rank_start      varchar(50)     NOT NULL, -- from this belt color...
	rank_end        varchar(50),              -- ...to this one, if different (nullable value)
    age_start       smallint        NOT NULL, -- from this age...
	age_end         smallint,                 -- ...to this one, if different (nullable value)
	event_id        integer         REFERENCES sch1.events
	-- category was replaced by age (?)
);


CREATE TABLE sch1.game_types ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.games ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.tour_participations ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.game_participations ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.game_scores ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.technical_points ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.participation_teams ( --
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.clubs ( --             -- also aliased as "local_associations" using a VIEW
	id              int PRIMARY KEY,
);


CREATE TABLE sch1.app_users (
	username        varchar(50)     PRIMARY KEY,
	password        char(32)        NOT NULL,
	person_mngment  boolean         NOT NULL DEFAULT false,
	event_mngment   boolean         NOT NULL DEFAULT false,
	lottery         boolean         NOT NULL DEFAULT false,
	game_support    boolean         NOT NULL DEFAULT false,
	reports         boolean         NOT NULL DEFAULT false,
	settings        boolean         NOT NULL DEFAULT false
);


CREATE TABLE sch1.app_setup (
	id              boolean         PRIMARY KEY, -- only one entry, if any?
	salt            char(50)
);


CREATE VIEW sch1.local_associations
SELECT * FROM sch1.clubs;

------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------


-- rollback transaction (useful for checking syntax):
ROLLBACK;

-- commit transaction (will destroy all existing tables and data):
--COMMIT;
