using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderDebug : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<SC_FPSController>())
    //    {
    //        GetComponent<BoxCollider>().enabled = false;//keep disabled otherwise you cant detect keypress
    //        //print("collided object name is: " + other.name);
    //    }
    //}
}
