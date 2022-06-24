using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap terrWalkable;
    public Tilemap terrCollide;

    public TileBase water;
    public TileBase grass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(bool[,] vals)
    {
        for (int x = 0; x < vals.GetLength(0); x++)
        {
            for (int y = 0; y < vals.GetLength(1); y++)
            {
                if(vals[x,y])
                {
                    terrWalkable.SetTile(new Vector3Int(x, y, 0), grass);
                }
                else
                {
                    terrCollide.SetTile(new Vector3Int(x, y, 0), water);
                }
            }
        }
    }
}
