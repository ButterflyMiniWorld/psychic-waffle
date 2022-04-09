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

    private int height;
    private int width;

    public Action OnTurnEnd;


    public void Init(BaseTile[,] map, int height, int width)
    {
        this.height = height;
        this.width = width;
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
        possibleTiles.Clear();
        possibleTiles.AddRange(currentUnit.TilesToMove(tilesMap, width, height));
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
