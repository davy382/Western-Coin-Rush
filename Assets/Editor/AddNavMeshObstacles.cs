using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class AddNavMeshObstacles : EditorWindow
{
    [MenuItem("Tools/Add NavMeshObstacle to Selection")]
    public static void AddObstaclesToSelection()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<NavMeshObstacle>() == null)
            {
                var obstacle = obj.AddComponent<NavMeshObstacle>();
                obstacle.carving = true;
            }
        }

        Debug.Log("✅ NavMeshObstacle with Carving added to selected GameObjects.");
    }
}