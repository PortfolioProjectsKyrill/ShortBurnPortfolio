using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreWallDoorCollision : MonoBehaviour
{
    void Start()
    {
        Physics.IgnoreLayerCollision(15, 16);
    }
}
