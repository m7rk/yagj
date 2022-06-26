using System;
using System.Collections;
using System.Collections.Generic;


/* Usage
  PathTo(startx, starty, endx, endy, terrain)
  - terrain is a int[,] 
  - returns: List<int[]> a list of the path

  Example usage:
    int[,] test_terrain = new int[,] {{0,1,0,1}, {0,0,0,2}, {1,0,0,5}};
    AStar astar = new AStar();
    List<int[]> path; 
    path = astar.pathTo(0,0,1,2,test_terrain);
*/


namespace AStarFunctions
{
  public class AStar
  {



    /////////////// The actual A* search stuff /////////////////////

    private List<Tuple<int, int>> build_path(Tuple<int, int> source, Tuple<int, int> target, Dictionary<Tuple<int, int>, Tuple<int, int>> backtrack)
    {
      //Back  tracks  to  get  the  path  from  source  to  the  target
      Tuple<int, int> curr = target;
      List<Tuple<int, int>> arr = new List<Tuple<int, int>>();
      arr.Add(target);

      while (curr != source)
      {
        curr = backtrack[curr];
        arr.Insert(0, curr);
      }
      return arr;
    }

    public double distance(Tuple<int, int> p, Tuple<int, int> q)
    {
      return Math.Sqrt(Math.Pow(p.Item1 - q.Item1, 2) + Math.Pow(p.Item2 - q.Item2, 2));
    }


    //Call this function to find a shortest path from the point (startx, starty) to (endx, endy) on terrain terrain
    public List<Tuple<int, int>> pathTo(int startx, int starty, int endx, int endy, int[,] terrain)
    {

      Tuple<int, int> source = new Tuple<int, int>(startx, starty);
      Tuple<int, int> target = new Tuple<int, int>(endx, endy);

      int map_width = terrain.GetLength(0);
      int map_height = terrain.GetLength(1);

      //Keep  track  of  which  nodes  seen  for  backtracking
      Dictionary<Tuple<int, int>, Tuple<int, int>> backtrack = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

      //Keeps  track  of  the  pathcost  of  each  node
      Dictionary<Tuple<int, int>, int> pathcost = new Dictionary<Tuple<int, int>, int>();

      //A  queue  for  sorting  nodes  using  the  A*  cost
      PriorityQueue<Tuple<int, int>, double> queue = new PriorityQueue<Tuple<int, int>, double>();
      pathcost[source] = 0;
      queue.Enqueue(source, distance(source, target));


      void Enqueue(int x, int y, Tuple<int, int> node)
      {
        if (x >= 0 && x < map_width && y >= 0 && y < map_height)
        {
          Tuple<int, int> p = new Tuple<int, int>(x, y);
          
          if (terrain[x,y] >= 0)
          {
            int pc =  pathcost[node] + terrain[x,y];
            double cost = pc + distance(p, target); 
            if (!pathcost.ContainsKey(p) || pc < pathcost[p])
            {
              pathcost[p] = pc;
              backtrack[p] = node;
              queue.Enqueue(p, cost);
            }
          }
        }
      }

      //do  the  search
      while (queue.Count > 0)
      {
        //queue.printQueue();
        //Console.WriteLine(queue);
        Tuple<int, int> node = queue.Dequeue();
        //Console.WriteLine(node);

        //If  target  is  reached  then  backtrack  and  return  the  path  from  source  to  target
        if (node.Equals(target))
        {
          return build_path(source, node, backtrack);
        }

        int x = node.Item1;
        int y = node.Item2;

        //Otherwise,  consider  the  next  step
        Enqueue(x + 1, y, node);
        Enqueue(x - 1, y, node);
        Enqueue(x, y - 1, node);
        Enqueue(x, y + 1, node);


      }
        //Return an empty list if no path is possible
        return new List<Tuple<int, int>>();
    }
  }
}