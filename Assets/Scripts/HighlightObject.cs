using UnityEngine;

/// <summary>
/// This class spawns in a light object to the object that the script is attached to.
/// </summary>
public class HighlightObject : MonoBehaviour
{
    #region Variables
    
    #region Vector3
    // Offset the position when creating the light.
    [SerializeField] private Vector3 offset;
    #endregion

    #region Light
    // The light prefab you want to spawn. 
    [SerializeField] private Light lightPrefab;
    #endregion

    #endregion

    #region Functions
    #region Start()
    private void Start()
    {
        // This calculates the position the object is going to spawn in.
        Vector3 position = transform.position + offset;

        // This Spawns in the object.
        //Light newLight   = Instantiate(lightPrefab, position, Quaternion.identity);
        //newLight.transform.SetParent(transform);
    }
    #endregion
    #endregion

}