ShoppingList list = new ShoppingList("items.txt");
list.Load();


while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    string choiceInput = Console.ReadLine();
    if (!int.TryParse(choiceInput, out int choice))
    {
        Console.WriteLine("Felaktig inmatning. Ange ett heltal mellan 1-5.");
        continue;
    }

    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();

        Console.Write("Pris: ");
        string priceInput = Console.ReadLine();
        if (int.TryParse(priceInput, out int price))
        {
            try
            {
                // Try to create new item
                // If the input is invalid an exception is thrown and caught below 
                Item newItem = new Item(name, price);

                // Check if the item is within the budget before adding to the list
                if (list.Add(newItem))
                {
                    Console.WriteLine($"{name} har lagts till i listan.");
                }
                else
                {
                    Console.WriteLine($"Kan inte lägga till varan. Totalbeloppet skulle överstiga budgettaket på {list.BudgetCap} kr.");
                }
            }
            // If the item was created incorrectly
            // Only need ArgumentException and not ArgumentOutOfRangeException
            // since ArgumentOutOfRangeException inherits from the parent ArgumentException
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Fel: {ex.Message} ");
            }
        }
        else
        {
            Console.WriteLine("Fel inmatning. Priset måste vara ett heltal. Varan lades inte till.");
        }
    }
    else if (choice == 2)
    {
        Console.Write("Nummer: ");
        string numberInput = Console.ReadLine();
        if (int.TryParse(numberInput, out int number))
        {
            list.RemoveAt(number);
        }
        else
        {
            Console.WriteLine("Fel: Inmatningen måste vara en siffra.");
        }
    }
    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
    else
    {
        Console.WriteLine("\nOgiltigt nummer. Välj mellan 1-5.");
    }
}
