using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TilesType
{
    Meadown,
    Sand,
    Swamp
}

public struct WorldCoordinate
{
    public WorldCoordinate(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public int x;
    public int z;
}


public class BaseTile : MonoBehaviour
{
    [SerializeField] protected TilesType typeOfTiles;

    [SerializeField] protected bool isFired;
    [SerializeField] protected bool isForest;
    [SerializeField] protected bool isMountain;
    [SerializeField] protected bool isPlain;

    [SerializeField] protected int movePointsCost;

    [SerializeField] protected BaseUnit unit;
    [SerializeField] protected GameObject marker;

    private WorldCoordinate coordinate;

    public WorldCoordinate Coordinate => coordinate;

    public BaseUnit GetUnit => unit;

    public bool TryGetUnit(out BaseUnit unit)
    {
        if (this.unit != null)
        {
            unit = this.unit;
            return true;
        }
        else
        {
            unit = null;
            return false;
        }
    }

    public void Init(int x, int z)
    {
        coordinate.x = x;
        coordinate.z = z;
    }

    private void Awake()
    {
        TurnAction.Instance.OnTurnEnd += DeSelected;
    }
    private void OnDisable()
    {
        TurnAction.Instance.OnTurnEnd -= DeSelected;
    }

    public void Selected()
    {
        marker.SetActive(true);
    }
    public void DeSelected()
    {
        marker.SetActive(false);
    }

    public void UnitMove()
    {
        unit = null;
    }
    public void UnitCome(BaseUnit unit)
    {
        this.unit = unit;
        Check_State();
    }

    public void SelectedToMoveTile()
    {
        TurnAction.Instance.SelectedTile(this);
    }
    private void Check_State()
    {
        if (isFired)
        {

        }
        if (isForest)
        {

        }
        if (isMountain)
        {
            State_Mountain();
        }
        if(isPlain)
        {
            State_Plain();
        }    
    }
    private void State_Mountain()
    {
        unit.transform.position = new Vector3(unit.transform.position.x, 1, unit.transform.position.z);
    }
    private void State_Plain()
    {
        unit.transform.position = new Vector3(unit.transform.position.x, (float)0.5, unit.transform.position.z);
    }
}
