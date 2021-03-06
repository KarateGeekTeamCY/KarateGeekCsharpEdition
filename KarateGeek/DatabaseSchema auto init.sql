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
--                    key shall reference the PRIMARY KEY of the referenced table."
-- **  Using the "CHAR" type only when the field length is absolutely fixed, eg. country codes
--       (since there is no performance advantage, it's a matter of style)
--
-- * From the PostgreSQL documentation: "While 'CHARacter(n)' has performance advantages in some
--    other database systems, there is no such advantage in PostgreSQL. [...] In most situations
--    'text' or 'CHARacter varying' should be used instead."
--
-- * Using 96 columns for this script, unless impossible
--
-- * Standardising VARCHAR lengths (maybe useful for GUI design) to: 12, 50, 80, 255
--
-- * I still don't know whether the "schema_name." table prefix is necessary (seems it isn't)
--
--

------------------------------------------------------------------------------------------------

-- begin transaction:
BEGIN;


drop schema IF EXISTS public CASCADE;      -- WARNING: This also deletes all tables!

-- table drops (not needed)
-- drop table countries cascade;
-- drop table cities cascade;
-- drop table addresses cascade;
-- drop table locations cascade;
-- drop table clubs cascade;
-- drop table persons cascade;
-- drop table athletes cascade;
-- drop table judges cascade;
-- drop table users cascade;
-- drop table events cascade;
-- drop table tournaments cascade;
-- drop table games cascade;
-- drop table team_tournament_participations cascade;
-- drop table tournament_participations cascade;
-- drop table game_participation cascade;
-- drop table game_score cascade;

create schema public;


--
-- Table creation:
--


CREATE TABLE countries (
    code            CHAR(2),
    name            VARCHAR(80)     NOT NULL,
    PRIMARY KEY (code)
);


create table cities (
  id                BIGSERIAL,
  name              VARCHAR(80)     NOT NULL UNIQUE,
  country_code      VARCHAR(2)      REFERENCES countries(code),
  PRIMARY KEY(id)
);


CREATE TABLE addresses (
    id              BIGSERIAL,
    street          VARCHAR(50)     NOT NULL,
    number          VARCHAR(12)     NOT NULL,
    city_id         INTEGER         REFERENCES cities(id),
    postal_code     VARCHAR(12)     NOT NULL,
    country_code    CHAR(2)         DEFAULT 'CY' REFERENCES countries(code),
    PRIMARY KEY(id)
);


CREATE TABLE locations (
    id              INTEGER         REFERENCES addresses(id),
    name            VARCHAR(80),
    phone           CHAR(15),       -- E.164 standard
    email           VARCHAR(50),
    PRIMARY KEY(id)
);


create table clubs (
    id              BIGSERIAL,
    name            VARCHAR(50)     NOT NULL UNIQUE,
    phone           CHAR(50),
    email           VARCHAR(50),
    logo            BYTEA,
    address_id      INTEGER         REFERENCES addresses( id ),
    country_code    CHAR(2)         DEFAULT 'CY'  REFERENCES countries(code),
    PRIMARY KEY (id)
);


CREATE TABLE persons (
    id              BIGSERIAL,      -- "BIGSERIAL" as a data type means an auto-incr. INTEGER
    first_name      VARCHAR(50)     NOT NULL,
    last_name       VARCHAR(50)     NOT NULL,
    fathers_name    VARCHAR(50),
    sex             VARCHAR(10)     NOT NULL,
    date_of_birth   DATE            NOT NULL,
    phone           CHAR(15)        NOT NULL,   -- E.164 standard
    secondary_phone CHAR(15),
    email           VARCHAR(50),
    address_id      INTEGER         REFERENCES addresses( id ),
    PRIMARY KEY(id)
    --,
    --CHECK (email ~* '^[_a-zA-Z0-9-+]+(.[_a-zA-Z0-9-+]+)*@[a-zA-Z0-9-]+(.[a-zA-Z0-9-]+)*.([a-zA-Z]{2,6})$')
);


CREATE TABLE athletes (
    id              INTEGER         REFERENCES persons(id)  ON DELETE NO ACTION, -- not CASCADE!
    rank            VARCHAR(50)     NOT NULL,                                    -- CHAR or int?
    club_id         INTEGER         REFERENCES clubs(id),   -- this was "NOT NULL" initially
    PRIMARY KEY(id)
);


CREATE TABLE judges (
    id              INTEGER         REFERENCES persons(id)  ON DELETE NO ACTION,
    rank            VARCHAR(50)     NOT NULL,                                    -- CHAR or int?
    class           CHARacter(1)    NOT NULL,
    PRIMARY KEY(id)
);


create table users (
    id              INTEGER         REFERENCES persons(id)  ON DELETE NO ACTION,
    username        VARCHAR(255)    NOT NULL UNIQUE,
    password        VARCHAR(255)    NOT NULL,
    person_management   BOOLEAN     NOT NULL DEFAULT FALSE,
    event_management    BOOLEAN     NOT NULL DEFAULT FALSE,
    lottery         BOOLEAN         NOT NULL DEFAULT FALSE,
    game_support    BOOLEAN         NOT NULL DEFAULT FALSE,
    reports         BOOLEAN         NOT NULL DEFAULT FALSE,
    settings        BOOLEAN         NOT NULL DEFAULT FALSE,
    PRIMARY KEY (id)
);


--create table game_types (
--  id              BIGSERIAL,
--  name            VARCHAR(50),
--  description     VARCHAR(255),
--  PRIMARY KEY(id)
--);


create table events (
    id              BIGSERIAL,
    name            VARCHAR(80)     NOT NULL,
    date            DATE,
    official        BOOLEAN         NOT NULL DEFAULT true, -- default true according to specs
    location_id     INTEGER         REFERENCES locations(id),
    PRIMARY KEY (id)
);


CREATE TABLE tournaments (
    id              BIGSERIAL,
    name            VARCHAR(80)     NOT NULL,
    sex             VARCHAR(10)     NOT NULL,
    age_from        INTEGER         NOT NULL,
    age_to          INTEGER         NOT NULL,
    level_from      VARCHAR(50)     NOT NULL,
    level_to        VARCHAR(50)     NOT NULL,
    game_type       VARCHAR(50)     NOT NULL,   -- eg. "KATA", "FUGU-GO", ...
    scoring_type    VARCHAR(12)     NOT NULL,   -- "flag|point" -> for "FUGU-GO"
                                                -- "score"      -> arithmitika
                                                -- "flag"       -> simees
                                                -- "point"      -> ippon ktl.
    event_id        INTEGER         REFERENCES events(id),
    PRIMARY KEY(id)
);


CREATE TABLE games (
    id              BIGSERIAL,
    phase           INTEGER         NOT NULL,
    position        INTEGER         NOT NULL,
    tournament_id   INTEGER         REFERENCES tournaments(id),
    PRIMARY KEY(id)
);

create table team_tournament_participations (
    id              BIGSERIAL,
    position        INTEGER,
    --club_id       INTEGER         REFERENCES clubs(id),
    tournament_id   INTEGER         REFERENCES tournaments(id),
    PRIMARY KEY (id)
);


CREATE TABLE tournament_participations (
    athlete_id      INTEGER         REFERENCES athletes(id),
    tournament_id   INTEGER         REFERENCES tournaments(id),
    rank_at_time    VARCHAR(50)     NOT NULL,
    position        INTEGER,
    team_participation_id INTEGER   REFERENCES team_tournament_participations(id),
    PRIMARY KEY (athlete_id, tournament_id)
);


create table game_participation (       -- gia atomika
                                        -- tha mpainoun dio tetia entries gia versus
                                        -- gia atomika parousiasi 1
                                        -- gia omadiko vs 6 h 4 anepisima
    athlete_id      INTEGER         REFERENCES athletes (id),
    game_id         INTEGER         REFERENCES games (id),
    PRIMARY KEY (athlete_id, game_id)
);


CREATE TABLE game_score(

    game_id         BIGSERIAL,
    athlete_id      INTEGER         REFERENCES athletes(id),
    technical_point INTEGER,
    technical_point_desc VARCHAR(50),

    is_points       BOOLEAN,    -- afto tha ksexorizi to kata poso i ponti einai points diladi
                  -- apo 1-10 allios tha simeni oti einai flag system kai ta VALUES tha einai 0-1
                  -- opou 0 -> red
                  --  1 -> white

    head_score      INTEGER,
    score1          INTEGER,
    score2          INTEGER,
    score3          INTEGER,
    score4          INTEGER,

    head_judge_id   INTEGER         REFERENCES judges(id),
    judge1          INTEGER         REFERENCES judges(id),
    judge2          INTEGER         REFERENCES judges(id),
    judge3          INTEGER         REFERENCES judges(id),
    judge4          INTEGER         REFERENCES judges(id),

    PRIMARY KEY (game_id, athlete_id)
);


-- rollback transaction (useful for checking syntax):
--ROLLBACK;

-- commit transaction (will destroy all existing tables and data):
COMMIT;


--
-- Initialisation with data
--


-- begin transaction:
BEGIN;


INSERT INTO countries (code , name) VALUES('CY', 'Cyprus');
INSERT INTO countries (code , name) VALUES('GR', 'Greece');


INSERT INTO cities (name, country_code) VALUES ('Limassol', 'CY');
INSERT INTO cities (name, country_code) VALUES ('Nicosia', 'CY');
INSERT INTO cities (name, country_code) VALUES ('Paphos', 'CY');
INSERT INTO cities (name, country_code) VALUES ('Larnaca', 'CY');
INSERT INTO cities (name, country_code) VALUES ('Amochostos', 'CY');
INSERT INTO cities (name, country_code) VALUES ('Thessaloniki', 'GR');


INSERT INTO addresses (id , street, "number", city_id , postal_code , country_code ) VALUES ('0' , 'tepak' , '1' , '1' , '3025' , 'CY');


INSERT INTO clubs (name, address_id, country_code) VALUES ('power fight club', 0, 'CY');


INSERT INTO persons (id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('0' , 'administrator' , 'xampis' , 'administrator', '02-10-1990' , 'male', '99123144' , null , 'email@gmail.com' , '0');

INSERT INTO persons (id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('1' , 'athl1' , 'athl1father' , 'athl1last', '02-10-1991' , 'male', '99123145' , null , 'athl1@gmail.com' , '0');

INSERT INTO persons (id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('2' , 'athl2' , 'athl2father' , 'athl2last', '02-10-1992' , 'male', '99123146' , null , 'athl2@gmail.com' , '0');

-- not an athlete, to check if auto-increment (BIGSERIAL) works... EDIT: IT DOESN'T FROM THE SCRIPT!
--INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
--VALUES ('NotAnAthll' , 'NotAnAthllfather' , 'NotAnAthlllast', '02-10-1992' , 'male', '99123146' , null , 'NotAnAthll@gmail.com' , '0');


-- adding user: "admin" pass: "admin" (will be removed in the final releases!)
INSERT INTO users (id , username , password, person_management, event_management , lottery , game_support , reports , settings)
VALUES ( '0', 'admin' , '3039283064aa2a9ca939b1fe23954698' , '1' , '1' , '1' , '1' , '1' , '1');


INSERT INTO athletes (id, rank, club_id ) VALUES ('0', 'black', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('1', 'black', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('2', 'black', NULL );


INSERT INTO locations (id, name, phone, email)
VALUES (0, 'location 1', '77778888', 'loc@locloc.com');


INSERT INTO events (name, date, location_id)
VALUES ('Big Event', '29/10/2012', 0);


INSERT INTO tournaments (name, sex, age_from, age_to, level_from, level_to, game_type, scoring_type, event_id)
VALUES ('Iron Fist Tournament', 'male', 1, 99, 'white', 'black 8 dan', 'deathmatch', 'flag', 1);


-- A NULL "position" means that the match hasn't taken place yet
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , position , team_participation_id)
VALUES (0, 1, 'black', NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , position , team_participation_id)
VALUES (1, 1, 'black', NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , position , team_participation_id)
VALUES (2, 1, 'black', NULL, NULL );


-- rollback transaction (useful for checking syntax):
--ROLLBACK;

-- commit transaction:
COMMIT;
