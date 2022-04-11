using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TilesType
{
    Mountain,
    Plain,
    Swamp
}

public enum SeasonType
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

    [SerializeField] protected int movePointsCost;

    [SerializeField] protected BaseUnit unit;
    [SerializeField] protected GameObject marker;

    [SerializeField] protected LineRenderer tileMarker;

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
        if (unit)
        {
            unit.Move(this);
        }
    }

    private void Awake()
    {
        TurnAction.Instance.OnTurnEnd += HideTileToMove;
    }
    private void OnDisable()
    {
        TurnAction.Instance.OnTurnEnd -= HideTileToMove;
    }

    #region Path Visualizer (Включается тайл если доступен для передвижения)

    public void TileAvailableToMove()
    {
        marker.SetActive(true);
    }
    public void HideTileToMove()
    {
        marker.SetActive(false);
    }
    #endregion

    #region SelectingTile (Включается обозначения для выбранного тайла)
    public void TileIsSelected()
    {
        tileMarker.enabled = true;
        TurnAction.Instance.SelectedTile(this);
    }
    public void TileDeSelected()
    {
        tileMarker.enabled = false;
    }
    #endregion

    #region Unit move
    public void UnitMove()
    {
        unit = null;
    }
    public void UnitCome(BaseUnit unit)
    {
        this.unit = unit;
        TileUnitPlacement();
    }

    protected virtual void TileUnitPlacement() { }
    #endregion
}
