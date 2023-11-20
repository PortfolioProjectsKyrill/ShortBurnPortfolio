using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class PrefabVariation : EditorWindow
{
    private GameObject selectedPrefab;
    private Vector3 positionOffset;
    private Vector3 rotationOffset;
    private Vector3 scaleOffset;

    private bool InstantiateVariation;

    [MenuItem("Window/Prefab Variation Tool")]
    public static void ShowWindow()
    {
        GetWindow<PrefabVariation>("Prefab Variation Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Variation Tool", EditorStyles.boldLabel);

        selectedPrefab = EditorGUILayout.ObjectField("Prefab", selectedPrefab, typeof(GameObject), true) as GameObject;
        positionOffset = EditorGUILayout.Vector3Field("Position Offset", positionOffset);
        rotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", rotationOffset);
        scaleOffset = EditorGUILayout.Vector3Field("Scale Offset", scaleOffset);
        EditorGUILayout.Space();
        InstantiateVariation = EditorGUILayout.Toggle("Instantiate Bool", InstantiateVariation);

        if (GUILayout.Button("Create Variation"))
        {
            CreatePrefabVariation();
        }
    }

    private void CreatePrefabVariation()
    {
        if (selectedPrefab == null)
        {
            Debug.LogError("No prefab selected");
            return;
        }

        // Create a new instance
        GameObject newPrefabInstance = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;

        // Apply position, rotation and scale
        newPrefabInstance.transform.position += positionOffset;
        newPrefabInstance.transform.rotation *= Quaternion.Euler(rotationOffset);
        newPrefabInstance.transform.localScale += scaleOffset;

        // Save the modified prefab
        string variationName = selectedPrefab.name + "_Variation";
        string prefabPath = AssetDatabase.GetAssetPath(selectedPrefab);
        string variationPath = prefabPath.Replace(selectedPrefab.name + ".prefab", variationName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(newPrefabInstance, variationPath);

        if (InstantiateVariation)
        {
            Instantiate(newPrefabInstance, newPrefabInstance.transform.position, Quaternion.identity);
            Debug.Log("Prefab Variation spawned in the scene: " + variationName + " at: " + newPrefabInstance.transform.position);
        }

        // Destroy the temporary instance
        DestroyImmediate(newPrefabInstance);

        Debug.Log("Prefab Variation created: " + variationName);
    }
}
#endif