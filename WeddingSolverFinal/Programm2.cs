using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingSolverFinal
{
    internal class Programm2
    {
        /*
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
            /// Establishes a friendship between two guests.
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
            /// Establishes a hostile relationship between two guests.
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
            /// <param name="fileName">The file name to load guest data from.</param>
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
            /// Thrown when a guest or group cannot be seated due to relationship constraints.
            /// </exception>
            public void AssignSeating()
            {
                // Step 1: Create groups of friends.
                var groups = CreateFriendGroups();
                foreach (var group in groups)
                {
                    if (group.Count > seatsPerTable)
                    {
                        throw new InvalidOperationException($"Group {string.Join(", ", group)} exceeds the table capacity.");
                    }
                }

                // Step 2: Assign groups to tables.
                if (!AssignGroupsToTables(groups, 0))
                {
                    throw new InvalidOperationException("Unable to seat all guests according to the given relationships.");
                }

                Console.WriteLine("Seating arrangement found:");
            }

            /// <summary>
            /// Recursively attempts to assign groups to tables.
            /// </summary>
            /// <param name="groups">The list of friend groups.</param>
            /// <param name="currentGroupIndex">The index of the current group being seated.</param>
            /// <returns>True if all groups are successfully seated; otherwise, false.</returns>
            private bool AssignGroupsToTables(List<List<string>> groups, int currentGroupIndex)
            {
                if (currentGroupIndex == groups.Count)
                {
                    return true; // All groups are seated successfully.
                }

                var group = groups[currentGroupIndex];
                foreach (var table in tables)
                {
                    if (CanSeatGroup(table, group))
                    {
                        table.AddRange(group); // Assign the group to the table.
                        if (AssignGroupsToTables(groups, currentGroupIndex + 1))
                        {
                            return true;
                        }
                        table.RemoveAll(group.Contains); // Backtrack.
                    }
                }

                return false; // No valid seating found for this configuration.
            }

            /// <summary>
            /// Checks if a group can be seated at a table.
            /// </summary>
            /// <param name="table">The table to check.</param>
            /// <param name="group">The group of guests to seat.</param>
            /// <returns>True if the group can be seated, otherwise false.</returns>
            private bool CanSeatGroup(List<string> table, List<string> group)
            {
                if (table.Count + group.Count > seatsPerTable)
                {
                    return false; // Not enough seats.
                }

                foreach (var seatedGuest in table)
                {
                    foreach (var groupMember in group)
                    {
                        int seatedIndex = guests.IndexOf(seatedGuest);
                        int groupIndex = guests.IndexOf(groupMember);
                        if (relationshipMatrix[seatedIndex, groupIndex] == -1)
                        {
                            return false; // Enemy relationship detected.
                        }
                    }
                }

                return true; // The group can be seated at the table.
            }

            /// <summary>
            /// Creates groups of friends based on the relationship matrix.
            /// </summary>
            /// <returns>A list of friend groups.</returns>
            private List<List<string>> CreateFriendGroups()
            {
                var visited = new HashSet<string>();            // Who is already in a friend group
                var groups = new List<List<string>>();

                foreach (var guest in guests)
                {
                    if (!visited.Contains(guest))
                    {
                        var group = new List<string>();
                        BuildGroup(guest, group, visited);
                        groups.Add(group);
                    }
                }

                return groups;
            }

            /// <summary>
            /// Recursively builds a group of friends starting from a guest.
            /// </summary>
            /// <param name="guest">The starting guest.</param>
            /// <param name="group">The group to populate.</param>
            /// <param name="visited">A set of already visited guests.</param>
            private void BuildGroup(string guest, List<string> group, HashSet<string> visited)
            {
                visited.Add(guest);
                group.Add(guest);

                int guestIndex = guests.IndexOf(guest);
                for (int i = 0; i < guests.Count; i++)
                {
                    if (relationshipMatrix[guestIndex, i] == 1 && !visited.Contains(guests[i]))
                    {
                        BuildGroup(guests[i], group, visited);
                    }
                }
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
                    WeddingSeatingSolver solver = new WeddingSeatingSolver(4, 5, guest); //"guest_data.txt"

                    //solver.AddFriend("Tom", "Daniel");
                    solver.AddFriend("Tom", "John");
                    solver.AddFriend("Tom", "Mia");
                    solver.AddFriend("Tom", "Clara");
                    solver.AddFriend("Clara", "Li");
                    solver.AddFriend("Emma", "Thomas");
                    solver.AddFriend("Roxy", "Bella");
                    solver.AddFriend("Anna", "Jeff");
                    solver.AddFriend("Mark", "Maria");
                    solver.AddEnemy("Marco", "Tom");
                    solver.AddEnemy("Marco", "Anna");
                    solver.AddEnemy("Marco", "Daniel");
                    solver.AddEnemy("Marco", "John");
                    solver.AddEnemy("Marco", "Mia");
                    solver.AddEnemy("Marco", "Jeff");
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
        */
    }
}
