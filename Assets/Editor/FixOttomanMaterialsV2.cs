using UnityEngine;
using UnityEditor;

public class FixOttomanMaterialsV2 : MonoBehaviour
{
    [MenuItem("Tools/Fix Ottoman Materials V2")]
    public static void Execute()
    {
        // Load textures
        Texture2D carpetTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/TurkishCarpet_Floor.png");
        Texture2D iznikTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/IznikTile_Wall.png");
        Texture2D kalemTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/KalemIsi_Wall.png");
        
        Debug.Log("Carpet texture: " + (carpetTex != null ? carpetTex.name : "NULL"));
        Debug.Log("Iznik texture: " + (iznikTex != null ? iznikTex.name : "NULL"));
        Debug.Log("Kalem texture: " + (kalemTex != null ? kalemTex.name : "NULL"));
        
        // Get the URP Lit shader
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        
        if (urpLit == null)
        {
            Debug.LogError("URP Lit shader not found!");
            return;
        }
        
        // Create new materials with very high tiling for the large model
        // The model is about 1400x400 units, so we need high tiling
        
        // Carpet Material - for floor
        Material carpetMat = new Material(urpLit);
        carpetMat.name = "TurkishCarpet_Floor_Mat";
        if (carpetTex != null)
        {
            carpetMat.SetTexture("_BaseMap", carpetTex);
            carpetMat.SetColor("_BaseColor", Color.white);
            carpetMat.SetFloat("_Smoothness", 0.1f);
            // Very high tiling for the large floor
            carpetMat.SetTextureScale("_BaseMap", new Vector2(50f, 30f));
        }
        AssetDatabase.CreateAsset(carpetMat, "Assets/Materials/Ottoman/TurkishCarpet_Floor_Mat_New.mat");
        
        // Iznik Material - for wall bottom
        Material iznikMat = new Material(urpLit);
        iznikMat.name = "IznikTile_Wall_Mat";
        if (iznikTex != null)
        {
            iznikMat.SetTexture("_BaseMap", iznikTex);
            iznikMat.SetColor("_BaseColor", Color.white);
            iznikMat.SetFloat("_Smoothness", 0.5f);
            // High tiling for walls
            iznikMat.SetTextureScale("_BaseMap", new Vector2(80f, 8f));
        }
        AssetDatabase.CreateAsset(iznikMat, "Assets/Materials/Ottoman/IznikTile_Wall_Mat_New.mat");
        
        // Kalem Isi Material - for wall upper
        Material kalemMat = new Material(urpLit);
        kalemMat.name = "KalemIsi_Wall_Mat";
        if (kalemTex != null)
        {
            kalemMat.SetTexture("_BaseMap", kalemTex);
            kalemMat.SetColor("_BaseColor", Color.white);
            kalemMat.SetFloat("_Smoothness", 0.2f);
            // High tiling for walls
            kalemMat.SetTextureScale("_BaseMap", new Vector2(80f, 20f));
        }
        AssetDatabase.CreateAsset(kalemMat, "Assets/Materials/Ottoman/KalemIsi_Wall_Mat_New.mat");
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // Assign materials
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.002", carpetMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.006", iznikMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.014", iznikMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.004", kalemMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.005", kalemMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.012", kalemMat);
        AssignMaterialToObject("odataslak/Sketchfab_model/Collada visual scene group/Material2.013", kalemMat);
        
        Debug.Log("Ottoman materials V2 fix complete!");
    }
    
    static void AssignMaterialToObject(string path, Material mat)
    {
        GameObject obj = GameObject.Find(path);
        if (obj != null)
        {
            MeshRenderer mr = obj.GetComponent<MeshRenderer>();
            if (mr != null && mat != null)
            {
                // Replace all materials
                Material[] mats = new Material[mr.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = mat;
                }
                mr.sharedMaterials = mats;
                Debug.Log("Material assigned to: " + path + " - Material: " + mat.name);
            }
        }
        else
        {
            Debug.LogWarning("Object not found: " + path);
        }
    }
}
