using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class MaterialChangerTool : EditorWindow
{
    [MenuItem("Tools/Material Changer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialChangerTool>("Material Changer");
    }

    /// <summary>
    /// This is when you select the object you want to change the material.
    /// </summary>
    private void OnSelectionChange()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject objects in selectedObjects)
        {
            Renderer renderer = objects.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = GetMaterialByName(objects.name);
                if (material != null)
                {
                    renderer.sharedMaterial = material;
                }
            }
        }
    }

    /// <summary>
    /// This function finds the Material you're trying to get and sees if it could find it if it did then its returning the material you're trying to get. 
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    private Material GetMaterialByName(string objectName)
    {
        // Use object name as the material name
        string materialName = objectName;

        // Search for the material in the project
        string[] guids   = AssetDatabase.FindAssets("t:Material " + materialName);
        if (guids.Length > 0)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        }

        // Material not found
        return null;
    }

    private void OnGUI()
    {
        #region Tool Explanation.
        
        //This explains what the tool does.

        GUILayout.Space(10f);

        GUILayout.Label("What does this tool do?", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        EditorGUILayout.LabelField("This tool automatically changes the material of selected objects based on their own names.");

        EditorGUILayout.LabelField("Select objects in the scene hierarchy, and their materials will be changed when there is a match.");
        #endregion

        #region Tool Example

        //This is an example case in which the tool is meant to be used.

        EditorGUILayout.Space(10f);

        GUILayout.Label("Example.", EditorStyles.boldLabel);

        EditorGUILayout.Space(5f);

        EditorGUILayout.LabelField("In the scene there is a table but it has cube material if you want to have it so it has MasterTable material change the object to MasterTabel.");
        #endregion

        #region Tool Trouble 
        
        //This is if you have trouble with using the tool and with this text hope to explain it to you.
   
        EditorGUILayout.Space(10f);

        GUILayout.Label("Trouble Shooting", EditorStyles.boldLabel);

        GUILayout.Space(5f);

        EditorGUILayout.LabelField("It doesn't matter how many you select.");

        EditorGUILayout.LabelField("If it doesn't change deselect the object and select it again.");

        EditorGUILayout.LabelField("It doesn't change if the name is 'ObjectName (1)' change it to 'ObjectName' then do the step above.");
        #endregion
    }
}
#endif