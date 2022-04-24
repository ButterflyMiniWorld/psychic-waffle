using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInfoHolder : Singleton<TurnInfoHolder>
{
    [HideInInspector] public List<BaseTile> allTiles = new List<BaseTile>();
    [HideInInspector] public List<BaseUnit> allUnits = new List<BaseUnit>();
    [HideInInspector] public BaseTile[,] tilesMap;

    public void InitMap(BaseTile[,] map)
    {
        tilesMap = map;
    }

    public void SearchUnits()
    {
        foreach (BaseTile tile in allTiles)
        {
            if (tile == null)
            {
                continue;
            }
            if (tile.TryGetUnit(out BaseUnit unit))
            {
                allUnits.Add(unit);
            }
        }
    } 
}
