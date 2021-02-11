using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint NextWayPoint;
    public WayPoint PreviousWayPoint;

    public List<WayPoint> Branches;
    [Range(0f, 1f)]
    public float Ratio = 0.5f;
    public float Width = 1f;

    public float Distance(Transform other)
    {
        return Vector3.Distance(transform.position, other.position);
    }

    public WayPoint GetRandomWaypoint()
    {
        return Branches[Random.Range(0, Branches.Count)];
    }

    public Vector3 GetPosition()
    {
        Vector3 minBounds = transform.position + transform.right * Width / 2f;
        Vector3 maxBounds = transform.position - transform.right * Width / 2f;

        return Vector3.Lerp(minBounds, maxBounds, Random.Range(0f, 1f));
    }
}