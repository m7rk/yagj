using System.Collections.Generic;

namespace CostAnalysis
{

  /*
     Key of build items:
        - 0 = none
        - 1 = Beavers
        - 2 = Golems
        - 3 = Pgram Factory
        - 4 = Caterpillars
        - 5 = Shed
        - 6 = Turret 1
        - 7 = Turret 2 
        - 8 = Turret 3
        - ...
        - n = Turret k
      So a list of [1,1,5,0,2] means build in order: Beaver, Beaver, Shed, Nothing, Golems

    Example usage:
        TurrentCostAnalysis tca = new TurrentCostAnalysis();
        int[] numberOfPieces = new int[] {1,1,1,1,1};
        int[] numberOfBuilds = new int[] {0,0,0,0,0,0,0,0};
        double[] estRateOfGathering = new double[] {0,1,0,1,0};
        List<int> osg = tca.Ordering_ExpensiveGreedy(numberOfPieces, numberOfBuilds, estRateOfGathering);

    AI currently implemented:)

    1) List<int> Ordering_ExpensiveGreedy(int[] havePieces, int[] haveBuilds,  double[] rates)
       - Returns a build order which builds the most expensive item first. 

    2)  List<int> Ordering_MostNeeded(int[] havePieces, int[] haveBuilds,  double[] rates)
       - Returns a build order which builds the most lacking item first. 

       
  */

  /*
  My assumptions and thinking:
    - The only tangram pieces we have are LT, MT, ST, S, P. Label them P_0, P_1, ..., P_4. 
    - I can represent the number of pieces the AI has with a 5-element list L, where L[i] = number of pieces of type P_i
    - The cost of a Turret can be represented by a 5-element list as well. 
    - Can go for a greedy approach but there are more than one way to be greedy 
   
  */


  class TurrentCostAnalysis
  {

    //An array keeping track of the cost of building an item. Sorted by their "strength"
    List<int[]> buildCosts; 

    public TurrentCostAnalysis(){
        //Order of pieces: LT, MT, ST, S, P.
        buildCosts = new List<int[]>();
        buildCosts.Add(new int[] {0,0,0,0,0}); //None
        buildCosts.Add(new int[] {0,0,1,1,0}); //Beaver
        buildCosts.Add(new int[] {0,0,2,1,0}); //Golem
        buildCosts.Add(new int[] {1,1,0,1,0}); //Pgram factory
        buildCosts.Add(new int[] {0,0,2,2,0}); //Caterpillar
        buildCosts.Add(new int[] {5,0,0,0,0}); //Shed
        buildCosts.Add(new int[] {0,0,3,1,2}); //Turret 1
        buildCosts.Add(new int[] {0,0,4,2,1}); //Turret 2
        buildCosts.Add(new int[] {0,0,6,1,2}); //Turret 3
    } 

    public bool vectorCompare(int[] list1, double[] list2)
    {
      return vectorCompare(Array.ConvertAll<int, double>(list1, x => (double)x), list2);
    }
    public bool vectorCompare(double[] list1, int[] list2)
    {
      return vectorCompare(list1, Array.ConvertAll<int, double>(list2, x => (double)x));
    }
    public bool vectorCompare(int[] list1, int[] list2)
    {
      return vectorCompare(Array.ConvertAll<int, double>(list1, x => (double)x), Array.ConvertAll<int, double>(list2, x => (double)x));
    }

    public bool vectorCompare(double[] list1, double[] list2)
    {
      //returns true if list2[i] <= list1[i] for all i
      for (int i = 0; i < list1.GetLength(0); i++)
      {
        if (list2[i] > list1[i]) { return false; }
      }
      return true;
    }
    public double[] vectorAdd(int[] list1, double[] list2)
    {
      return vectorAdd(Array.ConvertAll<int, double>(list1, x => (double)x), list2);
    }
    public double[] vectorAdd(double[] list1, double[] list2)
    {
      double[] sum = new double[list1.GetLength(0)];
      for (int i = 0; i < list1.GetLength(0); i++)
      {
        sum[i] = list1[i] + list2[i];
      }
      return sum;
    }

    //Updates the number of pieces that the AI has after purchasing an item
    public double[] buyPieces(double[] havePieces, int item){
      double[] diff = new double[havePieces.GetLength(0)];
      for (int i = 0; i < havePieces.GetLength(0); i++)
      {
        diff[i] = havePieces[i] - buildCosts[item][i];
      }
      return diff;
    }

    //Builds the most expensive item if possible. 
    //Ideally would result in the behaviour where it would create as many resource gathering builds at the beginning before building as many turrets.
    public List<int> Ordering_ExpensiveGreedy(int[] havePieces, int[] haveBuilds,  double[] rates)
    {
      /*
        Parameters:
          int[] havePieces 
              = the number of tangram pieces the AI currently has
          int[] haveBuilds
              = the number of each item (E.g. beaver, golem, etc) that the AI has
          List<int[]> turretCosts 
              = a list of costs to build each item. Assuming that the list is sorted from least powerful turret to most powerful. 
          double[] rates 
              = the rate at which the AI is gathering resources per second. (Subject to change if second is )

          Returns:
            List<int> output
               =  A list of indices of the items that the AI wants to build in the next k seconds. (where k is the length of the list)
                  If no items can be built at the next second, then an empty list is returned
      */

      List<int> output = new List<int>();
      double[] piecesLeft = Array.ConvertAll<int, double>(havePieces, x => (double)x);
      int index = buildCosts.Count - 1;
      while (index >= 0)
      {
        //If you can build the strongest turret at the next k second then build it.
        if (vectorCompare(piecesLeft, buildCosts[index]))
        {
          piecesLeft = buyPieces(piecesLeft, index);
          piecesLeft = vectorAdd(piecesLeft, rates);
          output.Add(index);
          index = buildCosts.Count - 1;
        }
        index--;
        //This is here to prevent infinite loops
        if (output.Count >= 8){break;}
      }

      return output;
    }

    //Produces an ordering which favours the item built the least, if possible. The cost is used as a tie breaker. 
    public List<int> Ordering_MostNeeded(int[] havePieces, int[] haveBuilds, double[] rates)
    {
      /*
        Parameters:
          int[] havePieces 
              = the number of tangram pieces the AI currently has
          int[] haveBuilds
              = the number of each item (E.g. beaver, golem, etc) that the AI has
          List<int[]> turretCosts 
              = a list of costs to build each item. Assuming that the list is sorted from least powerful turret to most powerful. 
          double[] rates 
              = the rate at which the AI is gathering resources per second. (Subject to change if second is )

          Returns:
            List<int> output
               =  A list of indices of the turrent that the AI wants to build in the next second.
                  If no turret can be built at the next second, then an empty list is returned
      */

      List<int> output = new List<int>();
      double[] piecesLeft = Array.ConvertAll<int, double>(havePieces, x => (double)x);

      int[] buildsMade = new int[haveBuilds.Count()];
      PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
      for (int i = 0;  i < haveBuilds.Count(); i++){
        buildsMade[i] = haveBuilds[i];
        queue.Enqueue(i, haveBuilds[i]);
      }

      while(queue.Count > 0){
          int item = queue.Dequeue();
          if (vectorCompare(piecesLeft, buildCosts[item]))
          {
            piecesLeft = buyPieces(piecesLeft, item);
            piecesLeft = vectorAdd(piecesLeft, rates);
            buildsMade[item] += 1;
            queue.Enqueue(item, buildsMade[item]);
            output.Add(item);
          }

          if (output.Count >= 8){break;}
      }
    
      return output;
    }
  }
}