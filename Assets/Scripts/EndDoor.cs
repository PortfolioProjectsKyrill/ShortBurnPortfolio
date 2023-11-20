using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EndDoor : MonoBehaviour
{
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;

    [SerializeField] private Quaternion[] _startRotations;
    [SerializeField] private Quaternion[] _endRotations;

    [SerializeField] private int _raycastRadius;
    [SerializeField] private List<CapsuleCollider> _cColliders;
    [SerializeField] private Collider[] c;
    private Coroutine _currentCoroutine;

    private bool _hasRotated = false;
    private bool _checking = false;
    private void Start()
    {
        _cColliders = new List<CapsuleCollider>(2);

        _startRotations[0] = Quaternion.identity;
        _startRotations[1] = Quaternion.identity;

        StartCoroutine(FreezeRot());
    }

    private void Update()
    {
        if (GameManager.Instance.gameCompleted && !_hasRotated)
        {
            _hasRotated = true;
            StartCoroutine(LerpFunction(_endRotations[0], 1.5f, _leftDoor));
            StartCoroutine(LerpFunction(_endRotations[1], 1.5f, _rightDoor));
        }

        if (_checking)
        {
            _currentCoroutine = StartCoroutine(CheckForPlayers());
        }
        if (_hasRotated)
        {
            StopCoroutine(_currentCoroutine);
        }
    }

    private IEnumerator LerpFunction(Quaternion endRot, float sec, GameObject gameObject)
    {
        float time = 0;
        Quaternion startRot = gameObject.transform.rotation;
        while (time < sec)
        {
            gameObject.transform.rotation = Quaternion.Lerp(startRot, endRot, time / sec);
            time += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.rotation = endRot;
    }

    private IEnumerator FreezeRot()
    {
        if (!_hasRotated)
        {
            _leftDoor.GetComponentInChildren<Rigidbody>().freezeRotation = true;
            yield return new WaitForSeconds(0.45f);
            StartCoroutine(FreezeRot());
        }
        else if (_hasRotated)
        {
            _leftDoor.GetComponentInChildren<Rigidbody>().freezeRotation = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SC_FPSController>())
        {
            _checking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _checking = false;
    }

    private IEnumerator CheckForPlayers()
    {
        _checking = false;
        c = Physics.OverlapSphere(transform.position, _raycastRadius);
        _cColliders.Clear();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i].GetType() == typeof(CapsuleCollider))
            {
                _cColliders.Add((CapsuleCollider)c[i]);
            }
        }
        if (_cColliders.Count == 2)
        {
            GameManager.Instance.gameCompleted = true;
        }
        print("the length of the cc array is: " + _cColliders.Count);
        
        yield return new WaitForSeconds(1);
        _checking = true;
    }
}
