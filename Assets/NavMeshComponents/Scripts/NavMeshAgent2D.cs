using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent2D : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0;

    [HideInInspector] // 常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;
    public Vector2 destination
    {
        get { return trace_area; }
        set
        {
            trace_area = value;
            Trace(transform.position, value);
        }
    }

    public Vector2 CurrentDirection { get; private set; } // 現在の移動方向を保持する変数

    public bool SetDestination(Vector2 target)
    {
        destination = target;
        return true;
    }

    private void Trace(Vector2 current, Vector2 target)
    {
        if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            CurrentDirection = Vector2.zero; // 移動が停止している場合は方向をゼロにする
            return;
        }

        // NavMeshに応じて経路を求める
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

        // corners配列が空でないか確認
        if (path.corners.Length == 0)
        {
            Debug.Log("経路が見つかりませんでした。");
            CurrentDirection = Vector2.zero;
            return;
        }

        Vector2 corner = path.corners[0];

        if (Vector2.Distance(current, corner) <= 0.05f)
        {
            if (path.corners.Length > 1)
            {
                corner = path.corners[1];
            }
            else
            {
                Debug.Log("次のコーナーが存在しません。");
                CurrentDirection = Vector2.zero;
                return;
            }
        }

        // 移動方向を計算
        CurrentDirection = (corner - current).normalized;

        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
    }
}


// using UnityEngine;
// using UnityEngine.AI;

// public class NavMeshAgent2D : MonoBehaviour
// {
//     [Header("Steering")]
//     public float speed = 1.0f;
//     public float stoppingDistance = 0;

//     [HideInInspector]//常にUnityエディタから非表示
//     private Vector2 trace_area=Vector2.zero;
//     public Vector2 destination
//     {
//         get { return trace_area; }
//         set
//         {
//             trace_area = value;
//             Trace(transform.position, value);
//         }
//     }
//     public bool SetDestination(Vector2 target)
//     {
//         destination = target;
//         return true;
//     }

//     private void Trace(Vector2 current,Vector2 target)
//     {
//         if (Vector2.Distance(current,target) <= stoppingDistance)
//         {
//             return;
//         }

//         // NavMesh に応じて経路を求める
//         NavMeshPath path = new NavMeshPath();
//         NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

//         // corners配列が空でないか確認
//         if (path.corners.Length == 0)
//         {
//             Debug.Log("経路が見つかりませんでした。");
//             return;
//         }


//         Vector2 corner = path.corners[0];

//         if (Vector2.Distance(current, corner) <= 0.05f)
//         {
//             if (path.corners.Length > 1)
//             {
//                 corner = path.corners[1];
//             }
//             else
//             {
//                 Debug.Log("次のコーナーが存在しません。");
//                 return;
//             }
//             // corner = path.corners[1];
//         }

//         transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
//     }
// }
