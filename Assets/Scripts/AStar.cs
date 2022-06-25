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

    //Have a hashable and comparable Pair object to reduce clutter in the code. 
    //This class is used to represent points
    public class Pair
    {
      public int x;
      public int y;
      public Pair(int a, int b) { x = a; y = b; }

      public override bool Equals(object obj)
      {
        if (obj is Pair)
        {
          Pair p = (Pair)obj;
          return p.x == x && p.y == y;
        }
        return false;
      }

      public override int GetHashCode() { return (x.GetHashCode() + y.GetHashCode()).GetHashCode(); }

      public int Abs(int x) { return (x >= 0) ? x : -x; }

      public int distance(Pair p)
      {
        //Distance is a function of the two points and the terrain 
        //Manhattan  distance,  but  can  be  changed  to  Euclidean  distance  if  leads  to  better  performance
        return Abs(this.x - p.x) + Abs(this.y - p.y);
      }

    }

    //My own impementation of PriorityQueue using a min heap.
    public class PriorityQueue
    {
      Dictionary<Pair, int> Pathcost;
      Pair Target;
      List<Pair> list;

      public int Compare(Pair p1, Pair p2)
      {
        int heuristic_1 = Pathcost[p1] + p1.distance(Target);
        int heuristic_2 = Pathcost[p2] + p1.distance(Target); ;
        if (heuristic_1 == heuristic_2) return 0;
        if (heuristic_1 < heuristic_2) return 1;
        return -1;
      }

      public PriorityQueue(ref Dictionary<Pair, int> pathcost, Pair target)
      {
        Pathcost = pathcost;
        Target = target;
        list = new List<Pair>();
      }

      public void siftUp(int i)
      {
        int smallest = i;
        int n = list.Count;

        if ((i / 2 >= 0) && (Compare(list[i / 2], list[smallest]) <= 0)) { smallest = i / 2; }

        if (smallest != i)
        {
          Pair temp = list[smallest];
          list[smallest] = list[i];
          list[i] = temp;
          siftUp(smallest);
        }
      }

      public void siftDown(int i)
      {
        int smallest = i;
        int n = list.Count;

        if ((2 * i < n) && (Compare(list[2 * i], list[smallest]) <= 0)) { smallest = 2 * i; }
        if ((2 * i + 1 < n) && (Compare(list[2 * i + 1], list[smallest]) <= 0)) { smallest = 2 * i + 1; }

        if (smallest != i)
        {
          Pair temp = list[smallest];
          list[smallest] = list[i];
          list[i] = temp;
          siftDown(smallest);
        }
      }

      //Insert and element to the queue
      public void Add(Pair p1)
      {
        if (!list.Contains(p1))
        {
          list.Add(p1);
          siftUp(list.Count - 1);
        }
      }

      //Returns true if queue is empty
      public bool empty() { return list.Count == 0; }

      //Pops and returns the minimum element
      public Pair pop()
      {
        Pair item = list[0];
        list[0] = list[list.Count - 1];
        siftDown(0);
        list.RemoveAt(list.Count - 1);
        return item;
      }
    }

    //A wrapper for the Proirity Queue so that it will work for a particular map
    public class myPriorityQueue
    {
      PriorityQueue queue;
      Dictionary<Pair, int> Pathcost;
      int Map_width;
      int Map_height;
      Dictionary<Pair, Pair> Backtrack;

      public myPriorityQueue(Pair source, Pair target, ref Dictionary<Pair, int> pathcost, int map_height, int map_width, ref Dictionary<Pair, Pair> backtrack)
      {
        queue = new PriorityQueue(ref pathcost, target);
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
          int v = Pathcost[prev] + terrain[y, x];
          if (!Pathcost.ContainsKey(p) || v < Pathcost[p])
          {
            Pathcost[p] = v;
            Backtrack[p] = prev;
            queue.Add(p);
          }
        }
      }

      public void Add(Pair item)
      {
        Pathcost[item] = 0;
        queue.Add(item);
      }
      public bool empty() { return queue.empty(); }
      public Pair pop() { return queue.pop(); }

    }


    /////////////// The actual A* search stuff /////////////////////

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

    //Call this function to find a shortest path from the point (startx, starty) to (endx, endy) on terrain terrain
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

      //A  queue  for  sorting  nodes  using  the  A*  cost
      myPriorityQueue queue = new myPriorityQueue(source, target, ref pathcost, map_height, map_width, ref backtrack);
      queue.Add(source);

      //do  the  search
      while (!queue.empty())
      {
        //queue.printQueue();
        Pair node = queue.pop();

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

      //Return an empty list if no path is possible
      return new List<int[]>();
    }

  }
}