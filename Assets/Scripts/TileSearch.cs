using System.Collections.Generic;
using System;

namespace TileSearch{


  /*
    Usage:
      public static Tuple<int,int> findTile(startx, int starty, T type, T[,] map);
      -- searches for the nearest tile of type "type" given a map starting from (startx, starty);
      -- returns a pair of numbers corresponding to the x and y position of that tile.
      
      Example usage:
      int[,] test_terrain = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 1}, { 0, 0, 0, 0, 0 }, {0, 0, 0, 0, 0}, {0, 0, 0, 1, 5}};
      Tuple<int,int> nearest = TileSearcher<int>.findTile(0,0,5,test_terrain);

  */
  class TileSearcher<T>{

      //searches for the closest "type" tile starting from position (startx, starty), using BFS
      public static Tuple<int,int> findTile(int startx, int starty, T type, T[,] map) {
          Queue<Tuple<int,int>> queue = new Queue<Tuple<int,int>>();
          HashSet<Tuple<int,int>> visited = new HashSet<Tuple<int,int>>();
          Tuple<int,int> source = new Tuple<int,int> (startx, starty);
          queue.Enqueue(source);
          visited.Add(source);
          int height = map.GetLength(0); 
          int width = map.GetLength(1); 

          while (queue.Count > 0 ){
              Tuple<int,int> node = queue.Dequeue();
              int x =node.Item1;
              int y = node.Item2; 
              if (map[x,y].Equals(type)){return node;}
             
              Tuple<int,int> left = new Tuple<int,int> (x-1, y);
              if (x-1 >= 0 && !(visited.Contains(left))){
                  visited.Add(left); queue.Enqueue(left);
              }
              Tuple<int,int> right = new Tuple<int,int> (x+1, y);
              if (x+1 < width && !(visited.Contains(right))){
                  visited.Add(right); queue.Enqueue(right);
              }
              Tuple<int,int> up = new Tuple<int,int> (x, y-1);
              if (y-1 >= 0 && !(visited.Contains(up))){
                  visited.Add(up); queue.Enqueue(up);
              }
              Tuple<int,int> down = new Tuple<int,int> (x, y+1);
              if (y+1 < height && !(visited.Contains(down))){
                  visited.Add(down); queue.Enqueue(down);
              }

          }

          return null; //return an empty tuple if no tile found. 
      }


  }


}