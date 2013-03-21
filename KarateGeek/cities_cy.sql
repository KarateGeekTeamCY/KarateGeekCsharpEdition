-- command: psql --username=postgres karategeek < "C:\Users\michael\Documents\GitHub\KarateGeekCsharpEdition\KarateGeek\insert_cities.sql"
-- search string: ^([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*\t)([^\t]*).*
-- replace with: \2\9

BEGIN;

CREATE TEMPORARY TABLE tempcities (
  id                SERIAL,       -- "SERIAL" as a data type means an auto-incrementing integer
  name              VARCHAR(80)     NOT NULL,
  country_code      VARCHAR(2),
  PRIMARY KEY(id)
);

COPY tempcities(name, country_code) FROM STDIN WITH DELIMITER '	';
Plates	CY
Kiti Tower	CY
Ayia Irini	CY
Ayios Leondios	CY
Kokkinadhia	CY
Ayios Yeoryios	CY
Limnes	CY
Elioudhia	CY
Kato Potamos	CY
Nikolies	CY
Kashanes	CY
Kafkarid Tepesi	CY
Pitsilli	CY
Kokkines	CY
Prountzidhes	CY
Ayia Marina	CY
Ayios Andronikos	CY
Mazeri	CY
Ayios Thomas	CY
Panayia Angeloktistos	CY
Ayios Yeoryios	CY
Tremithos-Kiti Dam	CY
Vouno	CY
Ayion Yeorgoudhi	CY
Ayios Andronikos	CY
Ayios Yeoryios tis Asperas	CY
Ambeloudhi	CY
Arpera Chiftlik	CY
Mazeri	CY
Pende Petres	CY
Akopetra	CY
Kolymbi	CY
Moutti tou Alexikou	CY
Pamboulos	CY
Mosphiloti	CY
Ayios Epiphanios	CY
Rodhino	CY
Jami Kebir Mosque	CY
Larnaca Castle	CY
Ayios Lazaros	CY
Ayia Phaneromeni	CY
Vyzakia City-Site	CY
Melissari	CY
Ayios Yeoryios Makris	CY
Pasha Chiftlik	CY
Vlakhos Chiftlik	CY
Kapsalos	CY
Exoyi	CY
Kavalla	CY
Pano Chiftlik	CY
Strongylia	CY
Mouttes	CY
Koja Seki	CY
Ormani	CY
Kathari	CY
Rakhmeli	CY
Ayios Savvas	CY
Ayia Paraskevi	CY
Ayia Anargyron	CY
Kochiniles	CY
Ayia Anastasia	CY
Chakkilera	CY
Panayia	CY
Kharcha	CY
Klimis	CY
Alona	CY
Rizoelia Forest	CY
Ayios Yeoryios	CY
Panayia Ematousa	CY
Kapsalero Moutti	CY
Ayios Mamas	CY
Vournia	CY
Pamboulari	CY
Kondes	CY
Moutti tou Pharangou	CY
Rotsos	CY
Kolokia	CY
Kouzoula	CY
Moutti tou Zyou	CY
Petounda	CY
Petounda	CY
Karpanalis	CY
Latourou	CY
Philimeni	CY
Akonia	CY
Kopros	CY
Merika	CY
Shiromandres	CY
Pendaskhinos	CY
Ayios Athanasios	CY
Kapses	CY
High Hill	CY
Kylindros	CY
Stephani	CY
Ayion Yeorgoudhi	CY
Yeropalloures	CY
Ayios Nikolaos	CY
Ayii Anargyri	CY
Kashones	CY
Xalona	CY
Pervolia City-Site	CY
Elioti	CY
Kochines tou Miri	CY
Katsaroumba	CY
Ayios Andronikos	CY
Ayia Marina	CY
Amala	CY
Petromoutti	CY
Pateli	CY
Kambanaris	CY
Kapsaloudhkia	CY
Timios Stavros	CY
Ayios Nikiphoros	CY
Panayia tou Kambou	CY
Ayios Yeoryios	CY
Ayios Iakovos	CY
Kaoukkos	CY
Kafkalla	CY
Zoulophtidhes	CY
Pamboulos	CY
Mikhail Arkhangelos	CY
Melissotrypa	CY
Koundourka	CY
Sterna	CY
Argaki tis Asgatas	CY
Ayii Apostoli	CY
Yerakarkon	CY
Asproyi	CY
Petra	CY
Limnes	CY
Ayios Xenophon	CY
Viklones	CY
Voudhomandres	CY
Kokkinadhia	CY
Kavallaridhes	CY
Sougles	CY
Strakhala	CY
Alaminos Tower	CY
Ayios Mamas	CY
Trakhonia	CY
Plakotos	CY
Argaki tis Mavroyis	CY
Ayios Athanasios and Ayios Kyrilios	CY
Aphendiko	CY
Panayia ton Astadhkion	CY
Argakia	CY
Dhrakondies	CY
Panayia	CY
Appiakia	CY
Kamitsis	CY
Vounon tis Kophinou	CY
Paliolino	CY
New Mavro	CY
Psyllos	CY
Zefkos	CY
Ayios Loukas	CY
Ayios Yeoryios	CY
Kholetra	CY
Piyi tou Kounna	CY
Khirokitia Settlement	CY
Ayia Paraskevi	CY
Panayia	CY
Laona tou Mavrou	CY
Kasparis	CY
Knight’s Templar Commandery	CY
Nemourka	CY
Pambakas	CY
Ayios Yeoryios	CY
Katsoura	CY
Ayios Spyridhon	CY
Xylosyrma	CY
Ayios Neophytos	CY
Ayios Yeoryios	CY
Aetomoutti tis Vavlas	CY
Mandrika	CY
Ayios Minas Monastery	CY
Kapsalia	CY
Petromoudhkia	CY
Panayia	CY
Rialas	CY
Koukos	CY
Arkhangelos	CY
Paliolinos	CY
Kourouklies	CY
Volikoudhia	CY
Mersines	CY
Pousomandra	CY
Ayios Yeoryios	CY
Ayios Therapon	CY
Ayios Epiphanios	CY
Mouttas	CY
Aspropetra	CY
Mavronero	CY
Kapsala	CY
Kamini	CY
Mazera	CY
Aetomoutti	CY
Gaidhouropniktos	CY
Kangellopetra	CY
Garinos	CY
Ayia Varvara Monastery	CY
Spalathovouno	CY
Vatos	CY
Stavrovouni Forest	CY
Kamenos	CY
Melanas	CY
Triandaphylia	CY
Kambos tou Simoni	CY
Asprospilios	CY
Koullouropetra	CY
New Mallia	CY
Kaloveros	CY
Zgouros	CY
Panayia Galaktotrophousa Monastery	CY
Mesovounon	CY
Mikhail Arkhangelos	CY
Argaki tou Mylou	CY
Pernaka	CY
Panayia Nikiphoros	CY
Mouttarka	CY
Shinokephalo	CY
Koupanos	CY
Kasi	CY
Potis	CY
Ayios Yeoryios	CY
Arkhangelos	CY
Ayios Thomas	CY
Mesa Moutti	CY
Marathos	CY
Ayios Timotheos	CY
Ayia Anastasia	CY
Ayia Marina	CY
Kavallaris	CY
Kalyphoudhia	CY
Kafkalia	CY
Prodhromos	CY
Arkhangelos Monastery	CY
Zanatophoulia	CY
Zakharovouni	CY
Vounon	CY
Panayia tis Agapis	CY
Ayii Saranda	CY
Kokkinokremmos	CY
Paliaplikon	CY
Panayia Stazousa	CY
Teratsotos	CY
Maratheftou	CY
Rakharka	CY
Tremithos	CY
Papalia	CY
Ayios Modhestos	CY
Linos Chiftlik	CY
Laxia tou Vashoti	CY
New Appidhaki	CY
Kaourotrypes	CY
Niolimata Chiftlik	CY
Evgali	CY
Ayia Marina	CY
Pyrga Royal Chapel	CY
Mouttarka tou Mina	CY
Argaki tou Symeou Efendi	CY
Pikrasidhi	CY
Ayios Yeoryios	CY
Jinarotos	CY
Ayia Thekla Monastery	CY
Kremarka	CY
Prodhromos	CY
Xindes	CY
Khalospita	CY
Kochinorashi	CY
Mazovouno	CY
Xylias	CY
Ayios Eftykhios	CY
Loukkos	CY
Khrysosotiros	CY
Moutti tou Poti	CY
Psathas	CY
Mouttara	CY
Kourtellorotsos	CY
Argaki ton Vaton	CY
Aetomoutti Forest	CY
Armyrias	CY
Kataxylos	CY
Kokkinopambouli	CY
Vulture Point	CY
Kephalos	CY
Kandilioros	CY
Phyta	CY
Dhidhymos	CY
Kyprovasa Chiftlik	CY
Ayios Yeoryios	CY
Kedhris	CY
Ayia Paraskevi	CY
Pano Teratsotos	CY
Makheras Forest	CY
Petromoutti	CY
Moutti tou Kornou	CY
Argaki tou Ayiou Yeoryiou	CY
Peristerkas	CY
Argaki ton Mandron	CY
Argaki ton Marathefti	CY
Oxys	CY
Argaki tou Neroupha	CY
Lazies tou Venetou	CY
Zonias	CY
Misiskopos	CY
Shinomoutti	CY
Adhkiajera	CY
Prophali	CY
Aetomoutti	CY
Vatoudhi	CY
Ayia Marina	CY
Ayia Marina	CY
Vyzakeri	CY
Ayia Marina	CY
Mandres	CY
Plaja	CY
Larkona	CY
Ayios Yeoryios	CY
Kambos tou Kkeli	CY
Vrysi	CY
Karis	CY
Ayios Ioannis	CY
Karis	CY
Lymbia Dam	CY
Kalamoulia	CY
Misiskopos	CY
Ayia Marina	CY
Aspropamboulos	CY
Ayios Konstantinos and Ayia Eleni	CY
Kaledhra	CY
Kokkinomoutti	CY
Trikormi	CY
Karakonas	CY
Armyras	CY
Ammos	CY
Ayia Varvara	CY
Trines	CY
Mouttarka	CY
Argaki tis Vasilous	CY
Argaki tou Ayiou Iona	CY
Panayia Khrysogalatousa	CY
Aspromoutti	CY
Platyvounia	CY
Kambos tou Mylou	CY
Asprokremmos	CY
Leska	CY
Argaki ton Yerakion	CY
Yerakes	CY
Koutsos	CY
Arkhangelos Arkhistratigos	CY
Koursos	CY
Pano Trakhonas	CY
Kourvellos	CY
Argaki ton Villourkon	CY
Argaki tou Vernan Bagche	CY
Lythrodhondas Dam	CY
Vylos	CY
Teredhia	CY
Argaki tis Telendis	CY
Argaki ton Pliktron	CY
Karamanitis	CY
Arkhangelos	CY
Psoumnidhes	CY
Ayios Dhimitrios	CY
Tyrannos	CY
Tsopanidhes	CY
Panayia Khrysogalatousa	CY
Arkolioudhia	CY
Argaki tou Syrkanou	CY
Kouris Dam	CY
Tarratsos	CY
Pamboula	CY
Loura	CY
Paliokhoraphkia	CY
Manjari	CY
Panayia Khryseleousa	CY
Panayia Galousa	CY
Podhimata	CY
Vathias	CY
Limassol Castle	CY
Chiftlikoudhkia	CY
Ayios Eraklidhios	CY
Ayios Yeoryios	CY
Berengaria Village	CY
Kondovouna	CY
Aloupi	CY
Ayios Nikolaos	CY
Kalenji Vouno	CY
Ayios Syllas	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Marsianos	CY
Ayios Viskanos	CY
Poupoutsis	CY
Ayios Efstathios	CY
Ayia Marina	CY
Shisti	CY
Ayios Antonios	CY
Kolossi Castle	CY
Ayios Yeoryios	CY
Stavros	CY
Phourremis	CY
Kaminia	CY
Petralona	CY
Pamboula	CY
Ayia Marina	CY
Panayia Khrysanayiotissa	CY
Kandou Dam	CY
Moundanous	CY
Kafkarotopia	CY
Lastringas	CY
Ayios Athanasios	CY
Khtista	CY
Gonies	CY
Ayios Yeoryios	CY
Kliousha	CY
Ayios Yeoryios	CY
Laonari	CY
Kokkatika	CY
Ayios Dhimitrianos	CY
Flourentzou	CY
Vournes	CY
Mikhalo	CY
Kamares	CY
Phajiola	CY
Anoacha	CY
Poulia	CY
Kholetres	CY
Ayios Yeoryios	CY
Koryphi	CY
Kokkinoyia	CY
Yermasoyia Dam	CY
Prosphoron	CY
Sphalangiotissa Monastery	CY
Pladhikia	CY
Mesovounaros	CY
Kourvoulas	CY
Polemidhia Forest	CY
Polemidhia Camp	CY
Stazousa	CY
Xi-Vouno	CY
Ayios Dhimitrianos	CY
Kommorotsos	CY
Polemidhia Dam	CY
Ayios Tithikos	CY
Mersinion	CY
Koundouros	CY
Argaki tou Dhamnous	CY
Panayia Evangelistria	CY
Arkhangelos	CY
Karmi Monastery	CY
Khortovounaron	CY
Potamos	CY
Ayia Irini	CY
Ayios Yeoryios tou Vikliou	CY
Kapsala	CY
Khortovouno	CY
Khalospita	CY
Laxia tou Stephani	CY
Sykarin	CY
Jiladha tou Anjeli	CY
Kokkinokolymbos	CY
Paliolania	CY
Soularavia	CY
Ayios Mamas	CY
Vounaros	CY
Kapparovounos	CY
Dhrakondopetra	CY
Kamenos Stavros	CY
Palialonon	CY
Dhiplojilaon	CY
Kanjellin	CY
Kalon Teratsoudhin	CY
Sikouestia	CY
Souni	CY
Vounon	CY
Kochinoudhkia	CY
Zanaja	CY
Ayios Salloumis	CY
Kafkalla	CY
Arkolies	CY
Kaji Skala	CY
Khistron Vounon	CY
Vathyronas	CY
Korajes	CY
Rophesia	CY
Ayii Akindhyn	CY
Karvouneri	CY
Sterakovou	CY
Ayios Efstathios	CY
Yiannou	CY
Panayia Perakhoritissa	CY
Mandra tou Lakhoou	CY
Perdhika	CY
Alonia	CY
Ayios Yeoryios	CY
Kliouphas	CY
Potamos tou Paramaliou	CY
Teratsotos	CY
Teratseros	CY
Kafkalla	CY
Panayia Dhkiakinousa	CY
Kyparissovouno	CY
Dhiplarkaka	CY
Vathyn Jiladhin	CY
Louristi	CY
Ayios Therapon	CY
Arkhangelos	CY
Kochini	CY
Ayia Eleni	CY
Kato Tsendedhes	CY
Xerolivadhia	CY
Kannies	CY
Ayios Kassianos	CY
Vournes	CY
Ayii Anaryiri	CY
Kokkines	CY
Aspri Moutti	CY
Ayios Eraklidhios	CY
Lachi	CY
Styllos	CY
Ayios Yeoryios Kephalos	CY
Mandres tou Mikhail	CY
Ayia Marina	CY
Ayios Yeoryios	CY
Kolymbos	CY
Asprous	CY
Ayia Mavri	CY
Paphitis	CY
Petromoutti	CY
Elia Koutsoulla	CY
Vikla	CY
Korakas	CY
Ayios Yeoryios Monastery	CY
Oupia	CY
Yerondas	CY
Ayii Anaryiri	CY
Ayios Epiphanios	CY
Moni Cement Works	CY
Ayia Marina	CY
Moni Power Station	CY
Argaki tou Pyrgou	CY
Oxies	CY
Yiannitsaros	CY
Ayia Varvara	CY
Kombinarka	CY
Timios Stavros	CY
Kafkallon tous Kremmous	CY
Kastros	CY
Ayios Tykhonas	CY
Boosura	CY
Mavrorotsos	CY
Melanda	CY
Konizera	CY
Ayios Mavrikios	CY
Strongylolaonon	CY
Kochina	CY
Kourvoulies	CY
Mandri	CY
Pelavi	CY
Kaloyenata Chiftlik	CY
Trikolo	CY
Pikrokremmos	CY
Koshes	CY
Apothikes	CY
Manolykos	CY
Anoyia	CY
Angathopetra	CY
Mersinoudhia	CY
Taizokremmos	CY
Kremmos tis Arkolias	CY
Kochinokremmos	CY
Khanolajes	CY
Trakhona	CY
Apostolos Andreas	CY
Ano Pedhkia	CY
Argaki tous Mayirous	CY
Nisiri	CY
Prophitis Elias Monastery	CY
Kyparisha	CY
Ayios Yeoryios	CY
Arka	CY
Argaki tis Shyllas	CY
Jinarota	CY
Limistiko	CY
Mazotos	CY
Lakkos Hussein Aga	CY
Trapezoni	CY
Kiokhlajeri	CY
Mosilovouno	CY
Phoraomandres	CY
Ayia Eleni	CY
Aspros	CY
Haji Alis	CY
Skotini	CY
Elia tou Lykou	CY
Palialono	CY
Laoni Kotsiri	CY
Rotsi	CY
Phrakti	CY
Mandra tou Khailou	CY
White Promontory	CY
Pangera	CY
Ayios Yeoryios	CY
Lingrin tou Dhyeni	CY
Randi Forest	CY
Dhasha	CY
Kapsalia	CY
Rakhokambos	CY
Moutti tou Pyrgou	CY
Sina Monastery	CY
Kochinadhkia	CY
Pigadhoulia	CY
Kouklia Chiftlik	CY
Xerojiladha	CY
Sakki	CY
Timios Stavros Monastery	CY
Kiladhia	CY
Mandri	CY
Tepeler	CY
Ayios Yeoryios	CY
Pokaties	CY
Jilain tou Kouloukoundi	CY
Ayia Varvara	CY
Arkosyja	CY
Stalies	CY
Varvatses	CY
Melisha	CY
Laona	CY
Trakhonas	CY
Ayios Kasojanos	CY
Teratsos	CY
Asprovounaros	CY
Moutti tis Shinias	CY
Lithovouno	CY
Ayii Pateres Hermitage	CY
Ayios Theodhosios	CY
Karpasas	CY
Ayios Yeoryios	CY
Kato Kochinokambos	CY
Mettin	CY
Paliostiadha	CY
Maokremmos	CY
Kochinokremmos	CY
Kochinoumini	CY
Glykomersino	CY
Oritis Forest	CY
Pano Papeti	CY
Moutti tis Arkolias	CY
Phatalas	CY
Teratsos	CY
Melanoudhkia	CY
Noukkoutoues	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Sakkoulia	CY
Kato Papeti	CY
Ayios Neophytos Old Hermitage	CY
Ayios Konstantinos	CY
Ayios Kournoutas	CY
Phonnes	CY
Lakkomata tous Kato Kremmous	CY
Loures	CY
Yiakoupis	CY
Panosavanja	CY
Kochinokambos	CY
Kremmistra	CY
Naftes	CY
Mouttokamara	CY
Jyra Eleni	CY
Royal Manor of Covocle	CY
Panayia Katholiki	CY
Ayia Marina	CY
Ayios Ioannis	CY
Kato Savanja	CY
Mersinoudhkia	CY
Tilaa	CY
Skotini	CY
Lambrodhori	CY
Asprokremmos Dam	CY
Ayios Minas	CY
Petra	CY
Kremmos	CY
Chiftlikin ton Lizaton	CY
Ayios Yeoryios	CY
Elioudhkia	CY
Panayia Iamatiki	CY
Ayios Onisiphoros	CY
Ayios Epiphanios	CY
Alonia	CY
Ayia Arkona	CY
Ayia Arkona	CY
Tromarkhis	CY
Arodhamnoua	CY
Koukkoullies	CY
Eidhoros	CY
Ayios Yeoryios	CY
Morokambos	CY
Atsoupopetra	CY
Phinijin	CY
Ayia Lavra	CY
Akhelia Chiftlik	CY
Ammovounos	CY
Ayios Leondios	CY
E. E. Base	CY
Vloueroues	CY
Ayios Polykhronios	CY
Ayios Theodhoros	CY
Ayios Kharalambas	CY
Metokhion Ayiou Neophytou	CY
Trimithkies tou Mizikou	CY
W. E. Base	CY
Chouvlijin tis Yermaninas	CY
Kaminia	CY
Maritena	CY
Teratsotos	CY
Pambakaes	CY
Vounos	CY
Moutti	CY
Kambos tou Papa	CY
Epilas	CY
Arkosykia	CY
Grigoridhes	CY
Ornithas	CY
Skliroudhia	CY
Pamboulos	CY
Krya Pigadhia	CY
Kokkinovounos	CY
Laxia tou Rokka	CY
Mazovounos	CY
Petrovounos	CY
Ayia Marina	CY
Eliotos	CY
Dhespotika	CY
Kakoskalin	CY
Kapsalia	CY
Kyparisha	CY
Aspous	CY
Yialiadhes	CY
Ayios Loukas	CY
Pano Teratsotos	CY
Trimithos	CY
Sosoma	CY
Kathara	CY
Argaki tou Venrto	CY
Krommyos	CY
Makris Pefkos	CY
Kali Teracha	CY
Akrounda Dam	CY
Ayios Yeoryios	CY
Koronos	CY
Trapeza ton Aeton	CY
Petralona	CY
Strongylos	CY
Stenomata	CY
Amyrou	CY
Kastron Ayiou Ioannou	CY
Zeus Lavranios Sanctuary	CY
Ayios Riginos	CY
Panayia Khryseleousa	CY
Koumana	CY
Pisseros	CY
Ayios Yeoryios	CY
Selemendies	CY
Ayios Epiphanios	CY
Ayia Anna	CY
Pernoti	CY
Asvetero	CY
Sotiros	CY
Megas	CY
New Kokkini	CY
Kourellos	CY
Vasilikos	CY
Karnizousa	CY
Panayia Theotokos	CY
Akapnou Forest	CY
Exovounia	CY
Ayios Dhimitrios	CY
Photinoues	CY
Panayia	CY
Moutti tou Aetou	CY
Ayios Ioannis	CY
Pefkarin Meron	CY
Moutti tous Antonies	CY
Panayia tou Glossa	CY
Phloudhia	CY
Ayios Nikolaos	CY
Panayia tou Kambou	CY
Petrovouno	CY
Ayios Yeoryios	CY
Koryphi	CY
Kapsalom	CY
Ayios Photios	CY
Ayios Yeoryios	CY
Throumbia	CY
Panayia	CY
Kambos	CY
Katta	CY
Platyn Oros	CY
Petromoutti	CY
Arkhangelos Mikhail	CY
Zonias	CY
Koshina	CY
Kandara	CY
Panayia Iamatiki	CY
Prophitis Elias	CY
Porta	CY
Kaminaki	CY
Peraoros	CY
Ayios Photios	CY
Argaki tis Kaloshis	CY
Argaki tis Prodhromitissas	CY
Paleomandra	CY
Argaki tis Xerokiladhas	CY
Argaki ton Klimaton	CY
Eloros	CY
Kyra	CY
Khones	CY
Ayios Mamas	CY
Ayios Yeoryios	CY
Koutsangokhoma	CY
Panayia Khrysogalatousa	CY
Petromoutti	CY
Ayia Paraskevi	CY
Throumbos	CY
Argaki tou Mavrou	CY
Moutti tou Dhios	CY
Arkoliromoutti	CY
Evgenou	CY
Koleas	CY
Khandaja	CY
Kokkinomoutti	CY
Panayia	CY
Stavromenos	CY
Kapparka	CY
Peristerokremmos	CY
Moutti tou Atou	CY
Potamos tou Ayiou Mina	CY
Moutti tou Gliori	CY
Ayia Marina	CY
Ayios Andronikos	CY
Ayii Anargyri	CY
Pharatsena	CY
Makrya Laona	CY
Moutti tou Zonarkou	CY
Komokorona	CY
Panayia Khryseleousa	CY
Vrysoudhia	CY
Ayia Marina	CY
Moutti tou Ayiou Yeoryiou	CY
Appimata	CY
Mallouri	CY
Stavropefkos	CY
Petromoutti	CY
Strongylos	CY
Mesovounos	CY
Ayios Dhimitrios	CY
Lambiris	CY
Karydhaki	CY
Prophitis Elias	CY
Stavros	CY
Laona	CY
Aetophoulies	CY
Argaki tis Papoutsas	CY
Mouskappis	CY
Ayiopetra	CY
Panayia	CY
Vateri	CY
Aphendikon	CY
Ayia Marina	CY
Anedhres	CY
Korphi	CY
Vournes	CY
Panayia Kivotos	CY
Listis	CY
Melissos	CY
Arkhangelos Mikhail	CY
Ayia Marina	CY
Lakkoudhia	CY
Mosphilovouno	CY
Zoodhokhos	CY
Kambia tis Terachas	CY
Moutti tis Athashas	CY
Prosepkhi	CY
Ayios Venetos	CY
Moutti tou Pelekanou	CY
Dhyo Mouttes	CY
Kakarmasa	CY
Makheras Forest	CY
Yialias	CY
Moutti tou Skasmenou	CY
Argaki tis Mandkas tou Kambiou	CY
Moutti tou Touriou	CY
Kionia	CY
Moutti tous Troullous	CY
Romios	CY
Kionia	CY
Moutti tou Kharaka	CY
Moutti tou Kousoulou	CY
Kimistron	CY
Moutti tous Shillinidhes	CY
Pertelezis	CY
Jissis	CY
Ayios Mamas	CY
Pharniakis	CY
Ayios Yeoryios	CY
Ayia Irini	CY
Megali Moutti	CY
Kambi	CY
Apliki	CY
Ayios Riginos	CY
Moutti tis Laipris	CY
Katranatola	CY
Alakoti	CY
Panayia Khrysopandanassa	CY
Metamorphosis tou Sotiros	CY
Ayii Anargyri	CY
Marines	CY
Palekhori	CY
Dhrepani	CY
Moutti tou Kholetrotou	CY
Platania	CY
Ayios Ioannis Vaptistis	CY
Timios Stavros	CY
Askas	CY
Ayia Khristina	CY
Khainatos	CY
Pitsilia	CY
Mavron Oros	CY
Kephalovryson	CY
Moutti tis Kolokas	CY
Alourotrypes	CY
Moni	CY
Ayios Yeoryios	CY
Vounarin	CY
Lephita	CY
Platys	CY
Panayia Kardhakiotissa	CY
Ayios Mamas	CY
Ayia Anna	CY
Prodhromos	CY
Lajeri	CY
Apostolos Andreas	CY
Ayios Elias	CY
Phteriki	CY
Mandra tou Kambiou	CY
Epsilos	CY
Argaki tou Ayiou Onouphriou	CY
Moutti tou Ioannari	CY
Karseoulia	CY
Argaki tis Kokkinovrysis	CY
Argaki tou Dhrakondospiliou	CY
Mazovounos	CY
Skortokephalos	CY
Ayios Onouphrios	CY
Argaki tou Kouphou	CY
Ayia Marina	CY
Klimatoudhin	CY
Stenokambia	CY
Agniotissa	CY
Petalloudhin	CY
Moutti tou Klimatou	CY
Ayii Apostoli	CY
Kamouyiaros	CY
Pyrgos	CY
Ayia Paraskevi	CY
Moutti Ayiasmatos	CY
Landa	CY
Ayios Yeoryios	CY
Vathyrgakas	CY
Kalokhorio Dam	CY
Rotsos	CY
Askopelos Forest	CY
Petritidhes	CY
Ayios Dhimitrios	CY
Ayios Nikolaos	CY
Ayia Koroni	CY
Ayios Mamas	CY
Palloura Forest	CY
Lakkoti	CY
Khalaris	CY
Moutti Seleas	CY
Argaki tis Monis	CY
Ayios Yeoryios	CY
Irgakas	CY
Ayios Yeoryios	CY
Moutti tou Aorati	CY
Arkhangelos	CY
Ayios Efstathios	CY
Khrysomilia	CY
Phterkhoudhi	CY
Moutti tis Shellotis	CY
Arodhaphnin	CY
Pophoullin	CY
Moutti tou Karoudhiou	CY
Vrysi tou Kasinou	CY
Stavros tou Ayiasmati	CY
Marathos	CY
Ayios Yeoryios	CY
Kourtellorotsos	CY
Moutti tou Katanappi	CY
Ayios Yeoryios	CY
Panayia tou Araka Monastery	CY
Ayios Ioannis	CY
Vounaros	CY
Kaminoudhia	CY
Argaki tis Yerokaminas	CY
Kyra	CY
Jilaos	CY
Kalokhi	CY
Theri	CY
Vrysi tou Korsou	CY
Plevra	CY
Vounarka	CY
Mandaras	CY
Kiladhes	CY
Vatin	CY
Koulounois	CY
Karvounaris	CY
Khasanos	CY
Amaladhes	CY
Vrysia	CY
Ayios Elias	CY
Ayios Yeoryios	CY
Arkolia	CY
Argaki tou Theodhoulou	CY
Stites	CY
Laonin tis Angastromenis	CY
Mavrijin	CY
Mavros	CY
Jileros	CY
Ays Sieros	CY
Phasinarka	CY
Strakota	CY
Appies	CY
Trapeza	CY
Kouseis	CY
Panayia Khrysospiliotissa	CY
Vryses	CY
Koloni	CY
Mersinia	CY
Timios Stavros	CY
Kavaja	CY
Moutti	CY
Ayia Elisavet	CY
Mouttallos	CY
Akrathkes	CY
Ayios Stephanos	CY
Amalaes	CY
Pilladhes	CY
Ayia Marina	CY
Vromenera	CY
Vromonero	CY
Plystres	CY
Vlou	CY
Meyremis	CY
Rakhasin	CY
Platania	CY
Mosphileri	CY
Kopiambeis	CY
Karin	CY
Pernies	CY
Ayios Yeoryios	CY
Panayia Khamachotissa	CY
Panayia Ambelikiotissa	CY
Potamos tou Limnati	CY
Kapnistos	CY
Alata	CY
Ayios Elias	CY
Ayia Marina	CY
Sterajidhkia	CY
Ayios Yeoryios	CY
Ayia	CY
Teratsoudhkia	CY
Mandonena	CY
Vounos	CY
Ayios Pandeleimon	CY
Ayios Yeoryios	CY
Lephtos	CY
Panayia Amaskon Monastery	CY
Ayios Yeoryios	CY
Panayia tis Pano Ekklishas	CY
Prophitis Elias	CY
Arkhangelos Mikhail Monastery	CY
Ayii Anargyri	CY
Mesolaona	CY
Kapparetis	CY
Plevra	CY
Avloitos	CY
Amolaes	CY
Ayios Yeoryios	CY
Ayia Varvara	CY
Ayios Yeoryios	CY
Korajes	CY
Vrysin	CY
Vathyronas	CY
Ayios Mamas	CY
Ayios Theodhoros	CY
Mersinoudhia	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Panayia	CY
Kremmistra	CY
Flites	CY
Konia	CY
Ayios Mnason	CY
Karydhajin	CY
Tophoulia	CY
Ayios Theodhoros	CY
Kolymbos	CY
Malletu	CY
Kolives	CY
Mosphilovounos	CY
Vasilakena	CY
Tyropios	CY
Kourvellos	CY
Mandra tou Tavrou	CY
Kaloyiorki	CY
Leles	CY
Platanoudhkia	CY
Koronias	CY
Varliyn	CY
Palaskas	CY
Manolies	CY
Ambelikos	CY
Xylourikos	CY
Amblistra	CY
Limassol Forest	CY
Ayios Mamas Forest	CY
Troumithos	CY
Mouskato	CY
Roathis	CY
Ayios Yeoryios	CY
Livadhkies	CY
Moni Forest	CY
Panayia Tou Valana	CY
Simonis	CY
Laonara	CY
Mersilies	CY
Ayiasman	CY
Trimiklini Dam	CY
Panayia Saittiotissa	CY
Arniadhes	CY
Ayios Mnason	CY
Lisas	CY
Saittas Agricultural Station	CY
Kato Ambelia	CY
Ayia Marina	CY
Laona	CY
Shilia	CY
Timios Stavros	CY
Vrysha	CY
Xilimbos	CY
Kharja	CY
Ayia Mavri	CY
Panayia	CY
Phterikoudhkia	CY
Kouphallis	CY
Ayios Ioannis	CY
Perapedhi Forest	CY
Perapedhi Dam	CY
Ayia Paraskevi	CY
Phaneromeni	CY
Katsourina	CY
Moutti tou Aphami	CY
Ayios Elias	CY
Ayios Yeoryios	CY
Korphi	CY
Atophoulies	CY
Kapsalia	CY
Ayios Spyridhon	CY
Ayia Paraskevi	CY
Kokkinovouno	CY
Koundourolaona	CY
Psysika	CY
Timios Stavros Monastery	CY
Apostolos Phillippos	CY
Shakoti	CY
Ayios Yeoryios	CY
Kremmos tis Laonas	CY
Phraktadhes	CY
Koshinas	CY
Grystallin	CY
Panayia Khryseleousa	CY
Arkhangelos	CY
Jephalas	CY
Platania	CY
Apostoli Dhodheka	CY
Panayia Khrysomilousa	CY
Yeraja	CY
Anemomylos	CY
Mitsarkajin	CY
Kolokasin	CY
Kourkoutas	CY
Vathkia	CY
Timios Stavros	CY
Khardhakhi	CY
Pelendria Forest	CY
Mondessi Forest	CY
Laonin tis Englisis	CY
Psilon	CY
Arkyrou	CY
Kakogyros Forest	CY
Kardama Forest	CY
Ayios Ioannis Prodhromos Monastery	CY
Kremmos tis Jeramis	CY
Poullaris	CY
Mouttalios	CY
Arkolakhania	CY
Loumata	CY
Almyrolivadhon	CY
Skaphes	CY
Psilo Dhendro	CY
South Shoulder	CY
Yerokeminia	CY
Panayia Samajotissa	CY
Moni tou Pellou	CY
Argaki ton Livadimidhion	CY
Argaki tou Xerokolymbou	CY
Kremmos tou Louvarkou	CY
Sendouka	CY
Psilo Dhendhro Trout Farm	CY
Kaminoudhi	CY
Trypa tou Kaourou	CY
Ayii Anargyri	CY
Ayios Yeoryios	CY
Ayia Marina	CY
Ambelijin	CY
Lakeras	CY
Ayios Yeoryios	CY
Lophos	CY
Moutti tou Tremithou	CY
Elea Bridge	CY
Roskolos	CY
Atophoulia	CY
Panayia	CY
Ayios Ermolaos	CY
Chattalotos	CY
Ayios Dhimitrios	CY
Argaki tou Kalamo	CY
Argaki ton Tsourkou	CY
Dhrakohda	CY
Melissin	CY
Katsari	CY
Korphi	CY
Timios Stavros	CY
Ayia Marina	CY
Madhari	CY
Moutti tou Dhia	CY
Moutti ton Spilion	CY
Phournia	CY
Ayios Yeoryios	CY
Karvounas	CY
Argaki tis Glias	CY
Livadhi tou Ppasha	CY
Moutti ton Mnimaton tous Episkopous	CY
Skotini	CY
Argaki ton Piteron	CY
Argaki ton Pikromiloudhion	CY
Kannoures	CY
Argaki tis Phterietis	CY
Koutsoulla	CY
Argaki tou Petkou	CY
Moutti tou Zoumi	CY
Palea Khora Fortified Settlement	CY
Troodos TV Tower	CY
Argaki tis Platana	CY
Prodhromos Reservoir	CY
North Shoulder	CY
Timios Stavros	CY
Koukkos	CY
Arkhangelos Mikhail	CY
Ayia Paraskevi	CY
Ayios Nikolaos	CY
Panayia Iamatik	CY
Ayios Theodhoros	CY
Letridhes	CY
Moutti tou Dhrakonda	CY
Ayios Vasilios	CY
Ayios Elias	CY
Ayios Yeoryios	CY
Ayios Avvakoum	CY
Moutti tou Angathiou	CY
Ayia Arka	CY
Tripodhin	CY
Ayios Yeoryios	CY
Laxia tou Leou	CY
Ayios Dhimitrios	CY
Timios Stavros	CY
Straoroudhia	CY
Ayios Nikolaos	CY
Panayia Khrysokourdhaliotissa	CY
Pervolia	CY
Moutti tis Kambanas	CY
Argaki tou Karvouna	CY
Staouties	CY
Esso Galata	CY
Vounaros	CY
Ayia Paraskevi	CY
Ayios Nikolaos	CY
Virviros	CY
Laxies ton Kalianon	CY
Ayios Konstantinos	CY
Sotiros	CY
Prophitis Elias	CY
Ayios Andronikos	CY
Ayios Ioannis Lambadhistis Monastery	CY
Panayia Theoskepasti	CY
Marathasa	CY
Panayia	CY
Arkhanglos Mikhail	CY
Kamaroudhin	CY
Argaki ton Korishon	CY
Pyknopidhia	CY
Ayia Triada	CY
Mouzolios	CY
Ayia Trias	CY
Ayia Trias	CY
Argaki tis Sendoukas	CY
Argaki tou Pyrki	CY
Mavroyeni	CY
Ayios Vasilios	CY
Astrophengia	CY
Verki	CY
Mesovounia	CY
Koutsopetri	CY
Vounarka	CY
PylaTomb	CY
Aspromoutti	CY
Platy	CY
Pyla Watch Tower	CY
Dhyospilio	CY
Troulli	CY
Alasso	CY
Kambos	CY
Stazousa	CY
Ayios Antonios	CY
Prophitis Elias	CY
Almyros	CY
Piayina	CY
Mavropetra	CY
Yerakomoutti	CY
Ayios Thomas	CY
Koliokremmos	CY
Ayios Yeoryios	CY
Troullia	CY
Plakas	CY
Kochina	CY
Ayios Neophytos	CY
Ayios Mamas	CY
Kouphomandra	CY
Asprovounaro	CY
Gonos	CY
Ailikos	CY
Xystarka	CY
Elias	CY
Marathovouno	CY
Laxia tou Paphiti	CY
Latomes	CY
Troulli	CY
Ayios Yeoryios	CY
Tepeler	CY
Tremithous City-Site	CY
Ayios Spyridhon Monastery	CY
Dokiountoulour	CY
Aloupotrypes	CY
Vounos	CY
Kapsalia	CY
Lachi	CY
Golgi City-Site	CY
Petra Stiti	CY
Ayios Photios	CY
Drakonda	CY
Lachin	CY
Karavous	CY
Kakoskala	CY
Aspromouttes	CY
Kremmos Khalasmenos	CY
Phourni	CY
Ayios Ioannis	CY
Porti	CY
Kondokoli	CY
Glypha	CY
Panayia	CY
Malos	CY
Ypsaros	CY
Photines	CY
Pallourokambos	CY
Lefkaromoutti	CY
Arkangelos Monastery	CY
Endealo	CY
Vouyes	CY
Mesovounos	CY
Malloura	CY
Kryon Neron	CY
Loura	CY
Kamini tou Makhera	CY
Zanettis House	CY
Koukkounis Settlement	CY
Yerakia	CY
Aspradhia	CY
Kafkalla tou Pyroiou	CY
Rocha	CY
Ayios Epiphanios	CY
Mesovouni	CY
Potamos tou Kremmou	CY
Ayios Yeoryios	CY
Ayios Mamas	CY
Vathkia Laxia	CY
Paliovrysi	CY
Orkos	CY
Ayios Synesios	CY
Ayios Yeoryios	CY
Ailasyka	CY
Ayios Evphimianos	CY
Vikles	CY
Pallourokambos	CY
Laxidhia	CY
Palioprastio	CY
Kondea Agricultural Station	CY
Koutsopetrika	CY
Niolita	CY
Omerika	CY
Laxia	CY
Kamini tou Pitsillou	CY
Dhrosisti	CY
Kochines	CY
Landa Makrya	CY
Temeller	CY
Kirmizi Kirach	CY
Timar	CY
Ayios Andronikos	CY
Sigouris Castle	CY
Panayia	CY
Margarita	CY
Lysi Yolu	CY
Dhyo Kopries	CY
Memier	CY
Jorjidhes	CY
Marakhoussi	CY
Sinda City-Site	CY
Chayiri	CY
Toumbalar	CY
Gyros	CY
Petrodhimmata	CY
Ayios Savvas	CY
Direklimara	CY
Niokhorkous	CY
Oxydena	CY
Argaki tis Apalestras	CY
Lortines	CY
Buyuk Toumba	CY
Apalestra Chiftlik	CY
Panayia	CY
Merradhes	CY
Arkaji	CY
Simnis	CY
Argaki tou Papa	CY
Kopris	CY
Koutoullis	CY
Loizies	CY
Ayios Yeoryios	CY
Mazeri	CY
Kharangas	CY
Andronikous	CY
Bekirou	CY
Ammoudhies	CY
Ayia Jinousa	CY
Paliorizia	CY
Ekatonskales	CY
Koundouros	CY
Koutsosheras	CY
Katselli	CY
Kharangas	CY
Kior Dere	CY
Ayios Mamas	CY
Katsara	CY
Vorka	CY
Khapipies	CY
Kambos	CY
Dhyopallouron	CY
Pannellopetra	CY
Gyros	CY
Krambenos	CY
Merras tou Strongylou	CY
Mazero	CY
Arkhangelos	CY
Havara Pit	CY
Triporti	CY
Spiliari	CY
Beyaz Tepe	CY
Zangalaki	CY
Arsiotika	CY
Orta Burun	CY
Katalymata	CY
Aphanies	CY
Kokkines	CY
Felekkidhes	CY
Pamboula	CY
Pikrolakkos	CY
Ornithi Chiftlik	CY
Ayios Theodhoros	CY
Panayia Theotokos	CY
Shilopotamo	CY
Mania Chiftlik	CY
Varkes	CY
Pharakkos	CY
Kamares	CY
Kamares Settlement	CY
Merika	CY
Armyros	CY
Trakhonas	CY
Aloupotrypes	CY
Kamilostrata	CY
Agli Dere	CY
Shukurluk	CY
Veranlik	CY
Sepi Dagh	CY
Meriko	CY
Lissinos	CY
Atlija Tepesi	CY
Ayii Saranda	CY
Kirklar Tekke	CY
Ayios Elias	CY
Ayia Marina	CY
Tymbou Airstrip	CY
Kumluk	CY
Fella Eshei	CY
Ali Efendi	CY
Chatal Tepesi	CY
Morkatika	CY
Pitsillo Yolu	CY
Loukkos	CY
Ayia Marina	CY
Haji Hasan	CY
Rothesia	CY
Ayios Yeoryios	CY
Panayia ton Pedhon	CY
Syka	CY
Konnolik	CY
Azghanli Dere	CY
Koumlou Dere	CY
Osman Dere	CY
Buyuk Kaya	CY
Piloudhes	CY
Argaki tis Kamaras	CY
Katouris	CY
Tamassos City-Site	CY
Ayios Iraklidhios	CY
Ayios Yeoryios	CY
Ayios Mnason Monastery	CY
Ayios Nikolaos	CY
Ammoyia	CY
Marmaras	CY
Mavroyi	CY
Kharangas	CY
Vlokkarka	CY
Koundouri	CY
Kokkinia	CY
Pendalia	CY
Koupparis	CY
Kokkinokremmos	CY
Konnidhes	CY
Melissotrypa	CY
Kochines	CY
Kambos tou Skotomenou	CY
Kara Kouseis	CY
Kokkala	CY
Ttouloumbes	CY
Prastio Chiftlik	CY
Panayia Khryseleousa	CY
Mouttes	CY
Paleoyephyron	CY
Aspros	CY
Ayios Vasilios	CY
Spitoudhia	CY
Armyridhia	CY
Mazeres	CY
Pezemata	CY
Kommorotsos	CY
Moutti tou Yeroyianni	CY
Ayios Yeoryios	CY
Jiliaries	CY
Plakostrotos	CY
Neokhorio Chiftlik	CY
Panayia Theotokos	CY
Arkhangelos Mikhail	CY
Phtanokafkalla	CY
Mandra tou Hajarkyrou	CY
Pikrokambos	CY
Vournia	CY
Panayia Khrysogalousa	CY
Ayii Anargyri	CY
Ayios Yeoryios	CY
Koutis	CY
Kamares	CY
Katouris	CY
Kyra tou Dhiakou	CY
Petosopoulis	CY
Angatheri	CY
Khrysovounos	CY
Laxia tou Ayiou Yeoryiou	CY
Vyzaja	CY
Koraka	CY
Rothesi	CY
Ayios Ioannis Prodhromos	CY
Koutsoulla	CY
Vamvakies	CY
Koladhes	CY
Panayia	CY
Ayios Yeoryios	CY
Khariis	CY
Palialona	CY
Peratis	CY
Panayia Khrysopandanassa	CY
Ayios Ioannis	CY
Kallikas	CY
Kouphos	CY
Aloupos	CY
Panayia Evangelistria	CY
Mavridhes	CY
Philippidhes	CY
Koutis	CY
Argaki tis Myrenas	CY
Ayios Mamas	CY
Phtelekha	CY
Panayia Khryseleousa	CY
Vournia	CY
Dhiplopotamon	CY
Ayios Yeoryios	CY
Kompetra Forest	CY
Komopetras	CY
Mathiatis	CY
Mazerka	CY
Kokkinoyia	CY
Kolokia	CY
Kryon Neron	CY
Mitsero Mine	CY
Ayia Varvara	CY
Arkhangelos Mikhail	CY
Panayia Khrysopandanassa	CY
Kreatos	CY
Lambodhousa	CY
Ayios Pandeleimon Monastery	CY
Ayios Kournoutas	CY
Kaki Skala	CY
Kourou Keuy	CY
Kandaros	CY
Koliokremmos	CY
Korivas	CY
Ayios Pandeleimon	CY
Ayia Marina	CY
Mouttokoundouron	CY
Vrysin tou Pierou	CY
Epsilos	CY
Likythia	CY
Alakatin	CY
Archisporos	CY
Ayios Kyprianos	CY
Kato Pyrgos	CY
Mouttes tous Aetous	CY
Laphazanidhes	CY
Lefkovounas	CY
Sotiros	CY
Ayia Varvara	CY
Panayia Khryseleousa	CY
Anephani	CY
Niokhoritika	CY
Kakkarista	CY
Trakhonistea	CY
Merika	CY
Ayios Kharalambos	CY
Ayios Romanos tis Kafkallas	CY
Rophos	CY
Lephji	CY
Arodhaphnin	CY
Palea Ekklisha	CY
Kapsalia	CY
Troullin tou Hajivasili	CY
Skylloura	CY
Liastres	CY
Koundouros	CY
Phournia	CY
Kamos	CY
Piphanies	CY
Khandaja	CY
Marina	CY
Ayios Ioannis	CY
Skylloura	CY
Ayia Marina	CY
Xylomodhousa	CY
Pyrgos	CY
Gypsia	CY
Prophitis Elias Monastery	CY
Ayios Mamas	CY
Prophitis Elias	CY
Lapatsa	CY
Angoulos	CY
Pano Korajissa	CY
Ayios Yeoryios	CY
Khalandra	CY
Ayios Yeoryios Righatos	CY
Messaron	CY
Lacha	CY
Sidheropetres	CY
Kaminia	CY
Kakoskala	CY
Kafkalla	CY
Papasavvas	CY
Philia Chiftlik	CY
Trakhonia	CY
Mazeri	CY
Ayios Antonios	CY
Zakerka	CY
Vyzajera	CY
Katsellos	CY
Ayios Yeoryios	CY
Ayia Marina	CY
Khiliomodhousa	CY
Marangos	CY
Vorines	CY
Ktirka	CY
Trakhonia	CY
Dhomes	CY
Kato Mazeri	CY
Ayios Varnavas and Ayios Ilarion	CY
Ayios Antonios	CY
Kapparka	CY
Kambos tis Mesis	CY
Litharomata	CY
Zaim Agha	CY
Vathia Laxia	CY
Ayios Loukas	CY
Ayios Nikolaos	CY
Koundourka	CY
Maoutsos	CY
Kambos tou Mylou	CY
Dhervishika	CY
Mazovouno	CY
Polykantho	CY
Ayia Marina	CY
Vounon tou Kavazi	CY
Metamorphosis tou Sotiros	CY
Ayii Eliophoti	CY
Ayios Antonios	CY
Phinikies	CY
Vasilis	CY
Kato Vounia	CY
Ayios Yeoryios	CY
Klouves	CY
Siphilos Chiftlik	CY
Palloura	CY
Ayios Nikolaos	CY
Ayios Nikolaos	CY
Moutti tis Aramis	CY
Mouttalia ton Yerakion	CY
Argaki tou Stavriniatou	CY
Argakas	CY
Khalospithkia	CY
Moutti tis Syllotis	CY
Rotseri	CY
Traoullophas	CY
Moutti tou Pelloyiannou	CY
Argaki tis Yerokaminas	CY
Aloupotrypes	CY
Kremmi tou Palevrou	CY
Moutti tou Kriniati	CY
Pallourin	CY
Kannavia	CY
Ayios Mamas	CY
Alestos	CY
Ayios Kyriakos	CY
Pano Kremasha	CY
Strongylovounaron	CY
Ayios Theodhoros	CY
Glyperka	CY
Xindous	CY
Ayia Ekaterini	CY
Arkhangelos Mikhail	CY
Vyzakia	CY
Ayios Andronikos	CY
Kassanios	CY
Potamitika	CY
Ayios Yeoryios	CY
Kambos tou Phesa	CY
Vathys	CY
Kapsalia	CY
Kochinokambos	CY
Kambos Megalos	CY
Kambos tou Kylindri	CY
Spilios tis Kouphis	CY
Anephani	CY
Laxia tis Ekklishas	CY
Dhromos tous Pitsyllous	CY
Pissaridhes	CY
Pissoyia	CY
Melissin	CY
Komitis	CY
Potami	CY
Dhromos tou Angolemi	CY
Pallouros	CY
Konni	CY
Pezounia	CY
Saranda Skales	CY
Dhromos tou Petrati	CY
Ayios Yeoryios	CY
Mesarkes	CY
Exindara	CY
Laxia	CY
Morphou Dam	CY
Kavallarka	CY
Katarraktis	CY
Kokkinoyia	CY
Kallinikos	CY
Ovgos Dam	CY
Kafkalla	CY
Ammoudhia	CY
Toumba tou Skourou Settlement	CY
Ammos	CY
Spilii	CY
Katsimata	CY
Kafkalla	CY
Plevra	CY
Xalona	CY
Vathykopia	CY
Mandres tou Jarti	CY
Mandres Aphendikes	CY
Petromandra	CY
Ammos	CY
Teratsoudhia	CY
Kalamidhia	CY
Stoma	CY
Pnasi	CY
Kalamionas	CY
Pnasi Monastery	CY
Aphendika	CY
Teraches	CY
Dhraomani	CY
Livadhia	CY
Ayios Mamas	CY
Eleoudhia	CY
Arnaoutidhes	CY
Paraji Monastery	CY
Xeropotamos	CY
Kakourtas	CY
Petra Stiti	CY
Laxia tou Piskali	CY
Mavri Palloura	CY
Asprovounas	CY
Vathia Laxia	CY
Plevra	CY
Mandra tou Gavrili	CY
Kouppi	CY
Hasan Kiayia	CY
Mazokambi	CY
Platies Vouches	CY
Kambos tou Aga	CY
Gadhouropniktis	CY
Koronia	CY
Troullin tis Dhaskalas	CY
Epta Skales	CY
Kamilaries	CY
Korphi	CY
Mouttalia	CY
Avroshilleri	CY
Stavros	CY
Panayia Phorviotissa	CY
Ayios Ioannis	CY
Petromoutti	CY
Argaki ton Potsoris	CY
Ayios Yeoryios	CY
Kapsalia	CY
Moutti ton Ambelion tou Paraskena	CY
Khandakia	CY
Kourdhai	CY
Kara ton Livadhion	CY
Kaminia tou Dhiakou	CY
Ayia Paraskevi	CY
Mavrokhoma	CY
Moutti ton Kaliphon	CY
Moutti tis Katholikis	CY
Karterouni	CY
Vrakhos	CY
Markos	CY
Ayios Seryios	CY
Asprokhomani	CY
Pallourka	CY
Kataramenos	CY
Ayios Yeoryios	CY
Ayios Anastasios	CY
Phoukasa	CY
Skouriotissa Monastery	CY
Lemonas	CY
Petra Dams	CY
Ayii Saranda	CY
Pano Peristeria	CY
Ayia Marina	CY
Panayia	CY
Stroumbou	CY
Lakhanis	CY
Ayios Theodhoros	CY
Ayios Loukas	CY
Ayios Kyriakos	CY
Shilleri	CY
Kylindhros	CY
Panayia Iamatiki	CY
Moutti tis Mosphileris	CY
Ayios Yeoryios	CY
Panayia Podhithou	CY
Akendou	CY
Theotokos	CY
Phroutsi	CY
Ayios Lazaros	CY
Pyrgomoutti	CY
Ayia Varvara	CY
Ayios Nikolaos	CY
Kalopanyiotis Dam	CY
Moutti tis Khalepis	CY
Ayii Anaryiri	CY
Ayia Anna	CY
Kaliana Khan	CY
Petromoutti	CY
Pikrathasoudhia	CY
Nannatidhes	CY
Mosphileri	CY
Moutti tis Phourkismenis	CY
Panayia Eleousa	CY
Eptamandilena	CY
Pano Akoliou	CY
Xerarkaka	CY
Ayia Varvara	CY
Ayios Yeoryios	CY
Kannoura	CY
Atratsa	CY
Marathasa Dam	CY
Kato Akoliou	CY
Ayios Epiphanios	CY
Panayia	CY
Ayia Mavri	CY
Pyrgolophos	CY
Akoliou Forest	CY
Apliki Forest	CY
Athkiakopetra	CY
Mazokambos	CY
Moutti tou Aphisaliou	CY
Moutti tou Pertikou	CY
Ayios Yeoryios	CY
Ayios Modhestos	CY
Prophitis Elias	CY
Aetos	CY
Pyryin	CY
Khosteri	CY
Xeropotamos Monastery	CY
Kakkarista	CY
Mousadhes	CY
Monodhendri	CY
Anephani	CY
Ayia Varvara	CY
Ayios Nikolaos	CY
Apostolos Andreas	CY
Potamos tou Kambou	CY
Ayios Kournoutas	CY
Lachi	CY
Aletri Mining Settlements	CY
Sandalin	CY
Mavrokremnia	CY
Laxies	CY
Ayios Yeoryios	CY
Kourvoula	CY
Moutti tou Kolotas	CY
Ayios Elias	CY
Kolanderka	CY
Lefka Kafixes Dam	CY
Moutti tis Skotinis	CY
Kanjelli	CY
Moutti tou Koriptou	CY
Kharcha	CY
Mazeri	CY
Glina	CY
Xeromoutta	CY
Ippos	CY
Argaki tis Ayias Triadhos	CY
Argaki tou Papadhourkou	CY
Yeraki	CY
Moutti tous Kaloyirous	CY
Kannaoura	CY
Angathi	CY
Moutti tou Jeou	CY
Leftolaonon	CY
Petrokephalon	CY
Moutti tou Khondrou	CY
Ayios Spyridhon	CY
Ayios Mamas	CY
Ayia Varvara	CY
Moutti tis Pallouras	CY
Kafkaroudhes	CY
Galini Dam	CY
Mavrikies	CY
Ayios Ioannis	CY
Arpidhak	CY
Lymbin	CY
Vournes	CY
Ayios Andronikos	CY
Apostolos Loukas	CY
Pertiko	CY
Khapotami	CY
Petra tou Koraka	CY
Phtelekha	CY
Skolarikou	CY
Vournes	CY
Kafkalla	CY
Kochina	CY
Agraktas	CY
Mnoukhos	CY
Kapsalos	CY
Pikra	CY
New Site of Kholetria	CY
Ayia Irini	CY
Merijes	CY
Beys	CY
Phernaja	CY
Kavalli	CY
Vrysha	CY
Potima	CY
Moni	CY
Phrangos	CY
Panayia	CY
Rock Dhora	CY
Ziouni	CY
Aphorismenos Kambos	CY
Ayios Savvas tis Karonos Monastery	CY
Kochino	CY
Xeron	CY
Arkoshilleron	CY
Konnos	CY
Ayios Yeoryios	CY
Argaki ton Kamaron	CY
Araos	CY
Ayiovounos	CY
Panayia	CY
Lachi	CY
Mouttotengia	CY
Kalivari	CY
Ayios Pandeleimon	CY
Stenos	CY
Jylistra	CY
Pernarka	CY
Vatoudhkia	CY
Konni	CY
Mouttovouni	CY
Petrovounia	CY
Platani	CY
Konia	CY
Pliskiou	CY
Kourtellorotsos	CY
Arkhangelos Gavriil	CY
Ayios Elias	CY
Kambos tou Katalanou	CY
Louveri	CY
Ophrakas	CY
Argaki tou Kounieri	CY
Argaki tou Kokkinototsou	CY
Stavros	CY
Limnes	CY
Kamines Mandres	CY
Argaki tis Kolemonas	CY
Xeraes	CY
Kalamos	CY
Kyra Eleousa	CY
Skasmata	CY
Kara tou Ayiou Yeoryiou	CY
Moutti tou Ayios Epiphaniou	CY
Vromonora	CY
Ayios Yeoryios	CY
Stavros	CY
Asprovounos	CY
Ayios Yeoryios	CY
Asproyi	CY
Ayios Yeoryios	CY
Xerargaka	CY
Eliomyli	CY
Paoura	CY
Ayios Kharalambos	CY
Kambos tou Rayiatikou	CY
Ayios Yeoryios	CY
Elia tou Papaktorou	CY
Argaki tis Potayis	CY
Koundourolaona	CY
Argaki tou Kouliati	CY
Arkokania	CY
Argaki tis Tremitheris	CY
Ayia Irini	CY
Hill Axylon	CY
Ayios Alexandros	CY
Khelidhoni	CY
Kouris	CY
Trozina Chiftlik	CY
Kambia	CY
Tremithos	CY
Ayios Antonios	CY
Vamvakera	CY
Manoilies	CY
Ammatopetra	CY
Kambos tou Throumbiou	CY
Argaki ton Peristerkon	CY
Ayia Marina	CY
Kamboudhia	CY
Ayios Yeoryios	CY
Panayia Zoodhokhou Piyis	CY
Klisidhia	CY
Loukkos	CY
Phillias	CY
Varkas	CY
Vouni	CY
Miamalos	CY
Apostolos Tykhikos	CY
Lilianos	CY
Ayios Yeoryios	CY
Panayia Eleousa	CY
Argaki tis Kryas Vrysis	CY
Troullouri	CY
Patria	CY
Argaki tou Spiliou	CY
Argaki tou Eliokhoriou	CY
Skhismorotsos	CY
Panayia Sindi Church	CY
Argaki tou Sinai	CY
Kafkaroues	CY
Koupanas	CY
Argaki tou Scerakotou	CY
Yerania	CY
Ayios Minas	CY
Lamaris	CY
Mouttes tous Aetous	CY
Sotira	CY
Karvounin	CY
Phyton	CY
Panayia	CY
Koutsoulla	CY
Argaki tis Silis	CY
Argaki tou Kortilou	CY
Vouni	CY
Ayios Ioannis	CY
Kalathos	CY
Argaki tou Leou	CY
Tremithi	CY
Argaki ton Lefkarkon	CY
Kokkinomoutti	CY
Lagoudhia Chiftlik	CY
Ayios Nikolaos	CY
Axylophos	CY
Laospitas	CY
Argaki tou Konnou	CY
Glora	CY
Palloekklisha	CY
Vounarka	CY
Ayia Varvara	CY
Stavros	CY
Ayios Yeoryios	CY
Argaki tis Limnis	CY
Saouri	CY
Plakota	CY
Gomaras	CY
Argaki tou Khanotari	CY
Roudhias Bridge	CY
Argaki tou Kouraliou	CY
Manzoura	CY
Ayios Epiphanios	CY
Kambos	CY
Statos and Ayios Photios	CY
Throumbera	CY
Argaki tis Plakotis	CY
Triandaphyllokremmos	CY
Mosphilovounos	CY
Khalaras	CY
Ayios Kononas	CY
Laonin	CY
Kelephos Bridge	CY
Argaki tou Skourka	CY
Moutti ton Kophinion	CY
Argaki tou Kou	CY
Ayios Yeoryios	CY
Kelephou Junction	CY
Peravasa	CY
Anemikon	CY
Ayios Mamas	CY
Teratsi	CY
Argaki tis Roudhias	CY
Khomatokremmos	CY
Trapeza	CY
Asvestokamina	CY
Ayia Paraskevi	CY
Panayia	CY
Prophitis Elias	CY
Ayia Moni Monastery	CY
Ayiasma	CY
Ayia Varvara	CY
Papaloukas	CY
Ayia Paraskevi	CY
Vouni	CY
Arkoklima	CY
Pellosmanos	CY
Ayios Dhimitrianos	CY
Ayios Yeoryios	CY
Garinos	CY
Lajeri	CY
Mavron Dhasos	CY
Argaki tis Kollachas	CY
Argaki tous Levandhes	CY
Irkon Lithari	CY
Tripodhi	CY
Argaki ton Mersinoudhion	CY
Argaki tis Adhras	CY
Argaki tous Koundourous	CY
Moutti tou Kydhoniti	CY
Ilakas	CY
Panayia	CY
Ayios Ioannis	CY
Trimithia	CY
Polimidhia	CY
Argaki tou Khaphtara	CY
Argaki tou Paleomylou	CY
Stavros	CY
Prodhromos	CY
Khryseleousa	CY
Kyparissin	CY
Ayios Mamas	CY
Laonas	CY
Moutti tous Kholetrotous	CY
Moutti ton Thirion	CY
Argaki tis Sykias	CY
Moutti tou Vatou	CY
Argaki tous Thietidhes	CY
Moutti tou Exarji	CY
Argaki ton Yephyrkon	CY
Steraja	CY
Petrovounos	CY
Ayios Kyriakos	CY
Xerarkaja	CY
Argaki tou Asprokremmou	CY
Moutti ton Elion	CY
Argaki tou Lakkou tis Sterajas	CY
Ayios Yeoryios	CY
Psilolophos	CY
Argaki tou Karydhiou	CY
Ayia Marina	CY
Ayios Mamas	CY
Ayios Zinon	CY
Ayios Yeoryios	CY
Ayios Dhimitrios	CY
Linoudhes	CY
Khalasmenos	CY
Moutti ton Trimidhkion	CY
Pano Vrysi	CY
Argaki tou Skouloudhrou	CY
Jinourko	CY
Moutti tou Anemomylou	CY
Moutti tis Karkas	CY
Ayios Epiphanios	CY
Asprokremia	CY
Argaki tou Theodhos	CY
Paleokhori	CY
Selladhin tis Kyparishas	CY
Moutti tous Anemous	CY
Argaki tis Ayias	CY
Vouni	CY
Moutti tis Koutsoullas	CY
Karvouneri	CY
Stavros ton Kratimaton	CY
Vasiliko	CY
Pefkos	CY
Katergas	CY
Aetomoutti	CY
Mavri Kremmi	CY
Argaki tou Athanasidhes	CY
Argaki tis Skotinis	CY
Laona tis Rashas	CY
Kourkoumis	CY
Sheromylia	CY
Kaloyirka	CY
Kaloyiros	CY
Moutti tou Kourkouta	CY
Petreri	CY
Apostolos Andreas	CY
Argaki tou Paradhisi	CY
Argaki tis Mersinis	CY
Moutti tis Angastinas	CY
Argaki ton Vryshon	CY
Argaki tou Pellon	CY
Moutti tis Sykias	CY
Argaki tis Dhaplinis	CY
Argaki tou Kalamiou	CY
Altophoulies	CY
Hill Kykko	CY
Argaki tou Kastroulion	CY
Argaki tis Irkas Sterajas	CY
Kilas ton Kedhron	CY
Moutti ton Koundouron	CY
Argaki tis Vrysis tou Kalou	CY
Ayia	CY
Polemidhia	CY
Vrysin	CY
Moutti tis Lajeris	CY
Ayios Photios	CY
Argaki tis Androuklias	CY
Kokkinokremmos	CY
Argaki tou Phonia	CY
Moutti tis Vroktelias	CY
Argaki tou Kapnismenou	CY
Moutti ton Kolymbaton	CY
Argaki tou Livadhiou	CY
Moutti tis Tremethousaras	CY
Appies	CY
Mallouri	CY
Moutti tis Ayias	CY
Moutti tou Klimakiou	CY
Moutti tou Apridhourkou	CY
Moutti tis Sykias	CY
Argaki tou Mounimourou	CY
Laona tou Phleri	CY
Mana tou Nerou	CY
Kakorkaka	CY
Eliopia	CY
Ayia Varvara	CY
Varvara	CY
Kochinovryses	CY
Trakhonia	CY
Ayios Patis	CY
Landoues	CY
Apitikos	CY
Arminiatis	CY
Neranos	CY
Kapkarolia	CY
Vounaros	CY
Ayios Ilarion	CY
Ayios Ilarion	CY
Makrona	CY
Agriolia	CY
Kato Kambos	CY
Ayios Yeoryios tou Asomatou	CY
Ayios Kirykos and Ayia Ioniliti	CY
Ayios Epiphanios	CY
Panayia Photolambousa	CY
Pretoras	CY
Khandrou	CY
Prophitis Elias	CY
Panayia Khryseleousa	CY
Sklinikari	CY
Ayios Theodhoros	CY
Theotokos	CY
Melanos	CY
Argaki tou Kalamidhiou	CY
Ayios Andronikos	CY
Karkies	CY
Ayios Yeoryios	CY
Ayios Minas	CY
Khryseleousa	CY
Ayios Yeoryios	CY
Skarphos Bridge	CY
Ayios Seryios	CY
Ayios Yeoryios	CY
Ayios Kharalambos	CY
Phlambouros	CY
Rocky Ridge	CY
Ayios Konstantinos	CY
Petro tou Limniti Settlement	CY
Petra tou Limniti	CY
Ayia Irini	CY
Ammadhies Forest	CY
Samajin	CY
Troullia	CY
Klouva	CY
Galoktisti	CY
Erakari	CY
Ayios Nikolaos	CY
Khartouliotissa	CY
Moutti tou Linou	CY
Papa Antonis	CY
Prophitis Elias	CY
Ayios Yeoryios	CY
Pano Pyrgos	CY
Tremithos	CY
Aetomoutti	CY
Ayios Andronikos	CY
Moutti tis Kamenis	CY
Ayia Varvara	CY
Xerovounos Forest B	CY
Ayios Loukas	CY
Xerovounos Forest D	CY
Loukkos	CY
Venetika	CY
Ayios Ioannis	CY
Moutti tou Vouniou	CY
Moutti tou Papayianni	CY
Koundourka	CY
Ayios Mamas	CY
Xerovounas Forest A	CY
Polemidhoura	CY
Ayios Yeoryios	CY
Ayios Mamas	CY
Moutti ton Tremithion	CY
Moutti tis Zounas	CY
Moutti tou Marathou	CY
Moutti tou Terachou	CY
Moutti tou Arodhaphniou	CY
Phlevas Sawmill	CY
Moutti tou Koundourou	CY
Kamini tou Yiakoumou	CY
Moutti tou Skortou	CY
Moutti tis Sklinijas	CY
Moutti tous Loukkous	CY
Koracha	CY
Petroudhia	CY
Moutti ton Plakoudhon	CY
Moutti tou Klimajion	CY
Moutti tous Koundourous	CY
Laja	CY
Moutti tou Tragouna	CY
Photinos	CY
Moutti tis Tremithias	CY
Kakovounaros	CY
Petromandra	CY
Ayios Mamas	CY
Kokkinokremmos	CY
Moutti tou Appiourkou	CY
Ayios Thomas	CY
Avroulies	CY
Moutti tou Kholetrou	CY
Moutti tous Spilious	CY
Partheni	CY
Ayios Kyriakos	CY
Ayia Anastasia	CY
Moutti tis Lajeris	CY
Vikles	CY
Koukkoutos	CY
Prophitis Elias	CY
Mavres Sykies	CY
Vyrsin ton Rouvanion	CY
Ayios Nikolaos	CY
Presis	CY
Moutti tis Papadhias	CY
Ayia Anastasia	CY
Ayios Mamas	CY
Panayia Elikon	CY
Steraji	CY
Argaki tou Nourou	CY
Argaki tis Mallakkouras	CY
Taroulas	CY
Moutti tou Plakiou	CY
Neron tis Androuklias	CY
Tripylos	CY
Dhodheka Anemi	CY
Kremmos tou Exo Mylou	CY
Plevra tou Papa Tourkou	CY
Androuklos	CY
Moutti tou Mesovounou	CY
Selladhin tou Mandiliou	CY
Moutti tou Khareri	CY
Laona tis Kilas	CY
Khomacha	CY
Xystaroudhin	CY
Moutti tou Kafkouna	CY
Ayios tis Phlevas	CY
Moutti Anagyrmeni	CY
Moutti tou Tsourouna	CY
Moutti tou Almyroudhiou	CY
Appiourka	CY
Moutti tou Trypitou	CY
Moutti tou Petalloudhiou	CY
Moutti tou Karavotorou	CY
Ayios Yeoryios	CY
Akoni	CY
Kakorocha	CY
Moutti tou Vathiou	CY
Pyrgos Dam	CY
Khatourka	CY
Stavros	CY
Ayios Kharalambos	CY
Ayios Ioannis	CY
Shylles	CY
Dhaphni	CY
Moutti tou Yeronisou	CY
Ayios Theodhoros	CY
Majango	CY
Moutti tis Malis	CY
Vrysi tis Athousas	CY
Larnia	CY
Ayia Marina	CY
Ayios Theodhoros	CY
Ayios Yeoryios	CY
Ghalephtiri	CY
Ayia Eleni	CY
Koukkoupha	CY
Vouni	CY
Ayios Yeoryios	CY
Ayios Ioannis	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Anemomylos	CY
Moutti tou Karyakiou	CY
Pakhyammos Forest	CY
Paliambela	CY
Khryseleousa	CY
Melissa	CY
Ayia Ekaterini	CY
Pomos Dam	CY
Hill Top	CY
Khrysopateritissa	CY
Mouttallia	CY
Konizin	CY
Ayios Mamas	CY
Moutti tou Asprovounou	CY
Moutti ton Pomelidhion	CY
Moutti tou Khondrou	CY
Moutti tis Theoras	CY
Koutsoullorefkos	CY
Ayios Philimon	CY
Ayia Ekaterini	CY
Kolotas	CY
Moutti tou Vounarkou	CY
Moutti tou Kouvarorotsous	CY
Mouttallos	CY
Appies	CY
Moutti tous Phytillous	CY
Appis tis Kkloa	CY
Moutti tou Xylodhryka	CY
Vorvora	CY
Lavramis	CY
Trypimeni	CY
Moutti tou Spiliou	CY
Phinokli	CY
Dhromonas	CY
Agrioteracha	CY
Kremmos tis Xylis	CY
Pouvistroudhi	CY
Selladhin tou Maziou	CY
Petrokoundouros	CY
Zakharou	CY
Pelekanous	CY
Pepkeri	CY
Kephalos tis Melanatis	CY
Selladhin tou Stavrou	CY
Karkavas	CY
Moutti ton Phytillon	CY
Repnia	CY
Moutti tis Ktistis	CY
Moutti tou Tornou	CY
Ayios Theodhoros	CY
Moutti tis Mosphilias	CY
Xeropiyi	CY
Laona ton Arnion	CY
Argaki tou Mavrokoutsoullou	CY
Argaki tou Atoupou	CY
Argaki tou Kouphoplatanou	CY
Kokkinokremmos	CY
Tornos	CY
Moummouros	CY
Phleris	CY
Perilophos	CY
Stavros	CY
Ayios Yeoryios	CY
Ayii Saranda	CY
Abdoulinas Junction	CY
Katashi	CY
Dhikhalos	CY
Psilos	CY
Teratsouin	CY
Kinousa Mine	CY
Kimisis tis Theotokou	CY
Mavro	CY
Ayios Arkadhios	CY
Paleomylos	CY
Koutsoullorakhi	CY
Kokkini	CY
Argaka Magounda Dam	CY
Eso Magounda	CY
Riyena	CY
Vouni	CY
Asomatos	CY
Moutti tou Leprou	CY
Kalotopia	CY
Koutsoyiannika	CY
Evo Moutti	CY
Argaki tou Khondrovounarou	CY
Koraki	CY
Asproyia	CY
Xeropotamos	CY
Ayios Nikolaos	CY
Ayios Mamas	CY
Poros tou Sykarkou	CY
Pano Vouni	CY
Elia tou Tsansari	CY
Ormani	CY
Mersimoudhi	CY
Ayios Yeoryios	CY
Athera tou Teratsoudhiou	CY
Ayia Marina	CY
Kakkinokambos	CY
Selladhi tou Potamou	CY
Ayia Marina Dam	CY
Ayia Marina	CY
Moutti tou Vouniou	CY
Phterouin	CY
Apotos	CY
Moutti ton Katarnou	CY
Argaki tou Tremithou	CY
Shinovouno	CY
Nea Dhimmata	CY
Ayios Yeoryios	CY
Panayia Galaterousa	CY
Argaki tou Symvoulou	CY
Moutti tous Kouzatous	CY
Moutti tou Sellia Dhiou tou Kalamou	CY
Ayios Yeoryios	CY
Pervolin	CY
Vikles	CY
Ayios Efxiphios	CY
Kaminia	CY
Ayia Marina	CY
Piania	CY
Karkotis	CY
Kochatis	CY
Ammarina	CY
Ayios Yeoryios	CY
Vatera	CY
Pertikopia	CY
Miji	CY
Asproyi	CY
Spitoudhia	CY
Ayios Dhimitrianos	CY
Ayios Avajeros	CY
Onespotika	CY
Ayios Avajeros	CY
Marmaraes	CY
Kakorkaka	CY
Ayia Marina	CY
Ayios Mamas	CY
Petrovounos	CY
Sklinikas	CY
Ayia Mavri	CY
Vouni	CY
Titsiros	CY
Kochatis	CY
Aetovounos	CY
Lartoyiannis	CY
Stavros tis Mythas	CY
Ayios Efstathios	CY
Vouni tou Lazarou	CY
Ayios Konstantinos	CY
Plymatta	CY
Ayia Eleni	CY
Dhikavounos	CY
Asprovounos	CY
Vakla	CY
Ayios Minas	CY
Ayios Yeoryios	CY
Prozymi	CY
Argaki Kolaseos	CY
Potima	CY
Prodhromos	CY
Ayia Marina	CY
Ayios Yeoryios	CY
Alakatin	CY
Kattoudheri	CY
Ayios Vasilios	CY
Sakkovounos	CY
Ayios Yeoryios	CY
Ayri Apostoli	CY
Angelos	CY
Arkhistratigos	CY
Lophos	CY
Ammatin	CY
Ayios Yeoryios	CY
Theotokoudha	CY
Ayios Nipios	CY
Ayios Tryphon	CY
Kalimerena	CY
Argaki ton Jinklon	CY
Neradhes	CY
Argaki tis Kambouras	CY
Laourkous	CY
Merika	CY
Kornies	CY
Trapezitis	CY
Prophitis Elias	CY
Prophitis Elias	CY
Ayia Marina	CY
Argaki ton Kolojon	CY
Krommydhin	CY
Matsikorithies	CY
Kathari	CY
Mosphilovounos	CY
Ayios Theodhoros	CY
Merijes	CY
Ayios Yeoryios	CY
Ayios Arilos	CY
Kapsalia	CY
Vizaja	CY
Arkhangelos	CY
Panayia	CY
Ayios Yeoryios	CY
Ayia Ekaterini	CY
Ayios Elias	CY
Ayios Theodhoros	CY
Khryseleousa	CY
Klavaris	CY
Ayia Paraskevi	CY
Korakou	CY
Ayios Yeoryios	CY
Ayia Marina	CY
Ayios Symeon	CY
Argaki tis Skalas	CY
Kyparisha	CY
Panayia Khrysospiliotissa	CY
Pitharolakkos	CY
Pernia	CY
Ayios Ioannis	CY
Ayios Elias	CY
Katsarkes	CY
Panayia Evangelista	CY
Ayia Marina	CY
Ayios Yeoryios	CY
Ayios Agrippas	CY
Kyra Eleousa	CY
Lakkos tou Frangou	CY
Krya Vrysi	CY
Ayia Varvara	CY
Aetokremmos	CY
Melissovounos	CY
Ayia Ekaterini	CY
Ayios Yeoryios	CY
Karydhia	CY
Thelimataris	CY
Ayios Yeoryios	CY
Argaki tou Vasilikou	CY
Koshinas	CY
Ellinospilii	CY
Laonarka	CY
Limnarka	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayia Paraskevi	CY
Haji Smith House	CY
Apollo Hylates Sanctuary	CY
Panayia Theoskepasti	CY
Panayia Galatarkotissa	CY
Ayia Mavri	CY
Ayios Yeoryios	CY
Kato Paphos Castle	CY
Ayios Antonios	CY
Ayia Marina	CY
Saranda Kolones Castle	CY
Ayia Solomoni Catacomb	CY
Glykyneron	CY
Ayios Epiphanios	CY
Ayios Yeoryios	CY
Tombs of the Kings	CY
Pyromasha	CY
Niolimata	CY
Panayia Khryseleousa	CY
Ayios Yeoryios	CY
Phinikin	CY
Panayia Khryseleousa	CY
Agriokalami	CY
Ayia Paraskevi	CY
Potima	CY
Kokkinokambos	CY
Ayios Yeoryios	CY
Ayia Marina	CY
Petromandra	CY
Mavrokolymbos Dam	CY
Tremithos	CY
Psoromandra	CY
Katarraktis	CY
Yerakopetra	CY
Tremithia	CY
Yephyrka	CY
Ayios Yeoryios	CY
Arodhaphna	CY
Lashi	CY
Khryseleousa	CY
Ayia Aphendrika	CY
Ayios Epiphanios	CY
Vourni	CY
Stavros	CY
Laomilia	CY
Teratsokambos	CY
Vlambouros	CY
Kaiphinas	CY
Petres tou Leka	CY
Pittokopos	CY
Levji	CY
Karpasitis	CY
Lara	CY
Ayios Yeoryios	CY
Elin Petras	CY
Rakhovounos	CY
Ara	CY
Skala	CY
Plevra tou Liondakiou	CY
Argaki tou Klimi	CY
Inia	CY
Peyia Forest	CY
Stafka	CY
Kandara	CY
Lipati	CY
Argaki ton Kouphon	CY
Akonia	CY
Kokkinoloura	CY
Farkonies	CY
Konnarkes	CY
Kapsades	CY
Atsouparkakon	CY
Plakaja	CY
Kalyphes	CY
Panayia	CY
Gyros tis Rizas	CY
Dhijimi	CY
Panayia ton Zalajon	CY
Ayia Paraskevi	CY
Pappara	CY
Panayia Agridhiotissa	CY
Prophitis Elias	CY
Louvias	CY
Vournia	CY
Asproyia	CY
Kochinoyin	CY
Keratidhi	CY
Maa	CY
Katsiris	CY
Paleokastron	CY
Coral Bay	CY
Rouskia	CY
Argaki tis Mishenas	CY
Petra tou Pozou	CY
Meleti Forest	CY
Stavros	CY
Kalamoulli	CY
Ayios Theodhoros	CY
Aprillokambos	CY
Argaki tis Trypimenis	CY
Ayii Phanendes	CY
Kalamos	CY
Argaki tou Mykhou	CY
Argaki tou Pelli	CY
Argaki tis Kaskias	CY
Angones	CY
Alyki tis Laras	CY
Kryos Kolymbos	CY
Mirillis	CY
Frangou	CY
Argaki tis Rodhias	CY
Panagiya tou Vlou	CY
Argaki tis Markarous	CY
Argaki tou Yiousouphi	CY
Argaki tous Kaloyerous	CY
Korakas	CY
Tigani Settlement	CY
Aphrodite’s Temple	CY
Ayii Anargyri	CY
Mazera	CY
Arkakoudhia Forest	CY
Mana tou Nerou Forest	CY
Panayia Dam	CY
Panayia	CY
Lefkolia City-Site	CY
Protaras	CY
Ayios Yeoryios	CY
Ammos tou Kambouri	CY
Korajistres	CY
Kouklias	CY
Strongylovounaro	CY
Glyky Neron Forest	CY
Ayia Napa Forest	CY
Ayios Epiphanios	CY
Ayios Ioannis Forest	CY
Ayios Ioannis	CY
Ayii Saranda Forest	CY
Ayii Saranda	CY
Lakkos tou Papayioryi	CY
Kremmadhkia	CY
Apissotou	CY
Kambos	CY
Prophitis Elias	CY
Ayios Pandeleimon	CY
Lakkos tou Skarou	CY
Pernera	CY
Kanas	CY
Pinia	CY
Ayios Mamas	CY
Louma	CY
Minas	CY
Ayia Varvara	CY
Kamaroudhi tis Laxias	CY
Ayia Trias	CY
Kartapinna	CY
Kamaroudhi	CY
Armyropigano	CY
Ayios Memnon	CY
Kambos tis Dherinias	CY
Pamboula tis Skalas	CY
Ayia Marina	CY
Ayios Antonios	CY
Ayios Konstantinos	CY
Church of the Nativity	CY
Ayios Dhimitrios	CY
Panayia	CY
Arkhangelos	CY
Anaphotia	CY
Katajilia	CY
Ayios Antonios	CY
Ayia Napa Monastery	CY
Ayia Napa Sea Block Forest	CY
Ayia Thekla	CY
Ayios Nikandros Forest	CY
Ayia Varvara Forest	CY
Ayia Varvara	CY
Sotira Forest	CY
Sotira Dam	CY
Ayios Mamas	CY
Panayia Khordajotissa	CY
Ayios Theodhoros	CY
Ayios Yeoryios	CY
Ayia Varvara	CY
Ayios Andronikos	CY
Ayia Marina	CY
Kamini	CY
Kokkinoyia	CY
Ayia Zoni	CY
Ayia Paraskevi	CY
Panayia Khrysospiliotissa	CY
Ayios Nikolaos	CY
Ayia Ekaterini	CY
Ayia Trias	CY
Ayios Ioannis	CY
Santa Sophia Mosque	CY
Othello’s Tower	CY
Ayios Yeoryios	CY
Limni Forest	CY
Ayios Yeoryios Dam	CY
Ayios Yeoryios	CY
Kremmos	CY
Pernepoudhia	CY
Dherinia Dam	CY
Ayia Irini	CY
Arkhangelos Mikhail	CY
Ayia Anargyri	CY
Teratsoudhi	CY
Kaminin tou Skotomenou	CY
Limnia	CY
Troullos tou Skortou	CY
Ayios Efstathios	CY
Liopetri Dam	CY
Kokkina Kaminia	CY
Trakhonia tou Koukoula	CY
Kamini tou Markomenou	CY
Ayios Andronikos	CY
Ayios Andreas	CY
Kamini tou Birinji	CY
Pyroyia	CY
Khalospidhkia	CY
Pallouroudhia	CY
Phournia	CY
Panayia Asprovouniotissa Monastery	CY
Ayios Kendeas Monastery	CY
Ayios Kendeas	CY
Kaminin ton Pigadhion	CY
Ayios Yeoryios	CY
Mavrommatis	CY
Ayios Mamas	CY
Kaminospilios	CY
Ayia Paraskevi	CY
Panayia Khrysopolitissa	CY
Spilios tou Agrou	CY
Yiophyri tis Trapezas	CY
Kopries	CY
Kambia	CY
Engomi Tumulus	CY
Engomi City-Site	CY
Salamis Necropolis	CY
Ayia Katerina	CY
Tomb of St. Barnabas	CY
Lakkos tou Hajivassili	CY
Konstantia	CY
Salamis Forest	CY
Kythrea Salamis	CY
Ayios Sozomenos	CY
Arkhangelos	CY
Kolymbos Forest	CY
Ayios Elias	CY
Ayios Seryios and Bacchos	CY
Hajipandelis House	CY
Kavallos	CY
Kambanolakkos	CY
Shiromylia	CY
Pallourotopia	CY
Ambelia Forest	CY
Paradhisi	CY
Panayia Paradhishotissa	CY
Ayios Loukas	CY
Lishines	CY
Afkolia ton Livadhion	CY
Afkolia Tay Yiorki	CY
Ayia Marina	CY
Ayios Loukas	CY
Kaminoudhi	CY
Sklinjia tis Ayias Marinas	CY
Kochines	CY
Trakhonas	CY
Mazera	CY
Kaminjik	CY
Kamini Sarandanas	CY
Panayia Avgasidha	CY
Koja Tarlasi	CY
Mezarlik	CY
Asproyia	CY
Lakkos tou Hajikakouri	CY
Plakos	CY
Ayios Efstathios	CY
Pendapsoumi	CY
Kopris	CY
Kharangas	CY
Pende Litharka	CY
Lishines	CY
Bey Tarlas	CY
Loumata	CY
Mouttolishino	CY
Mazerka	CY
Alopa	CY
Mavrokolymbos	CY
Chattalia	CY
Ayios Yeoryios	CY
Amistaros	CY
Pamboula tis Kataramenis	CY
Mosphilia	CY
Pamboula tou Kharanga	CY
Phoradhomandra	CY
Koumenia	CY
Xylna	CY
Koukkoullas	CY
Pamboula	CY
Ayios Yeoryios	CY
Ayii Anargyri	CY
Ayia Marina	CY
Vouppies tou Mosphiliou	CY
Ayios Yeoryios	CY
Lakkos tou Hajidhmitri	CY
Lakkos tou Hajisavva	CY
Chiftlik	CY
Alambrona	CY
Ayios Yeoryios Forest	CY
Athna Forest	CY
Ayia Theodhora	CY
Panayia tis Trashas	CY
Ayios Vlasios	CY
Trypitos Spilios	CY
Khandaja	CY
Pamboula tou Liassi	CY
Kapsalia	CY
Troullos	CY
Ayia Thekli	CY
Petradhes	CY
Merras tis Kalopsidhas	CY
Merras ton Kouklion	CY
Skalin tou Khasani	CY
Mandres tou Prastiou	CY
Arkaja	CY
Merras tou Prastiou	CY
Panayia tou Ktistou	CY
Argaki tou Markou	CY
Kavallos	CY
Kenouris	CY
Ayios Yeoryios	CY
Toumba tou Ayios Yiorki	CY
Toumba Niolita	CY
Haji Moustafa	CY
Langouphes	CY
Enia Skales	CY
Mandrika Elias	CY
Kochines	CY
Kouzoulies	CY
Koukkoutaes	CY
Miliotis	CY
Xeropotamos	CY
Ayios Yeoryios	CY
Ayios Iakovos	CY
Mavroyiannies	CY
Katsourophtas	CY
Kokkinovouppos	CY
Vouni	CY
Maniari	CY
Aletou	CY
Arkoshyliera	CY
Sorephti	CY
Ayia Varvara	CY
Paleovikla	CY
Mosphilokambos	CY
Karioulasi	CY
Aphendrika	CY
Ayios Epiphanios	CY
Apostolos Andreas	CY
Ayios Tryphon	CY
Ayios Yeoryios	CY
Xalona	CY
Ayia Marina	CY
Plises	CY
Lakkos	CY
Kondylia	CY
Panayia Khorteni	CY
Aragki tis Khortinis	CY
Mirmikoph	CY
Phteliatis	CY
Kosmaes	CY
Marion/Arsinoe City-Site	CY
Ayios Ioannis	CY
Stavropetra	CY
Pouliokambos	CY
Vervellou	CY
Petratis	CY
Venetiotissa	CY
Khalimes	CY
Argaki tou Ayiou Ioanni	CY
Anatoliki	CY
Kouroullas	CY
Skotini	CY
Pefkous	CY
Argaki ton Khalavron	CY
Argaki tou Varou	CY
Argaki ton Almyron	CY
Skala	CY
Ayia Marina	CY
Aspropetra	CY
Klisha	CY
Potami Chiftlik	CY
Kakoskali	CY
Ayios Yeoryios	CY
Xylomandron	CY
Asprospilios	CY
Argaki tou Pyrgou	CY
Dhyo Potami Settlement	CY
Yiolou Chiftlik	CY
Stavropiyi	CY
Rigopoulos	CY
Magnisia	CY
Mavri Shinia	CY
Smiyes	CY
Ayios Minas	CY
Grami Chiftlik	CY
Karamoullas	CY
Pissouromoutti	CY
Ayia Paraskevi	CY
Megalos	CY
Koudhellis	CY
Ayios Konon	CY
Kapsalomoutti	CY
Argaki ton Theorakion	CY
Klimata	CY
Koudhounas	CY
Samakia	CY
Dhamalospilios	CY
Spilios Magnisias	CY
Osia Maria	CY
Argaki ton Kavourotrypon	CY
Kephalovrysha	CY
Koumarka	CY
Ayios Yeoryios	CY
Sarandakloni	CY
Glyky Myrsini	CY
Pano Vakhines	CY
Kato Vakhines	CY
Pondikospilios	CY
Argaki ton Kolymboudhion	CY
Panormi	CY
Petra Kionos	CY
Seryios	CY
Kionos Island	CY
Argaki tou Koundourokambou	CY
Argaki tou Ayiou Kononos	CY
Argaki tou Kannoudhiou	CY
Argaki tou Mouzouri	CY
Argaki tis Aspris Vrysis	CY
Argaki tou Apopoulou	CY
Psindrou	CY
Mesoyia	CY
Ayios Nikolaos	CY
Argaki tou Psindrou	CY
Argaki tou Plakiou	CY
Manolis	CY
Aorateri	CY
Argaki tous Exosyrondas	CY
Argaki tis Klokkarkas	CY
Kakomolia	CY
Moutti tou Athou	CY
Ayios Epiphanios	CY
Argaki tis Farkonias	CY
Sykaroua Mavrokourellos	CY
Eleovounos	CY
Perohikia	CY
Thena	CY
Kamares	CY
Vikla	CY
Kapsalia	CY
Kokkinokremmas	CY
Troulia	CY
Klisouro Vouno	CY
Kroteri	CY
Khalasta	CY
Kondinari Vouno	CY
Aphrodite Akraia	CY
Kastros Settlements	CY
Kinanero Vouno	CY
Ammi	CY
Peristeri	CY
Melena	CY
Shistres	CY
Almyrokolymbos	CY
Cape Proti	CY
Kordhylia Islands	CY
Mora Psaria	CY
Vyzakia	CY
Pendayii	CY
Koraka	CY
Apostolos Andreas Forest	CY
Sarakino Vouno Forest	CY
Koukoulos Forest	CY
Prendis Forest	CY
Notadhes	CY
Nangomi	CY
Tigania Forest	CY
Kamenos Forest	CY
Pyla Forest	CY
Ayios Phylon at Agridhia	CY
Ayia Varvara	CY
Ourania City-Site	CY
Ayios Dhimitrios	CY
Argaki tis Mersinias	CY
Argaki tis Koukkouvayias	CY
Asomatos	CY
Afendrika	CY
Glaro Island	CY
Ayios Yeoryios	CY
Panayia Afendrika	CY
Lefkoniso	CY
Argaki tis Gournas	CY
Ayios Yeoryios	CY
Khalasta Forest	CY
Voukranos Forest	CY
Ayios Nikolaos	CY
Stylos	CY
Aspro Island	CY
Pyra Khoraphkia	CY
Panayia Apakou	CY
Panayia Paleokhoriou	CY
Ayia Athanasia	CY
Ayios Theodhoros	CY
Ayios Pandeleimon	CY
Ayios Andronikos	CY
Argaki ton Lachon	CY
Kitersi	CY
Litharkes	CY
Olos	CY
Melissakros Forest	CY
Karkalos Forest	CY
Ayios Yeoryios	CY
Phinikia	CY
Ayios Iakovos	CY
Ayios Synesios	CY
Ayia Trias	CY
Khristos	CY
Karpasia City-Site	CY
Marathini	CY
Krikos	CY
Khalasta	CY
Skonas	CY
Ayia Mavra	CY
Ayia Marina	CY
Eleousa Forest	CY
Prophitis Dhaniil	CY
Ayios Ioannis	CY
Ayios Yeoryios	CY
Asprokolymbos	CY
Ronna	CY
Laris	CY
Panayia Eleousa Monastery	CY
Monastiraki	CY
Ambell	CY
Phtellekhos	CY
Limanouri	CY
Yerania	CY
Phonias	CY
Krevatia Forest	CY
Dhaphnonda Forest	CY
Panayia Dhaphnonda	CY
Karpas Forest	CY
Koukkoumar Forest	CY
Sykadha	CY
Serghies	CY
Argaki ton Kamaron	CY
Trakhonas-Galinoporni Forest	CY
Kapsalia Forest	CY
Vouno Forest	CY
Panayia Afendrika	CY
Therion	CY
Selenias	CY
Ayios Photios	CY
Nisha	CY
Villoures	CY
Ayia	CY
Litharos	CY
Ayios Dhimitrios	CY
Mnimata Tous Sarakinous	CY
Spitia Tous Romious	CY
Argaki tis Phlevas	CY
Skoutelli Forest	CY
Pitidhia Forest	CY
Mesanos Forest	CY
Argaki ton Lachon	CY
Nektovika Forest	CY
Nektovikla Fortress	CY
Ayios Mamas	CY
Ayia Varvara	CY
Ayia Varvara Forest	CY
Trakhonas-Khalasmata Forest	CY
Paleovikla Forest	CY
Gonies Vouna	CY
Agathangelos	CY
Ayios Thyrsos	CY
Ayia Marina	CY
Lini	CY
Kankelia	CY
Vouno	CY
Masherkonas	CY
Ayios Yeoryios Sakka	CY
Melini	CY
Point Glykiotissa	CY
Alyki	CY
Vrokhos	CY
Optomolia	CY
Eleomylos	CY
Ayia Trias Basilica	CY
Kakopetri	CY
Ayios Phanourios	CY
Zygo Vouno	CY
Kannidhiastres	CY
Kapsalos	CY
Dhamali	CY
Anemos	CY
Elisis	CY
Ammoyia	CY
Kharangi	CY
Ayios Phokas	CY
Kastros	CY
Kapsalia	CY
Harcee Forest	CY
Ayios Seryios Forest	CY
Issakos Forest	CY
Ayios Seryios	CY
Zorras Forest	CY
Panayia Eleousa	CY
Pambakistra	CY
Selena	CY
Nisha	CY
Yerania	CY
Vouppos	CY
Lourka	CY
Stalos	CY
Moutti tou Mylou Forest	CY
Appidhia	CY
Mavri Mazeri	CY
Ayios Theodhoros	CY
Ayios Yeoryios	CY
Panayia	CY
Ayios Konstantinos	CY
Katalymata	CY
Kavallis Forest	CY
Kavallos	CY
Tenios	CY
Romanos	CY
Ayios Polykhronios	CY
Ayia Solomoni	CY
Solomoni	CY
Panayia Zoodhokhou Piyis	CY
Kaminia	CY
Kalamia	CY
Ayia Thekla	CY
Ayios Yeoryios	CY
Petrovounia	CY
Arkastirka	CY
Ayia Glykeria	CY
Akamas	CY
Limionas	CY
Skaloudhia	CY
Dhemoniaris	CY
Koutoulis	CY
Orkos	CY
Skamni	CY
Aspropetra	CY
Ayia Thekli	CY
Trakhonas	CY
Pavleti	CY
Peristeria Forest	CY
Myli	CY
Maliotis	CY
Dhyo Vounarka	CY
Khores	CY
Panayia Angariotissa	CY
Koprista Forest	CY
Platses	CY
Lithari	CY
Ayios Andronikos	CY
Kilanemos	CY
Mandra tou Kilanemou	CY
Akradhes	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Mavrokremmos	CY
Kafkalla Forest	CY
Troullia Fortress	CY
Panayia Kanakaria Monastery	CY
Ayios Vasilios	CY
Ayios Antonios	CY
Mazeri Forest	CY
Kattoues	CY
Yerovlakas	CY
Versari Forest	CY
Ozalos	CY
Kakozonara	CY
Madeli Forest	CY
Ayia Anna	CY
Skaliphourda Forest	CY
Mersinia	CY
Ayia Sophia	CY
Ayia Solomoni	CY
Troulloti	CY
Mazoulia	CY
Potamia	CY
Koutsopetros	CY
Koronia Forest	CY
Pano Koronia Forest	CY
Ayios Prokopios	CY
Kapsalia	CY
Mazerka	CY
Dhipsali Forest	CY
Ayios Nikolaos	CY
Ayia Solomoni Forest	CY
Phrangomata	CY
Kolymbia	CY
Pertiki Forest	CY
Koukoulos	CY
Toumba Forest	CY
Anydhra Forest	CY
Aspromoutti Forest	CY
Aspromoutti	CY
Perikephalos Forest	CY
Moutti	CY
Aranas	CY
Dhounia	CY
Vazaja	CY
Sheromylia Forest	CY
Pakhys Ammos	CY
Petra	CY
Mouttarka	CY
Vouppos	CY
Kamares	CY
Plakes	CY
Psarolimni	CY
Pigadhi	CY
Petrovounia	CY
Kernathion Island	CY
Koutsopetri	CY
Sernos Island	CY
Vasilies	CY
Pyropetra	CY
Titsiros	CY
Aetopetra Forest	CY
Spiliris	CY
Ayios Epiphanios	CY
Kourtoulas	CY
Trakhonas	CY
Skasmata	CY
Mikhail Arkhangelos	CY
Melionas	CY
Kavazies	CY
Mandres	CY
Toumbes	CY
Tarmasos Galatia Forest	CY
Yerokolymbos	CY
Patelia	CY
Kalyphjik	CY
Korphos	CY
Ayia Marina	CY
Kyra	CY
Kombos	CY
Ayios Seryios	CY
Kophinarka	CY
Pyroyia Forest	CY
Ayia Thekli	CY
Paleomylos	CY
Theanoulia Forest	CY
Vounaropoullos	CY
Livadhia	CY
Paleomylos	CY
Vokolidha Forest	CY
Troullin	CY
Jylistres	CY
Shirospilios	CY
Makroulli	CY
Stavrovoutsa	CY
Knidhos City-Site	CY
Ayios Ioannis	CY
Eliokhoria	CY
Palloures	CY
Kokkinoloukkos	CY
Valia Forest	CY
Styllarka	CY
Vokolomandres	CY
Kondostephani	CY
Mazeres	CY
Ayios Theodhoros	CY
Ayios Evloyios	CY
Pilinia	CY
Ayia Varvara	CY
Pavlouris	CY
Vournia	CY
Panayia tis Kyras	CY
Varkes	CY
Titsiros	CY
Troullia	CY
Phellas	CY
Mandrika Forest	CY
Katalymata Forest	CY
Ayia Irini	CY
Kavallaropetra	CY
Tarmasos Ephtakomi Forest	CY
Ayios Loukas	CY
Trapezouli	CY
Ayia Agathi	CY
Yiouti Forest	CY
Dhrakondas	CY
Scales Island	CY
Phlouje	CY
Ayias	CY
Pharkonia	CY
Ayios Nikolaos Monastery	CY
Trapeza	CY
Stouppo	CY
Kambi	CY
Kakos	CY
Katsmata	CY
Pastarto	CY
Tenios	CY
Dhiplarkaka	CY
Phramenos	CY
Lithosourka Tomb	CY
Ayios Ioannis	CY
Klimata	CY
Vikla	CY
Laopoulis	CY
Dhkio Vounarka	CY
Akkovounara	CY
Rigatiko	CY
Moutti Mavrokavlia	CY
Ayios Dhimitrios	CY
Mylopetri	CY
Vounos	CY
Argaki tou Lourka	CY
Argaki ton Villourkon	CY
Joovli Dere	CY
Argaki ton Gouppon	CY
Kapsalia	CY
Panayia Evangelistria	CY
Arno Forest	CY
Toumbes	CY
Anomoutti	CY
Jonies	CY
Kandara Monastery	CY
Kandara Forest	CY
Koronia	CY
Kavallaropetra	CY
Toumba	CY
Pigadhi	CY
Potamoudhia	CY
Ayios Sozomenos	CY
Pyrgos	CY
Gouppos	CY
Jipia	CY
Argaki tous Villouraes	CY
Katarraktis	CY
Thepos	CY
Asproyia	CY
Aloupos	CY
Klima	CY
Vorni	CY
Yiouti Forest	CY
Gonies	CY
Klephtis	CY
Keros	CY
Ayios Onouphrios	CY
Ayios Photios	CY
Blekti Forest	CY
Rakht tou Katsoura Forest	CY
Paliokhanoton	CY
Aspropezoula	CY
Kakovathra Forest	CY
Eliomylos	CY
Phournaja Platsa Forest	CY
Kouzoules	CY
Ayia Varvara	CY
Kouzalos Forest	CY
Ambelo	CY
Arodhaphni	CY
Kapsalos	CY
Kokkinovounia	CY
Platino Platsa Forest	CY
Trypitos	CY
Skala	CY
Oumaes	CY
Kourdhelles	CY
Lishnoudhkia	CY
Kalyphi tou Photi	CY
Patriki	CY
Ambelia	CY
Phrangolakkos	CY
Rammos	CY
Prodhromos	CY
Valia Forest	CY
Kastros and Strongylos Castle	CY
Kourollos	CY
Gypsaros	CY
Okniaris	CY
Rothesi	CY
Jinaneris	CY
Argaki ton Gastron	CY
Ayios Mamas	CY
Cholos	CY
Jipia	CY
Kharangas	CY
Katevas	CY
Ayios Spyridhon	CY
Ayia Paraskevi	CY
Yeropotamos	CY
Ayios Dhimitrios	CY
Kalyphoudhes	CY
Ayios Yeoryios	CY
Argaki ton Katharon	CY
Matsoukia	CY
Vikles Forest	CY
Mouttes	CY
Kapsalos	CY
Stalos Forest	CY
Khalestori	CY
Phoradhomandra	CY
Stalos Forest	CY
Loukkou Forest	CY
Nevrojeries	CY
Moutti tou Pashali Forest	CY
Laxia Forest	CY
Kafkalla Forest	CY
Kaminia	CY
Laxies	CY
Platy Vouno Forest	CY
Vounari tou Solomou	CY
Platses	CY
Kokkines	CY
Hajinikoloues	CY
Trikomo	CY
Rothesha	CY
Ayios Iakovos	CY
Panayia Theotokos	CY
Ayios Epiphanios	CY
Kopreradhes	CY
Kavallos	CY
Ayia Ekaterini	CY
New Tekkele Khan	CY
Anathrijera	CY
Koudhounarka	CY
Argaki tou Ayiou Yeoryiou	CY
Ayia Marina	CY
Ayios Prokopios	CY
Panayia	CY
Krines	CY
Vouni	CY
Asprovikla Forest	CY
Kalamoulis	CY
Katsounas	CY
Ayios Savvas	CY
Katsounas	CY
Argaki tou Karakouna	CY
Plousha Forest	CY
Ellinokhori	CY
Panayia tou Tokiniou	CY
Argaki tou Plousha	CY
Kokkmoliarouvouno	CY
Ayios Ioannis	CY
Teratsi	CY
Gouppa ton Skalion	CY
Pefkos	CY
Skalia	CY
Melissa	CY
Panayia Melissa	CY
Vounari Stronghold	CY
Kochines	CY
Ayios Therapon	CY
Phytefia	CY
Peristeria	CY
Melissa	CY
Lakkos	CY
Ayios Kharalambos	CY
Ayios Kharalambos	CY
Trika Teracha	CY
Panayia Kanakarias	CY
Argaki ton Tenion	CY
Argaki tis Trodhkias	CY
Knasa	CY
Kathoumeni	CY
Arrona	CY
Argaki tou Piperoudhia	CY
Phonias	CY
Arkhangelos	CY
Panayia Galaktini	CY
Spalathovouno Forest	CY
Argaki tis Galaktinis	CY
Argaki tou Gouppi	CY
Moutti tou Kakotri	CY
Melia	CY
Melia Forest	CY
New Asprovikla	CY
Ayios Iakovos	CY
Plaka	CY
Kafkos	CY
Vounaroudhia	CY
Shinieri	CY
Kateva	CY
Mouttotrakhonas	CY
Merikeros	CY
Vizakovounia	CY
Tris Pedhes	CY
Pervolia	CY
Trakhonas	CY
Prophitis Elias	CY
Plakoti	CY
Gypsos	CY
Stalos	CY
Syrka	CY
Argaki tis Yephyras	CY
Pikros	CY
Gypsos Dam	CY
Vathys	CY
Khones	CY
Jilikas	CY
Argaki ton Skalion	CY
Phardhys Vounos	CY
Argaki tou Pitsivi	CY
Argaki tou Ayiou Yeoryiou	CY
Ayios Yeoryios	CY
Koutsoura	CY
Ypsarovouno Forest	CY
Vounaros	CY
Khatoupos	CY
Pervola tou Mavrou	CY
Ayia Irini	CY
Yerajes	CY
Iephalis	CY
Mavri Skala	CY
Sotiros	CY
Koutsopetrika	CY
Ayia Photini	CY
Ayios Merkourios	CY
Lillis	CY
Eleovounos	CY
Ayios Pavlos	CY
Eliopotami	CY
Ayios Drikas	CY
Khalkopotamos	CY
Panayia Pergamiatissa	CY
Pergamon City-Site	CY
Ayii Pandes	CY
Ayios Mikhalos	CY
Ayios Theodhoros	CY
Aphrodhision City-Site	CY
Arkhangelos Mikhail	CY
Neraidhes	CY
Kalami	CY
Stalos	CY
Nava	CY
Styrakoulia	CY
Kakotris	CY
Argaki tis Pera Mandras	CY
Argaki tis Yeroeleas	CY
Angouli	CY
Ayios Savvas	CY
Ayia Varvara	CY
Ayios Ioannis	CY
Kamara tou Kambiou	CY
Olymbos	CY
Kantara Forest	CY
Jirkavli Mevkisi	CY
Konovari	CY
Sivri Tepe	CY
Vathys	CY
Platys	CY
Araba Dere	CY
Ayios Phokas	CY
Skoutelladhes	CY
Kryos	CY
Vikles	CY
Peyaz Tepe	CY
Pelapan	CY
Ayios Yeoryios	CY
Jinaroto	CY
Vyzajeri	CY
Ayia Paraskevi	CY
Eleovouno	CY
Sheromylos	CY
Laxia	CY
Ayios Nikolaos	CY
Viklari	CY
Xerarkakon	CY
Kapsalos	CY
Ayios Yeoryios	CY
Ayia Marina	CY
Moulos	CY
Makaria City	CY
Kastros	CY
Argaki ton Kangelion	CY
Argaki tous Maronites	CY
Mallina	CY
Melounda Forest	CY
Ayios Polykhronios	CY
Kopsari	CY
Ayios Nikolaos	CY
Ayios Antonios	CY
Kitsis	CY
Kephalas	CY
Epta Potami	CY
Denarka	CY
Denarka Forest	CY
Yerokolymbos	CY
Liastres	CY
Shezias	CY
Strata ton Gouphon	CY
Yerokolymbos	CY
Skopos	CY
Eminje	CY
Mazeri	CY
Otoun Yiolou	CY
Mazeri	CY
Merjanlar	CY
Peristeria	CY
Trapezia	CY
Argaki tou Akoniou	CY
Mersines	CY
Argaki tou Askoupelou	CY
Kour Dere	CY
Knodhara	CY
Koukkoutaris	CY
Lachi Kraji	CY
Lachi Dere	CY
Argaki ton Peristerion	CY
Panayia Eleousa	CY
Argaki tou Mavrovouniou	CY
Zygo Vouno	CY
Pilikas	CY
Laghoudhia	CY
Ayios Athanasios	CY
Argaki tis Aetophoulias	CY
Trakhonas	CY
Kamnia	CY
Chakilli Dere	CY
Jatitika	CY
Injipler	CY
Koja Dere	CY
Keuy Dere	CY
Ga Dere	CY
Yatagan Belenk	CY
Kara Belenk	CY
Keuyzlar Bagche	CY
Palea Vrysi	CY
Pano Kremmos	CY
Sikiler	CY
Belenk Arasi	CY
Koja Yer Dere	CY
Koja Dere	CY
Ak Belenk	CY
Savva Belenk	CY
Tepeler	CY
Pharphari	CY
Roukapia	CY
Klapatha	CY
Kastroulli	CY
Avrovounia	CY
Ayios Nikolaos	CY
Mazeri	CY
Louma	CY
Kapsaloudhia	CY
Dhrakondas	CY
Ayios Mamas	CY
Argaki ton Yephyrion	CY
Kakopotamos	CY
Panayia Melandryna Monastery	CY
Glyphonera	CY
Limni	CY
Ayios Yeoryios	CY
Melandryna Forest Number 2	CY
Pittaroulla Forest	CY
Melandryna Forest Number 1	CY
Panayia Apati	CY
Argaki tis Kalamias	CY
Pelipetra	CY
Mersinia Imeri	CY
Baro Forest	CY
Argaki tou Kaloyerou	CY
Ayios Sindis	CY
Argaki tou Kamenou	CY
Yiannoukos	CY
Pernarotos Forest	CY
Arkhangelos	CY
Kharcha Forest	CY
Ambelia	CY
Trakhoni Forest	CY
Melanisyko	CY
Ornousa	CY
Argaki tis Gliastras	CY
Argaki ton Karavion	CY
Ayios Dhimitrianos	CY
Kokkinospita	CY
Vasilion	CY
Plaka	CY
Argaki ton Limnon	CY
Prophitis Elias	CY
Katarraktis	CY
Kaki Zoni	CY
Ayia Paraskevi	CY
Mandra	CY
Ayios Yeoryios	CY
Ayia Mavri	CY
Vounon tou Armeni	CY
East Plataniotissa Forest	CY
Kyrilia	CY
Haji Astour Dere	CY
Argaki ton Kamaroudhion	CY
Ayios Yeoryios	CY
Argaki ton Alakation	CY
Alakati	CY
Panayia Plataniotissa	CY
Kavak Zeytin	CY
Keupek Dere	CY
Aksou Dere	CY
Koja Dere	CY
Argaki tou Ayiou Vasiliou	CY
Ayios Vasilios	CY
Mandra tou Vounou	CY
Vrysi tous Kkouniaes	CY
Khytri City-Site	CY
Ayios Andronikos	CY
Ayios Dhimitrianos	CY
Panayia Khardhakiotissa	CY
Ayios Yeoryios	CY
Ayia Anna	CY
Ayios Antonios	CY
Kremmos tou Plataniou	CY
Kokkinokremmos	CY
Pendadhaktylos Forest	CY
Phileri	CY
Phileri	CY
Lakkovounara	CY
Ayios Epiphanios	CY
West Paltaniotissa Forest	CY
Kyparissovouno	CY
Gyrilia	CY
Kapparovounos	CY
Ayios Yeoryios	CY
Makedhoni	CY
Alakati	CY
Troulli Settlement	CY
Ayia Ekaterini	CY
Daoud	CY
Argaki tis Pervolas	CY
Ayios Vasilios	CY
Palea Trapeza	CY
Mavrokolymbos	CY
Moutti tou Korakou	CY
Panayia Karkotissa	CY
Toumba tou Mikola	CY
Argaki tou Avlakou	CY
Ayios Yeoryios	CY
Palea	CY
Kipia	CY
Mavronera	CY
Kapsalos	CY
Vikla	CY
Vrysin Settlement	CY
Argaki ton Lakanon	CY
Argaki tou Mersiniona	CY
Prophitis Elias	CY
Pakhyammos	CY
Maratha	CY
Arkolia	CY
Ayios Pavlos	CY
Meletenes	CY
Ayiopetra	CY
Panayia	CY
Ayios Loukas	CY
Ayios Yeoryios	CY
Ayios Andreas	CY
Pouziaris	CY
Buffavento Forest	CY
Ayios Ioannis Prodhromos	CY
Ayios Yeoryios	CY
Ayios Ioannis Khrysostomos	CY
Khristos and Panayia Aphendrika	CY
Kokkina	CY
Ayia Photini	CY
Platy Vouno	CY
Kythrea Forest	CY
Mandra tou Salikhi	CY
Khoraphin tou Mavrou	CY
Vrakas	CY
Argaki tou Vlou	CY
Spiloudhin	CY
Ayios Romanos	CY
Kafkallophterka	CY
Panayia Absinthiotissa	CY
Kokkinokremmos	CY
Ayios Theodhoros	CY
Kafkalia	CY
Ayia Stephani	CY
Ayios Nikolaos	CY
Pervolies	CY
Hazreti Omer	CY
Ayios Yeoryios	CY
Akhelias	CY
Voulgaris	CY
Ayios Yeoryios	CY
Argaki ton Kammaron	CY
Teratsarkon tou Papa	CY
Panayia Potamitissa	CY
Ayios Antonios	CY
Ayios Photios	CY
Bellapais Abbey	CY
Vounaroudhia	CY
Stavros	CY
Lachin	CY
Chikkouri	CY
Pefkos	CY
Kazaphaniostrata	CY
Yiatri	CY
Pashoundanis	CY
Aspro Vounon	CY
Lakkos tou Leondi	CY
Potamos tis Trypas	CY
Pharangas	CY
Ayia Marina	CY
Ayios Yeoryios	CY
Kyrenia Road Forest	CY
Ayios Andronikos	CY
Argaki ton Magalon Khoraphion	CY
House D. Lipertis	CY
Panayia	CY
Ayia Mavra	CY
Kyrenia Shipwreck	CY
Kyrenia Castle	CY
Combonisi	CY
Ayios Grigorios	CY
Chingen Baghchesi	CY
Ayios Mamas	CY
Templestowe	CY
Argaki tou Yakimi	CY
Boghazi	CY
Aspri Moutti	CY
Dhikomo and Bellapais Forest	CY
Onisha tou Paskhali Chiftlik	CY
Ayia Marina	CY
Prophitis Zakharias	CY
Klimos tou Markou	CY
Panayia	CY
Arkakia	CY
Petra	CY
Arodhaphnes	CY
Bostanlikkin	CY
Kafkalia	CY
Prophitis Elias	CY
Ayios Yeoryios	CY
Ayios Grigorios	CY
Hajisolomou	CY
Alakati	CY
Panayia Phaneromeni	CY
Khomatovounon	CY
Paleoekklisia	CY
Kharniblik	CY
Churuk	CY
Kalava	CY
Ayia Varvara	CY
Panayia Glykyotissa	CY
Ayios Phanourios	CY
Chourouk Tarla	CY
Kokkinoyia	CY
Mandra tis Kojakaris	CY
Skalin	CY
Troullin	CY
Laxies tou Emiri	CY
Duz Alank	CY
Kapusan Chiftlik	CY
Boz Belenk	CY
Pash Tarla	CY
Kouphopetra	CY
Mazeri	CY
Potamoudhia	CY
Melissi	CY
Ayia Marina	CY
Ayios Pandeleimon	CY
Sotiros	CY
Palialona Cemetery	CY
Ayios Yeoryios	CY
Ayios Evstratios	CY
Phounji Chiftlik	CY
Sernikola	CY
Khrysotimithiotissa	CY
Kokkinoyia	CY
Syrmata	CY
Ayios Yeoryios	CY
Trashas	CY
Karnistos	CY
Ayios Nikolaos	CY
Kremmos	CY
Kalamoullin	CY
Kalamoulia Island	CY
Platani	CY
Kalamoullin	CY
Pikron Neron	CY
Ayios Yeoryios	CY
Pyrokremnos	CY
Kourtella	CY
Prophitis Elias Tower	CY
Karmi Forest	CY
Yormi	CY
Yaniklar	CY
Tamli Belenk	CY
Tari Yeri	CY
Semiz Tarla	CY
Plesha	CY
Kalapakki	CY
Toumbekli Yaka	CY
Gomaristra	CY
Mosphilia	CY
Ayios Pavlos	CY
Kyriako Tepesi	CY
Arkhangelos Mikhail	CY
Ayios Nikolaos	CY
Arkhangelos	CY
Ayios Petros and Pavlos	CY
Kalemboros	CY
Ayios Andreas	CY
Pigadhoula	CY
Lapithos/Lambousa City-Site	CY
Panayia Akhiropiitos Monastery	CY
Ayios Eulalios	CY
Mavrolimni	CY
Vathyrkakas	CY
Kamara	CY
Ayia Varvara	CY
Airkotissa	CY
Ayios Sotiros	CY
Ayia Ematousa	CY
Panayia Galaterousa	CY
Kephalovrysos	CY
Lapithos and Karavas Forest	CY
Lakkos tou Assa	CY
Ayia Evdhokia	CY
Melandrou	CY
Kafkalla	CY
Ayios Ermolaos	CY
Seppelegiz	CY
Yephyroudhia	CY
Kimistron	CY
Spitoudhin tou Pyrgou	CY
Kanjelia	CY
Stenomata	CY
Mazeri	CY
Melisha	CY
Krinema	CY
Ayios Gordhios	CY
Lonikos	CY
Moutti tou Dhaphniou	CY
Arkhangelos	CY
Katsouris	CY
Ayia Varvara	CY
Ayia Marina	CY
Vivai tou Parpa Cemetery	CY
Vasiliatis	CY
Panayia Akhrandon	CY
Lidhramies	CY
Trandaphyllia	CY
Arodhaphnoudhia	CY
Ayios Yeoryios	CY
Sina Monastery	CY
Sterakokremmos	CY
Korno-Vouno Forest	CY
Panayia Kriniotissa	CY
Korphi Forest	CY
Phones	CY
Panayia ton Katharon Monastery	CY
Troullia	CY
Skotini	CY
Tendes	CY
Argakia	CY
Plevra ton Mandron	CY
Kastrovounos	CY
Panayia	CY
Trakhonas	CY
Vounon tou Papa	CY
Skasmata	CY
Dhaphnia	CY
Ayios Photios	CY
Ayios Ioannis	CY
Yiannitsaros	CY
Mersini	CY
Dhyo Mouttes	CY
Mavrohoma	CY
Panagra	CY
Asprovounaron	CY
Margi Chiftlik	CY
Panayia	CY
Ayios Pandeleimon Monastery	CY
Timios Stavros	CY
Pigadhes Settlement	CY
Vathypotamos	CY
Tsaeroudhia	CY
Kochinoyi	CY
Tremithia	CY
Ayios Yeoryios	CY
Styllomenos	CY
Karpasha Forest	CY
Ayia Marina	CY
Dhiorios Forest	CY
Livadhia	CY
Titsiri	CY
Apostolos Andreas	CY
Masties	CY
Porneti	CY
Yerondas	CY
Ayios Yeoryios	CY
Kapsaloudhia	CY
Ayia Martha	CY
Ayia Mavri	CY
Mandres Forest	CY
Pyrgos	CY
Stiadhkia	CY
Vokolomandia	CY
Mosphilia	CY
Rakhi tis Kamilas	CY
Dhiorios Forest	CY
Pedellia	CY
Makhania Chiftlik	CY
Arkhangelos Mikhail	CY
Prophitis Elias	CY
Panayia Monastery	CY
Panayia	CY
Veria	CY
Kokkinokremmos	CY
Kormakiti	CY
Nava	CY
Mandra	CY
Stiadhkia	CY
Angoulos	CY
Angoulos Chiftlik	CY
Aloupos Chiftlik	CY
Makhania	CY
Ayios Pandeleimon Forest	CY
Stoman tou Aloupou	CY
Merrastis Plathkias Pallouras	CY
Kattoudheri	CY
Ayia Irini Forest	CY
Livadhkia	CY
Laxia ton Phillichon	CY
Kryphtes	CY
Kochinokhoma	CY
Paleokastro City-Site	CY
Argaki tou Paleokastrou	CY
Kormakiti Forest Block Number 1	CY
Kipos	CY
Mylos	CY
Arkapparos	CY
Ayios Chiftlik	CY
Ayios Basilica	CY
Phinikia	CY
Papavasilis	CY
Kormakiti Forest Block Number 2	CY
Pakhyammos	CY
Sherissa Chiftlik	CY
Ayios Ioannis Prodhromos	CY
Khoraphia tou Theli	CY
Merijin	CY
Mala	CY
Kolia Chiftlik	CY
Kormakiti Forest Block Number 5	CY
Kormakiti Forest Block Number 3	CY
Kormakiti Forest Block Number 4	CY
Phonin	CY
Pano Ghalala Basilica	CY
Ayios Yeoryios	CY
Kava	CY
Shirmala	CY
Laotrypes	CY
Mersinia	CY
Ayios Konstantinos and Ayia Eleni	CY
Kamolika	CY
Limanoudhi Bay	CY
Kormakiti Island	CY
Vikles	CY
Stavros	CY
Yappi	CY
Vounari tou Adhkiou	CY
Markoullos	CY
Stavros	CY
Laxies	CY
Ayia Varvara	CY
Ayia Philidhiotissa	CY
Ayios Epiphanios	CY
Ayia Marina	CY
Limbourka	CY
Kapsalia	CY
Sklinikoudhia	CY
Aphendika	CY
Stavros	CY
Skasmata	CY
Livadhi	CY
Ayia Ekaterini	CY
Ayia Marina	CY
Dhekaokto Skales	CY
Panayia Pallouriotissa	CY
Barsak	CY
Ayia Marina	CY
Ayios Antipas	CY
Ambelia	CY
Mazera	CY
Dhromos tis Jirkas	CY
Ayios Prodhromos	CY
Kimistra	CY
Kokkinokolymbos	CY
Ayia Thekla	CY
Kameni Laxia	CY
Stavri	CY
Kokkines	CY
Aspra Phteri	CY
Kamini	CY
Pillakas	CY
Rocha	CY
Aspromoutti	CY
Phlanja	CY
Aloupophournos	CY
Kamilopetra	CY
Mandra tou Neophytou	CY
Dhya Petres	CY
Kokkinadhia	CY
Rothesha	CY
Mandra tou Hajilouka	CY
Mandra tou Tsimbitou	CY
Almyros	CY
Toumba tou Lakkotrypi	CY
Laxies tou Vouniou	CY
Meliou Chiftlik	CY
Landes	CY
Alakatin	CY
Paliokklisha	CY
Ayios Theodhoros	CY
Kuru Dere	CY
Bakhar Oglou	CY
Mandra tou Mevliti	CY
Ayios Yeoryios	CY
Karapetros	CY
Landes Makries	CY
Ayios Yeoryios	CY
Kharangas	CY
Kara Tepe	CY
Kaloyiros	CY
Arnologos	CY
Almyros	CY
Ouzoum Laxia	CY
Bostanlikia	CY
Kokkinoyi	CY
Mia Milea Dams	CY
Ayios Yeoryios	CY
Vounaros tis Palioekklishas	CY
Phouryistos	CY
Skouroupathos	CY
Armeni	CY
Asprokremmos	CY
Papayeoryi	CY
Varkies	CY
Hajandonoudhia	CY
Potamoudhia	CY
Spathies	CY
Landes	CY
Ayios Dhimitrios	CY
Vournes	CY
Loukkomatis	CY
Orie	CY
Xomoutta	CY
Kokkinoyes	CY
Aspropamboulo	CY
Kolymbes	CY
Philarcha	CY
Almyrokolymbi	CY
Vathys	CY
Aronas	CY
Ayios Yeoryios	CY
Kathari	CY
Kaphizin	CY
La Cava Castle	CY
Argakas	CY
Athalassa Forest	CY
Athalassa Dam	CY
Kaourotrypa	CY
Pouyeros	CY
Vounaroudhia	CY
Pallourokambos	CY
Mazeres	CY
Athanasis	CY
Teratsidhia	CY
Kolymbos tou Cholaki	CY
Kimistra tou Yerolemou	CY
Yephyrka	CY
Lakkoudhia	CY
Dhimmata	CY
Laxia ton Kattoudhion	CY
Kokkinokolymbi	CY
Makries Mixes	CY
Ridhes	CY
Laxia tou Louvsmenou	CY
Kattoudhia	CY
Ayios Sozomenos	CY
Ayios Mamas	CY
Dhrakondospilios	CY
Gyrilladhia	CY
Peyaz Tepe	CY
Phtelia	CY
Laoudhera	CY
Jeleftis	CY
Arkosyja	CY
Arkhangelos	CY
Ayios Yeoryios tou Alykou	CY
Ayios Theodhoros	CY
Ayios Dhimitrianos	CY
Limbourena	CY
Ayios Mamas	CY
Ayios Eftykhios	CY
Ayios Yeoryios	CY
Ayii Apostoli	CY
Vasilika	CY
Kokkinovrysi	CY
Asproamboulos	CY
Yerokarka	CY
Ayia Paraskevi	CY
Asprokremmos	CY
Pharaklos	CY
Koukourtis	CY
Sierkas	CY
Syrimia	CY
Kaminia	CY
Aspros	CY
Chiftlikoudhin	CY
Kalogrea	CY
Angalin	CY
Almyros	CY
Mandres	CY
Ammos	CY
Aloupotrypa	CY
Katsokhiri	CY
Kandara	CY
Mavrokremmos	CY
Tria Mouttaria	CY
Kakkaristra	CY
Asprokhomata	CY
Asprokhomata	CY
Jinarota	CY
Koukos	CY
Ayia Photini	CY
Laxia San	CY
Ayia Marina	CY
Mazera	CY
Kaloyeros	CY
Mavrospilios	CY
Kamiles	CY
Koliokremmos	CY
Mouttokholetrin	CY
Dhyo Kaminia	CY
Mandolaxia	CY
Kimistra	CY
Ledra City-Site	CY
Bimbashi Chiftlik	CY
Mintzeli Bridge	CY
Khalomandra	CY
Hesna Katin	CY
Kafkalla	CY
Elies tis Yeoryinas	CY
Mandres	CY
Bostanlik	CY
Makrokolymbos	CY
Aloupotrypa	CY
Merras	CY
Makrorashi	CY
Almyros	CY
Kurin Tepesi	CY
Kurtu Tepsi	CY
Geunyeli Dam	CY
Eski Mandra	CY
Kerinio Strata	CY
Katsarona	CY
Saranda Skales	CY
Landa Tepesi	CY
Ghaziveran	CY
Jinnar	CY
Buyuk Ova	CY
Media Chiftlik	CY
Lishines	CY
Korona	CY
Mandroudhia	CY
Vounaria tous Pellous	CY
Koudhellopetres	CY
Pernera	CY
Cyprus Telecommunications Authority	CY
Laxia tou Avraami	CY
Laxia tou Konstandi	CY
Ayios Mamas	CY
Khales	CY
Argaki tis Mandissas	CY
Kioroglou Chiftlik	CY
Ayios Yeoryios	CY
Kokkines	CY
Pefkos	CY
Argaki ton Nomikou	CY
Strakka	CY
Kokkines	CY
Glyphos	CY
Asproyiadhes	CY
Kephales	CY
Kokkinomoutti	CY
Mazeradhes	CY
Plymanda	CY
Mesovounos	CY
Platynarin	CY
Kakasangas	CY
Rexmo	CY
Pyros	CY
Argaki tou Pyros	CY
Mazovounos	CY
Ayia Marina	CY
Argaki ton Mersinikion	CY
Petres	CY
Psoumnidhes	CY
Argaki ton Galinion	CY
Khrysosotiros	CY
Asproyi	CY
Adhkiakomoutti	CY
Argaki tis Asproyis	CY
Ayia Mavri	CY
Argaki tou Petrati	CY
Panayia Eleousa	CY
Pano Vouno	CY
Arodhaphni	CY
Yerovounos	CY
Moutti	CY
Stavropetra	CY
Kamini	CY
Ayiatika	CY
Khrysospilliotissa	CY
Laxia tou Mita	CY
Kolymbia	CY
Vouniotika	CY
Pano Petalia	CY
Kapsaleri	CY
Asprokremmos	CY
Kokkinokafkalla	CY
Petalia	CY
Troullin	CY
Kafkallokambos	CY
Nicosia International Airport	CY
Kafkallin tis Ayias	CY
Kandara tou Mavrospiliou	CY
Khalopetres tou Mandrika	CY
Pervolta tou Aivaz	CY
Pasha Belenk	CY
Aspropamboulos	CY
Vareta	CY
Ammidhes tou Moustafa	CY
Spitoudhia	CY
Pambajera	CY
Phournos	CY
Jepheridhes	CY
Kafkalia	CY
Vounia	CY
Kara Tepe	CY
Chakili Ova	CY
Laxies	CY
Haji Batakji Chiftlik	CY
Kanli Dam	CY
Yanik Kaya	CY
Zyyi	CY
Zoopiyi	CY
Zakharou	CY
Zakharia	CY
Zakaki	CY
Ypsonas	CY
Yiouti	CY
Yiolou	CY
Yialousa	CY
Yialias	CY
Yialia	CY
Yerovasa	CY
Yeroskipos	CY
Yeronisos	CY
Yerolakkos	CY
Potamos tis Yermasoyia	CY
Yermasoyia	CY
Yeri	CY
Yerasa	CY
Cape Yeranisos	CY
Yerani	CY
Yerakies	CY
Yenagra	CY
Yailas	CY
Xylotymbou	CY
Xyliatos	CY
Xylias	CY
Xerovounos	CY
Xeros	CY
Xeros	CY
Xeros	CY
Xeropotamos	CY
Xeros Potamos	CY
Xeri	CY
Vroisha	CY
Vrecha	CY
Vouno	CY
Vouni Palace	CY
Vouni	CY
Voroklini	CY
Voni	CY
Vokolidha	CY
Vyzakia	CY
Vitsadha	CY
Vourkaris	CY
Vikla	CY
Vavla	CY
Vavilas	CY
Vavilas	CY
Vavatsinia	CY
Vatili	CY
Vathylakas	CY
Vasilikos	CY
Vasilikos	CY
Vasilikos	CY
Vasilia	CY
Vasili	CY
Vasa	CY
Vasa	CY
Varosha	CY
Varisha	CY
Valia Forest	CY
Tymbou	CY
Tsadha	CY
Trypimeni	CY
Trypa tou Vounou	CY
Troulli	CY
Troodos	CY
Troodos Forest	CY
Troodos	CY
Troodhitissa Monastery	CY
Tris Elies	CY
Tripylos	CY
Trimithousa	CY
Trimithousa	CY
Trimithi	CY
Trimiklini	CY
Trikoukkia	CY
Trikomo	CY
Tremithos	CY
Tremethousha	CY
Trapeza	CY
Trakhypedhoula	CY
Trakhoni	CY
Trakhonas	CY
Tokhni	CY
Timi	CY
Tillyria	CY
Thermia	CY
Theletra	CY
Tersephanou	CY
Terra	CY
Tembria	CY
Temblos	CY
Tavros	CY
Tala	CY
Syrkatis	CY
Syrianokhori	CY
Syngrasis Reservoir	CY
Syngrasis	CY
Sykopetra	CY
Sykhari	CY
Styllos	CY
Strovolos	CY
Stroumbi	CY
Strongylos	CY
Steni	CY
Stazousa Point	CY
Stavrovouni Monastery	CY
Stavros	CY
Stavromeni	CY
Stavrokono	CY
Statos	CY
Spitali	CY
Spilia	CY
Spathariko	CY
Souskiou	CY
Sotira	CY
Sotira	CY
Sophtadhes	CY
Soli City-Site	CY
Skouriotissa Mine	CY
Skouriotissa	CY
Skoulli	CY
Skylloura	CY
Skarinou	CY
Sisklipos	CY
Sinda	CY
Sina Oros	CY
Sina Oros	CY
Simou	CY
Silikou	CY
Shemmos	CY
Sha	CY
Setrakhos	CY
Serrakhis	CY
Selain T’appi	CY
Selladhi tou Petrou	CY
Selemani	CY
Scala	CY
Saranta Spilii	CY
Sarandi	CY
Sarama River	CY
Sarama	CY
Sanidha	CY
Sandalaris	CY
Salt Lake	CY
Salamis/Konstantia City-Site	CY
Salamiou	CY
Saittas	CY
Saint Hilarion Castle	CY
Rotsos ton Skaphon	CY
Ronnas Bay	CY
Rizokarpaso	CY
Pyroi	CY
Pyrgos	CY
Pyrga	CY
Pyla	CY
Potamos tou Pyrgou	CY
Psimolofou	CY
Psyllatos	CY
Psevdhas	CY
Psematismenos	CY
Psathi	CY
Prophitis Elias Monastery	CY
Prophitis Elias	CY
Prodhromos	CY
Prodhromi	CY
Pretori	CY
Prastio	CY
Prastion	CY
Prastio	CY
Prastio	CY
Prastio	CY
Pouzis	CY
Potima Chiftlik	CY
Potamitissa	CY
Potamiou	CY
Potamia	CY
Potami	CY
Pomos Point	CY
Pomos	CY
Polystipos	CY
Politiko	CY
Polis	CY
Polemi	CY
Platymatis	CY
Platanistasa	CY
Platanisso	CY
Plataniskia	CY
Platania	CY
Platani	CY
Cape Plakoti	CY
Piyi	CY
Piyenia	CY
Pitargou	CY
Pissouri Bay	CY
Pissouri	CY
Pileri	CY
Phterikoudhi	CY
Phterykha	CY
Phrenaros	CY
Phourni Rock	CY
Photta	CY
Phlamoudhi	CY
Phiti	CY
Phinikas	CY
Phinikaria	CY
Phini	CY
Philousa	CY
Philousa	CY
Philia	CY
Phileyia	CY
Philani	CY
Phikardhou	CY
Phasoula	CY
Phasoula	CY
Phasli	CY
Pharmakas	CY
Phanos	CY
Phalia	CY
Peyia	CY
Petrophani	CY
Petra tou Romiou	CY
Cape Limniti	CY
Petra tou Dhiyeni	CY
Petra	CY
Petounda Point	CY
Perivolia tou Trikomou	CY
Perivolia	CY
Peristeronari	CY
Potamos tis Peristeronas	CY
Peristerona	CY
Peristerona	CY
Peristerona	CY
Pergamos	CY
Perapedhi	CY
Perakhorio	CY
Pera	CY
Pendadhaktylos	CY
Pendayia	CY
Pendaskhinos	CY
Pendalia	CY
Pendakomo	CY
Pelendria	CY
Pelathousa	CY
Pedhoulas	CY
Pedhieos	CY
Patriki	CY
Parsata	CY
Parekklisha	CY
Paramytha	CY
Paramali	CY
Paralimni Lake	CY
Paralimni	CY
Papoutsa	CY
Paphos Point	CY
Paphos Forest	CY
Eparchía Páfou	CY
Paphos	CY
Pano Zodhia	CY
Pano Polemidhia	CY
Pano Platres	CY
Pano Phlasou	CY
Pano Panayia	CY
Pano Lefkara	CY
Pano Lakatamia	CY
Pano Koutraphas	CY
Pano Kividhes	CY
Pano Dhikomo	CY
Pano Dheftera	CY
Pano Arodhes	CY
Pano Arkhimandrita	CY
Pano Amiandos	CY
Pano Akourdhalia	CY
Panayia	CY
Panagra	CY
Pamboulos	CY
Pallouriotissa	CY
Palodhia	CY
Palloura	CY
Paleosophos	CY
Paleomylos	CY
Paleometokho	CY
Palekythro	CY
Palekhori	CY
Palea Paphos	CY
Pakhyammos	CY
Pakhyammos	CY
Pakhna	CY
Oxo Island	CY
Ovgos	CY
Ovgoros	CY
Orta Keuy	CY
Orounda	CY
Ornithi	CY
Ormidhia	CY
Orga	CY
Ora	CY
Omorphita	CY
Omodhos	CY
Mount Olympus	CY
Olymbos	CY
Odhou	CY
Nisou	CY
Manijin	CY
Nisarka	CY
Nikos	CY
Nikoklia	CY
Nikitas	CY
Nikitari	CY
Eparchía Lefkosías	CY
Nicosia	CY
Neta	CY
Neokhorio	CY
Neo Chorio	CY
Kato Paphos	CY
Nata	CY
Nangomi Bay	CY
Myrtou	CY
Mirmikoph	CY
Moutoullas	CY
Moutayiaka	CY
Mousoulita	CY
Mousere	CY
Moulia Rocks	CY
Motidhes	CY
Mosphiloti	CY
Mosphileri	CY
Morphou Bay	CY
Morphou	CY
Morosyko	CY
Moronero	CY
Mora	CY
Argaki tis Monis	CY
Moniatis	CY
Moni	CY
Monarga	CY
Monagroulli	CY
Monagri	CY
Mitsero	CY
Miliou	CY
Milikouri	CY
Milia	CY
Milea	CY
Mia Milea	CY
Messa Nisi	CY
Mesoyi	CY
Mesapotamos Monastery	CY
Mesayitonia	CY
Mesaoria	CY
Mesana	CY
Mesa Khorio	CY
Mersiniki	CY
Menoyia	CY
Meneou	CY
Melousha	CY
Melissakros Point	CY
Melini	CY
Melandra	CY
Melanarga	CY
Melamiou	CY
Meladhia	CY
Mazotos	CY
Mazokambos	CY
Mazaki Island	CY
Mavrovouni Mine	CY
Mavron Oros Forest	CY
Mavroli	CY
Mavrokolymbos	CY
Mathikoloni	CY
Mathiatis	CY
Masari	CY
Maroullenas	CY
Potamos tou Ayiou Mina	CY
Maroni	CY
Marona	CY
Mari	CY
Marko	CY
Marki	CY
Marathovouno	CY
Marathounda	CY
Maratha	CY
Mansoura	CY
Mandria	CY
Mandria	CY
Mandres	CY
Mandres	CY
Mandres	CY
Mamoundali	CY
Mamonia	CY
Mammari	CY
Melounda	CY
Malounda	CY
Malounda	CY
Mallia	CY
Makrasyka	CY
Panayia tou Makhera	CY
Makedhonitissa Monastery	CY
Potamos tis Magoundas	CY
Magounda	CY
Mount Adelphi	CY
Lythrodhondas	CY
Lythrangomi	CY
Lysos	CY
Lysi	CY
Lymbia	CY
Louvaras	CY
Loutros	CY
Louroujina	CY
Loukrounou	CY
Lorovouno	CY
Lophos	CY
Liveras	CY
Livadhia	CY
Livadhia	CY
Livadhia	CY
Livadhi	CY
Livadhi	CY
Liopetri	CY
Linou	CY
Potamos tou Limniti	CY
Limnitis	CY
Limni Mine	CY
Limnia	CY
Limnatis	CY
Limassol Forest	CY
Eparchía Lemesoú	CY
Limassol	CY
Letimbou	CY
Leonarisso	CY
Lemona	CY
Lemithou	CY
Lembos	CY
Lemba	CY
Lefkoniko	CY
Lefka	CY
Lazania	CY
Layia	CY
Laxia	CY
Lasa	CY
Larnaka	CY
Larnaca District	CY
Larnaca Bay	CY
Larnaca	CY
Lara Point	CY
Lara	CY
Lapithos	CY
Lapithiou	CY
Lapathos	CY
Lania	CY
Lagoudhera	CY
Lachi	CY
Kythrea	CY
Pendadhaktylos Range	CY
Eparchía Kerýneias	CY
Kyrenia	CY
Kyra	CY
Kyperounda	CY
Kyparissovounon	CY
Kyparissia Potamos	CY
Kyparisha	CY
Metokhion Kykkou	CY
Panayia tou Kykkou Monastery	CY
Ktima	CY
Kryos	CY
Kritou Terra	CY
Kritou Marottou	CY
Krini	CY
Kridhia	CY
Kremama Kamilou	CY
Koutsovendis	CY
Kourtaka	CY
Kourou Monastir	CY
Kourdhali	CY
Kouklia Reservoir	CY
Kouklia	CY
Kouklia	CY
Kouka	CY
Korphi	CY
Korovia	CY
Kornos	CY
Kornos	CY
Kornokipos	CY
Cape Kormakiti	CY
Kormakiti	CY
Korakou	CY
Koppo Island	CY
Kophinou	CY
Konia	CY
Kondemenos	CY
Kondea	CY
Komi Kebir	CY
Koma tou Yialou	CY
Kolossi	CY
Koloni	CY
Kolokoshi	CY
Kokkinorotsos	CY
Kokkini Trimithia	CY
Kokkina Point	CY
Kokkina	CY
Kochati	CY
Knodhara	CY
Klonari	CY
Klirou	CY
Klidhes Islands	CY
Klepini	CY
Klemos	CY
Klavdhia	CY
Kivisil	CY
Kition City-Site	CY
Cape Kiti	CY
Kiti	CY
Kithasi	CY
Kissousa	CY
Kissonerga	CY
Makheras	CY
Kinousa	CY
Kilinia	CY
Kili	CY
Kilani	CY
Kilanemos	CY
Panayia Khrysorroyiatissa Monastery	CY
Khrysokhou Bay	CY
Potamos tou Stavrou tis Psokas	CY
Khrysokhou	CY
Khrysiliou	CY
Khrysidha	CY
Khoulou	CY
Khoti Island	CY
Khondrovounaros	CY
Kholi	CY
Kholetria	CY
Khlorakas	CY
Khirokitia	CY
Khiomi Point	CY
Khelones	CY
Khelones City-Site	CY
Kharcha	CY
Kharangas	CY
Khapotami	CY
Khandria	CY
Khamilis Island	CY
Khaleri	CY
Khalassa	CY
Keumurju	CY
Keradhidi	CY
Kephalovryso	CY
Kelokedhara	CY
Kellia	CY
Kellaki	CY
Kedhares	CY
Ozanköy	CY
Katydhata	CY
Kato Zodhia	CY
Kato Yialia	CY
Katouris	CY
Pyrga	CY
Kato Pyrgos	CY
Kato Polemidhia	CY
Kato Platres	CY
Kato Phlasou	CY
Kato Panayia	CY
Kato Mylos	CY
Kato Moni	CY
Kato Lefkara	CY
Kato Lakatamia	CY
Kato Koutraphas	CY
Kato Kopia	CY
Kato Kividhes	CY
Kato Dhrys	CY
Kato Dhikomo	CY
Kato Dheftera	CY
Kato Arodhes	CY
Kato Arkhimandrita	CY
Kato Amiandos	CY
Kato Akourdhalia	CY
Kathikas	CY
Kataliondas	CY
Argaki tou Kastrou	CY
Karyotis	CY
Kartal Dagh Forest	CY
Karpas Peninsula	CY
Karpasia	CY
Karpasha	CY
Karmi	CY
Karavostasi	CY
Karavopetra	CY
Karavas	CY
Karaolos Plantation	CY
Karamoullidhes	CY
Karakoumi	CY
Kapsalon River	CY
Kapoura	CY
Kapilio	CY
Kaphiris	CY
Kapedhes	CY
Kandara Castle	CY
Kandara	CY
Kannoudhion Island	CY
Kannaviou	CY
Kannavia	CY
Kanli	CY
Kandou	CY
Kaminaria	CY
Kambyli	CY
Potamos tou Kambou	CY
Kambos	CY
Kambia	CY
Kambi	CY
Kalyvakia	CY
Kalopsidha	CY
Kalopanayiotis	CY
Kalokhorio	CY
Kalokhorio	CY
Kalokhorio	CY
Kalokhorio	CY
Kalokhorio	CY
Kalogrea	CY
Kallepia	CY
Kaliana	CY
Kalavasos Mine	CY
Kalavasos	CY
Kalamoulia	CY
Kakoskala	CY
Kakopetria	CY
Kaimakli	CY
Jubilee Shoal	CY
Jila Island	CY
Jemali Bridge Junction	CY
Istinjon	CY
Inia	CY
Idhalion City-Site	CY
Halevga	CY
Hala Sultan Tekke	CY
Gypsos	CY
Cape Greco	CY
Gourri	CY
Gouphes	CY
Goudhi	CY
Goshi	CY
Glykyotissa Island	CY
Ghaziveran	CY
Geunyeli	CY
Gastria Bay	CY
Gastria	CY
Garyllis	CY
Galounopetra Point	CY
Galounia	CY
Galounia City-Site	CY
Galinoporni	CY
Galini	CY
Galatia	CY
Galataria	CY
Galata	CY
Gaidhouras	CY
Fontana Amorosa	CY
Eparchía Ammochóstou	CY
Famagusta Bay	CY
Famagusta	CY
Potamos tis Ezousas	CY
Eylenja	CY
Exometokhi	CY
Exarkhos Bay	CY
Evrykhou	CY
Evretou	CY
Evretou	CY
Evdhimou	CY
Erimi	CY
Episkopion	CY
Episkopi Bay	CY
Episkopi	CY
Episkopi	CY
Epikho	CY
Ephtakomi	CY
Ephtagonia	CY
Engomi	CY
Engomi	CY
Emba	CY
Mavroyi	CY
Ayii Eliophoti	CY
Eledhiou	CY
Cape Elaia	CY
Potamos tis Elias	CY
Elea	CY
Elea	CY
Cape Drepanum	CY
Dhrapia	CY
Cape Dolos	CY
Dhyo Potami	CY
Dhymes	CY
Dhrymou	CY
Dhrousha	CY
Dhromolaxia	CY
Dhrinia	CY
Dhoros	CY
Dhora	CY
Dhiorios	CY
Dhierona	CY
Dhiarizos	CY
Dherinia	CY
Dhenia	CY
Dhavlos	CY
Dhali	CY
Derin Dere	CY
Delikipo	CY
Republic of Cyprus	CY
Cyprus	CY
Chumlekji Chiftlik	CY
Cherkez	CY
Chatos	CY
Chakistra	CY
Buffavento Castle	CY
Boghaz	CY
Black Rock	CY
Bey Keuy	CY
Bellapais	CY
Baths of Aphrodhite	CY
Ayios Yeoryios Island	CY
Ayios Yeoryios Bay	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Yeoryios	CY
Ayios Vasilios	CY
Ayios Tykhonas	CY
Ayios Thyrsos	CY
Ayios Thomas	CY
Ayios Therapon	CY
Ayios Theodhoros	CY
Ayios Theodhoros	CY
Ayios Theodhoros	CY
Ayios Theodhoros	CY
Ayios Theodhoros	CY
Ayios Symeon	CY
Ayios Sozomenos	CY
Ayios Seryios	CY
Ayios Savvas	CY
Ayios Photios	CY
Ayios Phylon	CY
Ayios Pavlos	CY
Ayios Nikolaos	CY
Ayios Nikolaos	CY
Ayios Nikolaos tis Steyis	CY
Ayios Nikolaos	CY
Ayios Neophytos Monastery	CY
Ayios Merkourios	CY
Ayios Memnon	CY
Ayios Mamas	CY
Ayios Loukas	CY
Ayios Konstantinos	CY
Ayios Khariton	CY
Ayios Isidhoros	CY
Ayios Ioannis	CY
Ayios Ioannis	CY
Ayios Ioannis	CY
Ayios Iakovos	CY
Ayios Efstathios	CY
Ayios Ermolaos	CY
Ayios Epiphanios	CY
Ayios Epiphanios	CY
Ayios Epiktitos	CY
Ayios Elias	CY
Ayios Dhometios	CY
Ayios Dhimitrios	CY
Ayios Dhimitrianos	CY
Ayios Athanasios	CY
Ayios Andronikos	CY
Ayios Andronikos	CY
Ayios Amvrosios	CY
Ayios Amvrosios	CY
Ayion Yeorgoudhi	CY
Ayii Vavatsinias	CY
Ayii Trimithias	CY
Ayii Omoloyitadhes	CY
Ayia Varvara	CY
Ayia Varvara	CY
Ayia Trias	CY
Ayia Phyla	CY
Ayia Napa	CY
Ayia Marinoudha	CY
Ayia Marina	CY
Ayia Marina	CY
Ayia Marina	CY
Ayia Marina	CY
Ayia Irini	CY
Ayia Irini	CY
Ayia Anna	CY
Ayia	CY
Ayia	CY
Axylou	CY
Avlona	CY
Avgorou	CY
Avgas	CY
Avgalidha	CY
Avdhellero	CY
Atsas	CY
Athrakos	CY
Athna	CY
Athienou	CY
Athalassa	CY
Astromeritis	CY
Asproyia	CY
Aspros	CY
Cape Aspro	CY
Asomatos	CY
Asomatos	CY
Askas	CY
Asinou	CY
Asinou	CY
Asha	CY
Asgata	CY
Artemi	CY
Arsos	CY
Arsos	CY
Cape Arnauti	CY
Arnadhi	CY
Armou	CY
Arminou	CY
Armenokhori	CY
Armenian Monastery	CY
Arkhangelos Mikhail Monastery	CY
Ergates	CY
Argaki	CY
Argaka	CY
Ardhana	CY
Arakapas	CY
Aradhippou	CY
Aradhiou	CY
Apsiou	CY
Apostolos Varnavas Monastery	CY
Apostolos Andreas Monastery	CY
Cape Apostolos Andreas	CY
Apliki	CY
Apliki	CY
Aplanda	CY
Apis	CY
Panayia Afendrika	CY
Aphania	CY
Apesha	CY
Antiphonitis Monastery	CY
Anoyira	CY
Angolemi	CY
Anglisidhes	CY
Angastina	CY
Androlikou	CY
Anayia	CY
Anavargos	CY
Anarita	CY
Anaphotia	CY
Analjondas	CY
Anadhiou	CY
Ambelikou	CY
Amathus City-Site	CY
Amargeti	CY
Ammadhies	CY
Aloupos	CY
Alona	CY
Aloa	CY
Alithinou	CY
Alykos	CY
Alevga	CY
Alethriko	CY
Alekhtora	CY
Alaminos	CY
Alambra	CY
Akrounda	CY
Akrotiri Bay	CY
Akoursos	CY
Akhyritou Reservoir	CY
Akhyritou	CY
Akhiropiitos Point	CY
Akhelia	CY
Akapnou	CY
Akanthou	CY
Akamas Forest	CY
Akamas	CY
Potamos tou Akakiou	CY
Akaki	CY
Agros	CY
Agroladhou	CY
Agrokipia	CY
Agridhia	CY
Agridhaki	CY
Aghirda	CY
Adelphi Forest	CY
Dhekelia British Military Cemetary	CY
Agridhia	CY
Ayios Savvas	CY
Kephalovryso	CY
Karpaseia	CY
Lakatamia	CY
Spilia	CY
Makrasyka	CY
Ayios Yeoryios	CY
Ayia	CY
Ayios Epiktotes	CY
Teknecik Elektrik Santralı	CY
Geçitköy Göleti	CY
Şirinevler Göleti	CY
Yalı	CY
Yalısu Göleti	CY
Mersinlik Göleti	CY
Akamas Range	CY
Lefke Göleti	CY
Moutti tou Dhia	CY
Meniko	CY
Köşklü Çiftlik	CY
Yenişehir	CY
Marmara	CY
Kumsal	CY
Çağlayan	CY
Aghirda	CY
Kato Mandres	CY
Sarıklı Tepe	CY
Sendarlı Göleti	CY
Famagusta Walled City	CY
Karakol	CY
Dumlupınar	CY
Baykal	CY
Namık Kemal	CY
Canbulat	CY
Kamilpaşa	CY
Pertevpaşa	CY
Karakol Burnu	CY
Toxéftra Beach	CY
Avgás Gorge	CY
Kerati	CY
Mesani	CY
Nea Paphos City-Site	CY
Mazi	CY
Tsarta	CY
Moummouros	CY
Cedar Valley	CY
Alonoudi Junction	CY
Uzundere Göleti	CY
Dolunay Göleti	CY
Çataldere Göleti	CY
Bostundere	CY
Bostundere Göleti	CY
Serdarlı Göleti	CY
Eğridere Göleti	CY
Ergazi Göleti	CY
Uyku Tepesi	CY
Xeros Potamos	CY
Xistaroudi Junction	CY
Caledonian Falls	CY
Presidential Cottage	CY
Olympus Range	CY
Sun Valley Ski Lift	CY
Amiandos Asbestos Mine	CY
West Shoulder	CY
Makriá Kondárka	CY
Paphos International Airport	CY
Sinoas	CY
Connecting Point	CY
Platies	CY
Kourion Stadium	CY
Papavounos	CY
Sikorin	CY
Governor’s Beach	CY
St Raphael Marina	CY
Limassol	CY
Limassol Pier	CY
Akrotiri Harbour	CY
Larnaca International Airport	CY
Dades Point	CY
Larnaca Marina	CY
Larnaca	CY
Evdhimiou West Locality	CY
Melanda Locality	CY
Paramali	CY
Evdhimou East Locality	CY
Sotira West Locality	CY
Sotira East Locality	CY
Agioi Anargyroi Monastery	CY
Kokkina	CY
Pambuli Adası	CY
Glossa Burnu	CY
Yeniizmir	CY
Sakarya	CY
Yenişehir	CY
Ayia Irini	CY
Ayii Apostoli	CY
Ayios Nikolaos	CY
Ayios Yeoryios Peyias	CY
Ercan	CY
Athalassa	CY
Alion Beach Hotel	CY
Adams Beach Hotel	CY
GrandResort	CY
Londa Hotel	CY
Louis Phaethon Beach Hotel	CY
Four Seasons Hotel	CY
Almyra	CY
Hilton Park Nicosia	CY
Nissi Beach Hotel	CY
St Raphael Resort	CY
The Annabelle	CY
Atlantica Aeneas Hotel	CY
Avlida	CY
Paphos Gardens	CY
Bella Napa	CY
Holiday Inn	CY
Cyprotel Marathon Beach Hotel	CY
Elias Beach Hotel and Country Club	CY
Le Méridien Limassol Spa & Resort	CY
Aliathon Holiday Village	CY
Queen's Bay Hotel	CY
Beau Rivage Beach Hotel and Club	CY
Palm Beach	CY
Louis Princess Beach	CY
Pavlo Napa Beach	CY
Paphos Amathus Beach	CY
Azia Beach	CY
Alexander The Great	CY
Luca Cypria Florida	CY
Anesis	CY
Louis Imperial Beach	CY
Dome Beach Resort	CY
Riu Cipria Maris	CY
Luca Cypria Laura Beach	CY
Grecian Park Hotel	CY
Lordos Beach	CY
Sandy Beach	CY
Stamatia	CY
Capo Bay	CY
Castelli	CY
Golden Bay	CY
Iberostar Ledra Beach	CY
Louis Phaethon Beach	CY
Coral Beach	CY
Louis Princess Beach Hotel	CY
Lordos Beach Hotel	CY
Mediterranean Beach Hotel	CY
Golden Tulip Golden Bay Beach Hotel	CY
Mayfair Hotel & Apartments	CY
Eleni Tourist Village	CY
Kermia Beach Bungalow Hotel	CY
Athena Beach Hotel	CY
Pioneer Beach Hotel	CY
Athena Royal Beach Hotel	CY
Paphian Bay Hotel	CY
Antigoni Hotel	CY
Paramount Hotel Apartments	CY
Sanctanapa Hotel	CY
Atlantica Sungarden Beach Hotel	CY
Nestor Hotel	CY
Thalassines Beach Villas	CY
Helios Bay Hotel Apts	CY
Thalassa Boutique Hotel & Spa	CY
Faros Hotel	CY
Venus Beach Hotel	CY
Artemis Hotel Apartments	CY
Columbia Beach Hotel	CY
Kefalo Beach Holiday Village	CY
Sun Hall Hotel	CY
Michael's Beach Hotel Apartments	CY
Columbia Beach Resort	CY
Anastasia Beach Hotel	CY
Pernera Beach Hotel	CY
Hilton Cyprus	CY
Odessa Hotel	CY
Malama Holiday Village	CY
Azia Club and Spa at Azia Resort	CY
Senator Hotel Apartments	CY
Atlantica Miramare Beach	CY
Crown Resorts Horizon Hotel	CY
Nelia Beach	CY
King Richard Beach	CY
Capo Bay Hotel	CY
Louis Imperial Beach	CY
Cyprotel Poseidonia Beach Hotel	CY
Pavlonapa Hotel	CY
Napa Mermaid Hotel	CY
Apollonia Beach Hotel	CY
Louis Nausicaa Beach	CY
Cyprotel Laura Beach	CY
Napa Plaza Hotel	CY
The Golden Coast Hotel	CY
Crown Resorts Henipa Hotel	CY
Elias Beach Hotel & Country Club	CY
Avanti Holiday Village	CY
Riu Cypria Bay Hotel	CY
Palm Beach Hotel and Bungalows	CY
Cyprotel Florida Beach Hotel	CY
Navarria Hotel	CY
Bella Napa Bay Hotel	CY
Asterias Beach Hotel	CY
Amathus Beach Hotel	CY
Cavo Maris Beach Hotel	CY
Grecian Bay Hotel	CY
Golden Arches Hotel	CY
Elia Latchi Holiday Village	CY
Paphos Gardens Holiday Resort	CY
Sunrise Beach Hotel	CY
Flamingo Beach Hotel	CY
Elysium Hotel	CY
Azia Blue At Azia Resort	CY
Socoriky Beach Club	CY
Coral Beach Hotel and Resort	CY
Crown Resorts Elamaris	CY
Maroulla Hotel	CY
Grecian Park Hotel	CY
Anassa	CY
Amorgos Hotel	CY
Crown Resorts Henipa	CY
Andreas   Melani Beach Hotel	CY
Columbia Pissouri Beach Hotel	CY
Faros Holiday Village	CY
Elias Beach Hotel	CY
Marion Hotel	CY
Kings Hotel	CY
Posidonia Hotel	CY
Portopafos Hotel	CY
Chrysland Hotel And Gardens Clu	CY
Alphamega Hypermarket	CY
US Embassy	CY
Paphos	CY
Aphrodite Hills	CY
Ayia Thekla	CY
So Nice Boutique Suites	CY
Makronisos Bay Beach	CY
Finikoudes Promenade	CY
Petra tou Romiou	CY
WaterWorld Water Park	CY
Supermarket	CY
Stratigou Timagia	CY
Larnaca Port	CY
Makariou III	CY
Larnaca Intercollege	CY
Aphrodite Hills	CY
Fig Tree Bay	CY
Hotel Agapinor	CY
Ajax Hotel	CY
Akteon Holiday Village	CY
Hotel Aloe	CY
Hotel Aquasol Holiday Village	CY
Ascos Coral Beach	CY
Basilica Complex Holiday Resort	CY
Hotel Cynthiana Beach	CY
Damon	CY
Hotel Eleni Holiday Village	CY
Kanika Pantheon	CY
Kefalos Beach Tourist Village Hotel	CY
Veronica	CY
The Church by St Paul's Pillar	CY
Papo Island	CY
Pafos transmitter	CY
Nicosia Mediumwave Transmitter	CY
Limassol Mediumwave Transmitter	CY
Baths of Aphrodite	CY
Lara Bay	CY
Adonis Baths	CY
Limmassol Mediumwave Transmitter	CY
Lady's Mile Mediumwave Transmitter, Tower Northwest	CY
Lady's Mile Mediumwave Transmitter, Tower Southwest	CY
Lady's Mile Mediumwave Transmitter, Tower Northeast	CY
Lady's Mile Mediumwave Transmitter, Tower Southeast	CY
Cap Greco Mediumwave Transmitter, Mast Northwest	CY
Cap Greco Mediumwave Transmitter, Mast Northeast	CY
Cap Greco Mediumwave Transmitter, Mast Southwest	CY
Cap Greco Mediumwave Transmitter, Mast Southeast	CY
Yeni Iskele Mediumwave Transmitter	CY
Cape Greco Mediumwave Transmitter, Antenna Array, Mast 1	CY
Cape Greco Mediumwave Transmitter, Antenna Array, Mast 2	CY
Cape Greco Mediumwave Transmitter, Antenna Array, Mast 3	CY
Geçitkale Air Base	CY
Pedieos River	CY
Thera Complex	CY
The Residence	CY
Le Méridien pier	CY
Northern Cyprus	CY
Cyprus	CY
\.

INSERT INTO cities(name,country_code)
SELECT DISTINCT name, country_code FROM tempcities;

COMMIT;
ANALYSE;
