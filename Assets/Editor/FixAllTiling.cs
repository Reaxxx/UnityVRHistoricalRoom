using UnityEngine;
using UnityEditor;

public class FixAllTiling : MonoBehaviour
{
    [MenuItem("Tools/Fix All Tiling")]
    public static void Execute()
    {
        // Fix Ottoman Wood material - make patterns MUCH larger (reduce tiling)
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/OttomanWood_Beam_Mat.mat");
        if (woodMat != null)
        {
            // Reduce tiling significantly so patterns are visible
            // From 30x15 to 3x2 - patterns will be 10x larger
            woodMat.SetTextureScale("_BaseMap", new Vector2(3f, 2f));
            EditorUtility.SetDirty(woodMat);
            Debug.Log("Wood material tiling fixed to 3x2 - patterns should now be visible");
        }
        
        // Fix Carpet material - make it look like fewer, larger carpets
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/TurkishCarpet_Floor_Mat_New.mat");
        if (carpetMat != null)
        {
            // Reduce tiling to 3x2 for large individual carpet look
            carpetMat.SetTextureScale("_BaseMap", new Vector2(3f, 2f));
            EditorUtility.SetDirty(carpetMat);
            Debug.Log("Carpet material tiling fixed to 3x2 - should look like individual large carpets");
        }
        
        // Also fix Kalem Isi material if it has small patterns
        Material kalemMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/KalemIsi_Wall_Mat_New.mat");
        if (kalemMat != null)
        {
            // Reduce tiling for visible patterns
            kalemMat.SetTextureScale("_BaseMap", new Vector2(5f, 3f));
            EditorUtility.SetDirty(kalemMat);
            Debug.Log("Kalem Isi material tiling fixed to 5x3");
        }
        
        // Fix Iznik tile material
        Material iznikMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/IznikTile_Wall_Mat_New.mat");
        if (iznikMat != null)
        {
            // Reduce tiling for visible patterns
            iznikMat.SetTextureScale("_BaseMap", new Vector2(8f, 2f));
            EditorUtility.SetDirty(iznikMat);
            Debug.Log("Iznik material tiling fixed to 8x2");
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("All tiling values fixed!");
    }
}
