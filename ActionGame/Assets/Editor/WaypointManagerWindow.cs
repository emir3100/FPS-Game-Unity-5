//Made by Daniel Abdulahad
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform wayPointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("wayPointRoot"));

        if (wayPointRoot == null)
        {
            EditorGUILayout.HelpBox("Root tranform must be selected. bla bla bla", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            if (GUILayout.Button("Add Branch Waypoint"))
            {
                CreateBranch();
            }
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }
            if (GUILayout.Button("Create Waypoint After"))
            {
                CreateWaypointAfter();
            }
            if (GUILayout.Button("Remove Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }

    private void CreateBranch()
    {
        GameObject waypointObj = new GameObject("Waypoint " + wayPointRoot.childCount, typeof(WayPoint));
        waypointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wayPoint = waypointObj.GetComponent<WayPoint>();
        WayPoint branchedFrom = Selection.activeGameObject.GetComponent<WayPoint>();
        branchedFrom.Branches.Add(wayPoint);

        wayPoint.PreviousWayPoint = branchedFrom;
        wayPoint.transform.position = branchedFrom.transform.position;
        wayPoint.transform.forward = branchedFrom.transform.forward;

        Selection.activeGameObject = wayPoint.gameObject;
    }

    private void CreateWaypoint()
    {
        GameObject waypointObj = new GameObject("Waypoint " + wayPointRoot.childCount, typeof(WayPoint));
        waypointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wayPoint = waypointObj.GetComponent<WayPoint>();
        if (wayPointRoot.childCount > 1)
        {
            wayPoint.PreviousWayPoint = wayPointRoot.GetChild(wayPointRoot.childCount - 2).GetComponent<WayPoint>();
            wayPoint.PreviousWayPoint.NextWayPoint = wayPoint;

            wayPoint.transform.position = wayPoint.PreviousWayPoint.transform.position;
            wayPoint.transform.forward = wayPoint.PreviousWayPoint.transform.forward;
        }

        Selection.activeGameObject = wayPoint.gameObject;
    }

    private void CreateWaypointBefore()
    {
        GameObject waypointObj = new GameObject("Waypoint " + wayPointRoot.childCount, typeof(WayPoint));
        waypointObj.transform.SetParent(wayPointRoot, false);

        WayPoint newWaypoint = waypointObj.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();
        waypointObj.transform.position = selectedWaypoint.transform.position;
        waypointObj.transform.forward = selectedWaypoint.transform.forward;

        if (selectedWaypoint.PreviousWayPoint != null)
        {
            newWaypoint.PreviousWayPoint = selectedWaypoint.PreviousWayPoint;
            selectedWaypoint.PreviousWayPoint.NextWayPoint = newWaypoint;
        }

        newWaypoint.NextWayPoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    private void CreateWaypointAfter()
    {
        GameObject waypointObj = new GameObject("Waypoint " + wayPointRoot.childCount, typeof(WayPoint));
        waypointObj.transform.SetParent(wayPointRoot, false);

        WayPoint newWaypoint = waypointObj.GetComponent<WayPoint>();

        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();
        waypointObj.transform.position = selectedWaypoint.transform.position;
        waypointObj.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.PreviousWayPoint = selectedWaypoint;

        if (selectedWaypoint.NextWayPoint != null)
        {
            selectedWaypoint.NextWayPoint.PreviousWayPoint = newWaypoint;
            newWaypoint.NextWayPoint = selectedWaypoint.NextWayPoint;
        }

        selectedWaypoint.NextWayPoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    private void RemoveWaypoint()
    {
        WayPoint selectedWaypoint = Selection.activeGameObject.GetComponent<WayPoint>();

        if (selectedWaypoint.NextWayPoint != null)
        {
            selectedWaypoint.NextWayPoint.PreviousWayPoint = selectedWaypoint.PreviousWayPoint;
        }
        if (selectedWaypoint.PreviousWayPoint != null)
        {
            selectedWaypoint.PreviousWayPoint.NextWayPoint = selectedWaypoint.NextWayPoint;
            Selection.activeGameObject = selectedWaypoint.PreviousWayPoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }
}
