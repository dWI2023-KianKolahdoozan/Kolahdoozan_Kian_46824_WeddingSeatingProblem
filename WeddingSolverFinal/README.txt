Projekt: 
Korrekte wissenschaftliche Vorgehensweise 
(Grundlegend verwendete Literatur aufgezeigt, Wenn Einsatz KI: Zitiert) 
Überblick Problemstellung & Beantwortung Fragen 
(Grundlegender Überblick über Problemstellung und Vorgehensweise, beantwortung vorher festgelegter Fragen) 
Dokumentation 
(Wichtigste Stellen kommentiert, Code/Lösungsstrategie nachvollziehbar[roter Faden], Klassendiagramm oder Programmablaufplan vorhanden) 
Code

Problemstellung:

Das Wedding Seating Problem befasst sich mit der Verteilung von Gästen auf begrenzte Menge an Sitzplätzen bei einer Hochzeit. Dabei sollen die Beziehungen der Gäste berücksichtigt werden, damit alle Freunde an einem Tisch sitzen und die Feinde an einem anderen Tisch.
Das Ziel ist, dass alle Gäste zufrieden sind mit ihren Platz und Konflikte zwischen Gästen verhindert wird.
Es ist ein Optiemierungsproblem, wo eine heuristische Lösung in Betracht kommt.

Vorgehensweise:

Der Gast mit den meisten Beziehungen wird an ein Tisch zugeordnet.
Dann wird der Gast mit den zweitmeisten Beziehungen zugeordnet, unter der Bedingung, dass es kein Feind ist (no enemy).
Wenn das Tisch voll mit Gästen sind, die keine Feinde sind, wird bei jedem Gast überprüft, ob all die Freunde auch auf dem Tisch sitzen. Wenn ja, dann ist das Tisch mit den Gästen fertig belegt und der nächste Tisch wird mit Gästen belegt.
Ich habe mich für MCV entschieden, da ich dachte, dass die mit den meisten Beziehung die größten Probleme bereiten, weshalb sie als erstes ein Platz bekommen. Dabei wird erstmal nei der Zuordnung nur darauf geachtet, dass keine Feinde zusammensitzen, da sie wie die Freunde auch einen hohen Anzahl an Beziehungen haben und deshlab weiter vorne in der list als neutrale Kanditaten.
Dadurch sollte sich die Wahrscheinlichkeit erhöhen, dass Freunde zusammengesetzt werden. Mit der Nachbedingung, dass falls doch nicht alle Freunde zusammensitzen, wird kontrolliert

Im Nachhinein wurde ich MRV nutzen, da es effiezenter ist, den Gast mit den wenigsten Möglichkeiten als erstes zu setzen und dann die anderen, die mehr Auswahl haben. Mit MCV funktioniert es auch, aber im schlimmsten Fall probiert der Code alle Möglichkeiten aus, bis es (k)eine Lösung findet.

Verwenden eines CSPs:
1) Wie wird das CSP modelliert 
2) Wie wird ein State modelliert
3) Was gibt das Programm im Erfolgs/Fehlerfall aus
4) Welche Heuristische Funktionen werden verwendet
5) Wie modellieren Sie die friends/enemy Beziehung? 

1)

17 Gäste und vier Tische mit jeweils fünf Plätzen
T1 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T2 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T3 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T4 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
T5 = {x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11, x12, x13, x14, x15, x16, x17}
Friends = {x01&x02, x12&x13, x06&x11} -> Tisch x01 = Tisch x02
Enemies = {x04&x05, x14&x16, x07&x15} -> Tisch x04 != Tisch x05

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

3)

Erfolgsfall: Seating arrangement found: {Ergebnis}
Fehlerfall je nach Fehlermeldung, sonst im Allgemeinen: Unable to seat all guests according to the given relationships.

4)

MCV
Was passiert wenn zwei gleich viele Beziehung haben? --> Testen
Greedy best-first search
f(n) = 0 + h(n)

5)

Die friends und enemies Beziehung werden in einer Matrix festgehalten.
int[,] relationshipMatrix = new int[guests.Count, guests.Count]
{
    { 0,  1,  0, -1,  0 },      //  0 = neutral
    { 1,  0,  0,  0,  1 },      //  1 = friends
    { 0,  0,  0, -1,  0 },      // -1 = enemies
    {-1,  0, -1,  0,  0 },
    { 0,  1,  0,  0,  0 } 
};
