using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WaypointSetting : EditorWindow
{
    private CinemachineSmoothPath.Waypoint[] waypoints;
    public GameObject[] empties;

    private bool _hasNotSpawned = true;

    private CameraManager camManager;
    Vector3 lvlPOS = Vector3.zero;

    SerializedObject serialObj;

    public bool cellOne = false;
    public bool cellTwo = false;
    public bool cellThree = false;
    //public List<bool> cells;

    private int areYouSure;

    private string _confirmMessage = "Press again to reset the waypoint positions (USE WITH CARE, DON'T @ ME)";

    private UnityEvent switchEvent;

    [MenuItem("Window/Waypoint Setting Tool")]
    public static void ShowWindow()
    {
        if (!Application.isPlaying)
            GetWindow<WaypointSetting>("Waypoint Setting Tool");
    }

    private void OnEnable()
    {
        serialObj = new SerializedObject(this);
    }

    private void OnGUI()
    {
        //write down what the window is used for
        GUILayout.Label("Used for easy configuration of all the waypoints in the scene");

        //technically obsolete but smd
        camManager = (CameraManager)EditorGUILayout.ObjectField(camManager, typeof(CameraManager));
        camManager = FindObjectOfType<CameraManager>();

        waypoints = new CinemachineSmoothPath.Waypoint[camManager._1stVCamWaypoints.Length];
        empties = new GameObject[camManager._1stVCamWaypoints.Length];
        //cells = new List<bool>(camManager._1stVCamWaypoints.Length);

        cellOne = EditorGUILayout.Toggle("CellOne", cellOne);
        cellTwo = EditorGUILayout.Toggle("CellTwo", cellTwo);
        cellThree = EditorGUILayout.Toggle("CellThree", cellThree);

        //IDFK WHY THIS IS HERE
        //waypoints = camManager._1stVCamWaypoints;

        SerializedProperty serialProp = serialObj.FindProperty("empties");

        EditorGUILayout.PropertyField(serialProp, true);

        if (GUILayout.Button("Spawn Empties") && _hasNotSpawned)
        {
            SpawnEntities();

            lvlPOS = Vector3.zero;
        }
        GUILayout.Space(10);

        if (GUILayout.Button("Reset all Waypoint Positions"))
        {
            areYouSure++;
            if (areYouSure == 1)
            {
                ShowNotification(new GUIContent(_confirmMessage));
            }
            else if (areYouSure == 2)
            {
                for (int i = 0; i < empties.Length; i++)
                {
                    (serialProp.GetArrayElementAtIndex(i).objectReferenceValue as GameObject).transform.position = Vector3.zero;
                    ResetBools();
                }
                areYouSure = 0;
            }
        }

        serialObj.ApplyModifiedProperties();
    }

    private void SpawnEntities()
    {
        _hasNotSpawned = false;
        SerializedProperty serialProp = serialObj.FindProperty("empties");
        serialProp.arraySize = empties.Length;
        for (int i = 0; i < empties.Length; i++)
        {
            var gameObject = empties[i] = new GameObject("empty: " + i);
            empties[i].transform.position = camManager._1stVCamWaypoints[i].position;
            gameObject.transform.position = camManager._1stVCamWaypoints[i].position;
            gameObject.tag = "EmptyTag";
            gameObject.AddComponent<WaypointDebug>();
            serialProp.GetArrayElementAtIndex(i).objectReferenceValue = gameObject;
            //Debug.Log("the index is: " + i);
            //lvlPOS.x += 1;
        }
        serialObj.ApplyModifiedProperties();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        Despawn();
    }

    private void Update()
    {
        CheckForRuntime();

        SerializedProperty serialProp = serialObj.FindProperty("empties");

        //set the empties to the waypoints
        if (_hasNotSpawned == false)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                //gets the empties[] object, casts it to a gameobject then gets the vector3 of that gameobject.
                waypoints[i].position = (serialProp.GetArrayElementAtIndex(i).objectReferenceValue as GameObject).transform.position;
            }
        }

        //set the waypoints to waypoints in script

        //export to function
        BooleanControl();

        serialObj.ApplyModifiedProperties();
    }

    private void CheckForRuntime()
    {
        //ISSUE ATM IS THAT YOU CANT DESTROY THE SCENE OBJECTS ANYMORE WHEN THE GAME HAS ALREADY STARTED
        if (Application.isPlaying)
        {
            Debug.Log("this be happening matey");
            Despawn();
            Close();
        }
    }

    private void Despawn()
    {
        SerializedProperty serialProp = serialObj.FindProperty("empties");
        for (int i = 0; i < empties.Length; i++)
        {
            var obj = serialProp.GetArrayElementAtIndex(i).objectReferenceValue;
            DestroyImmediate(obj);
        }
        serialObj.ApplyModifiedProperties();
    }

    private void ResetBools()
    {
        cellOne = false;
        cellTwo = false;
        cellThree = false;
    }

    private void BooleanControl()
    {
        if (cellOne)
        {
            camManager._1stVCamWaypoints = waypoints;

            cellTwo = false;
            cellThree = false;
        }
        else if (cellTwo)
        {
            camManager._2ndVCamWaypoints = waypoints;

            cellThree = false;
            cellOne = false;
        }
        else if (cellThree)
        {
            camManager._3rdVCamWaypoints = waypoints;

            cellOne = false;
            cellTwo = false;
        }
    }
}