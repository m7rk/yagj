using System.Collections.Generic;

namespace TileSearch{
  class TileSearcher<T>{

      //searches for the closest "type" tile starting from position (startx, starty), using BFS
      public static int[] findTile(int startx, int starty, T type, T[,] map) {
          Queue<int[]> queue = new Queue<int[]>();
          HashSet<int[]> visited = new HashSet<int[]>();
          int[] source = new int[] {startx, starty};
          queue.Enqueue(source);
          visited.Add(source);
          int height = map.GetLength(0); 
          int width = map.GetLength(1); 

          while (queue.Count > 0){
              int[] node = queue.Dequeue();
              int x = node[0];
              int y = node[1]; 
              if (map[y,x].Equals(type)){return node;}
             
              int[] left = new int[] {x-1, y};
              if (x-1 >= 0 && !visited.Contains(left)){
                  visited.Add(left); queue.Enqueue(left);
              }
              int[] right = new int[] {x+1, y};
              if (x+1 < width && !visited.Contains(right)){
                  visited.Add(right); queue.Enqueue(right);
              }
              int[] up = new int[] {x, y-1};
              if (y-1 >= 0 && !visited.Contains(up)){
                  visited.Add(up); queue.Enqueue(up);
              }
              int[] down = new int[] {x, y+1};
              if (y+1 < height && !visited.Contains(down)){
                  visited.Add(down); queue.Enqueue(down);
              }

          }

          return new int[0]; //return an empty array if no tile found. 
      }


  }


}