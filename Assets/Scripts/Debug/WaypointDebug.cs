using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDebug : MonoBehaviour
{
    private float _radius = 0.3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(gameObject.transform.position, _radius);
    }
}
