//Made by Daniel Abdulahad
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(WayPoint wayPoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(wayPoint.transform.position, 0.3f);
        Gizmos.DrawLine(wayPoint.transform.position + (wayPoint.transform.right * wayPoint.Width / 2f),
            wayPoint.transform.position - (wayPoint.transform.right * wayPoint.Width / 2f));

        if (wayPoint.PreviousWayPoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wayPoint.transform.right * wayPoint.Width / 2f;
            Vector3 offsetTo = wayPoint.PreviousWayPoint.transform.right * wayPoint.PreviousWayPoint.Width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.PreviousWayPoint.transform.position + offsetTo);
        }
        if (wayPoint.NextWayPoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = wayPoint.transform.right * -wayPoint.Width / 2f;
            Vector3 offsetTo = wayPoint.NextWayPoint.transform.right *- wayPoint.NextWayPoint.Width / 2f;

            Gizmos.DrawLine(wayPoint.transform.position + offset, wayPoint.NextWayPoint.transform.position + offsetTo);
        }

        if (wayPoint.Branches != null)
        {
            foreach (WayPoint branch in wayPoint.Branches)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wayPoint.transform.position, branch.transform.position);
            }
        }
    }
}
