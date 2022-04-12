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
    private List<PossibleTile> availableTilesToMove = new List<PossibleTile>();

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

    public void Move(BaseTile newTile, int actionPointCost = 0)
    {
        currentTile = newTile;
        actionPointsLeft -= actionPointCost;
    }

    public void EndTurn()
    {
        actionPointsLeft = actionPoints;
    }

    public virtual List<PossibleTile> TilesToMove(BaseTile[,] map, int width, int height)
    {
        availableTilesToMove.Clear();
        tilesMap = map;

        mapWidth = width;
        mapHeight = height;

        TilesReccursionCheck(currentTile, 0);

        return availableTilesToMove;
    }

    private void TilesReccursionCheck (BaseTile tile, int countRecursion)
    {
        if (tile == null)
        {
            return;
        }

        PossibleTile possibleTile = new PossibleTile(tile, countRecursion);
        tile.TileAvailableToMove();

        if (!availableTilesToMove.Contains(possibleTile))
        {
            availableTilesToMove.Add(possibleTile);
        }

        if (countRecursion >= ActionPointsLeft)
        {
            return;
        }

        for (int x = tile.Coordinate.x - 1; x <= tile.Coordinate.x + 1; x++)
        {
            for (int z = tile.Coordinate.z - 1; z <= tile.Coordinate.z + 1; z++)
            {
                if (x < 0 || z < 0 || x > mapHeight  - 1 || z > mapWidth - 1)
                {
                    continue;
                }
                if (tile == tilesMap[x, z] && currentTile == tilesMap[x, z])
                {
                    continue;
                }
                TilesReccursionCheck(tilesMap[x, z], countRecursion + 1);
            }
        }
    }
}
