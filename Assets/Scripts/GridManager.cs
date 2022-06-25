using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap terrWalkable;
    public Tilemap terrCollide;

    public TileBase water;
    public List<TileBase> grass;
    public List<TileBase> decors;

    public List<Tilemap> terrDecor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(GameState.TerrainType[,] vals)
    {
        for (int x = 0; x < vals.GetLength(0); x++)
        {
            for (int y = 0; y < vals.GetLength(1); y++)
            {
                if(vals[x,y] > 0)
                {
                    terrWalkable.SetTile(new Vector3Int(x, y, 0), grass[((x*947)+(y*953)) % grass.Count]);
                }
                else
                {
                    terrCollide.SetTile(new Vector3Int(x, y, 0), water);

                    // add terrains as we need to 
                    int i = 0;
                    for (int dx = -1; dx != 2; dx++)
                    {
                        for (int dy = -1; dy != 2; dy++)
                        {
                            var not_oob = x + dx > 0 && y + dy > 0 && x + dx < vals.GetLength(0) && y + dy < vals.GetLength(1);

                            if (not_oob && (dx != 0 || dy != 0) && vals[x + dx, y + dy] != GameState.TerrainType.WATER)
                            {
                                terrDecor[i].SetTile(new Vector3Int(x, y, 0),decors[i]);
                            }
                            if((dx != 0 || dy != 0))
                                i++;
                        }
                    }
                }
            }
        }
    }
}
