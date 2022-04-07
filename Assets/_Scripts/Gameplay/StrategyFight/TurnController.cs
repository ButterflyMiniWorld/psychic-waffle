using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] private List<BaseUnit> allUnits = new List<BaseUnit>();

    [SerializeField] private List<BaseUnit> sortedUnits = new List<BaseUnit>();

    private TurnAction turnAction;
    private ReadTilesMap reader;
    private BaseTile[,] tilesMap = new BaseTile[100,100];

    private int currentUnitId = 0;

    public static Action OnTurnEnd;

    private void Awake()
    {
        turnAction = GetComponent<TurnAction>();
        reader = GetComponent<ReadTilesMap>();
        turnAction.OnTurnEnd += NextTurn;
        ReadTilesMap.OnMapRead += InitMap;
    }

    private void OnDisable()
    {
        turnAction.OnTurnEnd -= NextTurn;
        ReadTilesMap.OnMapRead -= InitMap;
    }

    public void InitMap (BaseTile[,] map, int height, int width)
    {
        Debug.Log("Map is initialized");

        tilesMap = map;

        foreach (BaseTile tile in tilesMap)
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

        turnAction.Init(tilesMap, height, width);
        StartFight();
    }

    public void StartFight()
    {
        sortedUnits.Clear();
        foreach (BaseUnit unit in allUnits)
        {
            bool forIsBreak = false;
            if(sortedUnits.Count <= 0)
            {
                sortedUnits.Add(unit);
            }
            else
            {
                for(int i = 0; i < sortedUnits.Count; i++)
                {
                    if(unit.Initiative > sortedUnits[i].Initiative)
                    {
                        sortedUnits.Insert(i, unit);
                        forIsBreak = true;
                        break;
                    }
                }
                if (forIsBreak == false)
                {
                    sortedUnits.Add(unit);
                }
            }
        }
        currentUnitId = 0;

        NextTurn();
    }

    private void NextTurn()
    {
        if (currentUnitId == sortedUnits.Count)
        {
            StartFight();
            return;
        }

        turnAction.TurnIsStart(sortedUnits[currentUnitId]);
    }

    public void TurnEnd()
    {
        OnTurnEnd?.Invoke();
        currentUnitId++;
        NextTurn();
    }
}
