Name: Kian Kolahdoozan
Matr.-Nr.: 46824

Problemstellung:

Das Wedding Seating Problem befasst sich mit der Verteilung von Gästen auf eine begrenzte Menge an Sitzplätzen bei einer Hochzeit. Dabei sollen die Beziehungen der Gäste berücksichtigt werden, damit alle Freunde an einem Tisch sitzen und die Feinde an einem anderen Tisch.
Das Ziel ist es, dass alle Gäste mit ihrem Platz zufrieden sind und Konflikte zwischen Gästen verhindert werden.
Es ist ein Optimierungsproblem, bei welchem eine heuristische Lösung in Betracht kommt.

Vorgehensweise:
Die Datei mit den Gäste, Freunde und Feinde wird gelesen.
Eine Relationshipmatrix wird anhand der Beziehungen erstellt.
Die Liste mit den Gästen wird anhand der Anzahl der Beziehungen absteigend sortiert.
Der Gast mit dem meisten Beziehungen wird einem Tisch zugeordnet.
Dann wird der Gast mit den zweitmeisten Beziehungen zugeordnet, unter der Bedingung, dass es kein Feind ist (no enemy).
Wenn der Tisch voll mit Gästen ist, die keine Feinde sind, wird bei jedem Gast überprüft, ob all die Freunde auch an dem Tisch sitzen. Wenn ja, dann ist der Tisch mit den Gästen fertig belegt und der nächste Tisch wird mit Gästen belegt.
Ich habe mich für MCV entschieden, da ich dachte, dass die mit den meisten Beziehung die größten Probleme bereiten, weshalb sie als Erstes einen Platz bekommen. Dabei wird erstmal bei der Zuordnung nur darauf geachtet, dass keine Feinde zusammensitzen, da sie wie die Freunde auch eine hohe Anzahl an Beziehungen haben und deshalb weiter vorne in der Liste als neutrale Kandidaten.
Dadurch sollte sich die Wahrscheinlichkeit erhöhen, dass Freunde zusammengesetzt werden. Mit der Nachbedingung, dass falls doch nicht alle Freunde zusammensitzen und der Bedingung, dass kein Gast an ein Tisch zuweisen darf, wenn an einem anderen Tisch ein Freund sitzt, wird garantiert, dass Freunde immer zusammen sitzen.

Im Nachhinein würde ich erst MRV und dann MCV zusammen nutzen, da es effizienter ist, den Gast mit den wenigsten Möglichkeiten als Erstes zu setzen und dann die anderen, die mehr Auswahl haben. Eine Kombination der heuristischen Funktionen wäre sinnvoll, falls bei einer der heuristischen Funktionen zwei gleiche Werte rauskommen und die Reihenfolge nicht klar ist. Da kann die zweite heuristische Funktion die genaue Reihenfolge besser ermitteln.
Mit nur MCV funktioniert es auch, aber im schlimmsten Fall probiert der Code alle Möglichkeiten aus, bis es (k)eine Lösung findet.

Falls ein Paar in der txt-Datei als Freunde und als Feinde eingegeben wird, werden die nur als Feinde dargestellt. Bei der manuellen Eingabe wird die Beziehung übernommen, die als Letztes hinzugefügt wurde.

Ich habe beim Testen des Codes gesehen, dass es bei zu vielen Freunden keine Zuordnung findet, obwohl eine Zuordnung möglich wäre. Die genaue Ursache für diesen Bug habe ich nicht gefunden. Stattdessen habe ich einen anderen Ansatz genommen, wo die Freunde als eine Gruppe gespeichert werden und die Gruppe zusammen auf einem Tisch platziert wird. Diesen Ansatz habe ich in der Datei "Programm2" gespeichert, für das Testen den einkommentierten Code in Program.cs kopieren und ausführen.
In dem Ansatz wird MCV nicht mehr in dem ursprünglichen Ansatz genutzt, da die Anzahl der Beziehungen durch die Große der Gruppe ersetzt wird, wo aber die (freundlichen) Beziehungen eine Rolle spielen. Ich habe die folgenden Aufgaben nur für den Code in "Program.cs" beantwortet, weil das der ursprüngliche Code ist.

Verwenden eines CSPs:
1) Wie wird das CSP modelliert 
2) Wie wird ein State modelliert
3) Was gibt das Programm im Erfolgs/Fehlerfall aus
4) Welche Heuristische Funktionen werden verwendet
5) Wie modellieren Sie die friends/enemy Beziehung? 

1)
17 Gäste und vier Tische mit jeweils fünf Plätzen
Variablen: {T1, T2, T3, T4, T5}
Domain: {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}

T1 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T2 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T3 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T4 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T5 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
Beispielhafte Einschränkungen (Constrains), die dann ein Einfluss auf die Zuweisung der Tische haben:
Friends = {x01=x02, x12=x13, x06=x11} -> Tisch x01 = Tisch x02, Tisch x12 = Tisch x13, Tisch x06 = Tisch x11
Enemies = {x04!=x05, x14!=x16, x07!=x15} -> Tisch x04 != Tisch x05, Tisch x14 != Tisch x16, Tisch x07 != Tisch x15
2)
States:

T1 = {x01}
T2 = {}
T3 = {}
T4 = {}
T5 = {}

T1 = {x01, x03}
T2 = {}
T3 = {}
T4 = {}
T5 = {}

T1 = {x01, x03, x04}
T2 = {}
T3 = {}
T4 = {}
T5 = {}

T1 = {x01, x03, x04}
T2 = {x08}
T3 = {}
T4 = {}
T5 = {}

T1 = {x01, x03, x04, x05}
T2 = {x08}
T3 = {}
T4 = {}
T5 = {}

T1 = {x01, x03, x04, x05, x07}
T2 = {x08}
T3 = {}
T4 = {}
T5 = {}

usw. (sonst die PrintTables() Funktion in AssignSeatingHelper auskommentieren)
Änderungen verlaufen rekursiv durch AssignSeatingHelper und wird durch das Backtracking zurückgesetzt, falls es zu Konflikten kommt.
3)

Erfolgsfall: Seating arrangement found: {Ergebnis}
Fehlerfall je nach Fehlermeldung, sonst im Allgemeinen: Unable to seat all guests according to the given relationships.

4)

MCV
Gäste werden nach der Anzahl ihrer Beziehungen (Freunde und Feinde) absteigend sortiert (GetMCVList). Gäste mit den meisten Beziehungen (Constrains) werden zuerst gesetzt.
Was passiert wenn zwei gleich viele Beziehung haben? --> Testen


5)

Die friends und enemies Beziehung werden in einer Matrix festgehalten.
Hier beispielsweise für fünf Gäste:

int[,] relationshipMatrix = new int[guests.Count, guests.Count]
{
    { 0,  1,  0, -1,  0 },      //  0 = neutral
    { 1,  0,  0,  0,  1 },      //  1 = friends
    { 0,  0,  0, -1,  0 },      // -1 = enemies
    {-1,  0, -1,  0,  0 },
    { 0,  1,  0,  0,  0 } 
};
