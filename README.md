# .NET MAUI Leltár – Tesztfeladat

Egy egyszerű készletkezelő MAUI alkalmazás helyi SQLite adatbázissal, amit a tesztfeladat specifikációja alapján raktam össze. A cél egy átlátható MVVM architektúra kialakítása volt megbízható lokális adattárolással és a több ezer elemes lista gördülékeny kezelésével.

## Megvalósítás és működés

Az alkalmazás az adatokat lokálisan, egy SQLite adatbázisban tárolja az sqlite-net-pcl csomag segítségével. Az adatbázis-műveleteket és a kapcsolat inicializálását egy dedikált ProductService osztály végzi, amit Dependency Injection-ön keresztül kapnak meg a ViewModel-ek. 

Annak érdekében, hogy az alkalmazás rögtön tesztelhető legyen nagyobb adatmennyiséggel is, az első indításkor automatikusan legenerál és beszúr 2500 darab tesztterméket véletlenszerű adatokkal és készletekkel.

A főoldalon egy CollectionView jeleníti meg a termékeket, ami a beépített virtualizáció miatt több ezer tétel esetén is simán, akadás nélkül görgethető. A 0 darabszámos tételeket egy egyszerű XAML konverter (QuantityToColorConverter) emeli ki piros színnel.

A keresés név és vonalkód alapján is működik. A felhasználói élmény és az adatbázis kímélése érdekében beépítettem egy 300 ms-os debounce késleltetést (Task.Delay és CancellationTokenSource segítségével), így gépelés közben nem fut le felesleges lekérdezés minden egyes leütött karakterre.

Az új termék rögzítése és a meglévő tételek szerkesztése külön oldalakon történik. A navigációhoz a .NET MAUI Shell útvonalait használtam; a részletező oldal a termék azonosítóját (id) query paraméterként kapja meg, majd betölti a hozzá tartozó adatokat az adatbázisból.

## Technikai döntések

- MVVM és CommunityToolkit.Mvvm: A rétegek szétválasztásához az MVVM mintát választottam. A tipikus boilerplate kódok (értesítések, parancsok) elkerülésére a hivatalos Microsoft MVVM Toolkitet használtam.
- SQLite indexelés: A termék nevét és a vonalkódját külön indexeltem az adatbázisban, hogy a keresési lekérdezések több ezer rekord esetén is azonnal lefussanak.
- CollectionView használata: A korábbi ListView-hoz képest a CollectionView sokkal hatékonyabban kezeli a virtuális görgetést és a memóriát ekkora tételszámnál.

## Mit csinálnék másképp több idő esetén?

- Lapozás: Jelenleg a lekérdezés az összes illeszkedő elemet egyszerre tölti be a memóriába. Éles környezetben, tízezres vagy százezres nagyságrendnél adatbázis szintű Skip/Take lapozást kötnék be a CollectionView inkrementális betöltéséhez (RemainingItemsThresholdReached).
- Egységtesztek: Írnék xUnit teszteket az adatbázis logikára és a ViewModel-ek működésére, mockolva a függőségeket.

## Továbbfejlesztési lehetőségek

- Kamerás vonalkód-leolvasás, hogy a vonalkódot ne csak kézzel lehessen bepötyögni.
- Termékek törlése vagy inaktiválása a felületről.
- Adatok exportálása és importálása CSV formátumban.