using UnityEngine;
using UnityEditor;

public class OttomanMuseumSetup : MonoBehaviour
{
    [MenuItem("Tools/Setup Ottoman Museum")]
    public static void Execute()
    {
        // Find or create the OttomanMuseum parent
        GameObject museum = GameObject.Find("OttomanMuseum");
        if (museum == null)
        {
            museum = new GameObject("OttomanMuseum");
        }
        
        // Room dimensions (scaled down from odataslak)
        float roomWidth = 30f;   // X axis
        float roomHeight = 6f;   // Y axis
        float roomDepth = 20f;   // Z axis
        
        // Create Walls container
        GameObject walls = new GameObject("Walls");
        walls.transform.SetParent(museum.transform);
        walls.transform.localPosition = Vector3.zero;
        
        // Create Floor with Turkish Carpet
        CreateFloor(museum.transform, roomWidth, roomDepth);
        
        // Create Ceiling with wooden beams
        CreateCeiling(museum.transform, roomWidth, roomHeight, roomDepth);
        
        // Create Walls with Iznik tiles (bottom) and Kalem isi (top)
        CreateWalls(walls.transform, roomWidth, roomHeight, roomDepth);
        
        // Create Sedirs (low sofas) in corners
        CreateSedirs(museum.transform, roomWidth, roomHeight, roomDepth);
        
        // Create Sehpas (coffee tables)
        CreateSehpas(museum.transform, roomWidth, roomDepth);
        
        // Create Ottoman Lanterns with Point Lights
        CreateLanterns(museum.transform, roomWidth, roomHeight, roomDepth);
        
        // Create Display Vitrines
        CreateVitrines(museum.transform, roomWidth, roomDepth);
        
        Debug.Log("Ottoman Museum setup complete!");
    }
    
    static void CreateFloor(Transform parent, float width, float depth)
    {
        // Main floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.SetParent(parent);
        floor.transform.localPosition = new Vector3(0, 0, 0);
        floor.transform.localScale = new Vector3(width / 10f, 1, depth / 10f);
        
        // Apply carpet material
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TurkishCarpet_Mat.mat");
        if (carpetMat != null)
        {
            floor.GetComponent<Renderer>().material = carpetMat;
        }
        
        // Create individual carpet pieces for decoration
        float[] carpetPositionsX = { -8f, 0f, 8f };
        float[] carpetPositionsZ = { -5f, 5f };
        int carpetIndex = 0;
        
        foreach (float x in carpetPositionsX)
        {
            foreach (float z in carpetPositionsZ)
            {
                GameObject carpet = GameObject.CreatePrimitive(PrimitiveType.Plane);
                carpet.name = "Carpet_" + carpetIndex++;
                carpet.transform.SetParent(parent);
                carpet.transform.localPosition = new Vector3(x, 0.01f, z);
                carpet.transform.localScale = new Vector3(0.5f, 1, 0.4f);
                
                if (carpetMat != null)
                {
                    carpet.GetComponent<Renderer>().material = carpetMat;
                }
            }
        }
    }
    
    static void CreateCeiling(Transform parent, float width, float height, float depth)
    {
        GameObject ceilingContainer = new GameObject("Ceiling");
        ceilingContainer.transform.SetParent(parent);
        ceilingContainer.transform.localPosition = Vector3.zero;
        
        // Main ceiling plane
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceiling.name = "CeilingPlane";
        ceiling.transform.SetParent(ceilingContainer.transform);
        ceiling.transform.localPosition = new Vector3(0, height, 0);
        ceiling.transform.localRotation = Quaternion.Euler(180, 0, 0);
        ceiling.transform.localScale = new Vector3(width / 10f, 1, depth / 10f);
        
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WoodCeiling_Mat.mat");
        if (woodMat != null)
        {
            ceiling.GetComponent<Renderer>().material = woodMat;
        }
        
        // Create wooden beams
        Material ottomanWoodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/OttomanWood_Mat.mat");
        
        // Longitudinal beams
        for (int i = -2; i <= 2; i++)
        {
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.name = "Beam_Long_" + (i + 2);
            beam.transform.SetParent(ceilingContainer.transform);
            beam.transform.localPosition = new Vector3(i * 6f, height - 0.15f, 0);
            beam.transform.localScale = new Vector3(0.4f, 0.3f, depth - 1f);
            
            if (ottomanWoodMat != null)
            {
                beam.GetComponent<Renderer>().material = ottomanWoodMat;
            }
        }
        
        // Cross beams
        for (int i = -3; i <= 3; i++)
        {
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.name = "Beam_Cross_" + (i + 3);
            beam.transform.SetParent(ceilingContainer.transform);
            beam.transform.localPosition = new Vector3(0, height - 0.15f, i * 3f);
            beam.transform.localScale = new Vector3(width - 1f, 0.25f, 0.3f);
            
            if (ottomanWoodMat != null)
            {
                beam.GetComponent<Renderer>().material = ottomanWoodMat;
            }
        }
    }
    
    static void CreateWalls(Transform parent, float width, float height, float depth)
    {
        Material iznikMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/IznikTile_Mat.mat");
        Material kalemMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/KalemIsi_Mat.mat");
        
        float tileHeight = height * 0.4f; // Bottom 40% is Iznik tiles
        float kalemHeight = height * 0.6f; // Top 60% is Kalem isi
        
        // Wall positions and rotations
        Vector3[] wallPositions = {
            new Vector3(0, 0, -depth / 2f),      // Back wall
            new Vector3(0, 0, depth / 2f),       // Front wall
            new Vector3(-width / 2f, 0, 0),      // Left wall
            new Vector3(width / 2f, 0, 0)        // Right wall
        };
        
        Vector3[] wallRotations = {
            new Vector3(0, 0, 0),
            new Vector3(0, 180, 0),
            new Vector3(0, 90, 0),
            new Vector3(0, -90, 0)
        };
        
        Vector3[] wallScales = {
            new Vector3(width, 1, 1),
            new Vector3(width, 1, 1),
            new Vector3(depth, 1, 1),
            new Vector3(depth, 1, 1)
        };
        
        string[] wallNames = { "BackWall", "FrontWall", "LeftWall", "RightWall" };
        
        for (int i = 0; i < 4; i++)
        {
            GameObject wallContainer = new GameObject(wallNames[i]);
            wallContainer.transform.SetParent(parent);
            wallContainer.transform.localPosition = wallPositions[i];
            wallContainer.transform.localRotation = Quaternion.Euler(wallRotations[i]);
            
            // Bottom part - Iznik tiles
            GameObject bottomWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomWall.name = "IznikTiles";
            bottomWall.transform.SetParent(wallContainer.transform);
            bottomWall.transform.localPosition = new Vector3(0, tileHeight / 2f, 0);
            bottomWall.transform.localScale = new Vector3(wallScales[i].x, tileHeight, 0.1f);
            
            if (iznikMat != null)
            {
                bottomWall.GetComponent<Renderer>().material = iznikMat;
            }
            
            // Top part - Kalem isi
            GameObject topWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topWall.name = "KalemIsi";
            topWall.transform.SetParent(wallContainer.transform);
            topWall.transform.localPosition = new Vector3(0, tileHeight + kalemHeight / 2f, 0);
            topWall.transform.localScale = new Vector3(wallScales[i].x, kalemHeight, 0.1f);
            
            if (kalemMat != null)
            {
                topWall.GetComponent<Renderer>().material = kalemMat;
            }
        }
    }
    
    static void CreateSedirs(Transform parent, float width, float height, float depth)
    {
        GameObject sedirsContainer = new GameObject("Sedirs");
        sedirsContainer.transform.SetParent(parent);
        sedirsContainer.transform.localPosition = Vector3.zero;
        
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/OttomanWood_Mat.mat");
        Material fabricMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/SedirFabric_Mat.mat");
        
        // Sedir positions (along walls)
        Vector3[] sedirPositions = {
            new Vector3(-width / 2f + 1.5f, 0, -depth / 2f + 1.5f),  // Back left corner
            new Vector3(width / 2f - 1.5f, 0, -depth / 2f + 1.5f),   // Back right corner
            new Vector3(-width / 2f + 1.5f, 0, depth / 2f - 1.5f),   // Front left corner
            new Vector3(width / 2f - 1.5f, 0, depth / 2f - 1.5f)     // Front right corner
        };
        
        float[] sedirRotations = { 45f, -45f, 135f, -135f };
        
        for (int i = 0; i < sedirPositions.Length; i++)
        {
            GameObject sedir = new GameObject("Sedir_" + i);
            sedir.transform.SetParent(sedirsContainer.transform);
            sedir.transform.localPosition = sedirPositions[i];
            sedir.transform.localRotation = Quaternion.Euler(0, sedirRotations[i], 0);
            
            // Wooden base
            GameObject sedirBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sedirBase.name = "Base";
            sedirBase.transform.SetParent(sedir.transform);
            sedirBase.transform.localPosition = new Vector3(0, 0.15f, 0);
            sedirBase.transform.localScale = new Vector3(3f, 0.3f, 1f);
            
            if (woodMat != null)
            {
                sedirBase.GetComponent<Renderer>().material = woodMat;
            }
            
            // Cushion
            GameObject cushion = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cushion.name = "Cushion";
            cushion.transform.SetParent(sedir.transform);
            cushion.transform.localPosition = new Vector3(0, 0.4f, 0);
            cushion.transform.localScale = new Vector3(2.8f, 0.2f, 0.9f);
            
            if (fabricMat != null)
            {
                cushion.GetComponent<Renderer>().material = fabricMat;
            }
            
            // Back cushion
            GameObject backCushion = GameObject.CreatePrimitive(PrimitiveType.Cube);
            backCushion.name = "BackCushion";
            backCushion.transform.SetParent(sedir.transform);
            backCushion.transform.localPosition = new Vector3(0, 0.6f, -0.35f);
            backCushion.transform.localScale = new Vector3(2.8f, 0.5f, 0.2f);
            
            if (fabricMat != null)
            {
                backCushion.GetComponent<Renderer>().material = fabricMat;
            }
        }
    }
    
    static void CreateSehpas(Transform parent, float width, float depth)
    {
        GameObject sehpasContainer = new GameObject("Sehpas");
        sehpasContainer.transform.SetParent(parent);
        sehpasContainer.transform.localPosition = Vector3.zero;
        
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/OttomanWood_Mat.mat");
        
        // Sehpa positions (near sedirs)
        Vector3[] sehpaPositions = {
            new Vector3(-width / 2f + 3f, 0, -depth / 2f + 3f),
            new Vector3(width / 2f - 3f, 0, -depth / 2f + 3f),
            new Vector3(-width / 2f + 3f, 0, depth / 2f - 3f),
            new Vector3(width / 2f - 3f, 0, depth / 2f - 3f)
        };
        
        for (int i = 0; i < sehpaPositions.Length; i++)
        {
            GameObject sehpa = new GameObject("Sehpa_" + i);
            sehpa.transform.SetParent(sehpasContainer.transform);
            sehpa.transform.localPosition = sehpaPositions[i];
            
            // Table top
            GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            top.name = "Top";
            top.transform.SetParent(sehpa.transform);
            top.transform.localPosition = new Vector3(0, 0.35f, 0);
            top.transform.localScale = new Vector3(0.8f, 0.05f, 0.8f);
            
            if (woodMat != null)
            {
                top.GetComponent<Renderer>().material = woodMat;
            }
            
            // Legs
            for (int j = 0; j < 4; j++)
            {
                float angle = j * 90f * Mathf.Deg2Rad;
                float legX = Mathf.Cos(angle) * 0.25f;
                float legZ = Mathf.Sin(angle) * 0.25f;
                
                GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leg.name = "Leg_" + j;
                leg.transform.SetParent(sehpa.transform);
                leg.transform.localPosition = new Vector3(legX, 0.15f, legZ);
                leg.transform.localScale = new Vector3(0.08f, 0.3f, 0.08f);
                
                if (woodMat != null)
                {
                    leg.GetComponent<Renderer>().material = woodMat;
                }
            }
        }
    }
    
    static void CreateLanterns(Transform parent, float width, float height, float depth)
    {
        GameObject lanternsContainer = new GameObject("Lanterns");
        lanternsContainer.transform.SetParent(parent);
        lanternsContainer.transform.localPosition = Vector3.zero;
        
        Material brassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Brass_Mat.mat");
        
        // Lantern positions
        Vector3[] lanternPositions = {
            new Vector3(-8f, height - 1f, -5f),
            new Vector3(0f, height - 1f, -5f),
            new Vector3(8f, height - 1f, -5f),
            new Vector3(-8f, height - 1f, 0f),
            new Vector3(0f, height - 1.5f, 0f),  // Center lantern lower
            new Vector3(8f, height - 1f, 0f),
            new Vector3(-8f, height - 1f, 5f),
            new Vector3(0f, height - 1f, 5f),
            new Vector3(8f, height - 1f, 5f)
        };
        
        for (int i = 0; i < lanternPositions.Length; i++)
        {
            GameObject lantern = new GameObject("Lantern_" + i);
            lantern.transform.SetParent(lanternsContainer.transform);
            lantern.transform.localPosition = lanternPositions[i];
            
            // Chain
            GameObject chain = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            chain.name = "Chain";
            chain.transform.SetParent(lantern.transform);
            chain.transform.localPosition = new Vector3(0, 0.5f, 0);
            chain.transform.localScale = new Vector3(0.02f, 0.5f, 0.02f);
            
            if (brassMat != null)
            {
                chain.GetComponent<Renderer>().material = brassMat;
            }
            
            // Lantern body (octagonal approximated with cylinder)
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "Body";
            body.transform.SetParent(lantern.transform);
            body.transform.localPosition = new Vector3(0, 0, 0);
            body.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);
            
            if (brassMat != null)
            {
                body.GetComponent<Renderer>().material = brassMat;
            }
            
            // Top cap
            GameObject topCap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            topCap.name = "TopCap";
            topCap.transform.SetParent(lantern.transform);
            topCap.transform.localPosition = new Vector3(0, 0.3f, 0);
            topCap.transform.localScale = new Vector3(0.35f, 0.15f, 0.35f);
            
            if (brassMat != null)
            {
                topCap.GetComponent<Renderer>().material = brassMat;
            }
            
            // Bottom cap
            GameObject bottomCap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bottomCap.name = "BottomCap";
            bottomCap.transform.SetParent(lantern.transform);
            bottomCap.transform.localPosition = new Vector3(0, -0.3f, 0);
            bottomCap.transform.localScale = new Vector3(0.2f, 0.1f, 0.2f);
            
            if (brassMat != null)
            {
                bottomCap.GetComponent<Renderer>().material = brassMat;
            }
            
            // Point Light
            GameObject lightObj = new GameObject("PointLight");
            lightObj.transform.SetParent(lantern.transform);
            lightObj.transform.localPosition = new Vector3(0, 0, 0);
            
            Light pointLight = lightObj.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.color = new Color(1f, 0.85f, 0.5f); // Warm yellow
            pointLight.intensity = i == 4 ? 3f : 2f; // Center light brighter
            pointLight.range = 8f;
            pointLight.shadows = LightShadows.Soft;
        }
    }
    
    static void CreateVitrines(Transform parent, float width, float depth)
    {
        GameObject vitrinesContainer = new GameObject("Vitrines");
        vitrinesContainer.transform.SetParent(parent);
        vitrinesContainer.transform.localPosition = Vector3.zero;
        
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/OttomanWood_Mat.mat");
        Material glassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Glass_Mat.mat");
        
        // Vitrine positions (center area)
        Vector3[] vitrinePositions = {
            new Vector3(-6f, 0, 0),
            new Vector3(0f, 0, 0),
            new Vector3(6f, 0, 0),
            new Vector3(-3f, 0, -4f),
            new Vector3(3f, 0, -4f),
            new Vector3(-3f, 0, 4f),
            new Vector3(3f, 0, 4f)
        };
        
        for (int i = 0; i < vitrinePositions.Length; i++)
        {
            GameObject vitrine = new GameObject("Vitrine_" + i);
            vitrine.transform.SetParent(vitrinesContainer.transform);
            vitrine.transform.localPosition = vitrinePositions[i];
            
            // Wooden base/pedestal
            GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pedestal.name = "Pedestal";
            pedestal.transform.SetParent(vitrine.transform);
            pedestal.transform.localPosition = new Vector3(0, 0.4f, 0);
            pedestal.transform.localScale = new Vector3(1.2f, 0.8f, 0.8f);
            
            if (woodMat != null)
            {
                pedestal.GetComponent<Renderer>().material = woodMat;
            }
            
            // Glass case
            GameObject glassCase = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glassCase.name = "GlassCase";
            glassCase.transform.SetParent(vitrine.transform);
            glassCase.transform.localPosition = new Vector3(0, 1.1f, 0);
            glassCase.transform.localScale = new Vector3(1f, 0.6f, 0.6f);
            
            if (glassMat != null)
            {
                glassCase.GetComponent<Renderer>().material = glassMat;
            }
            
            // Wooden top frame
            GameObject topFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topFrame.name = "TopFrame";
            topFrame.transform.SetParent(vitrine.transform);
            topFrame.transform.localPosition = new Vector3(0, 1.45f, 0);
            topFrame.transform.localScale = new Vector3(1.1f, 0.1f, 0.7f);
            
            if (woodMat != null)
            {
                topFrame.GetComponent<Renderer>().material = woodMat;
            }
        }
    }
}
