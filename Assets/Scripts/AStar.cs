using Priority_Queue;


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
    public class Pair : FastPriorityQueueNode{
        public int Item1;
        public int Item2;
        public Tuple<int,int> tuple;
        public Pair(int a, int b){Item1 = a; Item2 = b; tuple = new Tuple<int,int>(a,b);}

        public override string ToString(){
          return tuple.ToString();
        }

      public override bool Equals(object? obj)
      {
        if (obj is Pair){
          Pair p =(Pair) obj;
          return p.tuple.Equals(this.tuple);
        }
        return false;
      }

      public override int GetHashCode()
      {
        return tuple.GetHashCode();
      }
    }

    /////////////// The actual A* search stuff /////////////////////

    private List<Tuple<int,int>> build_path(Pair source, Pair target, Dictionary<Pair, Pair> backtrack)
    {
      //Back  tracks  to  get  the  path  from  source  to  the  target
      Pair curr = target;
      List<Tuple<int,int>> arr = new List<Tuple<int,int>>();
      arr.Add(curr.tuple);

      while (curr != source)
      {
        curr = backtrack[curr];
        arr.Insert(0, curr.tuple);
      }
      return arr;
    }

    public float distance(Pair p, Pair q)
    {
      return (float) Math.Sqrt(Math.Pow(p.Item1 - q.Item1, 2) + Math.Pow(p.Item2 - q.Item2, 2));
    }


    //Call this function to find a shortest path from the point (startx, starty) to (endx, endy) on terrain terrain
    public List<Tuple<int,int>> pathTo(int startx, int starty, int endx, int endy, int[,] terrain)
    {

      Pair source = new Pair(startx, starty);
      Pair target = new Pair(endx, endy);

      int map_width = terrain.GetLength(0);
      int map_height = terrain.GetLength(1);

      //Keep  track  of  which  nodes  seen  for  backtracking
      Dictionary<Pair, Pair> backtrack = new Dictionary<Pair, Pair>();

      //Keeps  track  of  the  pathcost  of  each  node
      Dictionary<Pair, int> pathcost = new Dictionary<Pair, int>();

      //A  queue  for  sorting  nodes  using  the  A*  cost
      Priority_Queue.FastPriorityQueue<Pair> queue = new Priority_Queue.FastPriorityQueue<Pair>(map_width * map_height);
      pathcost[source] = 0;
      queue.Enqueue(source, distance(source, target));

      //int count = 0;
      void Enqueue(int x, int y, Pair node)
      {
        //count++;
        if (x >= 0 && x < map_width && y >= 0 && y < map_height)
        {
          Pair p = new Pair(x, y);
          
          if (terrain[x,y] >= 0)
          {
            int pc =  pathcost[node] + terrain[x,y];
            float cost = pc + distance(p, target); 
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
        Pair node = queue.Dequeue();
        //Console.WriteLine(node);

        //If  target  is  reached  then  backtrack  and  return  the  path  from  source  to  target
        if (node.Equals(target))
        {
         // Console.WriteLine(count);
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
        // Console.WriteLine(count);
        return new List<Tuple<int,int>>();
    }
  }
}