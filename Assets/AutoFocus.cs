using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class AutoFocus : MonoBehaviour
{
    [SerializeField] private GameObject focusOn;
    [SerializeField] private Volume vol;
    private DepthOfField Dof;

    private void Awake()
    {
        vol.profile.TryGet(out DepthOfField tmp);

        Dof = tmp;
    }

    private void Update()
    {
        float dist = Vector3.Distance(transform.position, focusOn.transform.position);
        Dof.focusDistance.value = dist;
    }
}
