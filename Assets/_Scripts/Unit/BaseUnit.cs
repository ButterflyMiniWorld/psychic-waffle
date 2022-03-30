using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] private int actionPoints;
    [SerializeField] private int initiative;
    [SerializeField] private BaseTile currrentTile;

    private int actionPointsLeft;

    public int Initiative => initiative;
    public int ActionPointsLeft => actionPointsLeft;
    public BaseTile CurrrentTile => currrentTile;

    private void Awake()
    {
        actionPointsLeft = actionPoints;
        TurnController.OnTurnEnd += EndTurn;
    }
    private void OnDisable()
    {
        TurnController.OnTurnEnd -= EndTurn;
    }

    public void Move(BaseTile newTile, int actionPointCost)
    {
        currrentTile = newTile;
        actionPointsLeft -= actionPointCost;
    }

    public void EndTurn()
    {
        actionPointsLeft = actionPoints;
    }
}
