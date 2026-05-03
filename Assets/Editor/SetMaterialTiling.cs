using UnityEngine;
using UnityEditor;

public class SetMaterialTiling : MonoBehaviour
{
    [MenuItem("Tools/Set Ottoman Material Tiling")]
    public static void Execute()
    {
        // Set tiling for floor carpet material
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/TurkishCarpet_Floor_Mat.mat");
        if (carpetMat != null)
        {
            carpetMat.mainTextureScale = new Vector2(15f, 10f);
            EditorUtility.SetDirty(carpetMat);
            Debug.Log("Carpet material tiling set to 15x10");
        }
        
        // Set tiling for Iznik tile material
        Material iznikMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/IznikTile_Wall_Mat.mat");
        if (iznikMat != null)
        {
            iznikMat.mainTextureScale = new Vector2(20f, 3f);
            EditorUtility.SetDirty(iznikMat);
            Debug.Log("Iznik tile material tiling set to 20x3");
        }
        
        // Set tiling for Kalem isi material
        Material kalemMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/KalemIsi_Wall_Mat.mat");
        if (kalemMat != null)
        {
            kalemMat.mainTextureScale = new Vector2(20f, 8f);
            EditorUtility.SetDirty(kalemMat);
            Debug.Log("Kalem isi material tiling set to 20x8");
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("Material tiling setup complete!");
    }
}
