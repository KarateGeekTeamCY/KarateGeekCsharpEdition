﻿--
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
-- **  CHECK as column or TABLE constraint
-- **  SQL standard: "Omitting the column list in the REFERENCES clause implies that the foreign
--                    key shall reference the PRIMARY KEY of the referenced TABLE."
-- **  Using the "CHAR" type only when the field length is absolutely fixed, eg. country codes
--       (since there is no performance advantage, it's a matter of style)
--
-- * From the PostgreSQL documentation: "While 'character(n)' has performance advantages in some
--    other database systems, there is no such advantage in PostgreSQL. [...] In most situations
--    'text' or 'character varying' should be used instead."
--
-- * Using 96 columns for this script, unless impossible
--
-- * Standardising VARCHAR lengths (maybe useful for GUI design) to: 12, 50, 80, 255
--
-- * I still don't know whether the "schema_name." TABLE prefix is necessary (seems it isn't)
--
--

------------------------------------------------------------------------------------------------

-- begin transaction:
BEGIN;


DROP SCHEMA IF EXISTS public CASCADE;      -- WARNING: This also deletes all TABLEs!

-- TABLE drops (not needed)
-- DROP TABLE countries cascade;
-- DROP TABLE cities cascade;
-- DROP TABLE addresses cascade;
-- DROP TABLE locations cascade;
-- DROP TABLE clubs cascade;
-- DROP TABLE persons cascade;
-- DROP TABLE athletes cascade;
-- DROP TABLE judges cascade;
-- DROP TABLE users cascade;
-- DROP TABLE events cascade;
-- DROP TABLE tournaments cascade;
-- DROP TABLE games cascade;
-- DROP TABLE team_tournament_participations cascade;
-- DROP TABLE tournament_participations cascade;
-- DROP TABLE game_participation cascade;
-- DROP TABLE game_score cascade;

CREATE SCHEMA public;


--
-- TABLE creation:
--


CREATE TABLE countries (
    code            CHAR(2),
    name            VARCHAR(80)     NOT NULL,
    PRIMARY KEY (code)
);


CREATE TABLE cities (
  id                SERIAL,             -- "SERIAL" as a data type means an auto-incr. INTEGER
  name              VARCHAR(80)     NOT NULL UNIQUE,
  country_code      VARCHAR(2)      REFERENCES countries(code),
  PRIMARY KEY(id)
);


CREATE TABLE addresses (
    id              SERIAL,
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


CREATE TABLE clubs (
    id              SERIAL,
    name            VARCHAR(50)     NOT NULL UNIQUE,
    phone           CHAR(50),
    email           VARCHAR(50),
    logo            BYTEA,          -- "binary string" data type
    address_id      INTEGER         REFERENCES addresses( id ),
    country_code    CHAR(2)         DEFAULT 'CY'  REFERENCES countries(code),
    PRIMARY KEY (id)
);


CREATE TABLE persons (
    id              SERIAL,
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


CREATE TABLE users (
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


--CREATE TABLE game_types (
--  id              SERIAL,
--  name            VARCHAR(50),
--  description     VARCHAR(255),
--  PRIMARY KEY(id)
--);


CREATE TABLE events (
    id              SERIAL,
    name            VARCHAR(80)     NOT NULL,
    date            DATE,
    official        BOOLEAN         NOT NULL DEFAULT true, -- default true according to specs
    location_id     INTEGER         REFERENCES locations(id),
    PRIMARY KEY (id)
);


CREATE TABLE tournaments (
    id              SERIAL,
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
    event_id        INTEGER         REFERENCES events(id) on delete cascade,
    PRIMARY KEY(id)
);



CREATE TABLE games (
    id              SERIAL,
    phase           INTEGER         NOT NULL,
    position        INTEGER         NOT NULL,
    tournament_id   INTEGER         REFERENCES tournaments(id),
    is_ready        BOOLEAN         NOT NULL DEFAULT false,
    is_finished     BOOLEAN         NOT NULL DEFAULT false,

    PRIMARY KEY(id)
);

CREATE TABLE team_tournament_participations (
    id              SERIAL,
    ranking         INTEGER,
    team	    INTEGER,
    --club_id       INTEGER         REFERENCES clubs(id),
    tournament_id   INTEGER         REFERENCES tournaments(id),
    PRIMARY KEY (id)
);


CREATE TABLE tournament_participations (
    athlete_id      INTEGER         REFERENCES athletes(id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id),
    tournament_id   INTEGER         REFERENCES tournaments(id),
    rank_at_time    VARCHAR(50)     NOT NULL,
    ranking         INTEGER,

    PRIMARY KEY (athlete_id, tournament_id)

    );


CREATE TABLE game_participations (      -- gia atomika
                                        -- tha mpainoun dio tetia entries gia versus
                                        -- gia atomika parousiasi 1
                                        -- gia omadiko vs 6 h 4 anepisima
    athlete_id      INTEGER         REFERENCES athletes (id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id),
    game_id         INTEGER         REFERENCES games (id),
    PRIMARY KEY (athlete_id, game_id)
);




CREATE TABLE game_points(

    id              SERIAL,
    game_id         INTEGER         REFERENCES games(id),
    athlete_id      INTEGER         REFERENCES athletes(id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id),

    technical_point         INTEGER,
    technical_point_desc    varchar(50),

    PRIMARY KEY (id)
);


CREATE TABLE game_score (
    id              SERIAL,
    game_id         INTEGER         REFERENCES games(id),
    athlete_id      INTEGER         REFERENCES athletes(id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id), --breaks naming conventions for brevity

    score1          INTEGER,
    score2          INTEGER,
    score3          INTEGER,
    score4          INTEGER,
    score5          INTEGER,
    mean_score      INTEGER,

    judge1          INTEGER         REFERENCES judges(id),
    judge2          INTEGER         REFERENCES judges(id),
    judge3          INTEGER         REFERENCES judges(id),
    judge4          INTEGER         REFERENCES judges(id),
    judge5          INTEGER         REFERENCES judges(id),

    PRIMARY KEY (id)
);


create table game_flag (
    id              SERIAL,
    game_id         INTEGER         REFERENCES games(id),
    athlete_id      INTEGER         REFERENCES athletes(id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id), --breaks naming conventions for brevity

    flag1           BOOLEAN,
    flag2           BOOLEAN,
    flag3           BOOLEAN,
    flag4           BOOLEAN,
    flag5           BOOLEAN,

    judge1          INTEGER         REFERENCES judges(id),
    judge2          INTEGER         REFERENCES judges(id),
    judge3          INTEGER         REFERENCES judges(id),
    judge4          INTEGER         REFERENCES judges(id),
    judge5          INTEGER         REFERENCES judges(id),

    PRIMARY KEY (id)
);

-- commit transaction (will destroy all existing TABLEs and data):
COMMIT;


-- rollback transaction (useful for checking syntax):
--ROLLBACK;



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


INSERT INTO clubs (name, address_id, country_code) VALUES ('Fight Club', 0, 'CY');
INSERT INTO clubs (name, address_id, country_code) VALUES ('Night Club', 0, 'GR'); --address is CY!


INSERT INTO persons (id , first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('0' , 'administrator' , 'xampis' , 'administrator', '02-10-1990' , 'male', '99123144' , null , 'email@gmail.com' , '0');

INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('athl1' , 'athl1_father' , 'athl1_last', '02-10-1991' , 'MALE', '99123145' , null , 'athl1@gmail.com' , '0');

INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('athl2' , 'athl2_father' , 'athl2_last', '02-10-1992' , 'MALE', '99123146' , null , 'athl2@gmail.com' , '0');

INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('athl3' , 'athl3_father' , 'athl3_last', '02-10-1997' , 'FEMALE', '99123145' , null , 'athl3@gmail.com' , '0');

INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('athl4' , 'athl4_father' , 'athl4_last', '02-10-1982' , 'MALE', '98123146' , null , 'athl4@gmail.com' , '0');

-- not an athlete, to check if auto-increment (SERIAL) works... EDIT: IT DOES,
-- BUT ONLY IF YOU NEVER ASSIGN IDs >0 MANUALLY! (Assigning 0 is OK, see above 'administrator')
INSERT INTO persons (first_name, fathers_name, last_name, date_of_birth, sex,  phone, secondary_phone, email, address_id)
VALUES ('NotAnAthll' , 'NotAnAthllfather' , 'NotAnAthlllast', '02-10-1992' , 'male', '99123146' , null , 'NotAnAthll@gmail.com' , '0');


-- adding user: "admin" pass: "admin" (will be removed in the final releases!)
INSERT INTO users (id , username , password, person_management, event_management , lottery , game_support , reports , settings)
VALUES ( '0', 'admin' , '3039283064aa2a9ca939b1fe23954698' , '1' , '1' , '1' , '1' , '1' , '1');


INSERT INTO athletes (id, rank, club_id ) VALUES ('0', 'Black  –  1st dan', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('1', 'Black  –  2nd dan', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('2', 'Black  –  1st dan', NULL );
INSERT INTO athletes (id, rank, club_id ) VALUES ('3', 'Blue   –  2nd kyu', '2'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('4', 'White/Red – 8th dan', '2' );

INSERT INTO locations (id, name, phone, email)
VALUES (0, 'location 1', '77778888', 'loc@locloc.com');

INSERT INTO events (name, date, location_id)
VALUES ('temp', '2012/10/30', 0);
INSERT INTO events (name, date, location_id)
VALUES ('Big Event', '2012/10/30', 0);

INSERT INTO tournaments (name, sex, age_from, age_to, level_from, level_to, game_type, scoring_type, event_id)
VALUES ('temp', 'male', 1, 99, 'White  –  6th kyu', 'Red – 10th dan', 'KATA' , 'FLAG', 1);
INSERT INTO tournaments (name, sex, age_from, age_to, level_from, level_to, game_type, scoring_type, event_id)
VALUES ('Iron Fist Tournament', 'male', 1, 99, 'Yellow –  5th kyu', 'White/Red – 8th dan', 'deathmatch', 'flag', 2);



-- A NULL "position" means that the match hasn't taken place yet
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (0, 2, (SELECT rank FROM athletes WHERE id = 0), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (1, 2, (SELECT rank FROM athletes WHERE id = 1), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (2, 2, (SELECT rank FROM athletes WHERE id = 2), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (3, 2, (SELECT rank FROM athletes WHERE id = 3), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (4, 2, (SELECT rank FROM athletes WHERE id = 4), NULL, NULL );




-- rollback transaction (useful for checking syntax):
--ROLLBACK;

-- commit transaction:
COMMIT;
