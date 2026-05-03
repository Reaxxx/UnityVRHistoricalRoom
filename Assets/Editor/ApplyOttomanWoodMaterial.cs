using UnityEngine;
using UnityEditor;

public class ApplyOttomanWoodMaterial : MonoBehaviour
{
    [MenuItem("Tools/Step3 - Apply Ottoman Wood Material")]
    public static void Execute()
    {
        // Load texture
        Texture2D woodTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Ottoman/OttomanWood_Beam.png");
        
        if (woodTex == null)
        {
            Debug.LogError("Ottoman wood texture not found!");
            return;
        }
        
        // Get URP Lit shader
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        
        // Create Ottoman Wood material
        Material ottomanWoodMat = new Material(urpLit);
        ottomanWoodMat.name = "OttomanWood_Beam_Mat";
        ottomanWoodMat.SetTexture("_BaseMap", woodTex);
        ottomanWoodMat.SetColor("_BaseColor", Color.white);
        ottomanWoodMat.SetFloat("_Smoothness", 0.3f);
        ottomanWoodMat.SetTextureScale("_BaseMap", new Vector2(30f, 15f));
        
        // Save material
        AssetDatabase.CreateAsset(ottomanWoodMat, "Assets/Materials/Ottoman/OttomanWood_Beam_Mat.mat");
        AssetDatabase.SaveAssets();
        
        Debug.Log("Ottoman wood material created");
        
        // Apply to gray areas (ceiling beams and window frames)
        string[] grayObjects = {
            "odataslak/Sketchfab_model/Collada visual scene group/Material2",      // Ceiling/roof structure
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.009",  // Ceiling/roof structure mirror
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.003",  // Window frames
            "odataslak/Sketchfab_model/Collada visual scene group/Material2.011"   // Window frames mirror
        };
        
        foreach (string path in grayObjects)
        {
            GameObject obj = GameObject.Find(path);
            if (obj != null)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.sharedMaterial = ottomanWoodMat;
                    Debug.Log("Ottoman wood material applied to: " + path);
                }
            }
            else
            {
                Debug.LogWarning("Object not found: " + path);
            }
        }
        
        Debug.Log("Step 3 complete - Ottoman wood material applied to gray areas!");
    }
}
