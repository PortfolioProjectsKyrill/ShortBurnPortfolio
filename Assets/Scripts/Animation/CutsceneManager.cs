using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private float _time;
    public static CutsceneManager Instance;
    [SerializeField] private KeyCode _key;
    [SerializeField] private BoxCollider[] _triggers;
    [SerializeField] private Animation[] _animation;
    [SerializeField] private Transform _playerTransform;
    private float _minPlayerDist;

    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private bool _animationIsPlaying = false;

    public int _cellIndex;

    private bool _hasLogged;
    
    //TODO implement events with unity events ipv alleen triggers

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(Instance);
    }
    private void Update()
    {
        _cellIndex = CurrentCellNumber();
        //ERROR OMDAT ER NIETS IN animations ZIT
        for (int i = 0; i < _triggers.Length; i++)
        {
            PlayerCheck(_triggers[i], _animation);
        }
    }

    private void PlayerCheck(BoxCollider trigger, Animation[] anim)
    {
        //check if there are any triggers assigned
        if (trigger.enabled)
        {
            //if-statement length check calculation
            _minPlayerDist = trigger.size.x <= trigger.size.z ? trigger.size.z : trigger.size.x;
            //calculate the distance between center of the box collider and the player
            float l_dist = Vector3.Distance(trigger.transform.position, _playerTransform.position);//implement with both player (FOR LOOP???)
            //if the key is pressed
            if (Input.GetKeyDown(_key) && l_dist <= _minPlayerDist && anim != null)
            {
                PlayerAnim(anim, _cellIndex);
                //colliders disables itself
            }
        }
        else if (!trigger.isTrigger)
        {
            Debug.LogError("Assigned BoxCollider isn't a non-trigger");
        }
        else if (!trigger.enabled && !_hasLogged)
        {
            _hasLogged = true;
            Debug.LogError("BoxCollider is disabled");
        }
    }

    private void PlayerAnim(Animation[] l_animation, int l_cellIndex)
    {
        if (_animationIsPlaying == false && l_animation.Length != 0)
        {
            //hopefully this also executes the animation
            _animationIsPlaying = l_animation[l_cellIndex].Play();//NEEDS TEST ANIMATIONS
        }
        //checks if there are any animations
        else if (l_animation.Length == 0)
        {
            Debug.LogError("animation array is not filled");
        }

        //gets set true in animation
        if (_animationIsPlaying == true)
        {
            //play the camera
            _cameraManager.PlayTrack(l_cellIndex);
        }
    }

    /// <summary>
    /// checks trigger boxcollider (used in cutscenes) to see which cell is complete (Works :) )
    /// </summary>
    /// <returns></returns>
    public int CurrentCellNumber()
    {
        //IF THIS DOESNT WORK IT'S BECAUSE TRIGGERS AREN'T BEING DISABLED
        int l_cellIndex = 0;

        if (!_triggers[0].enabled && !_triggers[1].enabled && _triggers[2].enabled)
        {
            l_cellIndex++;
            return l_cellIndex;
        }
        else if (!_triggers[0].enabled && !_triggers[1].enabled && !_triggers[2].enabled)
        {
            l_cellIndex = 2;
            return l_cellIndex;
        }
        else
        {
            return 0;
        }
    }
}
