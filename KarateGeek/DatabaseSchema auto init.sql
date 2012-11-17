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
-- **  Using "id" for PK, "person_id" for FK.
-- **  SQL KEYWORDS (but NOT data types) :   UPPERCASE
-- **  other identifiers :                   lowercase
-- **  REFERENCES as a column constraint
-- **  PRIMARY KEY (composite or not) and CHECK as table constraints
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
-- * Using the public schema. The "schema_name." table prefix is necessary otherwise,
--   unless one uses "SET search_path TO schema_name;" after initiating _every_ DB connection.
--

------------------------------------------------------------------------------------------------


-- begin transaction:
BEGIN;

DROP SCHEMA IF EXISTS public CASCADE;      -- WARNING: This also deletes all tables!

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
  id                SERIAL,       -- "SERIAL" as a data type means an auto-incrementing integer
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
    --CHECK (email ~* E'^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\.)+[A-Z]{2,}$') -- not very restrictive
);


CREATE TABLE athletes (
    id              INTEGER         REFERENCES persons(id)  ON DELETE NO ACTION, -- not CASCADE!
    rank            VARCHAR(50)     NOT NULL,
    club_id         INTEGER         REFERENCES clubs(id),   -- this was "NOT NULL" initially
    PRIMARY KEY(id)
);


CREATE TABLE judges (
    id              INTEGER         REFERENCES persons(id)  ON DELETE NO ACTION,
    rank            VARCHAR(50)     NOT NULL,
    class           CHARacter(1)    NOT NULL,
    PRIMARY KEY(id)
);


CREATE TABLE users (
    id              SERIAL,
    username        VARCHAR(255)    NOT NULL UNIQUE,
    password        VARCHAR(255)    NOT NULL,
    person_management   BOOLEAN     NOT NULL DEFAULT false,
    event_management    BOOLEAN     NOT NULL DEFAULT false,
    lottery         BOOLEAN         NOT NULL DEFAULT false,
    game_support    BOOLEAN         NOT NULL DEFAULT false,
    reports         BOOLEAN         NOT NULL DEFAULT false,
    settings        BOOLEAN         NOT NULL DEFAULT false,
    PRIMARY KEY (id)
);


CREATE TABLE events (
    id              SERIAL,
    name            VARCHAR(80)     NOT NULL,
    date            DATE,
    official        BOOLEAN         NOT NULL DEFAULT true,   -- default true according to specs
    location_id     INTEGER         REFERENCES locations(id),
    is_finished     BOOLEAN         NOT NULL DEFAULT false,
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
    event_id        INTEGER         REFERENCES events(id) ON DELETE CASCADE,
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
    team            INTEGER,        -- "team" is only used by the GUI code (doesn't interfere with the rest of the DB)
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


CREATE TABLE game_participations (      -- gia atomika (versus) tha mpainoun dio tetoia entries
                                        -- gia atomika (parousiasi) 1 entry
                                        -- gia omadiko (versus) 6 (OR 4, anepisima)
    id              SERIAL,	
    athlete_id      INTEGER         REFERENCES athletes (id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id),
    game_id         INTEGER         REFERENCES games (id),
    PRIMARY KEY (id)
);


CREATE TABLE game_points(
    id              SERIAL,
    game_id         INTEGER         REFERENCES games(id),
    athlete_id      INTEGER         REFERENCES athletes(id),
    team_id         INTEGER         REFERENCES team_tournament_participations(id),

    technical_point         INTEGER,
    technical_point_desc    VARCHAR(50),

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


-- rollback transaction (useful for checking syntax):
--ROLLBACK;

-- commit transaction (will destroy all existing tables and data):
COMMIT;


--
-- Initialisation with data
--


-- begin transaction:
BEGIN;


INSERT INTO countries (code, name) VALUES('AF', 'Afghanistan');
INSERT INTO countries (code, name) VALUES('AL', 'Albania');
INSERT INTO countries (code, name) VALUES('DZ', 'Algeria');
INSERT INTO countries (code, name) VALUES('AS', 'American Samoa');
INSERT INTO countries (code, name) VALUES('AD', 'Andorra');
INSERT INTO countries (code, name) VALUES('AO', 'Angola');
INSERT INTO countries (code, name) VALUES('AI', 'Anguilla');
INSERT INTO countries (code, name) VALUES('AQ', 'Antarctica');
INSERT INTO countries (code, name) VALUES('AG', 'Antigua and Barbuda');
INSERT INTO countries (code, name) VALUES('AR', 'Argentina');
INSERT INTO countries (code, name) VALUES('AM', 'Armenia');
INSERT INTO countries (code, name) VALUES('AW', 'Aruba');
INSERT INTO countries (code, name) VALUES('AU', 'Australia');
INSERT INTO countries (code, name) VALUES('AT', 'Austria');
INSERT INTO countries (code, name) VALUES('AZ', 'Azerbaijan');
INSERT INTO countries (code, name) VALUES('BS', 'Bahamas');
INSERT INTO countries (code, name) VALUES('BH', 'Bahrain');
INSERT INTO countries (code, name) VALUES('BD', 'Bangladesh');
INSERT INTO countries (code, name) VALUES('BB', 'Barbados');
INSERT INTO countries (code, name) VALUES('BY', 'Belarus');
INSERT INTO countries (code, name) VALUES('BE', 'Belgium');
INSERT INTO countries (code, name) VALUES('BZ', 'Belize');
INSERT INTO countries (code, name) VALUES('BJ', 'Benin');
INSERT INTO countries (code, name) VALUES('BM', 'Bermuda');
INSERT INTO countries (code, name) VALUES('BT', 'Bhutan');
INSERT INTO countries (code, name) VALUES('BO', 'Bolivia');
INSERT INTO countries (code, name) VALUES('BA', 'Bosnia and Herzegowina');
INSERT INTO countries (code, name) VALUES('BW', 'Botswana');
INSERT INTO countries (code, name) VALUES('BV', 'Bouvet Island');
INSERT INTO countries (code, name) VALUES('BR', 'Brazil');
INSERT INTO countries (code, name) VALUES('IO', 'British Indian Ocean Territory');
INSERT INTO countries (code, name) VALUES('BN', 'Brunei Darussalam');
INSERT INTO countries (code, name) VALUES('BG', 'Bulgaria');
INSERT INTO countries (code, name) VALUES('BF', 'Burkina Faso');
INSERT INTO countries (code, name) VALUES('BI', 'Burundi');
INSERT INTO countries (code, name) VALUES('KH', 'Cambodia');
INSERT INTO countries (code, name) VALUES('CM', 'Cameroon');
INSERT INTO countries (code, name) VALUES('CV', 'Cape Verde');
INSERT INTO countries (code, name) VALUES('KY', 'Cayman Islands');
INSERT INTO countries (code, name) VALUES('CF', 'Central African Republic');
INSERT INTO countries (code, name) VALUES('TD', 'Chad');
INSERT INTO countries (code, name) VALUES('CL', 'Chile');
INSERT INTO countries (code, name) VALUES('CN', 'China');
INSERT INTO countries (code, name) VALUES('CX', 'Christmas Island');
INSERT INTO countries (code, name) VALUES('CC', 'Cocos (Keeling) Islands');
INSERT INTO countries (code, name) VALUES('CO', 'Colombia');
INSERT INTO countries (code, name) VALUES('KM', 'Comoros');
INSERT INTO countries (code, name) VALUES('CG', 'Congo');
INSERT INTO countries (code, name) VALUES('CD', 'Congo, The Democratic Republic of the');
INSERT INTO countries (code, name) VALUES('CK', 'Cook Islands');
INSERT INTO countries (code, name) VALUES('CR', 'Costa Rica');
INSERT INTO countries (code, name) VALUES('CI', 'Cote D''Ivoire');
INSERT INTO countries (code, name) VALUES('HR', 'Croatia (local name: Hrvatska)');
INSERT INTO countries (code, name) VALUES('CU', 'Cuba');
INSERT INTO countries (code, name) VALUES('CY', 'Cyprus');
INSERT INTO countries (code, name) VALUES('CZ', 'Czech Republic');
INSERT INTO countries (code, name) VALUES('DK', 'Denmark');
INSERT INTO countries (code, name) VALUES('DJ', 'Djibouti');
INSERT INTO countries (code, name) VALUES('DM', 'Dominica');
INSERT INTO countries (code, name) VALUES('DO', 'Dominican Republic');
INSERT INTO countries (code, name) VALUES('TP', 'East Timor');
INSERT INTO countries (code, name) VALUES('EC', 'Ecuador');
INSERT INTO countries (code, name) VALUES('EG', 'Egypt');
INSERT INTO countries (code, name) VALUES('SV', 'El Salvador');
INSERT INTO countries (code, name) VALUES('GQ', 'Equatorial Guinea');
INSERT INTO countries (code, name) VALUES('ER', 'Eritrea');
INSERT INTO countries (code, name) VALUES('EE', 'Estonia');
INSERT INTO countries (code, name) VALUES('ET', 'Ethiopia');
INSERT INTO countries (code, name) VALUES('FK', 'Falkland Islands (Malvinas)');
INSERT INTO countries (code, name) VALUES('FO', 'Faroe Islands');
INSERT INTO countries (code, name) VALUES('FJ', 'Fiji');
INSERT INTO countries (code, name) VALUES('FI', 'Finland');
INSERT INTO countries (code, name) VALUES('FR', 'France');
INSERT INTO countries (code, name) VALUES('FX', 'France, Metropolitan');
INSERT INTO countries (code, name) VALUES('GF', 'French Guiana');
INSERT INTO countries (code, name) VALUES('PF', 'French Polynesia');
INSERT INTO countries (code, name) VALUES('TF', 'French Southern Territories');
INSERT INTO countries (code, name) VALUES('GA', 'Gabon');
INSERT INTO countries (code, name) VALUES('GM', 'Gambia');
INSERT INTO countries (code, name) VALUES('GE', 'Georgia');
INSERT INTO countries (code, name) VALUES('DE', 'Germany');
INSERT INTO countries (code, name) VALUES('GH', 'Ghana');
INSERT INTO countries (code, name) VALUES('GI', 'Gibraltar');
INSERT INTO countries (code, name) VALUES('GR', 'Greece');
INSERT INTO countries (code, name) VALUES('GL', 'Greenland');
INSERT INTO countries (code, name) VALUES('GD', 'Grenada');
INSERT INTO countries (code, name) VALUES('GP', 'Guadeloupe');
INSERT INTO countries (code, name) VALUES('GU', 'Guam');
INSERT INTO countries (code, name) VALUES('GT', 'Guatemala');
INSERT INTO countries (code, name) VALUES('GN', 'Guinea');
INSERT INTO countries (code, name) VALUES('GW', 'Guinea-Bissau');
INSERT INTO countries (code, name) VALUES('GY', 'Guyana');
INSERT INTO countries (code, name) VALUES('HT', 'Haiti');
INSERT INTO countries (code, name) VALUES('HM', 'Heard and McDonald Islands');
INSERT INTO countries (code, name) VALUES('VA', 'Holy See (Vatican City State)');
INSERT INTO countries (code, name) VALUES('HN', 'Honduras');
INSERT INTO countries (code, name) VALUES('HK', 'Hong Kong');
INSERT INTO countries (code, name) VALUES('HU', 'Hungary');
INSERT INTO countries (code, name) VALUES('IS', 'Iceland');
INSERT INTO countries (code, name) VALUES('IN', 'India');
INSERT INTO countries (code, name) VALUES('ID', 'Indonesia');
INSERT INTO countries (code, name) VALUES('IR', 'Iran (Islamic Republic of)');
INSERT INTO countries (code, name) VALUES('IQ', 'Iraq');
INSERT INTO countries (code, name) VALUES('IE', 'Ireland');
INSERT INTO countries (code, name) VALUES('IL', 'Israel');
INSERT INTO countries (code, name) VALUES('IT', 'Italy');
INSERT INTO countries (code, name) VALUES('JM', 'Jamaica');
INSERT INTO countries (code, name) VALUES('JP', 'Japan');
INSERT INTO countries (code, name) VALUES('JO', 'Jordan');
INSERT INTO countries (code, name) VALUES('KZ', 'Kazakhstan');
INSERT INTO countries (code, name) VALUES('KE', 'Kenya');
INSERT INTO countries (code, name) VALUES('KI', 'Kiribati');
INSERT INTO countries (code, name) VALUES('KP', 'Korea, Democratic People''s Republic of');
INSERT INTO countries (code, name) VALUES('KR', 'Korea, Republic of');
INSERT INTO countries (code, name) VALUES('KW', 'Kuwait');
INSERT INTO countries (code, name) VALUES('KG', 'Kyrgyzstan');
INSERT INTO countries (code, name) VALUES('LA', 'Lao People''s Democratic Republic');
INSERT INTO countries (code, name) VALUES('LV', 'Latvia');
INSERT INTO countries (code, name) VALUES('LB', 'Lebanon');
INSERT INTO countries (code, name) VALUES('LS', 'Lesotho');
INSERT INTO countries (code, name) VALUES('LR', 'Liberia');
INSERT INTO countries (code, name) VALUES('LY', 'Libyan Arab Jamahiriya');
INSERT INTO countries (code, name) VALUES('LI', 'Liechtenstein');
INSERT INTO countries (code, name) VALUES('LT', 'Lithuania');
INSERT INTO countries (code, name) VALUES('LU', 'Luxembourg');
INSERT INTO countries (code, name) VALUES('MO', 'Macau');
INSERT INTO countries (code, name) VALUES('MK', 'Macedonia, the Former Yugoslav Republic of');
INSERT INTO countries (code, name) VALUES('MG', 'Madagascar');
INSERT INTO countries (code, name) VALUES('MW', 'Malawi');
INSERT INTO countries (code, name) VALUES('MY', 'Malaysia');
INSERT INTO countries (code, name) VALUES('MV', 'Maldives');
INSERT INTO countries (code, name) VALUES('ML', 'Mali');
INSERT INTO countries (code, name) VALUES('MT', 'Malta');
INSERT INTO countries (code, name) VALUES('MH', 'Marshall Islands');
INSERT INTO countries (code, name) VALUES('MQ', 'Martinique');
INSERT INTO countries (code, name) VALUES('MR', 'Mauritania');
INSERT INTO countries (code, name) VALUES('MU', 'Mauritius');
INSERT INTO countries (code, name) VALUES('YT', 'Mayotte');
INSERT INTO countries (code, name) VALUES('FM', 'Micronesia, Federated States of');
INSERT INTO countries (code, name) VALUES('MD', 'Moldova, Republic of');
INSERT INTO countries (code, name) VALUES('MC', 'Monaco');
INSERT INTO countries (code, name) VALUES('MN', 'Mongolia');
INSERT INTO countries (code, name) VALUES('MS', 'Montserrat');
INSERT INTO countries (code, name) VALUES('MA', 'Morocco');
INSERT INTO countries (code, name) VALUES('MZ', 'Mozambique');
INSERT INTO countries (code, name) VALUES('MM', 'Myanmar');
INSERT INTO countries (code, name) VALUES('NA', 'Namibia');
INSERT INTO countries (code, name) VALUES('NR', 'Nauru');
INSERT INTO countries (code, name) VALUES('NP', 'Nepal');
INSERT INTO countries (code, name) VALUES('NL', 'Netherlands');
INSERT INTO countries (code, name) VALUES('AN', 'Netherlands Antilles');
INSERT INTO countries (code, name) VALUES('NC', 'New Caledonia');
INSERT INTO countries (code, name) VALUES('NZ', 'New Zealand');
INSERT INTO countries (code, name) VALUES('NI', 'Nicaragua');
INSERT INTO countries (code, name) VALUES('NE', 'Niger');
INSERT INTO countries (code, name) VALUES('NG', 'Nigeria');
INSERT INTO countries (code, name) VALUES('NU', 'Niue');
INSERT INTO countries (code, name) VALUES('NF', 'Norfolk Island');
INSERT INTO countries (code, name) VALUES('MP', 'Northern Mariana Islands');
INSERT INTO countries (code, name) VALUES('NO', 'Norway');
INSERT INTO countries (code, name) VALUES('OM', 'Oman');
INSERT INTO countries (code, name) VALUES('PK', 'Pakistan');
INSERT INTO countries (code, name) VALUES('PW', 'Palau');
INSERT INTO countries (code, name) VALUES('PS', 'Palestinian Territory, Occupied');
INSERT INTO countries (code, name) VALUES('PA', 'Panama');
INSERT INTO countries (code, name) VALUES('PG', 'Papua New Guinea');
INSERT INTO countries (code, name) VALUES('PY', 'Paraguay');
INSERT INTO countries (code, name) VALUES('PE', 'Peru');
INSERT INTO countries (code, name) VALUES('PH', 'Philippines');
INSERT INTO countries (code, name) VALUES('PN', 'Pitcairn');
INSERT INTO countries (code, name) VALUES('PL', 'Poland');
INSERT INTO countries (code, name) VALUES('PT', 'Portugal');
INSERT INTO countries (code, name) VALUES('PR', 'Puerto Rico');
INSERT INTO countries (code, name) VALUES('QA', 'Qatar');
INSERT INTO countries (code, name) VALUES('RE', 'Reunion');
INSERT INTO countries (code, name) VALUES('RO', 'Romania');
INSERT INTO countries (code, name) VALUES('RU', 'Russian Federation');
INSERT INTO countries (code, name) VALUES('RW', 'Rwanda');
INSERT INTO countries (code, name) VALUES('KN', 'Saint Kitts and Nevis');
INSERT INTO countries (code, name) VALUES('LC', 'Saint Lucia');
INSERT INTO countries (code, name) VALUES('VC', 'Saint Vincent and the Grenadines');
INSERT INTO countries (code, name) VALUES('WS', 'Samoa');
INSERT INTO countries (code, name) VALUES('SM', 'San Marino');
INSERT INTO countries (code, name) VALUES('ST', 'Sao Tome and Principe');
INSERT INTO countries (code, name) VALUES('SA', 'Saudi Arabia');
INSERT INTO countries (code, name) VALUES('SN', 'Senegal');
INSERT INTO countries (code, name) VALUES('CS', 'Serbia and Montenegro');
INSERT INTO countries (code, name) VALUES('SC', 'Seychelles');
INSERT INTO countries (code, name) VALUES('SL', 'Sierra Leone');
INSERT INTO countries (code, name) VALUES('SG', 'Singapore');
INSERT INTO countries (code, name) VALUES('SK', 'Slovakia (Slovak Republic)');
INSERT INTO countries (code, name) VALUES('SI', 'Slovenia');
INSERT INTO countries (code, name) VALUES('SB', 'Solomon Islands');
INSERT INTO countries (code, name) VALUES('SO', 'Somalia');
INSERT INTO countries (code, name) VALUES('ZA', 'South Africa');
INSERT INTO countries (code, name) VALUES('GS', 'South Georgia and the South Sandwich Islands');
INSERT INTO countries (code, name) VALUES('ES', 'Spain');
INSERT INTO countries (code, name) VALUES('LK', 'Sri Lanka');
INSERT INTO countries (code, name) VALUES('SH', 'St. Helena');
INSERT INTO countries (code, name) VALUES('PM', 'St. Pierre and Miquelon');
INSERT INTO countries (code, name) VALUES('SD', 'Sudan');
INSERT INTO countries (code, name) VALUES('SR', 'Suriname');
INSERT INTO countries (code, name) VALUES('SJ', 'Svalbard and Jan Mayen Islands');
INSERT INTO countries (code, name) VALUES('SZ', 'Swaziland');
INSERT INTO countries (code, name) VALUES('SE', 'Sweden');
INSERT INTO countries (code, name) VALUES('CH', 'Switzerland');
INSERT INTO countries (code, name) VALUES('SY', 'Syrian Arab Republic');
--INSERT INTO countries (code, name) VALUES('TW', 'Taiwan, Province of China');
INSERT INTO countries (code, name) VALUES('TW', 'Taiwan');
INSERT INTO countries (code, name) VALUES('TJ', 'Tajikistan');
INSERT INTO countries (code, name) VALUES('TZ', 'Tanzania, United Republic of');
INSERT INTO countries (code, name) VALUES('TH', 'Thailand');
INSERT INTO countries (code, name) VALUES('TL', 'Timor-Leste');
INSERT INTO countries (code, name) VALUES('TG', 'Togo');
INSERT INTO countries (code, name) VALUES('TK', 'Tokelau');
INSERT INTO countries (code, name) VALUES('TO', 'Tonga');
INSERT INTO countries (code, name) VALUES('TT', 'Trinidad and Tobago');
INSERT INTO countries (code, name) VALUES('TN', 'Tunisia');
INSERT INTO countries (code, name) VALUES('TR', 'Turkey');
INSERT INTO countries (code, name) VALUES('TM', 'Turkmenistan');
INSERT INTO countries (code, name) VALUES('TC', 'Turks and Caicos Islands');
INSERT INTO countries (code, name) VALUES('TV', 'Tuvalu');
INSERT INTO countries (code, name) VALUES('UG', 'Uganda');
INSERT INTO countries (code, name) VALUES('UA', 'Ukraine');
INSERT INTO countries (code, name) VALUES('AE', 'United Arab Emirates');
INSERT INTO countries (code, name) VALUES('UM', 'United States Minor Outlying Islands');
INSERT INTO countries (code, name) VALUES('UY', 'Uruguay');
INSERT INTO countries (code, name) VALUES('UZ', 'Uzbekistan');
INSERT INTO countries (code, name) VALUES('VU', 'Vanuatu');
INSERT INTO countries (code, name) VALUES('VE', 'Venezuela');
INSERT INTO countries (code, name) VALUES('VN', 'Viet Nam');
INSERT INTO countries (code, name) VALUES('VG', 'Virgin Islands (British)');
INSERT INTO countries (code, name) VALUES('VI', 'Virgin Islands (U.S.)');
INSERT INTO countries (code, name) VALUES('WF', 'Wallis and Futuna Islands');
INSERT INTO countries (code, name) VALUES('EH', 'Western Sahara');
INSERT INTO countries (code, name) VALUES('YE', 'Yemen');
INSERT INTO countries (code, name) VALUES('YU', 'Yugoslavia');
INSERT INTO countries (code, name) VALUES('ZM', 'Zambia');
INSERT INTO countries (code, name) VALUES('ZW', 'Zimbabwe');
INSERT INTO countries (code, name) VALUES('CA', 'Canada');
INSERT INTO countries (code, name) VALUES('MX', 'Mexico');
INSERT INTO countries (code, name) VALUES('GB', 'United Kingdom');
INSERT INTO countries (code, name) VALUES('US', 'United States');


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
VALUES ('NotAnAthll' , 'NotAnAthllfather' , 'NotAnAthlllast', '02-10-1992' , 'MALE', '99123146' , null , 'NotAnAthll@gmail.com' , '0');


-- adding user: "admin" pass: "admin" (will be removed in the final releases!)
INSERT INTO users (username , password, person_management, event_management , lottery , game_support , reports , settings)
VALUES ('admin' , '3039283064aa2a9ca939b1fe23954698' , '1' , '1' , '1' , '1' , '1' , '1');


INSERT INTO athletes (id, rank, club_id ) VALUES ('0', 'Black  –  1st dan', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('1', 'Black  –  2nd dan', '1'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('2', 'Black  –  1st dan', NULL );
INSERT INTO athletes (id, rank, club_id ) VALUES ('3', 'Blue   –  2nd kyu', '2'  );
INSERT INTO athletes (id, rank, club_id ) VALUES ('4', 'White/Red – 8th dan', '2' );

INSERT INTO locations (id, name, phone, email)
VALUES (0, 'location 1', '77778888', 'loc@locloc.com');


INSERT INTO events (name, date, location_id)
VALUES ('Big Event', '2012/10/30', 0);

INSERT INTO tournaments (name, sex, age_from, age_to, level_from, level_to, game_type, scoring_type, event_id)
VALUES ('Iron Fist Tournament', 'male', 1, 99, 'Yellow –  5th kyu', 'White/Red – 8th dan', 'IND|KUMITE', 'POINT', 1);



-- A NULL "position" means that the match hasn't taken place yet
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (0, 1, (SELECT rank FROM athletes WHERE id = 0), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (1, 1, (SELECT rank FROM athletes WHERE id = 1), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (2, 1, (SELECT rank FROM athletes WHERE id = 2), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (3, 1, (SELECT rank FROM athletes WHERE id = 3), NULL, NULL );
INSERT INTO tournament_participations ( athlete_id , tournament_id , rank_at_time , ranking , team_id)
VALUES (4, 1, (SELECT rank FROM athletes WHERE id = 4), NULL, NULL );

--select * from tournaments;

--select * from games;


-- rollback transaction (useful for checking syntax):
--ROLLBACK;

-- commit transaction:
COMMIT;




ANALYSE;
-- From the PostreSQL documentation:
--
-- ANALYZE collects statistics about the contents of tables in the database, and stores the
-- results in the pg_statistic system catalog. Subsequently, the query planner uses these
-- statistics to help determine the most efficient execution plans for queries.
--
-- With no parameter, ANALYZE examines every table in the current database. With a parameter,
-- ANALYZE examines only that table. It is further possible to give a list of column names, in
-- which case only the statistics for those columns are collected.
