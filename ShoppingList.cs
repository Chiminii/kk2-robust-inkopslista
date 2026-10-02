// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        int index = number - 1;
        // Check if user input is within the range of the list
        if(index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
        else
        {
            Console.WriteLine("Felaktigt nummer. Kan inte hitta varan.");
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
            if (item.Name == name)
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
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
        }
        catch
        {
            Console.WriteLine("Ett fel uppstod. Kunde inte spara listan.");
            return;
        }

        Console.WriteLine("Listan är sparad.");
    }

    // Reads the file back into the list.
    public void Load()
    {
        // Check if file exists
        if (!File.Exists(path))
        {
            Console.WriteLine($"Kan inte hitta filen {path}");
            return;
        }

        string text = File.ReadAllText(path);
        //string[] lines = text.Split('\n');
        string [] lines = text.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');

            // Check if you can split it up into two parts before adding the item
            // And check that it has the right user input on the price so it doesn't crash
            if(parts.Length == 2 && int.TryParse(parts[0], out int price))
            {
                items.Add(new Item(parts[1], price));
            }
            else
            {
                Console.WriteLine( $"Kan inte läsa raden: '{line}'. Felaktigt format.");
            }
        }
    }
}
