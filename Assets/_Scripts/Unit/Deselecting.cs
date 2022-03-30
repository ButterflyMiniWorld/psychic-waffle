using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using Lean.Common;

public class Deselecting : MonoBehaviour
{
    [SerializeField] private LeanSelect lean;
 
    public void DesSelected(Vector2 delta)
    {
        if (delta.magnitude > 0.5f)
        {
            lean.DeselectAll();
        }
    }
}
