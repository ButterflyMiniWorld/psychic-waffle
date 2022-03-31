using System;
using System.Collections.Generic;
using UnityEngine;

public struct PossibleTile
{
    public PossibleTile(BaseTile tile, int cost)
    {
        this.tile = tile;
        actionCost = cost;
    }
    public BaseTile tile;
    public int actionCost;
}

public class TurnAction : Singleton<TurnAction>
{
    private BaseUnit currentUnit;
    private BaseTile currentTile;

    private BaseTile[,] tilesMap;
    private List<PossibleTile> possibleTiles = new List<PossibleTile>();

    private BaseTile tileToMove;
    private int actionCost;

    public Action OnTurnEnd;

    public void Init(BaseTile[,] map)
    {
        tilesMap = map;
    }

    public void TurnIsStart(BaseUnit unit)
    {
        currentUnit = unit;
        currentTile = currentUnit.CurrrentTile;
        GlowTiles();
    }

    private void GlowTiles()
    {
        int actionPoints = currentUnit.ActionPointsLeft;
        WorldCoordinate start = new WorldCoordinate(currentTile.Coordinate.x, currentTile.Coordinate.z);

        Recursion(currentTile, 0);
    }

    private void Recursion(BaseTile tile, int countRecursion)
    {
        if (tile != null)
        {
            PossibleTile possibleTile = new PossibleTile(tile, countRecursion);
            tile.Selected();

            if (!possibleTiles.Contains(possibleTile))
            {
                possibleTiles.Add(possibleTile);
            }
        }
        else
        {
            return;
        }
        if (countRecursion >= currentUnit.ActionPointsLeft)
        {
            return;
        }

        for (int x = tile.Coordinate.x - 1; x <= tile.Coordinate.x + 1; x++)
        {
            for (int z = tile.Coordinate.z - 1; z <= tile.Coordinate.z + 1; z++)
            {
                Debug.Log(tilesMap.GetLength(0) + "  :   " + tilesMap.GetLength(1));
                if(x < 0 || z < 0 || x > tilesMap.GetLength(0) || z > tilesMap.GetLength(1))
                {
                    break;
                }
                if (tile == tilesMap[x, z] && currentTile == tilesMap[x,z])
                {
                    break;
                }
                Recursion(tilesMap[x, z], countRecursion + 1);
            }
        }
    }

    public void SelectedTile(BaseTile selectedTile)
    {
        tileToMove = selectedTile;
    }

    public void EndTurn()
    {
        if(ContainsInPossible(tileToMove))
        {
            Vector3 pos = tileToMove.transform.position;
            pos.y = 0.5f;
            currentUnit.transform.position = pos;

            currentUnit.Move(tileToMove, actionCost);
            currentTile.UnitMove();
            tileToMove.UnitCome(currentUnit);

            possibleTiles.Clear();

            OnTurnEnd?.Invoke();
        }

    }
    public bool ContainsInPossible(BaseTile tileToMove)
    {
        for (int i = 0; i < possibleTiles.Count; i++)
        {
            if (possibleTiles[i].tile == tileToMove)
            {
                actionCost = possibleTiles[i].actionCost;
                return true;
            }
        }
        return false;
    }
}
