using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTile : BaseTile
{
    protected override void TileUnitPlacement()
    {
        unit.transform.position = new Vector3(unit.transform.position.x, (float)0.5, unit.transform.position.z);
    }
}
