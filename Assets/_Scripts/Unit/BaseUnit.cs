using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] private int actionPoints;
    [SerializeField] private int initiative;
    [SerializeField] private BaseTile currentTile;


    private int actionPointsLeft;

    private BaseTile[,] tilesMap;
    private BaseTile lastTile = null;

    private List<PossibleTile> availableTilesToMove = new List<PossibleTile>();
    private int costToMove;
    private PossibleTile possibleTile;

    private int mapHeight;
    private int mapWidth;

    public int Initiative => initiative;
    public int ActionPointsLeft => actionPointsLeft;
    public BaseTile CurrrentTile => currentTile;


    private void Awake()
    {
        actionPointsLeft = actionPoints;
        TurnController.OnTurnEnd += EndTurn;
    }
    private void OnDisable()
    {
        TurnController.OnTurnEnd -= EndTurn;
    }

    public void EndTurn()
    {
        actionPointsLeft = actionPoints;
    }
   
    #region Move
    public void Move(BaseTile newTile, int actionPointCost = 0)
    {
        currentTile = newTile;
        actionPointsLeft -= actionPointCost;
    }

    public virtual List<PossibleTile> TilesToMove(BaseTile[,] map, int width, int height)
    {
        availableTilesToMove.Clear();
        tilesMap = map;

        lastTile = null;

        mapWidth = width;
        mapHeight = height;

        TilesReccursionCheck(currentTile, 0);

        return availableTilesToMove;
    }

    private void TilesReccursionCheck(BaseTile tile, int costMove)
    {
        if (tile == null)
        {
            return;
        }

        if (lastTile == null)
        {
            lastTile = currentTile;
        }
        else
        {
            if(tile.GetUnit)
            {
                return;
            }

            if (tile.GetRegionID != lastTile.GetRegionID)
            {
                costMove += 2;
            }
            else
            {
                costMove += 1;
            }

            if (costMove > ActionPointsLeft)
            {
                return;
            }

            tile.TileAvailableToMove();
            possibleTile = new PossibleTile(tile, costMove);

            if (!availableTilesToMove.Contains(possibleTile))
            {
                availableTilesToMove.Add(possibleTile);
            }
        }

        for (int x = tile.Coordinate.x - 1; x <= tile.Coordinate.x + 1; x++)
        {
            for (int z = tile.Coordinate.z - 1; z <= tile.Coordinate.z + 1; z++)
            {
                if (x < 0 || z < 0 || x > mapHeight - 1 || z > mapWidth - 1)
                {
                    continue;
                }
                if (tile == tilesMap[x, z] && currentTile == tilesMap[x, z])
                {
                    continue;
                }
                lastTile = tile;
                TilesReccursionCheck(tilesMap[x, z], costMove);
            }
        }
    }
    #endregion

}
