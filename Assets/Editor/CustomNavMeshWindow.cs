using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomNavMeshWindow : EditorWindow
{
    static bool isEnabled = false;
    static GameObject graphObj;
    static CustomNavMesh graph;
    static CustomNavMeshWindow window;
    static GameObject graphVertex;


    [MenuItem("Window/CustomNavMeshWindow")]
    static void Init()
    {
        window = EditorWindow.GetWindow<CustomNavMeshWindow>();
        window.title = "CustomNavMeshEindow";
        SceneView.onSceneGUIDelegate += OnScene;
        graphObj = GameObject.Find("CustomNavMesh");
        if (graphObj == null)
        {
            graphObj = new GameObject("CustomNavMesh");
            graph = graphObj.AddComponent<CustomNavMesh>();
        }
        else
        {
            graph = graphObj.GetComponent<CustomNavMesh>();
        }

    }

    private static void OnScene(SceneView sceneView)
    {
        if (!isEnabled)
        {
            return;
        }
        if (Event.current.type == EventType.MouseDown)
        {
            graphVertex = graph.vertexPrefab;
            if (graphVertex == null)
            {
                Debug.LogError("Non vertex prefab");
                return;
            }
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            GameObject newV;


            if (Physics.Raycast(ray, out hit))
            {
                GameObject obj = hit.collider.gameObject;
                Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                Vector3 pos;
                for (int i  = 0; i < mesh.triangles.Length; i += 3)
                {
                    int i0 = mesh.triangles[i];
                    int i1 = mesh.triangles[i + 1];
                    int i2 = mesh.triangles[i + 2];

                    pos = mesh.vertices[i0];
                    pos += mesh.vertices[i1];
                    pos += mesh.vertices[i2];
                    pos /= 3;

                    newV = Instantiate(graphVertex, pos, Quaternion.identity) as GameObject;
                    newV.transform.Translate(obj.transform.position);
                    newV.transform.parent = graphObj.transform;
                }
            }
        }
    }

    private void OnGUI()
    {
        isEnabled = EditorGUILayout.Toggle("Enable Mesh Picking", isEnabled);

        if (GUILayout.Button("Build Edges"))
        {
            if (graph != null)
            {
                graph.Load();
            }

        }
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
    }

}
