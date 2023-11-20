using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;
using UnityEditor;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.HighDefinition;

public class InspectRayCast : MonoBehaviour
{
    public Camera cam;

    public float rayDist;

    public bool isInspecting;

    public CharacterController characterController;

    public string compareTag;

    void Start()
    {
        isInspecting = false;
    }

    void Update()
    {
        //InspectRay(compareTag);
    }

    //private void InspectRay(string compareTag)
    //{
    //    RaycastHit hit;
    //    //points where the mouse in pointing
    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //    //throws a raycast
    //    if (Physics.Raycast(ray, out hit, rayDist))
    //    {
    //        //if it's level 3
    //        if (GameManager.Instance._isCell[2] && hit.collider.CompareTag(compareTag))
    //        {
    //            if (Input.GetKeyDown(KeyCode.E) && isInspecting == false)
    //            {
    //                ZoomIn();
    //            }
    //        }
    //        else if (GameManager.Instance._isCell[3] && hit.collider.CompareTag(compareTag))
    //        {
    //            //show text
    //            if (Input.GetKeyDown(KeyCode.E))
    //            {
    //                hit.collider.gameObject.GetComponentInParent<TempDoor>().RevertDoor();
    //            }
    //        }
    //    }
    //    if (GameManager.Instance._isCell[2])
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape) && isInspecting == true)
    //        {
    //            ZoomOut();
    //        }

    //        if (GameManager.Instance.gameCompleted && GameManager.Instance != null)
    //        {
    //            ZoomOut();
    //        }
    //    }
    //}
}
