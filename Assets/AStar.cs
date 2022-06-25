using System;
using System.Collections;
using System.Collections.Generic;

/* Usage
  PathTo(startx, starty, endx, endy, terrain)
  -  terrain is 

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
    //C# does not have its own Priority Queue surprisingly
    //This is simply an implementation of a Priority Queue 
    //TODO: actually implement it properly with a minheap
    //Maybe even put it in its own file
    public class PriorityQueue
    {
      Dictionary<Pair, int> Pathcost;
      Pair Target;
      List<Pair> list;

      public PriorityQueue(Dictionary<Pair, int> pathcost, Pair target)
      {
        Pathcost = pathcost;
        Target = target;
        list = new List<Pair>();
      }
      public int Abs(int x){ return (x >= 0) ? x : -x;}
      
      public int distance(Pair p1, Pair p2)
      {
        //Distance is a function of the two points and the terrain 
        //Manhattan  distance,  but  can  be  changed  to  Euclidean  distance  if  leads  to  better  performance
        return Abs(p1.x - p2.x) + Abs(p1.y - p2.y);
      }

      public int Compare(Pair p1, Pair p2)
      {
        int heuristic_1 = Pathcost[p1];
        int heuristic_2 = Pathcost[p2];
        if (heuristic_1 == heuristic_2) return 0;
        if (heuristic_1 < heuristic_2) return -1;
        return 1;
      }

      //Insert and element to the queue
      public void Add(Pair p1){list.Add(p1);}

      //Returns true if queue is empty
      public bool empty() {return list.Count == 0;}

      //Pops and returns the minimum element
      public Pair pop()
      {
        Pair item = list[0];
        list.RemoveAt(0);
        return item;
      }
    }

    //A Pair object to keep track of coordinates
    public class Pair
    {
      public int x;
      public int y;
      public Pair(int a, int b) { x = a; y = b; }
      public void printPair()
      {
        Console.Write("(");
        Console.Write(x);
        Console.Write(",");
        Console.Write(y);
        Console.Write(")");
      }

      public override bool Equals(object obj)
      {
        if (obj is Pair)
        {
          Pair p = (Pair)obj;
          return p.x == x && p.y == y;
        }
        return false;
      }

      public override int GetHashCode(){return (x.GetHashCode() + y.GetHashCode()).GetHashCode();}
    }

    //A wrapper for the Proirity Queue so that it will work for a particular map
    public class myPriorityQueue
    {
      PriorityQueue queue;
      Dictionary<Pair, int> Pathcost;
      int Map_width;
      int Map_height;
      Dictionary<Pair, Pair> Backtrack;

      public myPriorityQueue(Pair source, Pair target, Dictionary<Pair, int> pathcost, int map_height, int map_width, Dictionary<Pair, Pair> backtrack)
      {
        queue = new PriorityQueue(Pathcost, target);
        Pathcost = pathcost;
        Map_height = map_height;
        Map_width = map_width;
        Backtrack = backtrack;
      }

      public void enqueue(int x, int y, Pair prev, int[,] terrain)
      {
        if ((x < Map_width) && (x >= 0) && (y >= 0) && (y < Map_height))
        {
          Pair p = new Pair(x, y);
          if (!Pathcost.ContainsKey(p))
          {
            Pathcost[p] = Pathcost[prev] + terrain[y,x];
            Backtrack[p] = prev;
            queue.Add(p);
          }
        }
      }

      public void add(Pair item){queue.Add(item);}
      public bool empty() {return queue.empty();}
      public Pair pop(){return queue.pop();}
    }

    private List<int[]> build_path(Pair source, Pair target, Dictionary<Pair, Pair> backtrack)
    {
      //Back  tracks  to  get  the  path  from  source  to  the  target
      Pair curr = target;
      List<int[]> arr = new List<int[]>();
      int[] target_list = new int[] { target.x, target.y };
      arr.Add(target_list);

      while (curr != source)
      {
        curr = backtrack[curr];
        int[] curr_list = new int[] { curr.x, curr.y };
        arr.Insert(0, curr_list);
      }
      return arr;
    }

    public List<int[]> pathTo(int startx, int starty, int endx, int endy, int[,] terrain)
    {

      Pair source = new Pair(startx, starty);
      Pair target = new Pair(endx, endy);

      int map_width = terrain.GetLength(1); 
      int map_height = terrain.GetLength(0);

      //Keep  track  of  which  nodes  seen  for  backtracking
      Dictionary<Pair, Pair> backtrack = new Dictionary<Pair, Pair>();

      //Keeps  track  of  the  pathcost  of  each  node
      Dictionary<Pair, int> pathcost = new Dictionary<Pair, int>();
      pathcost[source] = 0;

      //A  queue  for  sorting  nodes  using  the  A*  cost
      myPriorityQueue queue = new myPriorityQueue(source, target, pathcost, map_height, map_width, backtrack);

      queue.add(source);

      //do  the  search
      while (!queue.empty())
      {

        Pair node = queue.pop();
        //node.printPair();
        //If  target  is  reached  then  backtrach  and  return  the  path  from  source  to  target
        if (node.Equals(target))
        {
          return build_path(source, target, backtrack);
        }

        int x = node.x;
        int y = node.y;
        //Otherwise,  consider  the  next  step
        queue.enqueue(x - 1, y, node, terrain);
        queue.enqueue(x + 1, y, node, terrain);
        queue.enqueue(x, y - 1, node, terrain);
        queue.enqueue(x, y + 1, node, terrain);
      }

      return new List<int[]>();
    }

  }
}