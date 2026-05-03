using UnityEngine;
using UnityEditor;

public class OttomanMuseumSetupV2 : MonoBehaviour
{
    [MenuItem("Tools/Setup Ottoman Museum V2")]
    public static void Execute()
    {
        // Odataslak bounds: 
        // Size: 1472 x 235 x 430
        // Min: (-736, -17, -215)
        // Max: (736, 218, 215)
        // Center: (0, 100, 0)
        
        // Room dimensions based on odataslak
        float roomWidth = 1400f;   // X axis
        float roomHeight = 200f;   // Y axis  
        float roomDepth = 400f;    // Z axis
        float floorY = 0f;         // Floor level
        float ceilingY = 180f;     // Ceiling level
        
        // Create main container
        GameObject museum = new GameObject("OttomanMuseumElements");
        museum.transform.position = Vector3.zero;
        
        // Load materials
        Material iznikMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/IznikTile_Mat.mat");
        Material kalemMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/KalemIsi_Mat.mat");
        Material carpetMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TurkishCarpet_Mat.mat");
        Material woodMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/OttomanWood_Mat.mat");
        Material ceilingMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/WoodCeiling_Mat.mat");
        Material fabricMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/SedirFabric_Mat.mat");
        Material brassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Brass_Mat.mat");
        Material glassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Glass_Mat.mat");
        
        // Create Floor with carpets
        CreateFloor(museum.transform, roomWidth, roomDepth, floorY, carpetMat);
        
        // Create Ceiling with wooden beams
        CreateCeiling(museum.transform, roomWidth, roomDepth, ceilingY, ceilingMat, woodMat);
        
        // Create Walls with Iznik tiles (bottom) and Kalem isi (top)
        CreateWalls(museum.transform, roomWidth, roomHeight, roomDepth, floorY, iznikMat, kalemMat);
        
        // Create Sedirs (low sofas) along walls
        CreateSedirs(museum.transform, roomWidth, roomDepth, floorY, woodMat, fabricMat);
        
        // Create Sehpas (coffee tables)
        CreateSehpas(museum.transform, roomWidth, roomDepth, floorY, woodMat);
        
        // Create Ottoman Lanterns with Point Lights
        CreateLanterns(museum.transform, roomWidth, roomDepth, ceilingY, brassMat);
        
        // Create Display Vitrines
        CreateVitrines(museum.transform, roomWidth, roomDepth, floorY, woodMat, glassMat);
        
        Debug.Log("Ottoman Museum V2 setup complete!");
    }
    
    static void CreateFloor(Transform parent, float width, float depth, float floorY, Material carpetMat)
    {
        GameObject floorContainer = new GameObject("Floor");
        floorContainer.transform.SetParent(parent);
        floorContainer.transform.localPosition = Vector3.zero;
        
        // Create multiple carpet pieces across the floor
        float carpetWidth = 150f;
        float carpetDepth = 100f;
        
        int carpetsX = 5;
        int carpetsZ = 3;
        
        float startX = -width / 2f + 150f;
        float startZ = -depth / 2f + 80f;
        float spacingX = (width - 300f) / (carpetsX - 1);
        float spacingZ = (depth - 160f) / (carpetsZ - 1);
        
        int carpetIndex = 0;
        for (int x = 0; x < carpetsX; x++)
        {
            for (int z = 0; z < carpetsZ; z++)
            {
                GameObject carpet = GameObject.CreatePrimitive(PrimitiveType.Plane);
                carpet.name = "Carpet_" + carpetIndex++;
                carpet.transform.SetParent(floorContainer.transform);
                carpet.transform.localPosition = new Vector3(
                    startX + x * spacingX,
                    floorY + 0.5f,
                    startZ + z * spacingZ
                );
                carpet.transform.localScale = new Vector3(carpetWidth / 10f, 1, carpetDepth / 10f);
                
                if (carpetMat != null)
                {
                    carpet.GetComponent<Renderer>().material = carpetMat;
                }
            }
        }
    }
    
    static void CreateCeiling(Transform parent, float width, float depth, float ceilingY, Material ceilingMat, Material woodMat)
    {
        GameObject ceilingContainer = new GameObject("Ceiling");
        ceilingContainer.transform.SetParent(parent);
        ceilingContainer.transform.localPosition = Vector3.zero;
        
        // Main ceiling plane
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceiling.name = "CeilingPlane";
        ceiling.transform.SetParent(ceilingContainer.transform);
        ceiling.transform.localPosition = new Vector3(0, ceilingY, 0);
        ceiling.transform.localRotation = Quaternion.Euler(180, 0, 0);
        ceiling.transform.localScale = new Vector3(width / 10f, 1, depth / 10f);
        
        if (ceilingMat != null)
        {
            ceiling.GetComponent<Renderer>().material = ceilingMat;
        }
        
        // Create wooden beams - Longitudinal (along X)
        int numLongBeams = 7;
        float beamSpacingZ = depth / (numLongBeams + 1);
        
        for (int i = 1; i <= numLongBeams; i++)
        {
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.name = "Beam_Long_" + i;
            beam.transform.SetParent(ceilingContainer.transform);
            beam.transform.localPosition = new Vector3(0, ceilingY - 8f, -depth / 2f + i * beamSpacingZ);
            beam.transform.localScale = new Vector3(width - 50f, 15f, 12f);
            
            if (woodMat != null)
            {
                beam.GetComponent<Renderer>().material = woodMat;
            }
        }
        
        // Cross beams (along Z)
        int numCrossBeams = 11;
        float beamSpacingX = width / (numCrossBeams + 1);
        
        for (int i = 1; i <= numCrossBeams; i++)
        {
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.name = "Beam_Cross_" + i;
            beam.transform.SetParent(ceilingContainer.transform);
            beam.transform.localPosition = new Vector3(-width / 2f + i * beamSpacingX, ceilingY - 8f, 0);
            beam.transform.localScale = new Vector3(10f, 12f, depth - 50f);
            
            if (woodMat != null)
            {
                beam.GetComponent<Renderer>().material = woodMat;
            }
        }
    }
    
    static void CreateWalls(Transform parent, float width, float height, float depth, float floorY, Material iznikMat, Material kalemMat)
    {
        GameObject wallsContainer = new GameObject("Walls");
        wallsContainer.transform.SetParent(parent);
        wallsContainer.transform.localPosition = Vector3.zero;
        
        float tileHeight = height * 0.35f; // Bottom 35% is Iznik tiles
        float kalemHeight = height * 0.65f; // Top 65% is Kalem isi
        
        // Wall data: position, rotation, width
        System.Collections.Generic.List<(Vector3 pos, float rotY, float wallWidth, string name)> wallData = 
            new System.Collections.Generic.List<(Vector3, float, float, string)>
        {
            (new Vector3(0, 0, -depth / 2f + 5f), 0, width, "BackWall"),
            (new Vector3(0, 0, depth / 2f - 5f), 180, width, "FrontWall"),
            (new Vector3(-width / 2f + 5f, 0, 0), 90, depth, "LeftWall"),
            (new Vector3(width / 2f - 5f, 0, 0), -90, depth, "RightWall")
        };
        
        foreach (var wall in wallData)
        {
            GameObject wallContainer = new GameObject(wall.name);
            wallContainer.transform.SetParent(wallsContainer.transform);
            wallContainer.transform.localPosition = wall.pos;
            wallContainer.transform.localRotation = Quaternion.Euler(0, wall.rotY, 0);
            
            // Bottom part - Iznik tiles
            GameObject bottomWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bottomWall.name = "IznikTiles";
            bottomWall.transform.SetParent(wallContainer.transform);
            bottomWall.transform.localPosition = new Vector3(0, floorY + tileHeight / 2f, 0);
            bottomWall.transform.localScale = new Vector3(wall.wallWidth, tileHeight, 5f);
            
            if (iznikMat != null)
            {
                bottomWall.GetComponent<Renderer>().material = iznikMat;
            }
            
            // Top part - Kalem isi
            GameObject topWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topWall.name = "KalemIsi";
            topWall.transform.SetParent(wallContainer.transform);
            topWall.transform.localPosition = new Vector3(0, floorY + tileHeight + kalemHeight / 2f, 0);
            topWall.transform.localScale = new Vector3(wall.wallWidth, kalemHeight, 5f);
            
            if (kalemMat != null)
            {
                topWall.GetComponent<Renderer>().material = kalemMat;
            }
        }
    }
    
    static void CreateSedirs(Transform parent, float width, float depth, float floorY, Material woodMat, Material fabricMat)
    {
        GameObject sedirsContainer = new GameObject("Sedirs");
        sedirsContainer.transform.SetParent(parent);
        sedirsContainer.transform.localPosition = Vector3.zero;
        
        float sedirLength = 200f;
        float sedirWidth = 60f;
        float sedirHeight = 25f;
        
        // Sedirs along left wall
        float[] leftPositionsZ = { -120f, 0f, 120f };
        foreach (float z in leftPositionsZ)
        {
            CreateSingleSedir(sedirsContainer.transform, 
                new Vector3(-width / 2f + 80f, floorY, z), 
                90f, sedirLength, sedirWidth, sedirHeight, woodMat, fabricMat);
        }
        
        // Sedirs along right wall
        foreach (float z in leftPositionsZ)
        {
            CreateSingleSedir(sedirsContainer.transform, 
                new Vector3(width / 2f - 80f, floorY, z), 
                -90f, sedirLength, sedirWidth, sedirHeight, woodMat, fabricMat);
        }
        
        // Sedirs along back wall
        float[] backPositionsX = { -400f, -200f, 200f, 400f };
        foreach (float x in backPositionsX)
        {
            CreateSingleSedir(sedirsContainer.transform, 
                new Vector3(x, floorY, -depth / 2f + 80f), 
                0f, sedirLength, sedirWidth, sedirHeight, woodMat, fabricMat);
        }
    }
    
    static void CreateSingleSedir(Transform parent, Vector3 position, float rotationY, 
        float length, float width, float height, Material woodMat, Material fabricMat)
    {
        GameObject sedir = new GameObject("Sedir");
        sedir.transform.SetParent(parent);
        sedir.transform.localPosition = position;
        sedir.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        
        // Wooden base
        GameObject sedirBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sedirBase.name = "Base";
        sedirBase.transform.SetParent(sedir.transform);
        sedirBase.transform.localPosition = new Vector3(0, height / 2f, 0);
        sedirBase.transform.localScale = new Vector3(length, height, width);
        
        if (woodMat != null)
        {
            sedirBase.GetComponent<Renderer>().material = woodMat;
        }
        
        // Cushion
        GameObject cushion = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cushion.name = "Cushion";
        cushion.transform.SetParent(sedir.transform);
        cushion.transform.localPosition = new Vector3(0, height + 8f, 0);
        cushion.transform.localScale = new Vector3(length - 10f, 15f, width - 10f);
        
        if (fabricMat != null)
        {
            cushion.GetComponent<Renderer>().material = fabricMat;
        }
        
        // Back cushion
        GameObject backCushion = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backCushion.name = "BackCushion";
        backCushion.transform.SetParent(sedir.transform);
        backCushion.transform.localPosition = new Vector3(0, height + 30f, -width / 2f + 10f);
        backCushion.transform.localScale = new Vector3(length - 10f, 40f, 15f);
        
        if (fabricMat != null)
        {
            backCushion.GetComponent<Renderer>().material = fabricMat;
        }
    }
    
    static void CreateSehpas(Transform parent, float width, float depth, float floorY, Material woodMat)
    {
        GameObject sehpasContainer = new GameObject("Sehpas");
        sehpasContainer.transform.SetParent(parent);
        sehpasContainer.transform.localPosition = Vector3.zero;
        
        // Sehpa positions (near sedirs)
        Vector3[] sehpaPositions = {
            new Vector3(-width / 2f + 180f, floorY, -120f),
            new Vector3(-width / 2f + 180f, floorY, 0f),
            new Vector3(-width / 2f + 180f, floorY, 120f),
            new Vector3(width / 2f - 180f, floorY, -120f),
            new Vector3(width / 2f - 180f, floorY, 0f),
            new Vector3(width / 2f - 180f, floorY, 120f),
        };
        
        int sehpaIndex = 0;
        foreach (Vector3 pos in sehpaPositions)
        {
            CreateSingleSehpa(sehpasContainer.transform, pos, sehpaIndex++, woodMat);
        }
    }
    
    static void CreateSingleSehpa(Transform parent, Vector3 position, int index, Material woodMat)
    {
        GameObject sehpa = new GameObject("Sehpa_" + index);
        sehpa.transform.SetParent(parent);
        sehpa.transform.localPosition = position;
        
        float tableHeight = 30f;
        float tableRadius = 40f;
        
        // Table top (octagonal approximated with cylinder)
        GameObject top = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        top.name = "Top";
        top.transform.SetParent(sehpa.transform);
        top.transform.localPosition = new Vector3(0, tableHeight, 0);
        top.transform.localScale = new Vector3(tableRadius * 2f, 3f, tableRadius * 2f);
        
        if (woodMat != null)
        {
            top.GetComponent<Renderer>().material = woodMat;
        }
        
        // Center pedestal
        GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pedestal.name = "Pedestal";
        pedestal.transform.SetParent(sehpa.transform);
        pedestal.transform.localPosition = new Vector3(0, tableHeight / 2f, 0);
        pedestal.transform.localScale = new Vector3(20f, tableHeight / 2f, 20f);
        
        if (woodMat != null)
        {
            pedestal.GetComponent<Renderer>().material = woodMat;
        }
        
        // Legs
        for (int j = 0; j < 4; j++)
        {
            float angle = j * 90f * Mathf.Deg2Rad;
            float legX = Mathf.Cos(angle) * 25f;
            float legZ = Mathf.Sin(angle) * 25f;
            
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leg.name = "Leg_" + j;
            leg.transform.SetParent(sehpa.transform);
            leg.transform.localPosition = new Vector3(legX, tableHeight / 2f - 5f, legZ);
            leg.transform.localScale = new Vector3(8f, tableHeight - 10f, 8f);
            
            if (woodMat != null)
            {
                leg.GetComponent<Renderer>().material = woodMat;
            }
        }
    }
    
    static void CreateLanterns(Transform parent, float width, float depth, float ceilingY, Material brassMat)
    {
        GameObject lanternsContainer = new GameObject("Lanterns");
        lanternsContainer.transform.SetParent(parent);
        lanternsContainer.transform.localPosition = Vector3.zero;
        
        // Lantern grid positions
        float[] positionsX = { -500f, -250f, 0f, 250f, 500f };
        float[] positionsZ = { -120f, 0f, 120f };
        
        int lanternIndex = 0;
        foreach (float x in positionsX)
        {
            foreach (float z in positionsZ)
            {
                float hangHeight = (x == 0f && z == 0f) ? ceilingY - 80f : ceilingY - 50f;
                CreateSingleLantern(lanternsContainer.transform, 
                    new Vector3(x, hangHeight, z), 
                    lanternIndex++, brassMat, x == 0f && z == 0f);
            }
        }
    }
    
    static void CreateSingleLantern(Transform parent, Vector3 position, int index, Material brassMat, bool isCenterLantern)
    {
        GameObject lantern = new GameObject("Lantern_" + index);
        lantern.transform.SetParent(parent);
        lantern.transform.localPosition = position;
        
        float scale = isCenterLantern ? 1.5f : 1f;
        
        // Chain
        GameObject chain = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        chain.name = "Chain";
        chain.transform.SetParent(lantern.transform);
        chain.transform.localPosition = new Vector3(0, 25f * scale, 0);
        chain.transform.localScale = new Vector3(2f, 25f * scale, 2f);
        
        if (brassMat != null)
        {
            chain.GetComponent<Renderer>().material = brassMat;
        }
        
        // Lantern body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body.name = "Body";
        body.transform.SetParent(lantern.transform);
        body.transform.localPosition = new Vector3(0, 0, 0);
        body.transform.localScale = new Vector3(20f * scale, 15f * scale, 20f * scale);
        
        if (brassMat != null)
        {
            body.GetComponent<Renderer>().material = brassMat;
        }
        
        // Top cap (dome)
        GameObject topCap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        topCap.name = "TopCap";
        topCap.transform.SetParent(lantern.transform);
        topCap.transform.localPosition = new Vector3(0, 18f * scale, 0);
        topCap.transform.localScale = new Vector3(25f * scale, 12f * scale, 25f * scale);
        
        if (brassMat != null)
        {
            topCap.GetComponent<Renderer>().material = brassMat;
        }
        
        // Bottom finial
        GameObject bottomCap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bottomCap.name = "BottomFinial";
        bottomCap.transform.SetParent(lantern.transform);
        bottomCap.transform.localPosition = new Vector3(0, -18f * scale, 0);
        bottomCap.transform.localScale = new Vector3(12f * scale, 8f * scale, 12f * scale);
        
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
        pointLight.color = new Color(1f, 0.85f, 0.5f); // Warm yellow/orange
        pointLight.intensity = isCenterLantern ? 8f : 5f;
        pointLight.range = isCenterLantern ? 300f : 200f;
        pointLight.shadows = LightShadows.Soft;
    }
    
    static void CreateVitrines(Transform parent, float width, float depth, float floorY, Material woodMat, Material glassMat)
    {
        GameObject vitrinesContainer = new GameObject("Vitrines");
        vitrinesContainer.transform.SetParent(parent);
        vitrinesContainer.transform.localPosition = Vector3.zero;
        
        // Vitrine positions (center area of the room)
        Vector3[] vitrinePositions = {
            new Vector3(-350f, floorY, 0f),
            new Vector3(-175f, floorY, 0f),
            new Vector3(0f, floorY, 0f),
            new Vector3(175f, floorY, 0f),
            new Vector3(350f, floorY, 0f),
            new Vector3(-260f, floorY, -100f),
            new Vector3(0f, floorY, -100f),
            new Vector3(260f, floorY, -100f),
            new Vector3(-260f, floorY, 100f),
            new Vector3(0f, floorY, 100f),
            new Vector3(260f, floorY, 100f),
        };
        
        int vitrineIndex = 0;
        foreach (Vector3 pos in vitrinePositions)
        {
            CreateSingleVitrine(vitrinesContainer.transform, pos, vitrineIndex++, woodMat, glassMat);
        }
    }
    
    static void CreateSingleVitrine(Transform parent, Vector3 position, int index, Material woodMat, Material glassMat)
    {
        GameObject vitrine = new GameObject("Vitrine_" + index);
        vitrine.transform.SetParent(parent);
        vitrine.transform.localPosition = position;
        
        float pedestalHeight = 60f;
        float pedestalWidth = 80f;
        float glassHeight = 50f;
        
        // Wooden pedestal
        GameObject pedestal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pedestal.name = "Pedestal";
        pedestal.transform.SetParent(vitrine.transform);
        pedestal.transform.localPosition = new Vector3(0, pedestalHeight / 2f, 0);
        pedestal.transform.localScale = new Vector3(pedestalWidth, pedestalHeight, pedestalWidth * 0.7f);
        
        if (woodMat != null)
        {
            pedestal.GetComponent<Renderer>().material = woodMat;
        }
        
        // Glass case
        GameObject glassCase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        glassCase.name = "GlassCase";
        glassCase.transform.SetParent(vitrine.transform);
        glassCase.transform.localPosition = new Vector3(0, pedestalHeight + glassHeight / 2f + 2f, 0);
        glassCase.transform.localScale = new Vector3(pedestalWidth - 10f, glassHeight, pedestalWidth * 0.6f);
        
        if (glassMat != null)
        {
            glassCase.GetComponent<Renderer>().material = glassMat;
        }
        
        // Wooden top frame
        GameObject topFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topFrame.name = "TopFrame";
        topFrame.transform.SetParent(vitrine.transform);
        topFrame.transform.localPosition = new Vector3(0, pedestalHeight + glassHeight + 7f, 0);
        topFrame.transform.localScale = new Vector3(pedestalWidth - 5f, 8f, pedestalWidth * 0.65f);
        
        if (woodMat != null)
        {
            topFrame.GetComponent<Renderer>().material = woodMat;
        }
    }
}
