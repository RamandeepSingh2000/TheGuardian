using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RewardRoomGenerator : MonoBehaviour
{
    [SerializeField]
    private float size = 80;
    [SerializeField]
    private float pillarAreaSize = 60;
    [SerializeField]
    private GameObject pillarPrefab;
    [SerializeField]
    private float pillarRadius;
    [SerializeField]
    private Vector2Int pillarCountRange = new Vector2Int(5, 10);
    [SerializeField]
    private List<DecorInfo> decors;

    [SerializeField]
    private Flock flock;

    private Mesh pillarAreaGizmosMesh;
    private Mesh gapAreaGizmosMesh;
    private Mesh decorAreaGizmosMesh;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRewardRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnValidate()
    {
        if(pillarAreaGizmosMesh == null)
        {
            pillarAreaGizmosMesh = new Mesh();
        }
        if (gapAreaGizmosMesh == null)
        {
            gapAreaGizmosMesh = new Mesh();
        }
        if (decorAreaGizmosMesh == null)
        {
            decorAreaGizmosMesh = new Mesh();
        }
        UpdateAreaMesh(decorAreaGizmosMesh, size, 0.01f, transform.position);
        UpdateAreaMesh(gapAreaGizmosMesh, pillarAreaSize + 10f, 0.02f, transform.position);
        UpdateAreaMesh(pillarAreaGizmosMesh, pillarAreaSize, 0.03f, transform.position);

    }

    private void OnDrawGizmosSelected()
    {
        //Draw decor area        
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(decorAreaGizmosMesh);

        //Draw gap area        
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(gapAreaGizmosMesh);

        //Draw pillar area        
        Gizmos.color = Color.white;
        Gizmos.DrawMesh(pillarAreaGizmosMesh);
    }

    void UpdateAreaMesh(Mesh mesh, float size, float verticalOffset, Vector3 origin)
    {
        var vertices = new Vector3[4];
        vertices[0] = origin + new Vector3(-size / 2, verticalOffset, -size / 2);
        vertices[1] = origin + new Vector3(-size / 2, verticalOffset, size / 2);
        vertices[2] = origin + new Vector3(size / 2, verticalOffset, size / 2);
        vertices[3] = origin + new Vector3(size / 2, verticalOffset, -size / 2);
        var triangles = new int[]
        {
            0, 1, 2, 0, 2, 3
        };
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    
    void GenerateRewardRoom()
    {
        GeneratePillars();
        GenerateDecor();
    }
    void GeneratePillars()
    {
        if (pillarPrefab == null)
        {
            return;
        }
        var pillarCount = Random.Range(pillarCountRange.x, pillarCountRange.y);
        var pillarPoints = new Vector3[pillarCount];
        for(int i = 0; i < pillarCount; i++)
        {
            bool spawnPointValid = false;
            int safetyCheck = 0;
            while (!spawnPointValid && safetyCheck < 50)
            {
                safetyCheck++;
                var point = GetRandomPillarPoint();
                bool isOverlapping = false;
                foreach (var p in pillarPoints)
                {
                    if (Vector3.Distance(p, point) <= pillarRadius * 2)
                    {
                        isOverlapping = true;
                        break;
                    }
                }
                if (isOverlapping)
                {
                    continue;
                }
                pillarPoints[i] = point;
                spawnPointValid = true;
            }
        }  
        foreach(var p in pillarPoints)
        {
            Instantiate(pillarPrefab, p, Quaternion.identity);
        }
        if(flock != null)
        {
            flock.InjectPillarLocations(pillarPoints.ToList());
            flock.SpawnBoids();
        }
    }
    void GenerateDecor()
    {
        float spawnAreaWidth = (size - 10 - pillarAreaSize) / 2f;
        var spawnAreas = new DecorSpawnArea[]
        {
            //top area
            new DecorSpawnArea()
            {
                Centre = transform.position + new Vector3(0, 0, size / 2 - spawnAreaWidth / 2),
                Size = new Vector2(size, spawnAreaWidth)
            },
            //bottom area
            new DecorSpawnArea()
            {
                Centre = transform.position + new Vector3(0, 0, - size / 2 + spawnAreaWidth / 2),
                Size = new Vector2(size, spawnAreaWidth)
            },
            //left area
            new DecorSpawnArea()
            {
                Centre = transform.position + new Vector3(-size / 2 + spawnAreaWidth / 2, 0, 0),
                Size = new Vector2(spawnAreaWidth, size - spawnAreaWidth * 2)
            },
            //right area
            new DecorSpawnArea()
            {
                Centre = transform.position + new Vector3(size / 2 - spawnAreaWidth / 2, 0, 0),
                Size = new Vector2(spawnAreaWidth, size - spawnAreaWidth * 2)
            }
        };
        Dictionary<Bounds, DecorInfo> decorsToBePlaced = new();
        var playerSpawnAreaBounds = new Bounds(transform.position + new Vector3(0, 0, -size / 2 + spawnAreaWidth / 2), new Vector3(5f, 1, spawnAreaWidth));
        foreach (var decor in decors)
        {          
            int spawnCount = Random.Range(decor.DecorCountRange.x, decor.DecorCountRange.y);
            for(int i = 0; i < spawnCount; i++)
            {
                var spawnArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
                bool spawnPointValid = false;
                int safetyCheck = 0;
                while (!spawnPointValid && safetyCheck < 50)
                {
                    safetyCheck++;
                    var point = GetRandomDecorPoint(decor, spawnArea);
                    var bounds = new Bounds(point, new Vector3(decor.Size.x, 1f, decor.Size.y));
                    bool isOverlapping = false;
                    foreach (var decorToBePlaced in decorsToBePlaced)
                    {
                        if (decorToBePlaced.Key.Intersects(bounds))
                        {
                            isOverlapping = true;
                            break;
                        }
                    }
                    if (playerSpawnAreaBounds.Intersects(bounds))
                    {
                        isOverlapping = true;
                    }
                    if (isOverlapping)
                    {
                        continue;
                    }
                    decorsToBePlaced.Add(bounds, decor);
                    spawnPointValid = true;
                }                
            }
        }
        foreach(var decor in decorsToBePlaced)
        {
            Instantiate(decor.Value.DecorPrefab, decor.Key.center, Quaternion.identity);
        }
    }
    Vector3 GetRandomDecorPoint(DecorInfo decorInfo, DecorSpawnArea decorSpawnArea)
    {
        var rangeX = decorSpawnArea.Size.x - decorInfo.Size.x;
        var rangeY = decorSpawnArea.Size.y - decorInfo.Size.y;
        return decorSpawnArea.Centre + new Vector3(Random.Range(-rangeX/2, rangeX/2), 0, Random.Range(-rangeY/2, rangeY/2));
    }
    Vector3 GetRandomPillarPoint()
    {
        var pillarSpawnArea = pillarAreaSize - 2 * pillarRadius;
        var x = Random.Range(-pillarSpawnArea / 2, pillarSpawnArea / 2);
        var z = Random.Range(-pillarSpawnArea / 2, pillarSpawnArea / 2);
        return transform.position + new Vector3(x, 0, z);
    }
    private struct DecorSpawnArea
    {
        public Vector3 Centre { get; set; }
        public Vector2 Size { get; set; } 
    }
}

[System.Serializable]
public struct DecorInfo
{
    [SerializeField]
    private GameObject decorPrefab;
    [SerializeField]
    private Vector2 size;
    [SerializeField]
    private Vector2Int decorCountRange;

    public readonly GameObject DecorPrefab => decorPrefab;
    public readonly Vector2 Size => size;
    public readonly Vector2Int DecorCountRange => decorCountRange;
}
