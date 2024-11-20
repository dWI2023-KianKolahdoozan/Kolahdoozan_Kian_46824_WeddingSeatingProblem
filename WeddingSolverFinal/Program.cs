public class WeddingSeatingSolver
{
    private int numberTables;
    private int seatsPerTable;
    private List<string> guests;
    private int[,] relationshipMatrix;
    private List<List<string>> tables;
    public WeddingSeatingSolver(int numberTables, int seatsPerTable, List<string> guests)
    {
        this.numberTables = numberTables;
        this.seatsPerTable = seatsPerTable;
        this.guests = guests;
        relationshipMatrix = new int[guests.Count, guests.Count];
        for (int i = 0; i < guests.Count; i++)
        {
            for (int j = 0; j < guests.Count; j++)
            {
                relationshipMatrix[i, j] = 0;
            }
        }

        tables = new List<List<string>>();
        for (int i = 0; i < numberTables; i++)
        {
            tables.Add (new List<string> ());
        }

        if (guests.Count > numberTables * seatsPerTable)
        {
            throw new ArgumentException("The number of guests exceeds the total number of available seats.");
        }
    }
    public void AddFriend(string name1, string name2)
    {
        int index1 = guests.IndexOf(name1);
        int index2 = guests.IndexOf(name2);
        if (index1 == -1)
        {
            throw new Exception($"The name {name1} is not in the guests list.");
        }
        else if (index2 == -1)
        {
            throw new Exception($"The name {name2} is not in the guests list.");
        }
        relationshipMatrix[index1, index2] = 1;
        relationshipMatrix[index2, index1] = 1;
    }
    public void AddEnemy(string name1, string name2)
    {
        int index1 = guests.IndexOf(name1);
        int index2 = guests.IndexOf(name2);
        if (index1 == -1)
        {
            throw new Exception($"The name {name1} is not in the guests list.");
        }
        else if (index2 == -1)
        {
            throw new Exception($"The name {name2} is not in the guests list.");
        }
        relationshipMatrix[index1, index2] = -1;
        relationshipMatrix[index2, index1] = -1;
    }
    public void DisplayRelationshipMatrix()
    {
        Console.WriteLine("Relationship Matrix (1: friend, -1: enemy, 0: none):");
        for (int i = 0; i < guests.Count; i++)
        {
            for (int j = 0; j < guests.Count; j++)
            {
                Console.Write(relationshipMatrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
    public void AssignSeating()
    {
        var orderedGuests = FindInfluencer();

        foreach (var guest in orderedGuests)
        {
            if (guest.Item2 > seatsPerTable - 1)
            {

                int amountFriends = 0;
                for (int i = 0; i < guests.Count; i++)
                {
                    if (relationshipMatrix[guests.IndexOf(guest.Item1), i] == 1)
                    {
                        amountFriends++;
                    }
                }
                if (amountFriends > seatsPerTable - 1)
                {
                    throw new InvalidOperationException($"Guest {guest.Item1} has more desired seating companions than the available seats per table.");
                }

                int guestIndex = guests.IndexOf(guest.Item1);
                if (!CanBeSeated(guestIndex))
                {
                    throw new InvalidOperationException($"Guest {guest.Item1} cannot be seated as they have an enemy at every table.");
                }
            }

        }

        if (!AssignSeatingHelper(orderedGuests, 0))
        {
            foreach (var guest in orderedGuests)
            {
                int guestIndex = guests.IndexOf(guest.Item1);
                if (!CanBeSeated(guestIndex))
                {
                    throw new InvalidOperationException($"Guest {guest.Item1} cannot be seated as they have an enemy at every table.");
                }
            }
            throw new InvalidOperationException("Unable to seat all guests according to the given relationships.");
        }
    }
    
    private bool AssignSeatingHelper(List<(string Guest, int Count)> orderedGuests, int currentGuestIndex)
    {
        if (currentGuestIndex == orderedGuests.Count)
        {
            return true; // All guests have been seated successfully.
        }

        string currentGuest = orderedGuests[currentGuestIndex].Guest;
        int guestIndex = guests.IndexOf(currentGuest);

        for (int table = 0; table < numberTables; table++)
        {
            if (IsSeatAvailable(table, guestIndex))
            {
                tables[table].Add(currentGuest);
                if (AssignSeatingHelper(orderedGuests, currentGuestIndex + 1))
                {
                    return true;
                }
                tables[table].RemoveAt(tables[table].Count - 1); // Backtrack
            }
        }
        return false; // No valid seating found for this configuration.
    }
    private bool IsSeatAvailable(int table, int guestIndex)
    {
        if (tables[table].Count >= seatsPerTable)
        {
            return false;
        }

        foreach (string seatedGuest in tables[table])
        {
            int seatedIndex = guests.IndexOf(seatedGuest);
            if (relationshipMatrix[guestIndex, seatedIndex] == -1)
            {
                return false; // Cannot seat enemies together.
            }
        }
        return true;
    }
    private bool CanBeSeated(int guestIndex)
    {
        for (int table = 0; table < numberTables; table++)
        {
            bool hasEnemy = false;
            foreach (string seatedGuest in tables[table])
            {
                int seatedIndex = guests.IndexOf(seatedGuest);
                if (relationshipMatrix[guestIndex, seatedIndex] == -1)
                {
                    hasEnemy = true;
                    break;
                }
            }
            if (!hasEnemy) 
            {
                return true;
            }
        }
        return false;
    }

    public List<(string, int)> FindInfluencer()
    {
         // Count the number of relationships (friends and enemies) for each guest
        List<(string Guest, int Count)> orderedGuests = new List<(string, int)>();
        for (int i = 0; i < guests.Count; i++)
        {
            int count = 0;
            for (int j = 0; j < guests.Count; j++)
            {
                if (relationshipMatrix[i, j] != 0)
                {
                    count++;    // Count all relationships (friends and enemies)
                }
            }
            orderedGuests.Add((guests[i], count));
        }

        // Order the list by the number of relationships
        orderedGuests = orderedGuests.OrderByDescending(g => g.Count).ToList();
        /*
        foreach (var guest in orderedGuests)
        {
            Console.WriteLine($"{guest.Guest}: {guest.Count} relationships");
        }*/
        return orderedGuests;     
    }

    public void PrintTables()
    {
        for (int i = 0; i < numberTables; i++)
        {
            Console.WriteLine($"Table {i + 1}: {string.Join(", ", tables[i])}");
        }
    }

    public static void Main()
    {
        List<string> guest = new List<string> {"Tom", "Anna", "Daniel", "John", "Mia", "Jeff", "Clara", "Roxy", "Mark", "Maria", "Bella", "Marco", "Mustafa", "Li", "Peter", "Emma", "Thomas"};
        try
        {
            WeddingSeatingSolver solver = new WeddingSeatingSolver(4, 5, guest);
            solver.AddFriend("Tom", "Daniel");
            solver.AddFriend("Tom", "John");
            solver.AddFriend("Tom", "Mia");
            solver.AddFriend("Tom", "Clara");
            solver.AddFriend("Clara", "Li");
            solver.AddFriend("Emma", "Thomas");
            solver.AddFriend("Roxy", "Bella");
            //solver.AddFriend("Tom", "Jeff");
            solver.AddEnemy("Marco", "Tom");
            solver.AddEnemy("Marco", "Anna");
            solver.AddEnemy("Marco", "Daniel");
            solver.AddEnemy("Marco", "John");
            solver.AddEnemy("Marco", "Mia");
            solver.AddEnemy("Marco", "Jeff");
            solver.AddEnemy("Marco", "Clara");
            solver.AddEnemy("Marco", "Roxy");
            //solver.AddEnemy("Marco", "Mark");
            //solver.AddEnemy("Marco", "Maria");
            solver.AddEnemy("Marco", "Bella");
            solver.AddEnemy("Marco", "Mustafa");
            solver.AddEnemy("Marco", "Li");
            solver.AddEnemy("Marco", "Peter");
            solver.AddEnemy("Marco", "Emma");
            solver.AddEnemy("Marco", "Thomas");
            solver.FindInfluencer();
            // solver.DisplayRelationshipMatrix();
            solver.AssignSeating();
            solver.PrintTables();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}