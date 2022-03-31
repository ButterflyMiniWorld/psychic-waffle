using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] private List<BaseUnit> allUnits = new List<BaseUnit>();

    private List<BaseUnit> sortedUnits = new List<BaseUnit>();

    private TurnAction turnAction;
    private ReadTilesMap reader;
    private BaseTile[,] tilesMap;

    private int currentUnitId = 0;

    public static Action OnTurnEnd;

    private void Awake()
    {
        turnAction = GetComponent<TurnAction>();
        reader = GetComponent<ReadTilesMap>();
        turnAction.OnTurnEnd += GameStarted;
        ReadTilesMap.OnMapRead += tileMap => tilesMap = tileMap;
    }

    private void OnDisable()
    {
        turnAction.OnTurnEnd -= GameStarted;
        ReadTilesMap.OnMapRead -= tileMap => tilesMap = tileMap;
    }

    public void GameStarted()
    {
        foreach(BaseUnit unit in allUnits)
        {
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
                        break;
                    }
                }
            }
        }
        StartTurn();
    }

    private void StartTurn()
    {
        turnAction.Init(tilesMap);
        turnAction.TurnIsStart(sortedUnits[currentUnitId]);
    }

    public void TurnEnd()
    {
        OnTurnEnd?.Invoke();
        GameStarted();
    }
}
