using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReadTilesMap : MonoBehaviour
{
    private BaseTile[,] tileMap;

    private int left, right, down, up = 0;
    private int regionCounts = 0;

    private List<Transform> tilesTransform = new List<Transform>();
    private List<BaseTile> tiles = new List<BaseTile>();

    public static Action<BaseTile[,], int, int> OnMapRead;

    private BaseTile[,] BaseTiles => tileMap;

    public int GetWidth => -left + right;
    public int GetHeight => -down + up;


    private void Start()
    {
        foreach(Transform childTile in transform)
        {
            tilesTransform.Add(childTile);
        }

        foreach(Transform tile in tilesTransform)
        {
            tiles.Add(tile.GetComponent<BaseTile>());
            CheckPosition(tile.position);
        }

        TurnInfoHolder.Instance.allTiles = tiles;

        int width = (-left + right);
        int height = (-down + up);
        tileMap = new BaseTile[width, height];

        Debug.Log($"left: {left}; right: {right}; down: {down}; up: {up}; ");

        Debug.Log(tilesTransform.Count);

        foreach (Transform tile in tilesTransform)
        {
            int x = (int)((tile.position.x - left - 0.5));
            int z = (int)((tile.position.z - up + 0.5) * (-1));

            tileMap[x, z] = tile.GetComponent<BaseTile>();
            tileMap[x, z].InitCoordinate(x, z);

            if (tileMap[x, z].GetRegionID == 0)
            {
                regionCounts++;
                tileMap[x, z].InitRegion(regionCounts);
            }

            tileMap[x, z].SearchConnectedTiles();
        }

        TurnInfoHolder.Instance.InitMap(tileMap);

        GetComponent<TurnController>().InitMap(width, height);
    }

    private void CheckPosition(Vector3 tilePosition)
    {
        if (tilePosition.x < left)
        {
            left = (int)(tilePosition.x - 0.5);
        }
        if(tilePosition.x > right)
        {
           right = (int)(tilePosition.x + 0.5);
        }
        if (tilePosition.z < down)
        {
            down = (int)(tilePosition.z - 0.5);
        }
        if(tilePosition.z > up)
        {
            up = (int)(tilePosition.z + 0.5);
        }
    }
}
