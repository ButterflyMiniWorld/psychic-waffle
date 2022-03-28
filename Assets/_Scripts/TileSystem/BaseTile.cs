using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TilesType
{
    Meadown,
    Sand,
    Swamp
}


public class BaseTile : MonoBehaviour
{
    [SerializeField] protected TilesType typeOfTiles;

    [SerializeField] protected bool isFired;
    [SerializeField] protected bool isForest;

    [SerializeField] protected int movePointsCost;
}
