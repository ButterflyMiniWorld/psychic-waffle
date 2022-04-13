using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour
{
    [SerializeField] protected TilesType typeOfTile;

    [SerializeField] protected BaseUnit unit;
    [SerializeField] protected GameObject marker;

    [SerializeField] protected LineRenderer tileMarker;

    [SerializeField] protected List<Transform> pointsToConnect;

    protected int movePointsCost;
    [SerializeField] protected int regionID = 0;

    private WorldCoordinate coordinate;

    public WorldCoordinate Coordinate => coordinate;
    public BaseUnit GetUnit => unit;
    public TilesType TypeOfTile => typeOfTile;
    public int MovePointsCost => movePointsCost;
    public int GetRegionID => regionID;

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

    public void InitCoordinate(int x, int z)
    {
        coordinate.x = x;
        coordinate.z = z;
        if (unit)
        {
            unit.Move(this);
        }
    }

    public void InitRegion(int regionID)
    {
        this.regionID = regionID;
        SearchConnectedTiles();
    }

    private void Awake()
    {
        switch (typeOfTile)
        {
            case TilesType.Mountain:
            {
                movePointsCost = 2;
                break;
            }
            case TilesType.Plain:
            {
                movePointsCost = 1;
                break;
            }
            default:
            {
                movePointsCost = 1;
                break;
            }
        }

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

    #region Regions creating

    public void SearchConnectedTiles()
    {
        if (pointsToConnect.Count == 0)
        {
            return;
        }

        foreach (Transform connectionPoint in pointsToConnect)
        {
            Collider[] hitColliders = Physics.OverlapSphere(connectionPoint.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<BaseTile>(out BaseTile tile))
                {
                    if (tile.TypeOfTile == typeOfTile)
                    {
                        if(tile.GetRegionID != regionID)
                        {
                            tile.InitRegion(regionID);
                        }
                    }
                }
            }
        }
    }

    #endregion
}
