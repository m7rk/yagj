using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fuckin shitty java bfs stolen from the internet and shittily convd to csharp
// slow but i need a reliable BFS for testin
// have to run every frame because only fetches first step (bad!!)
public class CrapBFS
{
    private static bool OOB(Tuple<int, int> c, int xmax, int ymax)
    {
        return (c.Item1 < 0 || c.Item2 < 0 || c.Item1 >= xmax || c.Item2 >= ymax);
    }

    public static List<Tuple<int,int>> fadj()
    {
        return new List<Tuple<int, int>>() { new Tuple<int, int>(0, 1), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(-1, 0) };
    }

    // I want to find integer "i" in a 2d array
    // I can only move through the value "0"
    // Please route me to the nearest integer "i"
    public static Tuple<int, int> find(int startx, int starty, int searchVal, int[,] grid)
    {
        // dont edit any created tuples (consider them immutable) or bad things will happen.
        if (grid[startx,starty] == searchVal)
        {
            return new Tuple<int, int>(0, 0);
        }

        // cache things
        int xmax = grid.GetLength(0);
        int ymax = grid.GetLength(1);
        List<Tuple<int, int>> adj = fadj();

        // for each direction -> what initial step did we use to get there?
        Dictionary<Tuple<int, int>, Tuple<int, int>> visited = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
        // only contains VALID moves
        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();

        // pre cache with initial moves.
        foreach (var v in adj)
        {
            var next = new Tuple<int, int>(v.Item1 + startx, v.Item2 + starty);

            if (!OOB(next, xmax, ymax) && grid[next.Item1,next.Item2] == 0)
            {
                queue.Enqueue(next);
                visited[next] = v;
            }

            // we found it in initial moves, return
            if (!OOB(next, xmax, ymax) && grid[next.Item1,next.Item2] == searchVal)
            {
                return v;
            }
        }
        
        // it's not next to us, let's BFS
        while (queue.Count > 0)
        {
            var explore = queue.Dequeue();

            // Look at adj tiles.
            foreach (var v in adj)
            {
                var next = new Tuple<int, int>(v.Item1 + explore.Item1, v.Item2 + explore.Item2);
                // it's in bounds...
                if (!OOB(next, xmax, ymax))
                {                     
                    // Found it!!!
                    if(grid[next.Item1,next.Item2] == searchVal)
                    {
                        return visited[explore];
                    }

                    // i can move through it, also, i haven't visited this yet.
                    if (grid[next.Item1, next.Item2] == 0 && !visited.ContainsKey(next))
                    {
                        // cache how i got here
                        visited[next] = visited[explore];
                        queue.Enqueue(next);
                    }
                }
            }
        }
        return new Tuple<int, int>(0, 0);
    }
}
