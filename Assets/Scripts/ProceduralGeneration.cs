using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ProceduralGeneration : MonoBehaviour
{
    private Tilemap tm;
    public Tilemap tmWalls;
    public int[,] map;

    public int height;
    public int width;
    public string seed;
    public bool useRandomSeed;

    public Vector2 centerBoundX = new Vector2(19, 40);
    public Vector2 centerBoundY = new Vector2(15, 25);

    #region Tile Directions
    // BOTTOM
    List<Tuple<int, int>> bottom_L = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(-1, 0), Tuple.Create(0, -1) };
    List<Tuple<int, int>> bottom_L2 = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(-1, 0), Tuple.Create(0, -1), Tuple.Create(1, -1) };
    List<Tuple<int, int>> bottom_L3 = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(-1, 0), Tuple.Create(0, -1), Tuple.Create(-1, 1) };
    List<Tuple<int, int>> bottom_M = new List<Tuple<int, int>> { Tuple.Create(-1, -1), Tuple.Create(0, -1), Tuple.Create(1, -1) };
    List<Tuple<int, int>> bottom_M2 = new List<Tuple<int, int>> { Tuple.Create(0, -1), Tuple.Create(-1, -1) };
    List<Tuple<int, int>> bottom_M3 = new List<Tuple<int, int>> { Tuple.Create(1, -1), Tuple.Create(0, -1) };
    List<Tuple<int, int>> bottom_M4 = new List<Tuple<int, int>> { Tuple.Create(0, -1) };
    List<Tuple<int, int>> bottom_R = new List<Tuple<int, int>> { Tuple.Create(1, -1), Tuple.Create(1, 0), Tuple.Create(0, -1) };
    List<Tuple<int, int>> bottom_R2 = new List<Tuple<int, int>> { Tuple.Create(1, -1), Tuple.Create(1, 0), Tuple.Create(0, -1), Tuple.Create(1, 1) };
    List<Tuple<int, int>> bottom_R3 = new List<Tuple<int, int>> { Tuple.Create(1, -1), Tuple.Create(1, 0), Tuple.Create(0, -1), Tuple.Create(-1, -1) };

    // TOP
    List<Tuple<int, int>> top_L = new List<Tuple<int, int>> { Tuple.Create(-1, 0), Tuple.Create(-1, 1), Tuple.Create(0, 1) };
    List<Tuple<int, int>> top_L2 = new List<Tuple<int, int>> { Tuple.Create(-1, 0), Tuple.Create(-1, 1), Tuple.Create(0, 1), Tuple.Create(1, 1) };
    List<Tuple<int, int>> top_L3 = new List<Tuple<int, int>> { Tuple.Create(-1, 0), Tuple.Create(-1, 1), Tuple.Create(0, 1), Tuple.Create(-1, -1) };
    List<Tuple<int, int>> top_M = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(0, 1), Tuple.Create(1, 1) };
    List<Tuple<int, int>> top_M2 = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(0, 1) };
    List<Tuple<int, int>> top_M3 = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 1) };
    List<Tuple<int, int>> top_M4 = new List<Tuple<int, int>> { Tuple.Create(0, 1) };
    List<Tuple<int, int>> top_R = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0) };
    List<Tuple<int, int>> top_R2 = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0), Tuple.Create(-1, 1) };
    List<Tuple<int, int>> top_R3 = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(1, 0), Tuple.Create(1, -1) };

    // RIGHT LEFT
    List<Tuple<int, int>> right = new List<Tuple<int, int>> { Tuple.Create(1, 1), Tuple.Create(1, 0), Tuple.Create(1, -1) };
    List<Tuple<int, int>> right2 = new List<Tuple<int, int>> { Tuple.Create(1, 1), Tuple.Create(1, 0) };
    List<Tuple<int, int>> right3 = new List<Tuple<int, int>> { Tuple.Create(1, 0), Tuple.Create(1, -1) };
    List<Tuple<int, int>> right4 = new List<Tuple<int, int>> { Tuple.Create(1, 0) };
    List<Tuple<int, int>> left = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(-1, 0), Tuple.Create(-1, -1) };
    List<Tuple<int, int>> left2 = new List<Tuple<int, int>> { Tuple.Create(-1, 1), Tuple.Create(-1, 0) };
    List<Tuple<int, int>> left3 = new List<Tuple<int, int>> { Tuple.Create(-1, 0), Tuple.Create(-1, -1) };
    List<Tuple<int, int>> left4 = new List<Tuple<int, int>> { Tuple.Create(-1, 0) };

    // CORNERS
    List<Tuple<int, int>> bl = new List<Tuple<int, int>> { Tuple.Create(-1, -1) };
    List<Tuple<int, int>> br = new List<Tuple<int, int>> { Tuple.Create(1, -1) };
    List<Tuple<int, int>> tl = new List<Tuple<int, int>> { Tuple.Create(-1, 1) };
    List<Tuple<int, int>> tr = new List<Tuple<int, int>> { Tuple.Create(1, 1) };

    List<Tuple<int, int>> isEmptyAround = new List<Tuple<int, int>> { };

    #endregion Tile Directions

    [Range(0, 100)]
    public int randomFillPercent;
    public string pathToDecorationsFolder;

    GameObject[] decorationPrefabs;
    public GameObject decorationsGameObject;


    public Tile tile_plain, tile_wall,
        tile_bottom_L, tile_bottom_M,
        tile_bottom_R, tile_left,
        tile_right, tile_top_L,
        tile_top_M, tile_top_R;
    public Tile corner_b_L, corner_b_R, corner_t_L, corner_t_R;

    public Tile[] tileDecorations;

    private void Awake()
    {
        tm = GetComponent<Tilemap>();
    }
    void Start()
    {
        decorationPrefabs = Resources.LoadAll<GameObject>("Tiles/" + pathToDecorationsFolder);
        GenerateMap();
        fixWalls();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
        setMainTiles();
    }

    public void setMainTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    setTileWall(x, y, tile_wall);
                }
                if (map[x, y] == 0)
                {
                    setTile(x, y, tile_plain);
                }
            }
        }
    }

    public Tile decorationTiles()
    {
        int rand = UnityEngine.Random.Range(0, tileDecorations.Length);
        return tileDecorations[rand];
    }

    public void fixWalls()
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

    // function that returns a tile to the corresponding wall type
    Tile ChangeTile(int gridX, int gridY)
    {
        Tuple<int, int> tile;
        List<Tuple<int, int>> tiles = new List<Tuple<int, int>>();

        // checks neighbor tiles
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

        if (isSame(tiles, bottom_L) || isSame(tiles, bottom_L2) || isSame(tiles, bottom_L3))
        {
            return tile_bottom_L;
        }
        if (isSame(tiles, bottom_M) || isSame(tiles, bottom_M2) || isSame(tiles, bottom_M3) || isSame(tiles, bottom_M4))
        {
            return tile_bottom_M;
        }
        if (isSame(tiles, bottom_R) || isSame(tiles, bottom_R2) || isSame(tiles, bottom_R3))
        {
            return tile_bottom_R;
        }

        if (isSame(tiles, top_L) || isSame(tiles, top_L2) || isSame(tiles, top_L3))
        {
            return tile_top_L;
        }
        if (isSame(tiles, top_M) || isSame(tiles, top_M2) || isSame(tiles, top_M3) || isSame(tiles, top_M4))
        {
            return tile_top_M;
        }
        if (isSame(tiles, top_R) || isSame(tiles, top_R2) || isSame(tiles, top_R3))
        {
            return tile_top_R;
        }

        if (isSame(tiles, right) || isSame(tiles, right2) || isSame(tiles, right3) || isSame(tiles, right4))
        {
            return tile_right;
        }
        if (isSame(tiles, left) || isSame(tiles, left2) || isSame(tiles, left3) || isSame(tiles, left4))
        {
            return tile_left;
        }

        if (isSame(tiles, bl))
        {
            return corner_b_L;
        }

        if (isSame(tiles, br))
        {
            return corner_b_R;
        }

        if (isSame(tiles, tl))
        {
            return corner_t_L;
        }

        if (isSame(tiles, tr))
        {
            return corner_t_R;
        }

        int rand = UnityEngine.Random.Range(0, 20);

        if (rand == 1)
        {
            return decorationTiles();
        }
        else if (rand == 2 && isSame(tiles, isEmptyAround))
        {
            instantiateDecoration(new Vector3(gridX, gridY, 0));
        }
        return tile_plain;
    }

    bool isSame(List<Tuple<int, int>> first, List<Tuple<int, int>> second)
    {
        return Enumerable.SequenceEqual(first.OrderBy(t => t), second.OrderBy(t => t));
    }

    // https://stackoverflow.com/questions/3669970/compare-two-listt-objects-for-equality-ignoring-order
    // lmao what?? 
    public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
        var cnt = new Dictionary<T, int>();
        foreach (T s in list1)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]++;
            }
            else
            {
                cnt.Add(s, 1);
            }
        }
        foreach (T s in list2)
        {
            if (cnt.ContainsKey(s))
            {
                cnt[s]--;
            }
            else
            {
                return false;
            }
        }
        return cnt.Values.All(c => c == 0);
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
                // 1 is wall, 0 is walkable space

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else if ((x > centerBoundX.x && x < centerBoundX.y) && (y > centerBoundY.x && y < centerBoundY.y))
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = (psuedoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    public void SmoothMap()
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

    void setTileWall(int x, int y, Tile tile)
    {
        tmWalls.SetTile(new Vector3Int(x, y, 0), tile);
    }

    void instantiateDecoration(Vector3 pos)
    {
        int rand = UnityEngine.Random.Range(0, decorationPrefabs.Length);
        GameObject newDec = Instantiate(decorationPrefabs[rand], pos, Quaternion.identity);
        newDec.transform.parent = decorationsGameObject.transform;
    }
}
