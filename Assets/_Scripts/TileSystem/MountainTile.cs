using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTile : BaseTile
{
    protected override void TileUnitPlacement()
    {
        unit.transform.position = new Vector3(unit.transform.position.x, 1, unit.transform.position.z);
    }
}
