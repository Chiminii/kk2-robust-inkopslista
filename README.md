- ### Fyra buggar som får programmet att krascha
    - I Main() kommer programmet krascha när man på **int.Parse(Console.ReadLine())** inte matar in en siffra, dvs den kraschar vid bokstäver och tomma inmatningar. Man får errorn *FormatException*. För att lösa detta bör man använda **int.TryParse** istället som säkerställer att man gjort rätt inmatning.

    - I metoden RemoveAt(int number) i ShoppingList-klassen finns det inget som kontrollerar hur många varor som faktiskt finns i listan. Om användaren matar in ett negativt tal eller ett tal som är högre än antalet varor kommer man får errorn *ArgumentOutOfRangeException*. (I koden har det gjorts om till index = number - 1 , eftersom listor börjar på indexet noll) För att lösa detta behöver man ta reda på om siffran som användaren matar in existerar i listan med en if-sats.

    - I metoden Load() i ShoppingList-klassen finns det inget som kontrollerar att en textfil som man försöker läsa från faktiskt existerar. Försöker man läsa från en fil som inte finns får man errorn **FileNotFoundException** och programmet kraschar. För att förhindra detta kan man lägga till ett try-catch-block som kan fånga upp fel om det   uppstår utan att programmet kraschar.

    - I metoden Save() i ShoppingList-klassen läggs en extra radbrytning till på slutet av filen **+ "\r\n"**. När metoden Load() läser in filen och gör **text.Split('\n');** kommer den sista raden i arrayen bli tom. När programmet sedan försöker köra **items.Add(new Item(parts[1], int.Parse(parts[0])));** på den sista raden i arrayen kommer programmet att krascha eftersom **line.Split(';')** inte kommer hitta något semikolon vilket resulterar i att arrayen blir för kort, dvs det finns inget att lägga till parts[1] ,  (**IndexOutOfRangeException**) och int.Parse kan inte ta emot en tom sträng. För att lösa detta kan man tex i Load() skriva:
    **string [] lines = text.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);**
    Detta gör att text klipps antingen vid \r\n eller \n och StringSplitOptions.RemoveEmptyEntries tar automatiskt bort tomma rader. För att också säkerställa att raden delas upp korrekt innan man lägger till den i listan kan man lägga till en if-sats som kollar att parts.Length == 2.


- ### Ett fel som ger fel resultat utan att krascha
    - I metoden Total() i ShoppingList-klassen beräknas den totala summan fel, eftersom for-loopen börjar räkna på i = 1. I listor är det första indexet noll så det bör vara i = 0. Annars är det alltid den första varan(items[0]) som saknas vid beräkning av den totala summan.


- ### Ett fel som döljer att något gick fel
    - I metoden Save() i ShoppingList-klassen finns ett try-catch-block. Syftet är att catch ska fånga ett fel och förhindra att programmet kraschar, men just nu är den tom och gör ingenting. Programmet skriver också ut direkt efteråt att "Listan är sparad." vilket både ljuger för användaren och döljer ett problem. En lösning är att inuti catch-blocket skriva tex 
    **Console.WriteLine("Ett fel uppstod. Kunde inte spara listan.");**
    och avbryta metoden med
    **return;** så att 
    **Console.WriteLine("Listan är sparad.");** inte skrivs ut.



- ### Designval av budgettaket
    - Jag valde att Add-metoden skulle returnera en bool, dvs true or false istället för att kasta ett undantag. Anledningen är att det i det här fallet inte är något allvarligt fel som den behöver hantera, utan bara om du har råd med något. Det blir "tyngre" för datorns system om den behöver kasta ett undantag för varje gång du inte har råd.

        Men i Item-klassen är det viktigare att den kastar undantag när ett fel uppstår så att den inte skapar trasiga objekt och lägger till det i listan. 

        Jag valde att i Main kontrollera att man skapat ett item på ett korrekt sätt med ett try-catch-block. Om man skapat objektet fel fångas felet upp i catch, om man skapat den rätt går man vidare till en if-sats som först kontrollerar med en bool om det är inom budget innan du kan lägga till den i listan. 