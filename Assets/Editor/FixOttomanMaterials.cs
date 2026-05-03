using UnityEngine;
using UnityEditor;

public class FixOttomanMaterials : MonoBehaviour
{
    [MenuItem("Tools/Fix Ottoman Materials")]
    public static void Execute()
    {
        // Load textures
        Texture2D carpetTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/TurkishCarpet_Floor.png");
        Texture2D iznikTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/IznikTile_Wall.png");
        Texture2D kalemTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/KalemIsi_Wall.png");
        
        // Get the URP Lit shader
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        
        if (urpLit == null)
        {
            Debug.LogError("URP Lit shader not found!");
            return;
        }
        
        // Fix Carpet Material
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/TurkishCarpet_Floor_Mat.mat");
        if (carpetMat != null && carpetTex != null)
        {
            carpetMat.shader = urpLit;
            carpetMat.SetTexture("_BaseMap", carpetTex);
            carpetMat.SetColor("_BaseColor", Color.white);
            carpetMat.SetFloat("_Smoothness", 0.1f);
            carpetMat.mainTextureScale = new Vector2(8f, 6f);
            EditorUtility.SetDirty(carpetMat);
            Debug.Log("Carpet material fixed with texture: " + carpetTex.name);
        }
        
        // Fix Iznik Material
        Material iznikMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/IznikTile_Wall_Mat.mat");
        if (iznikMat != null && iznikTex != null)
        {
            iznikMat.shader = urpLit;
            iznikMat.SetTexture("_BaseMap", iznikTex);
            iznikMat.SetColor("_BaseColor", Color.white);
            iznikMat.SetFloat("_Smoothness", 0.5f);
            iznikMat.mainTextureScale = new Vector2(12f, 2f);
            EditorUtility.SetDirty(iznikMat);
            Debug.Log("Iznik material fixed with texture: " + iznikTex.name);
        }
        
        // Fix Kalem Isi Material
        Material kalemMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Ottoman/KalemIsi_Wall_Mat.mat");
        if (kalemMat != null && kalemTex != null)
        {
            kalemMat.shader = urpLit;
            kalemMat.SetTexture("_BaseMap", kalemTex);
            kalemMat.SetColor("_BaseColor", Color.white);
            kalemMat.SetFloat("_Smoothness", 0.2f);
            kalemMat.mainTextureScale = new Vector2(15f, 6f);
            EditorUtility.SetDirty(kalemMat);
            Debug.Log("Kalem isi material fixed with texture: " + kalemTex.name);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        // Now reassign materials to the mesh renderers
        ReassignMaterials(carpetMat, iznikMat, kalemMat);
        
        Debug.Log("Ottoman materials fix complete!");
    }
    
    static void ReassignMaterials(Material carpetMat, Material iznikMat, Material kalemMat)
    {
        // Floor - Material2.002
        GameObject floor = GameObject.Find("odataslak/Sketchfab_model/Collada visual scene group/Material2.002");
        if (floor != null)
        {
            MeshRenderer mr = floor.GetComponent<MeshRenderer>();
            if (mr != null && carpetMat != null)
            {
                Material[] mats = mr.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = carpetMat;
                }
                mr.sharedMaterials = mats;
                Debug.Log("Floor material reassigned");
            }
        }
        
        // Wall bottom - Material2.006 and Material2.014
        string[] iznikObjects = { 
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.006",
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.014"
        };
        
        foreach (string path in iznikObjects)
        {
            GameObject obj = GameObject.Find(path);
            if (obj != null)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (mr != null && iznikMat != null)
                {
                    mr.sharedMaterial = iznikMat;
                    Debug.Log("Iznik material assigned to: " + path);
                }
            }
        }
        
        // Wall upper - Material2.004, Material2.005, Material2.012, Material2.013
        string[] kalemObjects = { 
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.004",
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.005",
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.012",
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.013"
        };
        
        foreach (string path in kalemObjects)
        {
            GameObject obj = GameObject.Find(path);
            if (obj != null)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (mr != null && kalemMat != null)
                {
                    mr.sharedMaterial = kalemMat;
                    Debug.Log("Kalem isi material assigned to: " + path);
                }
            }
        }
    }
}
