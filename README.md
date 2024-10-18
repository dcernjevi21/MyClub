[![Open in Codespaces](https://classroom.github.com/assets/launch-codespace-2972f46106e565e64193e422d61a12cf1da4916b45550586e14ef0a7c637dd04.svg)](https://classroom.github.com/open-in-codespaces?assignment_repo_id=16529469)
# Inicijalne upute za prijavu projekta iz Razvoja programskih proizvoda

Poštovane kolegice i kolege, 

čestitamo vam jer ste uspješno prijavili svoj projektni tim na kolegiju Razvoj programskih proizvoda, te je za vas automatski kreiran repozitorij koji ćete koristiti za verzioniranje vašega koda, ali i za pisanje dokumentacije.

Ovaj dokument (README.md) predstavlja **osobnu iskaznicu vašeg projekta**. Vaš prvi zadatak je **prijaviti vlastiti projektni prijedlog** na način da ćete prijavu vašeg projekta, sukladno uputama danim u ovom tekstu, napisati upravo u ovaj dokument, umjesto ovoga teksta.

Za upute o sintaksi koju možete koristiti u ovom dokumentu i kod pisanje vaše projektne dokumentacije pogledajte [ovaj link](https://guides.github.com/features/mastering-markdown/).
Sav programski kod potrebno je verzionirati u glavnoj **master** grani i **obvezno** smjestiti u mapu Software. Sve artefakte (npr. slike) koje ćete koristiti u vašoj dokumentaciju obvezno verzionirati u posebnoj grani koja je već kreirana i koja se naziva **master-docs** i smjestiti u mapu Documentation.

Nakon vaše prijave bit će vam dodijeljen mentor s kojim ćete tijekom semestra raditi na ovom projektu. Mentor će vam slati povratne informacije kroz sekciju Discussions također dostupnu na GitHubu vašeg projekta. A sada, vrijeme je da prijavite vaš projekt. Za prijavu vašeg projektnog prijedloga molimo vas koristite **predložak** koji je naveden u nastavku, a započnite tako da kliknete na *olovku* u desnom gornjem kutu ovoga dokumenta :) 

# MyCLub

## Projektni tim

Ime i prezime | E-mail adresa (FOI) | JMBAG | Github korisničko ime
------------  | ------------------- | ----- | ---------------------
Dominik Černjević | dcernjevi21@student.foi.hr | 0016155459 | dcernjevi21
Emanuel Valec | evalec21@student.foi.hr | 0016156391 | evalec21
Fran Kundih | fkundih21@student.foi.hr | 0016153545  | fkundih21


## Opis domene

Ova aplikacija pokriva domenu upravljanja sportskim klubovima i omogućuje lakšu organizaciju, komunikaciju i praćenje sportskih aktivnosti unutar kluba. Ona olakšava rad sportskim trenerima, sportašima, kao i upravi kluba kroz digitalizaciju administrativnih zadataka, poput vođenja termina treninga, statistika momčadi, evidencije dolazaka i praćenja članarina.

Sportski klubovi često imaju problema s učinkovitim upravljanjem rasporedima treninga, komunikacijom između trenera i sportaša, vođenjem evidencije dolazaka, članarina i financijskih izvještaja. Ručno vođenje ovih zadataka može biti dugotrajno i sklono greškama, a često nedostaje i centralizirani sustav koji omogućuje svim korisnicima (sportašima, trenerima, upravi) jednostavan pregled i upravljanje ključnim informacijama u realnom vremenu.

## Specifikacija projekta
Umjesto ovih uputa opišite zahtjeve za funkcionalnošću programskog proizvoda. Pobrojite osnovne funkcionalnosti i za svaku naznačite ime odgovornog člana tima. Opišite buduću arhitekturu programskog proizvoda. Obratite pozornost da bi arhitektura trebala biti višeslojna s odvojenom (dislociranom) bazom podatka koju ćemo za vas mi pripremiti i dati vam pristup naknadno. Također uzmite u obzir da bi svaki član tima treba biti odgovorana za otprilike 3 funkcionalnosti, te da bi opterećenje članova tima trebalo biti ujednačeno. Priložite odgovarajuće dijagrame i skice gdje je to prikladno. Funkcionalnosti sustava bobrojite u tablici ispod koristeći predložak koji slijedi:

Oznaka | Naziv | Kratki opis | Odgovorni član tima
------ | ----- | ----------- | -------------------
F01 | Registracija/Prijava | Kod prvog pristupa aplikaciji korisnik će morati poslati prijavnicu klubu. Korisnik će unijeti mail i lozinku te dodati kratak opis sebe te će to proslijediti klubu koji odlučuje o primitku ili odbijanju upisa u klub. Ukoliko je korisnik primljen u klub pristupa aplikaciji pomoću e-mail adrese i lozinke koju je sam definirao prilikom registracije u aplikaciju. | -
F02 | Unos i uređivanje profila sportaša i trenera | Korisnici će upisati svoju dob i dodati sliku profila da bi stvorili svoj korisnički račun. Klub upravlja profilom trenera. | -
F03 | Upravljanje terminima treninga | Treneri će moći zakazati treninge ili odgoditi iste. | -
F04 | Prikaz traninga i utakmica | Korisnici kao i treneri imati će prikaz svih prošlih i nadolazećih treninga i utakmica. | -
F05 | Evidencija dolazaka | Treneri će voditi evidenciju dolaska te će korisnici imati pregled svojih dolazaka | -
F06 | Evidencija članarina i plaćanja | Klub će voditi evidenciju o plaćenim i neplaćenim članarinama te ostalim troškovima. | -
F07 | Izvještaji o financijskom stanju kluba | Klub će moći stvarati financijske izvještaje za željeno razdoblje. | -
F08 | Statistike momčadi | Cijeli klub će imati pregled međusobne statisike momčadi (rezultati natjecanja). | -
F09 | Notifikacije sportašima | Aplikacija će korisnica slati obavijesti o nadolazećim treninzima ili utakmicama. | -

Arhitektura programskog proizvoda će biti višeslojna. Slojevi su sljedeći:
* Korisnički sloj (UI - Windows Forms)
* Sloj poslovne logike
* Sloj pristupa podacima (Data Access Layer)
* Udaljena baza podataka
* Sigurnosni sloj

Korisnički sloj će biti odgovoran za interakciju s korisnikom i omogućava unos podataka i pregleda različitih podataka. Forme će biti dizajnirane tako da budu prilagođene specifičnim korisnicima (sportaši, treneri i administracija kluba).

Sloj podatkovne logike će biti implementiran kroz odvojene klase, svaka sa specifičnom funkcijom. Tako će se upravljati glavni procesi.

Sloj pristupa podacima će osiguravati komunikaciju aplikacije i baze podataka. Definirati će se SQL upiti za dohvaćanje, dodavanje, ažuriranje i brisanje podataka.

Udaljena baza podataka će služiti za pohranu svih bitnih podataka koji se koriste u aplikaciji.

Sigurnosni sloj će služiti za zaštitu baze od neovlaštenog pristupa. Svaki tip korisnika će moći pristupiti podacima koji su za njega namijenjeni nakon uspješne autorizacije. 

## Tehnologije i oprema

Za implementaciju rješenja koristit ćemo sljedeće tehnologije i alate:

* Razvojno okruženje: Visual Studio
* Programsko okruženje: .NET Framework
* Korisničko sučelje: WinForms
* Baza podataka: SqLite i MySQL Workbench
* Programski jezik: C#
* Verzioniranje koda: Git i GitHub
* Upravljanje zadacima: GitHub Projects
* Dokumentacija: GitHub Wiki


Umjesto ovih uputa jasno popišite sve tehnologije, alate i opremu koju ćete koristiti pri implementaciji vašeg rješenja. Projekti se razvijaju koristeći .Net Framework ili .Net Core razvojne okvire, a vrsta projekta može biti WinForms, WPF i UWP. Ne zaboravite planirati korištenje tehnologija u aktivnostima kao što su projektni menadžment ili priprema dokumentacije. Tehnologije koje ćete koristiti bi trebale biti javno dostupne, a ako ih ne budemo obrađivali na vježbama u vašoj dokumentaciji ćete morati navesti način preuzimanja, instaliranja i korištenja onih tehnologija koje su neopbodne kako bi se vaš programski proizvod preveo i pokrenuo. Pazite da svi alati koje ćete koristiti moraju imati odgovarajuću licencu. Što se tiče zahtjeva nastavnika, obvezno je koristiti git i GitHub za verzioniranje programskog koda, GitHub Wiki za pisanje tehničke i projektne dokumentacije, a projektne zadatke je potrebno planirati i pratiti u alatu GitHub projects. 
