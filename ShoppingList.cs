// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;
    public int BudgetCap { get; private set; } = 1000;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    // Changing it to a bool instead to check if you have enough money 
    // to add the item to your grocery list 
    public bool Add(Item item)
    {
        // Check if the new total cost is still within the budget
        if ((Total() + item.Price) > BudgetCap)
        {
            // If not within budget
            return false;
        }

        // Remove any whitespace before and after the start of the item name
        item.Name = item.Name.Trim();
        // If within budget
        items.Add(item);
        return true;
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        int index = number - 1;
        // Check if user input is within the range of the list
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
        else
        {
            Console.WriteLine("Fel: Det radnumret existerar inte.");
        }
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            // Convert all letters to lowercase
            // so that it doesn't matter if the user accidently wrote in 
            // upper or lowercase for the same item name 
            if (item.Name.ToLower() == name.ToLower())
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        try
        {
            // Trying to write to the file
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
        }
        catch (UnauthorizedAccessException)
        {
            // If you're not authorized to open the file
            Console.WriteLine($"Kunde inte spara listan. Programmet saknar behörighet till filen '{path}'.");
            return;
        }
        catch (IOException)
        {
            // If the file is blocked, for example it is already opened in another program
            // or the hard drive is full
            Console.WriteLine($"Kunde inte spara listan. Filen är låst av ett annat program eller hårddisken är full.");
            return;
        }
        catch (Exception ex)
        {
            // Other errors that have not been foreseen
            Console.WriteLine($"Ett oväntat fel uppstod: {ex.Message}");
            return;
        }

        Console.WriteLine("Listan är sparad.");
    }

    // Reads the file back into the list.
    public void Load()
    {
        string text = "";

        try
        {
            // Trying to read the file
            text = File.ReadAllText(path);
        }
        catch (FileNotFoundException)
        {
            // If the file-name doesn't exist
            Console.WriteLine($"Kan inte hitta filen '{path}'.");
            return;
        }
        catch (UnauthorizedAccessException)
        {
            // If you're not authorized to open the file
            Console.WriteLine($"Kan inte öppna filen '{path}'. Programmet saknar behörighet.");
            return;
        }
        catch (IOException)
        {
            // If the file is blocked, for example it is already opened in another program
            Console.WriteLine($"Kan inte öppna filen '{path}'. Filen är låst av ett annat program.");
            return;
        }
        catch (Exception ex)
        {
            // Other errors that have not been foreseen
            Console.WriteLine($"Ett oväntat fel uppstod: {ex.Message}");
            return;
        }

        string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');

            // Check if you can split it up into two parts before adding the item
            // And check that it has the right user input on the price so it doesn't crash
            if (parts.Length == 2 && int.TryParse(parts[0], out int price))
            {
                items.Add(new Item(parts[1], price));
            }
            else
            {
                Console.WriteLine($"Kan inte läsa raden: '{line}'. Felaktigt format.");
            }
        }
    }
}
