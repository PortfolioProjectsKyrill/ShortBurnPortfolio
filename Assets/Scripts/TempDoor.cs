using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDoor : MonoBehaviour
{
    Quaternion defaultOrientation;

    [SerializeField] private GameObject _realDoor;
    private bool _rotateBack = false;

    private HingeJoint hinge;

    private bool _isNormalDoor = true;

    [SerializeField] private float _playerDist;

    [SerializeField] private Transform _player;

    [SerializeField] private float _playerInRange;

    [SerializeField] private Animator _animator;

    private bool _hasReverted = false;

    public bool _doorUnlocked = false;

    private Rigidbody _rb;

    private void Start()
    {
        defaultOrientation = _realDoor.transform.rotation;
        hinge = _realDoor.GetComponent<HingeJoint>();

        _rb = GetComponentInChildren<Rigidbody>();

        _rb.freezeRotation = true;
    }

    private void Update()
    {
        if (_doorUnlocked == true)
        {
            _rb.freezeRotation = false;

            _playerDist = Vector3.Distance(transform.position, _player.transform.position);

            if (_playerDist <= _playerInRange && _hasReverted == false)
            {
                _animator.SetBool("isVisible", true);
            }
            else if (_playerDist >= _playerInRange)
            {
                _animator.SetBool("isVisible", false);
            }
        }
    }

    public void RevertDoor()
    {
        if (_playerDist <= _playerInRange)
        {
            _hasReverted = true;
            StartCoroutine(LerpFunction(defaultOrientation, 1.5f, _realDoor));
            _animator.SetBool("isVisible", false);
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
        transform.rotation = endRot;
        _rotateBack = false;
        _hasReverted = false;
    }
}