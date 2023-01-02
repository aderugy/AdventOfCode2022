using System.Collections;
using System.Text;

namespace Day12;

public class Part2
{
    private static int[,] map;
    private static int startX;
    private static int startY;
    
    // For the part II, we will start from the end and our stopping criteria will be to find an 'a' case
    public static void Main(string[] args)
    {
        // Setting up the data required by Dijkstra
        Parse();
        
        // Finding the shortest path
        Console.WriteLine(Dijkstra());
    }

    private static int Dijkstra()
    {
        PriorityQueue<Node, int> pq = new ();
        HashSet<Node> visited = new();

        // Initializing start node
        Node start = new (startX, startY)
        {
            Cost = 0
        };
        
        // Adding the starting node to the queue -> the algorithm will start searching from here
        pq.Enqueue(start, start.Cost);
        visited.Add(start);

        do
        {
            // Popping the element with shortest path from source so far
            Node current = pq.Dequeue();

            // Checking if we reached the end (stopping criteria: case contains an 'a' == 1)
            if (map[current.X, current.Y] == 1)
                return current.Cost;
            
            // Retrieving neighbours
            Node[] neighbours = GetNeighbours(current);

            // Coordinates of current node
            int currentVal = map[current.X, current.Y];
            
            foreach (Node neighbour in neighbours)
            {
                // Already visited node
                if (visited.Contains(neighbour))
                    continue;

                // Getting the coordinates of the neighbour
                int neighbourVal = map[neighbour.X, neighbour.Y];

                // Checking if the neighbour is valid
                if (neighbourVal != currentVal - 1 && neighbourVal < currentVal) continue;
                
                neighbour.Cost = current.Cost + 1;
                visited.Add(neighbour);
                pq.Enqueue(neighbour, neighbour.Cost);
            }
            
        } while (pq.Count > 0);

        throw new Exception("No path found.");
    }

    /**
     * Returns a list with all the neighbours of the node of coordinates (x, y) regardless of their height or visited status
     */
    public static Node[] GetNeighbours(Node current)
    {
        ArrayList results = new();

        int x = current.X;
        int y = current.Y;
        
        // Left
        if (x > 0)
            results.Add(new Node(x - 1, y));
        // Right
        if (x < map.GetLength(0) - 1)
            results.Add(new Node(x + 1, y));
        // Up
        if (y > 0)
            results.Add(new Node(x, y - 1));
        // Down
        if (y < map.GetLength(1) - 1)
            results.Add(new Node(x, y + 1));

        Node[] toReturn = new Node[results.Count];

        for (int i = 0; i < results.Count; i++)
        {
            toReturn[i] = (Node) results[i];
        }

        return toReturn;
    }

    /**
     * Parses the content of the input file into a 2D array with the height of the corresponding char
     * Retrieves and stores the starting and ending coordinates
     * x -> horizontal axis
     * y -> vertical axis
     * Height: a = 1 and z = 26
     */
    public static void Parse()
    {
        // Get content of the input file
        //string[] lines = File.ReadAllLines("/home/aderugy/Desktop/aderugy/AdventOfCode/Day12/input_example");
        string[] lines = File.ReadAllLines("/home/aderugy/Desktop/aderugy/AdventOfCode/Day12/input_solution");

        int height = lines.Length;
        int width = lines[0].Length;

        map = new int[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                char val = lines[y][x];
                
                // Get start and end coordinates and parsing their height
               if (val == 'E') { 
                        startX = x;
                        startY = y;
                        val = 'z';
                }

                if (val == 'S')
                   val = 'a';

                // Parsing char value into int from 1 to 26
                int heightValue = val - 'a' + 1;

                // Checking for invalid inputs
                if (heightValue is < 0 or > 26)
                    throw new InvalidCastException("Value input must be between 'a' and 'z'");

                // Initializing corresponding spot
                map[x, y] = heightValue;
            }
        }
    }
}