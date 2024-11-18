Verwenden eines CSPs:
1) Wie wird das CSP modelliert 
2) Wie wird ein State modelliert
3) was gibt das Programm im Erfolgs/Fehlerfall aus
4) welche Heuristische Funktionen werden verwendet
5) Wie modellieren Sie die friends/enemy Beziehung? 

1)
16 Gäste und vier Tische mit jeweils fünf Plätzen
x01 = {T1, T2, T3, T4, T5}
x02 = {T1, T2, T3, T4, T5}
x03 = {T1, T2, T3, T4, T5}
x04 = {T1, T2, T3, T4, T5}
x05 = {T1, T2, T3, T4, T5}
x06 = {T1, T2, T3, T4, T5}
x07 = {T1, T2, T3, T4, T5}
x08 = {T1, T2, T3, T4, T5}
x09 = {T1, T2, T3, T4, T5}
x10 = {T1, T2, T3, T4, T5}
x11 = {T1, T2, T3, T4, T5}
x12 = {T1, T2, T3, T4, T5}
x13 = {T1, T2, T3, T4, T5}
x14 = {T1, T2, T3, T4, T5}
x15 = {T1, T2, T3, T4, T5}
x16 = {T1, T2, T3, T4, T5}

Friends = {x01=x02, x12=x13, x06=x11}
Enemies = {x04=x05, x14=x16, x07=x15}

2)

3)
Erfolsfall: Jeder Gast wird zum ein Tisch zugewiesen, die Tischordnung wird ausgegeben

Fehlerfall: Fehlermeldung, wenn es mehr Gäste als Plätze gibt und nicht alle Wünsche umgesetzt wurden

4)

5)
int[,] relationshipMatrix = new int[numberOfGuests, numberOfGuests]
        {
            { 0,  1,  0, -1,  0 }, // Alice: friend with Bob, enemy with David
            { 1,  0,  0,  0,  1 }, // Bob: friend with Alice and Emma
            { 0,  0,  0, -1,  0 }, // Charlie: enemy with David
            {-1,  0, -1,  0,  0 }, // David: enemy with Alice and Charlie
            { 0,  1,  0,  0,  0 }  // Emma: friend with Bob
        };