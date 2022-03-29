using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Deselecting : MonoBehaviour
{
    [SerializeField] private LeanSelectableByFinger lean;
 
    public void DesSelected(Vector2 delta)
    {
        if (delta.magnitude > 0.5f)
        {
            lean.Deselect();
        }
    }
}
