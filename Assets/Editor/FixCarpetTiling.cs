using UnityEngine;
using UnityEditor;

public class FixCarpetTiling : MonoBehaviour
{
    [MenuItem("Tools/Step1 - Fix Carpet Tiling")]
    public static void Execute()
    {
        // Load the carpet material
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/TurkishCarpet_Floor_Mat_New.mat");
        
        if (carpetMat != null)
        {
            // Reduce tiling to make carpets look like individual pieces
            // Current is 50x30, reducing to 8x5 for larger, more distinct carpet patterns
            carpetMat.SetTextureScale("_BaseMap", new Vector2(8f, 5f));
            EditorUtility.SetDirty(carpetMat);
            AssetDatabase.SaveAssets();
            Debug.Log("Carpet tiling fixed to 8x5 - carpets should now look like individual pieces");
        }
        else
        {
            Debug.LogError("Carpet material not found!");
        }
    }
}
