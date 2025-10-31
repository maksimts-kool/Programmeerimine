# Tund7 – Windows Formsi rakendus

See projekt koosneb kolmest erinevast Windows Forms rakendusest (vormist), mis demonstreerivad C# graafiliste kasutajaliideste (GUI) ja loogika kasutamist praktilistes ülesannetes.

## 🧩 Ülevaade

Rakendus sisaldab järgmisi vorme:

- **Form1 – Pildinäitaja**  
  Lihtne piltide vaatamise ja efektide lisamise tööriist.

- **Form2 – Matemaatika viktoriin**  
  Interaktiivne õppemäng, kus saab lahendada matemaatikaülesandeid eri raskusastmetel.

- **Form3 – Kooste mäng**  
  Mälu- ja tähelepanumäng, kus tuleb leida sobivad pildipaarid.

---

## 📸 Form1 – Pildinäitaja

**Kirjeldus:**
- Võimaldab valida, kuvada ja puhastada pilte.
- Saab pilte salvestada erinevatesse formaatidesse (JPG, PNG, BMP, GIF).
- Piltidele on võimalik lisada **efekte**:
  - **Mustvalge filter (grayscale)**
  - **Ääris (border)** soovitud värvi ja paksusega.
- Kasutajaliides sisaldab:
  - „Näita pilti”, „Puhasta pilt”, „Salvesta kui...”, „Efektid”, „Sule” nuppe.
  - „Pikenda” ja „Kohanda akna suurusele” märkeruute.

**Kasutamine:**
1. Vajuta **"Näita pilti"** ja vali pildifail.  
2. Kasuta vajadusel **"Efektid"**, et lisada mustvalge toon või ääris.  
3. Salvesta tulemus **"Salvesta kui..."** nupuga.  
4. Sule vorm **"Sule"** nupuga.

---

## ➕ Form2 – Matemaatika viktoriin

**Kirjeldus:**
- Treenib matemaatikaoskuseid (liitmine, lahutamine, korrutamine, jagamine).  
- Sisaldab mitut raskusastet (**Lihtne**, **Keskmine**, **Raske**).  
- Iga raskusaste määrab aja ja tehtetüübid.
- Saab määrata ka sektsioonide arvu, kus iga sektsioonis on 4 ülesannet.
- Automaatne taimer ja pausivõimalus menüüs.

**Funktsioonid:**
- Arvutused genereeritakse automaatselt vastavalt raskusastmele.
- Sektsioone saab liikuda edasi nuppu **"Järgmine"** vajutades.
- Tulemus kuvatakse mängu lõpus koos detailse raportiga (õiged ja valed vastused).
- Kui aeg saab otsa, lõpetatakse viktoriin automaatselt.

**Kasutamine:**
1. Vali menüüst **raskusaste**.
2. Määra sektsioonide arv.
3. Vajuta **"Alusta viktoriini"**.
4. Lahenda ülesandeid, vajadusel kasuta **pausi** menüüst.
5. Lõpus kuvatakse tulemuste kokkuvõte.

---

## 🧠 Form3 – Kooste mäng

**Kirjeldus:**
- Mälumäng, kus eesmärgiks on leida sobivad ikoonipaarid.
- Kasutab kausta **`ikoonid`**, kus peavad olema vähemalt **8 PNG-pilti**.
- Igal ikoonil on paar, kokku 16 ruutu (4×4 laud).
- Aeg jookseb (60 sekundit).

**Funktsioonid:**
- Kui avad kaks ühesugust pilti järjest, saad boonust (**+4 s** pärast mitut järjestikust õiget paari).  
- Kui teed mitu viga järjest, kaotad aega (**-2 s**).  
- Kui aeg saab otsa – mäng lõpeb.  
- Kui kõik paarid on leitud, saad valida, kas alustada uut mängu.  
- Võimalus lisada uusi ikoone menüüst **"Lisa ikoone..."**.

**Paigaldamine:**
- Veendu, et projektikaustas oleks alamkaust `ikoonid`.
- Lisa sinna vähemalt 8 erinevat PNG-faili (näiteks `apple.png`, `car.png`, `star.png`, jne).

**Kasutamine:**
1. Klõpsa kaardil, et paljastada ikoon.  
2. Leia sobiv paar.  
3. Kui kõik paarid on leitud – oled võitnud!  

---

## 💾 Nõuded ja käivitamine

**Käivitamine:**
1. Ava projekt Visual Studios (`Tund7.sln` fail).  
2. Sea käivitusvormiks soovitud Form (`Form1`, `Form2` või `Form3`).  
3. Vajuta **Start / Käivita** (`F5`).

---

## 👨 Autor

**Autor:** *Maksim Tsikvasvili*
**Keel:** Eesti keel  
**Aasta:** 2025
