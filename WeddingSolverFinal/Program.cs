
public class WeddingSeatingSolver
{
    private int numberTables;
    private int seatsPerTable;
    private List<string> guests;
    private int[,] relationshipMatrix;
    private List<List<string>> tables;
    /// <summary>
    /// Initializes a new instance of the <see cref="WeddingSeatingSolver"/> class with a specified list of guests.
    /// </summary>
    /// <param name="numberTables">The number of tables available for seating guests.</param>
    /// <param name="seatsPerTable">The number of seats available at each table.</param>
    /// <param name="guests">A list of guest names to be seated.</param>
    /// <exception cref="ArgumentException">Thrown when the number of guests exceeds the total number of available seats.</exception>
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
                relationshipMatrix[i, j] = 0;       // Fill the relationshipMatrix with zeros.
            }
        }

        tables = new List<List<string>>();
        for (int i = 0; i < numberTables; i++)
        {
            tables.Add(new List<string>());       // Add tables in the table list.
        }

        if (guests.Count > numberTables * seatsPerTable)
        {
            throw new ArgumentException("The number of guests exceeds the total number of available seats.");
        }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="WeddingSeatingSolver"/> class using a file to load guest data.
    /// </summary>
    /// <param name="numberTables">The number of tables available for seating guests.</param>
    /// <param name="seatsPerTable">The number of seats available at each table.</param>
    /// <param name="filePath">The file path to load guest data from.</param>
    /// <exception cref="ArgumentException">Thrown when the number of guests exceeds the total number of available seats.</exception>
    public WeddingSeatingSolver(int numberTables, int seatsPerTable, string fileName)
    {
        this.numberTables = numberTables;
        this.seatsPerTable = seatsPerTable;
        guests = new List<string>();

        LoadFromFile(fileName);

        tables = new List<List<string>>();
        for (int i = 0; i < numberTables; i++)
        {
            tables.Add(new List<string>());       // Add tables in the table list.
        }

        if (guests.Count > numberTables * seatsPerTable)
        {
            throw new ArgumentException("The number of guests exceeds the total number of available seats.");
        }
    }
    /// <summary>
    /// Establishes a friendship relationship between two guests.
    /// </summary>
    /// <param name="name1">The name of the first guest.</param>
    /// <param name="name2">The name of the second guest.</param>
    /// <exception cref="Exception">Thrown when either guest name is not found in the guest list.</exception>
    public void AddFriend(string name1, string name2)
    {
        int index1 = guests.IndexOf(name1);
        int index2 = guests.IndexOf(name2);
        if (index1 == -1)
        {
            throw new Exception($"The name {name1} is not in the guests list.");
        }
        if (index2 == -1)
        {
            throw new Exception($"The name {name2} is not in the guests list.");
        }
        relationshipMatrix[index1, index2] = 1;     // Changes the value in the relationship matrix from 0 to 1.
        relationshipMatrix[index2, index1] = 1;     // Symetric changes the value in the relationship matrix from 0 to 1.
    }

    /// <summary>
    /// Establishes a unfriendship relationship between two guests.
    /// </summary>
    /// <param name="name1">The name of the first guest.</param>
    /// <param name="name2">The name of the second guest.</param>
    /// <exception cref="Exception">Thrown when either guest name is not found in the guest list.</exception>
    public void AddEnemy(string name1, string name2)
    {
        int index1 = guests.IndexOf(name1);
        int index2 = guests.IndexOf(name2);
        if (index1 == -1)
        {
            throw new Exception($"The name {name1} is not in the guests list.");
        }
        if (index2 == -1)
        {
            throw new Exception($"The name {name2} is not in the guests list.");
        }
        relationshipMatrix[index1, index2] = -1;    // Changes the value in the relationship matrix from 0 to -1.
        relationshipMatrix[index2, index1] = -1;    // Symetric changes the value in the relationship matrix from 0 to -1.
    }

    /// <summary>
    /// Loads guest data from a specified file, including guest names, friendships, and enmities.
    /// </summary>
    /// <param name="fileName">The file path to load guest data from.</param>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="ArgumentException">Thrown when the file does not have the required format or content.</exception>
    public void LoadFromFile(string fileName)
    {
        string path = Directory.GetCurrentDirectory();
        path = path + "\\" + fileName;
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file '{fileName}' does not exist.");                  // If file does not exist or the path is wrong.
        }
            
        var lines = File.ReadAllLines(fileName);
        if (lines.Length != 3)
        {
            throw new ArgumentException("The file must have three lines: guests, friends, and enemies.");
        } 

        // Parse guests
        var guestsFile = lines[0].Split(',').Select(name => name.Trim()).ToList();      // Trim the whitespaces.
        foreach (var guest in guestsFile)
        {
            guests.Add(guest);
        }

        relationshipMatrix = new int[guests.Count, guests.Count];                       // The relationship matrix can only filled with zeros if the guest count is known.
        for (int i = 0; i < guests.Count; i++)
        {
            for (int j = 0; j < guests.Count; j++)
            {
                relationshipMatrix[i, j] = 0;       // Fill the relationshipMatrix with zeros.
            }
        }

        // Parse friends
        var friends = lines[1].Split(',').Select(pair => pair.Trim());                  // Trim the whitespaces, get pair of friends.
        foreach (var pair in friends)
        {
            var friendsPair = pair.Split('&').Select(name => name.Trim()).ToArray();
            AddFriend(friendsPair[0], friendsPair[1]);
        }

        // Parse enemies
        var enemies = lines[2].Split(',').Select(pair => pair.Trim());               // Trim the whitespaces, get pair of enemies.
        foreach (var pair in enemies)
        {
            var enemiesPair = pair.Split('&').Select(name => name.Trim()).ToArray();
            AddEnemy(enemiesPair[0], enemiesPair[1]);
        }
    }

    /// <summary>
    /// Shows the relationship matrix for all the guest in the console.
    /// </summary>
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

    /// <summary>
    /// Assigns seating for guests based on their relationships, ensuring that friends are seated together and enemies are not.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a guest has more desired seating companions than available seats per table,
    /// or when a guest cannot be seated due to having an enemy at every table or other issues.
    /// </exception>
    public void AssignSeating()
    {
        var orderedGuests = FindInfluencer();       // Get the list of guest, ordered by the amount of relationships.
        foreach (var guest in orderedGuests)
        {
            if(GetFriends(guest.Item1).Count > seatsPerTable - 1)   // Checks if a guest has more friends than seats per table available
            {
                throw new InvalidOperationException($"Guest {guest.Item1} has more desired seating companions than the available seats per table.");
            }
        }

        if (!AssignSeatingHelper(orderedGuests, 0))                 // If not any assingment is found.
        {
            throw new InvalidOperationException("Unable to seat all guests according to the given relationships.");
        }
        else
        {
            Console.WriteLine("Seating arrangement found:");
        }
    }

    /// <summary>
    /// Recursively attempts to assign seating for guests based on their relationships.
    /// </summary>
    /// <param name="orderedGuests">A list of guests ordered by the number of relationships they have.</param>
    /// <param name="currentGuestIndex">The index of the current guest being seated.</param>
    /// <returns>True if all guests have been successfully seated; otherwise, false.</returns>
    private bool AssignSeatingHelper(List<(string Guest, int Count)> orderedGuests, int currentGuestIndex)
    {
        if (currentGuestIndex == orderedGuests.Count)
        {
            if (!CheckFriendsSitTogether(tables.Count-1))   // Checks if all friends sit together at the last table
            {
                return false;
            }
            return true; // All guests have been seated successfully.
        }

        string currentGuest = orderedGuests[currentGuestIndex].Guest;
        int guestIndex = guests.IndexOf(currentGuest);

        for (int table = 0; table < numberTables; table++)
        {
            if (IsSeatAvailable(table, guestIndex))                 // No enemy and table has seats
            {
                tables[table].Add(currentGuest);

                //PrintTables();

                if (AssignSeatingHelper(orderedGuests, currentGuestIndex + 1))
                {
                    return true;
                }
                tables[table].RemoveAt(tables[table].Count - 1);    // Backtrack!
            }
            if (!CheckFriendsSitTogether(table))                    // Checks if all friends sit together at the same table, last table is not checked
            {
                return false;                                   
            }
        }
        return false; // No valid seating found for this configuration.
    }

    /// <summary>
    /// Checks if all friends of a guest at a specific table are also seated together.
    /// </summary>
    /// <param name="table">The index of the table to check.</param>
    /// <returns>
    /// True if all friends of a guest at the specified table are also seated together; otherwise, false.
    /// </returns>
    private bool CheckFriendsSitTogether(int table)
    {
        for (int guestIndex = 0; guestIndex < tables[table].Count; guestIndex++)
        {
            List<string> friends = GetFriends(tables[table].ElementAt(guestIndex));     // Get the list of all the friends.
            foreach (string friend in friends)
            {
                if (!tables[table].Contains(friend))
                {
                    return false;
                }
            }
        }
        return true;                                                                   // All friends are seated on the same table
    }

    /// <summary>
    /// Gets a list of friends for a given guest.
    /// </summary>
    /// <param name="guest">The name of the guest.</param>
    /// <returns>A list of the guest's friends.</returns>
    private List<string> GetFriends(string guest)
    {
        int guestIndex = guests.IndexOf(guest);
        List<string> friends = new List<string>();

        for (int i = 0; i < guests.Count; i++)
        {
            if (relationshipMatrix[guestIndex, i] == 1) // Friends
            {
                friends.Add(guests[i]);
            }
        }
        return friends;         // Returns a list of the guest's friends.
    }

    /// <summary>
    /// Checks if a seat is available for a guest and all their friends at a specified table.
    /// </summary>
    /// <param name="table">The index of the table to check.</param>
    /// <param name="guestIndex">The index of the guest to be seated.</param>
    /// <returns>True if the table can accommodate the guest and all their friends, otherwise false.</returns>
    private bool IsSeatAvailable(int table, int guestIndex)
    {
        if (GetFriendCount(guestIndex) == 0)
        {
            if (tables[table].Count >= seatsPerTable)
            {
                return false; // Not enough seats for more guests.
            }
        }
        else
        {
            if (tables[table].Count + GetFriendCount(guestIndex) > seatsPerTable)
            {
                return false; // Not enough seats for the guest and their friends.
            }
        }

        foreach (string seatedGuest in tables[table])
        {
            int seatedIndex = guests.IndexOf(seatedGuest);
            if (relationshipMatrix[guestIndex, seatedIndex] == -1)
            {
                return false; // Cannot seat enemies together.
            }
        }

        return true; // Table is suitable for the guest and their friends.
    }

    /// <summary>
    /// Counts the number of friends of a guest.
    /// </summary>
    /// <param name="guestIndex">The index of the guest.</param>
    /// <returns>The number of friends the guest has.</returns>
    private int GetFriendCount(int guestIndex)
    {
        int friendCount = 0;
        for (int i = 0; i < guests.Count; i++)
        {
            if (relationshipMatrix[guestIndex, i] == 1)
            {
                friendCount++;              // Counts friends.
            }
        }
        return friendCount;
    }

    /// <summary>
    /// Create a list to store each guest and their number of relationships.
    /// Order the list by the number of relationships in descending order.
    /// </summary>
    /// <return>Return the ordered list of guests and their relationship counts.</return>
    private List<(string, int)> FindInfluencer()
    {
        // Count the number of relationships (friends and enemies) for each guest.
        List<(string Guest, int Count)> orderedGuests = new List<(string, int)>();
        for (int i = 0; i < guests.Count; i++)
        {
            int count = 0;
            for (int j = 0; j < guests.Count; j++)
            {
                if (relationshipMatrix[i, j] != 0)
                {
                    count++;    // Count all relationships (friends and enemies).
                }
            }
            orderedGuests.Add((guests[i], count));
        }

        // Order the list by the number of relationships
        orderedGuests = orderedGuests.OrderByDescending(g => g.Count).ToList();
        /*
        foreach (var guest in orderedGuests)
        {
            Console.WriteLine($"{guest.Guest}: {guest.Count} relationships");   // See who has how many relationships.
        }*/
        return orderedGuests;
    }

    /// <summary>
    /// Shows all the tables with the seated guests in the console.
    /// </summary>
    public void PrintTables()
    {
        for (int i = 0; i < numberTables; i++)
        {
            Console.WriteLine($"Table {i + 1}: {string.Join(", ", tables[i])}");
        }
    }
    /// <summary>
    /// The main entry point for the application. Initializes the guest list and seating solver,
    /// establishes relationships between guests, and attempts to assign seating based on these relationships.
    /// If the seating solver fails, a error is showed.
    /// </summary>
    public static void Main()
    {
        List<string> guest = new List<string> { "Tom", "Anna", "Daniel", "John", "Mia", "Jeff", "Clara", "Roxy", "Mark", "Maria", "Bella", "Marco", "Mustafa", "Li", "Peter", "Emma", "Thomas" };
        try
        {
            WeddingSeatingSolver solver = new WeddingSeatingSolver(4, 5, "guest_data.txt");
            /*
            //solver.AddFriend("Tom", "Daniel");
            solver.AddFriend("Tom", "John");
            solver.AddFriend("Tom", "Mia");
            solver.AddFriend("Tom", "Clara");
            solver.AddFriend("Clara", "Li");
            solver.AddFriend("Emma", "Thomas");
            //solver.AddFriend("Roxy", "Bella");
            //solver.AddFriend("Tom", "Jeff");
            solver.AddEnemy("Marco", "Tom");
            solver.AddEnemy("Marco", "Anna");
            solver.AddEnemy("Marco", "Daniel");
            solver.AddEnemy("Marco", "John");
            solver.AddEnemy("Marco", "Mia");
            //solver.AddEnemy("Marco", "Jeff");
            //solver.AddEnemy("Marco", "Clara");
            //solver.AddEnemy("Marco", "Roxy");
            //solver.AddEnemy("Marco", "Mark");
            //solver.AddEnemy("Marco", "Maria");
            //solver.AddEnemy("Marco", "Bella");
            //solver.AddEnemy("Marco", "Mustafa");
            //solver.AddEnemy("Marco", "Li");
            //solver.AddEnemy("Marco", "Peter");
            //solver.AddEnemy("Marco", "Emma");
            //solver.AddEnemy("Marco", "Thomas");
            */

            //solver.DisplayRelationshipMatrix();
            solver.AssignSeating();
            solver.PrintTables();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");  //Error message is displayed if something goes wrong.
        }
    }
}
