# Testjuhtumid – Keelekool (Tund12)

Testkeskkond: `https://localhost:XXXX` (dev server)

Testandmed (seedatud kasutajad):

| Roll     | Kasutajanimi (email) | Parool        |
|----------|-----------------------|---------------|
| Admin    | admin@kool.ee         | Admin123!     |
| Õpetaja  | mari@kool.ee          | Opetaja123!   |
| Õpilane  | opilane@kool.ee       | Opilane123!   |

## Testjuhtumite maatriks

| Case | Programmi osa | Roll     |
|------|----------------|----------|
| 000  | Registreerimine | Admin   |
| 001  | Registreerimine | Õpetaja |
| 002  | Registreerimine | Õpilane |
| 003  | Kursused        | Admin   |
| 004  | Kursused        | Õpetaja |
| 005  | Kursused        | Õpilane |
| 006  | Õpetajad        | Admin   |
| 007  | Õpetajad        | Õpetaja |
| 008  | Õpetajad        | Õpilane |
| 009  | Treeningud      | Admin   |
| 010  | Treeningud      | Õpetaja |
| 011  | Treeningud      | Õpilane |
| 012  | Registreerimiste haldus | Admin |
| 013  | Avaleht (dünaamiline sisu) | Kõik rollid |
| 014  | Autentimine ja seansihaldus | Kõik rollid |
| 015  | Sisendite valideerimine (vormid) | Admin |
| 016  | Õiguste kontroll / IDOR (horisontaalne) | Õpetaja, Õpilane |

---

## TestCase 000 – Registreerimine (Admin)

1. Mine Keelekool pealehele ilma sisse logimata
2. Vajuta "Registreeru" ja registreeri uus konto suvalise e-maili ja parooliga
3. Logi uue kontoga sisse ja proovi avada Admin-leht otse URL-i kaudu (nt `/Admin`)
4. Logi välja ja logi sisse testandmetest admin-kasutajaga (admin@kool.ee / Admin123!)
5. Vaata et:
   1. Avalikult registreerudes ei saa kasutaja Admin-rolli ega ligipääsu Admin-lehtedele
   2. Ainult ette seadistatud (seedatud) admin-kasutajaga sisse logides avaneb Admin-töölaud
   3. Admin-töölaual kuvatakse õiged koondarvud (kursused, õpetajad, koolitused, registreerimised, ootel registreerimised)
   4. **(negatiivne)** Vale kasutajanime või parooliga sisselogimisel kuvatakse veateade ja kasutajat ei logita sisse
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 001 – Registreerimine (Õpetaja)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Vajuta "Õpetajad" ja seejärel "Lisa õpetaja"
3. Sisesta uue õpetaja nimi, kvalifikatsioon, e-mail ja parool ning salvesta
4. Logi välja ja logi sisse äsja loodud õpetaja kontoga
5. Vaata et:
   1. Õpetaja lisamisel luuakse korraga kasutajakonto (rolliga "Opetaja") ja õpetaja profiil
   2. **(negatiivne)** Kohustuslike väljade (nimi, e-mail, parool) puudumisel kuvatakse veateade ja kontot ei looda
   3. **(negatiivne)** Juba kasutusel oleva e-mailiga uut kontot luua ei saa (Identity veateade)
   4. Äsja loodud õpetaja kontoga sisse logides avaneb Õpetaja töölaud, mitte Admin-vaade
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 002 – Registreerimine (Õpilane)

1. Mine Keelekool pealehele ilma sisse logimata
2. Vajuta "Registreeru"
3. Sisesta e-mail ja parool (vastavalt nõuetele, nt vähemalt 8 märki ja number) ning kinnita registreerimine
4. Logi äsja loodud kontoga sisse
5. Proovi mõnele koolitusele registreeruda
6. Vaata et:
   1. Registreerimisvorm kontrollib e-maili formaati ja parooli tugevust ning kuvab vigased väljad
   2. Konto luuakse ja kasutaja logitakse automaatselt sisse (või suunatakse sisselogimisele)
   3. Kontrolli, kas uus kasutaja saab koolitusele registreeruda – kuna avalik registreerimine ei määra automaatselt "Opilane" rolli, tuleks kontrollida, kas ligipääs keelatakse (õigustatud) või lubatakse ekslikult (viga)
   4. **(negatiivne)** Sama e-mailiga uuesti registreerimine ei ole lubatud
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 003 – Kursused (Admin)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Vajuta "Kursused" (Keelekursused)
3. Vajuta "Lisa kursus", täida nimetus, keel, tase ja kirjeldus ning salvesta
4. Muuda äsja lisatud kursuse andmeid ja salvesta
5. Vaata et:
   1. Kursuseid saab lisada koos kõigi väljadega (nimetus, keel, tase, kirjeldus)
   2. Kursuseid saab muuta ja muudatused kajastuvad nimekirjas kohe
   3. Kursuseid saab vaadata ja need on sorteeritud keele ja taseme järgi
   4. **(negatiivne)** Kursust, millega on seotud koolitusi, ei saa kustutada ilma veateateta (või kustutamine on takistatud)
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 004 – Kursused (Õpetaja)

1. Logi sisse õpetaja-kasutajaga (mari@kool.ee / Opetaja123!)
2. Kontrolli, kas menüüs on nähtaval kursuste halduse link
3. Proovi avada kursuste halduse leht otse URL-i kaudu (nt `/Admin/Keelekursused`)
4. Ava enda koolitus ja vaata sealt kuvatavat keelekursuse infot
5. Vaata et:
   1. Õpetaja menüüs ei kuvata kursuste halduse (Admin) linki
   2. **(negatiivne)** Otse URL-i kaudu Admin-lehele minnes ligipääs keelatakse (nt suunatakse sisselogimisele/veale, mitte ei avata Admin-vaadet)
   3. Õpetaja näeb oma koolituse juures seotud keelekursuse nime, keelt ja taset (lugemisõigus, mitte muutmisõigus)
   4. **(negatiivne)** Õpetaja ei saa kursuse andmeid muuta ega kustutada (ka mitte otsese POST-päringuga vormiväliselt)
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 005 – Kursused (Õpilane)

1. Logi sisse õpilase-kasutajaga (opilane@kool.ee / Opilane123!) (või jää sisse logimata)
2. Vajuta "Koolitused" ja kasuta keele/taseme filtrit, mis põhinevad keelekursustel
3. Proovi avada kursuste halduse leht otse URL-i kaudu (nt `/Admin/Keelekursused`)
4. Ava mõne koolituse detailvaade ja vaata sealt kuvatavat kursuse kirjeldust
5. Vaata et:
   1. Õpilane näeb keele ja taseme filtrit ning saab selle järgi koolitusi sõeluda
   2. Koolituse detailvaates kuvatakse kursuse nimetus, keel, tase ja kirjeldus
   3. **(negatiivne)** Õpilasele ei kuvata kursuste halduse (Admin) linke menüüs
   4. **(negatiivne)** Otse URL-i kaudu Admin-lehele minnes ligipääs keelatakse
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 006 – Õpetajad (Admin)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Vajuta "Õpetajad"
3. Muuda ühe õpetaja nime ja kvalifikatsiooni ning salvesta
4. Proovi kustutada õpetajat, kellel on seotud koolitusi
5. Proovi kustutada õpetajat, kellel koolitusi ei ole
6. Vaata et:
   1. Kõiki õpetajaid kuvatakse nimekirjas koos nende koolitustega
   2. Õpetaja andmete muutmine õnnestub ja kajastub kohe nimekirjas
   3. **(negatiivne)** Seotud koolitustega õpetajat kustutada ei saa – kuvatakse veateade "Õpetajat ei saa kustutada, sest tal on seotud koolitusi"
   4. Koolitusteta õpetaja profiili kustutamine õnnestub (kasutajakonto jääb alles, ainult profiil eemaldatakse)
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 007 – Õpetajad (Õpetaja)

1. Logi sisse õpetaja-kasutajaga (mari@kool.ee / Opetaja123!)
2. Kontrolli, kas menüüs on õpetajate halduse link
3. Proovi avada õpetajate halduse leht otse URL-i kaudu (nt `/Admin/Opetajad`)
4. Ava enda töölaud ja kontrolli enda profiiliandmete (nimi, kvalifikatsioon) kuvamist
5. Vaata et:
   1. Õpetaja menüüs ei kuvata teiste õpetajate halduse linki
   2. **(negatiivne)** Otse URL-i kaudu Admin/Opetajad lehele minnes ligipääs keelatakse
   3. **(negatiivne)** Õpetaja näeb enda töölaual oma profiiliandmeid (nimi, kvalifikatsioon), kuid ei saa neid ise muuta
   4. Kui sisselogitud kasutajal (rolliga "Opetaja") pole õpetaja profiili, kuvatakse selge teavitusleht, mitte viga
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 008 – Õpetajad (Õpilane)

1. Logi sisse õpilase-kasutajaga (opilane@kool.ee / Opilane123!) (või jää sisse logimata)
2. Ava mõne koolituse detailvaade
3. Proovi avada õpetajate halduse leht otse URL-i kaudu (nt `/Admin/Opetajad`)
4. Vaata et:
   1. Koolituse detailvaates kuvatakse õpetaja nimi ja kvalifikatsioon (lugemisõigus)
   2. **(negatiivne)** Õpilasele ei kuvata õpetajate halduse linki menüüs
   3. **(negatiivne)** Otse URL-i kaudu Admin/Opetajad lehele minnes ligipääs keelatakse
   4. **(negatiivne)** Õpetaja isiklikke andmeid (nt e-mail, kasutajakonto info) õpilasele ei avaldata
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 009 – Treeningud / Koolitused (Admin)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Vajuta "Koolitused"
3. Vajuta "Lisa koolitus", vali keelekursus ja õpetaja rippmenüüst, sisesta kuupäevad, hind ja max osalejate arv ning salvesta
4. Muuda äsja lisatud koolituse andmeid
5. Vali koolituse juures "Saada e-kiri", sisesta teema ja sisu ning saada
6. Proovi lisada koolitus, kus lõppkuupäev on enne algust ja/või max osalejate arv on negatiivne/null
7. Kustuta koolitus
8. Vaata et:
   1. Koolitust saab lisada koos kõigi väljadega ja see ilmub nimekirja
   2. Koolituse andmeid (kuupäevad, hind, kohtade arv, õpetaja) saab muuta
   3. E-kiri saadetakse ainult kinnitatud staatusega registreerunud õpilastele ning kuvatakse saadetud kirjade arv
   4. **(negatiivne)** Loogiliselt vigaseid andmeid (lõpp enne algust, negatiivne/null kohtade arv) ei tohiks lubada salvestada – kui valideerimist ei toimu, tuleb see vea/puudusena üles märkida
   5. Koolituse kustutamine eemaldab selle nimekirjast (koos seotud registreerimistega)
   6. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 010 – Treeningud / Koolitused (Õpetaja)

1. Logi sisse õpetaja-kasutajaga (mari@kool.ee / Opetaja123!)
2. Ava enda töölaud ("Minu koolitused")
3. Ava ühe koolituse juurest "Õpilased"/"Osalejad"
4. Proovi avada teise õpetaja koolituse osalejate leht otse URL-i kaudu (tundmatu/vale ID-ga)
5. Vaata et:
   1. Kuvatakse ainult sisselogitud õpetajaga seotud koolitused, mitte kõiki koolitusi
   2. Iga koolituse juures on näha kursus, ajavahemik ja registreerunud õpilaste arv
   3. Osalejate nimekirjas kuvatakse iga õpilase nimi/e-mail ja registreerimise staatus
   4. **(negatiivne)** Teise õpetaja koolituse osalejate lehele ligipääsu ei anta (kuvatakse "Not Found" või ligipääs keelatakse)
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 011 – Treeningud / Koolitused (Õpilane)

1. Logi sisse õpilase-kasutajaga (opilane@kool.ee / Opilane123!)
2. Vajuta "Koolitused", ava mõne koolituse detailvaade ja vajuta "Registreeru"
3. Proovi registreeruda samale koolitusele uuesti
4. Ava "Minu koolitused" ja tühista üks registreerimine
5. Proovi registreeruda koolitusele, mille grupp on täis (MaxOsalejaid täidetud)
6. Vaata et:
   1. Registreerumine õnnestub, staatuseks määratakse "Ootel" ja kuvatakse kinnitusteade
   2. **(negatiivne)** Korduval registreerumisel samale koolitusele kuvatakse veateade "Sa oled juba sellele koolitusele registreerunud"
   3. "Minu koolitused" lehel kuvatakse ainult enda registreerimised ning tühistamine muudab staatuse "Tühistatud"
   4. **(negatiivne)** Täis grupile registreerumisel kuvatakse veateade "Kahjuks on grupp täis" ja registreerimist ei looda
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 012 – Registreerimiste haldus (Admin)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Vajuta "Registreerimised"
3. Muuda ühe ootel registreerimise staatuseks "Kinnitatud"
4. Muuda mõne registreerimise staatuseks "Tühistatud"
5. Proovi muuta staatust olematu registreerimise ID-ga (nt otse POST-päringuga vale/väga suure ID-ga)
6. Vaata et:
   1. Kõiki registreerimisi kuvatakse koos õpilase, koolituse ja staatusega, uuemad eespool
   2. Staatuse muutmine kajastub kohe nii nimekirjas kui ka töölaua "Ootel registreerimiste" arvus
   3. Staatuse muutmisel kuvatakse kinnitusteade koos uue staatusega
   4. **(negatiivne)** Olematu registreerimise ID-ga staatuse muutmisel ei teki serveriviga (500) – rakendus käsitleb olukorda korrektselt (nt suunab tagasi nimekirja ilma muudatuseta)
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 013 – Avaleht ja dünaamiline sisu (Kõik rollid)

1. Ava avaleht ilma sisse logimata ja pane tähele kuvatavaid käimasolevaid koolitusi
2. Logi sisse õpilase-kasutajaga (opilane@kool.ee / Opilane123!) ja ava avaleht uuesti
3. Logi välja, logi sisse õpetaja-kasutajaga (mari@kool.ee / Opetaja123!) ja ava avaleht
4. Logi välja ja proovi avaleht ilma sisse logimata avada mobiilse ekraanisuurusega (responsiivsus)
5. Vaata et:
   1. Sisse logimata kasutajale kuvatakse kuni 3 käimasolevat koolitust, kuid mitte isiklikku sektsiooni
   2. Sisse logitud õpilasele kuvatakse lisaks tema enda kinnitatud koolitused ("Kinnitatud" staatusega)
   3. Navigatsioonimenüü muutub vastavalt sisselogimise olekule (Logi Sisse/Registreeru ↔ kasutajanimi/Logi Välja) ja rollile (Admin/Õpetaja lisamenüüd)
   4. **(negatiivne)** Kui koolitusi hetkel käimas ei ole, ei kuvata katkist/tühja plokki ilma selgitava tekstita
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi mobiilses ega lauaarvuti vaates (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 014 – Autentimine ja seansihaldus (Kõik rollid)

1. Mine sisselogimislehele ja sisesta olemasolev kasutajanimi, kuid vale parool (mitu korda järjest)
2. Logi sisse korrektsete andmetega (nt opilane@kool.ee / Opilane123!)
3. Vajuta "Logi välja"
4. Pärast väljalogimist vajuta brauseri "Tagasi" nuppu ja proovi pääseda "Minu koolitused" lehele
5. Sisesta otse URL-i kaudu kaitstud leht (nt `/Koolitused/MinuKoolitused`) ilma sisse logimata
6. Vaata et:
   1. Korrektse sisselogimise järel suunatakse kasutaja õigele lehele ja kuvatakse tema nimi/e-mail navigatsioonis
   2. **(negatiivne)** Mitme järjestikuse vale parooliga katse korral kuvatakse selge veateade (soovitavalt ilma kontot lukustamata liiga kergesti, kuid ka ilma tundlikku infot lekitamata, nt "vale kasutajanimi VÕI parool")
   3. Väljalogimise järel muutub kasutaja "Logi Sisse/Registreeru" vaates uuesti nähtavaks
   4. **(negatiivne)** Pärast väljalogimist ei tohi kaitstud sisu olla kättesaadav "Tagasi" nupu ega vahemällu jäänud lehe kaudu (nõuab uut sisselogimist)
   5. **(negatiivne)** Sisse logimata kaitstud lehele minnes suunatakse kasutaja sisselogimislehele, mitte ei kuvata sisu ega serveriviga
   6. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 015 – Sisendite valideerimine Admin-vormides (Admin)

1. Logi sisse admin-kasutajaga (admin@kool.ee / Admin123!)
2. Ava "Lisa kursus" ja proovi salvestada tühjade kohustuslike väljadega
3. Ava "Lisa õpetaja" ja proovi salvestada nõrga parooliga (nt "123") või vigase e-mailiga (nt "test")
4. Ava "Lisa koolitus" ja proovi sisestada negatiivne hind või tähtedega väli numbrivälja (Hind, MaxOsalejaid)
5. Vaata et:
   1. **(negatiivne)** Tühjade kohustuslike väljadega vormi ei salvestata ja kasutajale kuvatakse väljapõhised veateated
   2. **(negatiivne)** Nõrk parool või vigane e-mail lükatakse Identity poolt tagasi koos selge veateatega
   3. **(negatiivne)** Numbriväljadele sisestatud vigane sisend (tekst, negatiivne arv) ei põhjusta rakenduse krahhi, vaid kuvatakse valideerimisviga
   4. Kehtivate andmetega vormi täitmisel toimub salvestus edukalt ja kasutaja suunatakse nimekirja koos kinnitusteatega
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)

---

## TestCase 016 – Õiguste kontroll / IDOR (Õpetaja, Õpilane)

1. Logi sisse kahe erineva õpilase kontoga kahes brauseris/seansis (nt opilane@kool.ee ja mõni teine testkonto) ja registreeru mõlemaga erinevatele koolitustele
2. Ühe õpilase seansis vaata "Minu koolitused" ja pane tähele oma registreerimise ID-d (nt lingi/vormi kaudu)
3. Proovi teise õpilase seansis tühistada esimese õpilase registreerimist, asendades ID otse URL-is/vormis
4. Logi sisse õpetaja-kasutajaga ja proovi otse URL-i kaudu avada teise (mitteolemasoleva õpetaja) koolituse "Õpilased" lehte suvalise koolituse ID-ga
5. Vaata et:
   1. **(negatiivne)** Õpilane ei saa tühistada teisele kasutajale kuuluvat registreerimist, isegi kui ID on URL-is/vormis käsitsi muudetud
   2. **(negatiivne)** Sellisel katsel kuvatakse "Not Found" või vastav veateade, mitte ei toimu vaikimisi tühistamine
   3. **(negatiivne)** Õpetaja ei pääse ligi koolitusele/osalejate nimekirjale, mis ei kuulu talle, ka mitte suvalise/võltsitud ID-ga
   4. Õigustatud omal registreerimisel/koolitusel toimivad vastavad tegevused (tühistamine, vaatamine) endiselt korrektselt
   5. Et lehel ei oleks graafiliste kasutajaliideste elementide moonutusi (nagu tekst puudub, õigekirjavead, osaliselt kaetud elemendid, pildikuvamisvead)
