# TAR25ProgrammeerimineII

Ülesanne 6. Arhitektuur ja põhjendused (Library)
1. Milline on DbContext-i ülesanne:
DbContext ühendab projekti andmebaasiga. Selle kaudu saab andmebaasist andmeid lugeda ja sinna salvestada. Tabelid on DbSet-idena ning muudatused salvestatakse SaveChanges() abil.

2. Miks ei ole hea paigutada kõiki andmebaasipäringuid otse kasutajaliidese koodi?
- Kood läheb segaseks, sest kasutajaliides peaks ainult andmeid näitama.
- Sama päringut tuleb kirjutada mitmes kohas, ehk see muudab muudatused tülikaks.
- Andmebaasi muutmisel peab muutma ka kasutajaliidese koodi.
- Koodi on raske testida.

3. Millisesse kihti paigutaksid andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris?
MVC-s paigutaksin andmebaasiga suhtlemise Modelisse. Controller küsib andmed Modelilt ja annab need Vaatele, View ainult kuvab.
Põhjendus: Nii on kasutajaliides andmebaasist eraldatud, kood on selgem ning seda on lihtsam hooldada ja testida.
