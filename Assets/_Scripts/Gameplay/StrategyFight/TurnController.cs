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
    }

    private void OnDisable()
    {
        turnAction.OnTurnEnd -= NextTurn;
    }

    public void InitMap (int height, int width)
    {
        Debug.Log("Map is initialized");
        tilesMap = TurnInfoHolder.Instance.tilesMap;

        TurnInfoHolder.Instance.SearchUnits();

        allUnits = TurnInfoHolder.Instance.allUnits;

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
        print(sortedUnits[currentUnitId]);
        OnTurnEnd?.Invoke();
        currentUnitId++;
        NextTurn();
    }
}
