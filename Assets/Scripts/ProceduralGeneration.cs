using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ProceduralGeneration : MonoBehaviour
{
    private Tilemap tm;
    int[,] map;

    public int height = 20;
    public int width = 30;
    public string seed;
    public bool useRandomSeed;
    List<Tuple<int, int>> bottom_L = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(-1, 0), Tuple.Create(0, -1) };
    List<Tuple<int, int>> bottom_M = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(0, -1), Tuple.Create(1, -1) };
    List<Tuple<int, int>> bottom_R = new List<Tuple<int, int>> { Tuple.Create(1, -1), Tuple.Create(1, 0), Tuple.Create(0, -1) };

    List<Tuple<int, int>> top_L = new List<Tuple<int, int>> { Tuple.Create(-1, 0), Tuple.Create(-1, 1), Tuple.Create(0, 1) };
    List<Tuple<int, int>> top_M = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(0, 1), Tuple.Create(1, 1) };
    List<Tuple<int, int>> top_R = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(0, 1) };

    List<Tuple<int, int>> right = new List<Tuple<int, int>> { Tuple.Create(1, 1), Tuple.Create(1, 0), Tuple.Create(1, -1) };
    List<Tuple<int, int>> left = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(-1, 0), Tuple.Create(-1, -1) };

    [Range(0, 100)]
    public int randomFillPercent;


    public Tile tile_plain, tile_wall,
        tile_bottom_L, tile_bottom_M,
        tile_bottom_R, tile_left,
        tile_right, tile_top_L,
        tile_top_M, tile_top_R;

    private void Awake()
    {
        tm = GetComponent<Tilemap>();
    }
    void Start()
    {
        GenerateMap();
        fixWalls();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 10; i++)
        {
            SmoothMap();
        }
        setMainTiles();
    }

    void setMainTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    setTile(x, y, tile_wall);
                }
                if (map[x, y] == 0)
                {
                    setTile(x, y, tile_plain);
                }
            }
        }
    }

    void fixWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    setTile(x, y, ChangeTile(x, y));
                }

            }
        }
    }
    Tile ChangeTile(int gridX, int gridY)
    {
        Tuple<int, int> tile;
        List<Tuple<int, int>> tiles = new List<Tuple<int, int>>();
        for (int nbX = gridX - 1; nbX <= gridX + 1; nbX++)
        {
            for (int nbY = gridY - 1; nbY <= gridY + 1; nbY++)
            {

                if (nbX >= 0 && nbX < width && nbY >= 0 && nbY < height)
                {
                    if (map[nbX, nbY] == 1)
                    {
                        tile = Tuple.Create(nbX - gridX, nbY - gridY);
                        tiles.Add(tile);
                    }

                }
            }
        }

        // if (gridX == 5 && gridY == 19)
        // {

        //     foreach (Tuple<int, int> t in tiles)
        //     {
        //         print(t.ToString());
        //     }

        //     print(Enumerable.SequenceEqual(tiles.OrderBy(t => t), left.OrderBy(t => t)));

        // }

        if (isSame(tiles, bottom_L))
        {
            return tile_bottom_L;
        }
        if (isSame(tiles, bottom_M))
        {
            return tile_bottom_M;
        }
        if (isSame(tiles, bottom_R))
        {
            return tile_bottom_R;
        }

        if (isSame(tiles, top_L))
        {
            return tile_top_L;
        }
        if (isSame(tiles, top_M))
        {
            return tile_top_M;
        }
        if (isSame(tiles, top_R))
        {
            return tile_bottom_R;
        }

        if (isSame(tiles, right))
        {
            return tile_right;
        }
        if (isSame(tiles, left))
        {
            return tile_left;
        }

        return tile_plain;

    }

    bool isSame(List<Tuple<int, int>> first, List<Tuple<int, int>> second)
    {
        return Enumerable.SequenceEqual(first.OrderBy(t => t), second.OrderBy(t => t));
    }

    void RandomFillMap()
    {
        // if random seed, generates a new seed
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(0, 100000000).ToString();
        }

        System.Random psuedoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // map in (x,y) would either have a value of 1 or 0 depending on the fill percent
                // 1 is wall, 0 is empty space

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (psuedoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallTiles = GetSurroundingWallCount(x, y);
                if (neighborWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighborWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int nbX = gridX - 1; nbX <= gridX + 1; nbX++)
        {
            for (int nbY = gridY - 1; nbY <= gridY + 1; nbY++)
            {
                if (nbX >= 0 && nbX < width && nbY >= 0 && nbY < height)
                {
                    if (nbX != gridX || nbY != gridY)
                    {
                        wallCount += map[nbX, nbY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    void setTile(int x, int y, Tile tile)
    {
        tm.SetTile(new Vector3Int(x, y, 0), tile);
    }

    // private void OnDrawGizmos()
    // {
    //     if (map != null)
    //     {
    //         for (int x = 0; x < width; x++)
    //         {
    //             for (int y = 0; y < height; y++)
    //             {

    //             }
    //         }
    //     }
    // }
}
