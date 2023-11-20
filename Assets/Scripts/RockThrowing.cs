using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RockThrowing : MonoBehaviour
{
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private GameObject _objectHolder;
    [SerializeField] private GameObject _heldObject;

    [SerializeField] private Transform _originalParent;

    [SerializeField] private int _maxRocks;
    public int thrownRocks;

    public bool allowThrowing;

    private void Update()
    {
        //If throwing is possible
        if (allowThrowing)
        {
            //Mouse input
            if (Input.GetMouseButtonDown(1))
            {
                //Clear the heldObject if rock isnt found anymore
                if (GameObject.Find("Rock(Clone)") == null)
                {
                    _heldObject = null;
                }
                //If not holding a object
                if (_heldObject == null)
                {
                    //If not at max amount of rocks thrown yet
                    if (thrownRocks < _maxRocks)
                    {
                        //Set Held Object
                        _heldObject = Instantiate(_rockPrefab);
                        //Set parent for object and reset position
                        _heldObject.transform.SetParent(_originalParent);
                        _heldObject.transform.localPosition = Vector3.zero;
                        //Disable gravity
                        _heldObject.GetComponent<Rigidbody>().useGravity = false;

                        _heldObject.AddComponent<FixedJoint>();
                        FixedJoint l_fj = _heldObject.GetComponent<FixedJoint>();
                        l_fj.connectedBody = _objectHolder.GetComponent<Rigidbody>();
                        l_fj.enableCollision = true;
                        thrownRocks++;
                    }
                }
            }
            //Get mouse input
            else if (Input.GetMouseButtonUp(1))
            {
                //If holding a object
                if (_heldObject != null)
                {
                    //Clear parent
                    _heldObject.transform.SetParent(null);

                    //Disable gravity 
                    _heldObject.GetComponent<Rigidbody>().useGravity = true;
                    //Disable kinematic
                    _heldObject.GetComponent<Rigidbody>().isKinematic = false;
                    //Destroy joint
                    Destroy(_heldObject.GetComponent<FixedJoint>());

                    //Remove object from hand
                    _heldObject = null;
                }
            } 
        }
    }
}
