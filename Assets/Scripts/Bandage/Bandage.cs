using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering;

using SplineMesh;
using System.Collections;

[RequireComponent(typeof(Spline))]
public class Bandage : MonoBehaviour
{
    [SerializeField]
    private Transform attachedPoint;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float normalsAdditive;

    private List<BandagePoint> points = new();
    private Spline spline;

    public float totalDistance 
    {
        get
        {
            var positions = AllPositions;

            float distance = 0;

            for(int i = 0; i < positions.Count() - 1; i++)
            {
                distance += Vector3.Distance(positions[i], positions[i + 1]);
            }

            return distance;
        }
    }

    private Vector3[] AllPositions 
    {
        get 
        { 
            var result = new List<Vector3>();

            result.Add(transform.position);
            result.AddRange(points.Select(a=>a.transform.position));
            result.Add(attachedPoint.position);

            return result.ToArray();
        } 
    }

    private void Awake()
    {
        spline = GetComponent<Spline>();
    }
    private void LateUpdate()
    {
        RaysChackers();
        LastPointRayChecker();
    }
    private void OnDrawGizmos()
    {
        var list = AllPositions;

        for(int i = 0; i < list.Length - 1; i++)
        {
            Gizmos.DrawLine(list[i], list[i + 1]);
        }
    }

    private void LastPointRayChecker()
    {
        var list = AllPositions;
        if(points.Any())
        {
            if (!Physics.Linecast(list[list.Count() - 1], list[list.Count() - 2], out var hit, layerMask, QueryTriggerInteraction.Ignore)) 
            {
                Destroy(points[points.Count() - 1].gameObject);
                points.RemoveAt(points.Count() - 1);

                while(spline.nodes.Count > list.Count() - 1)
                {
                    spline.nodes.RemoveAt(spline.nodes.Count() - 1);
                    spline.RefreshCurves(); 
                }
            }
        }
    }

    private void RaysChackers()
    {
        if (Physics.Linecast(AllPositions[AllPositions.Count() - 2], AllPositions[AllPositions.Count() - 1], out var hit, layerMask, QueryTriggerInteraction.Ignore)) 
        {
            var pointObject = new GameObject("BandagePoint");
            
            pointObject.transform.SetParent(transform);
            pointObject.transform.position = hit.point + hit.normal * normalsAdditive;

            points.Add(pointObject.AddComponent<BandagePoint>());

            var localPose = transform.InverseTransformPoint(pointObject.transform.position);
            var node = new SplineNode(localPose, localPose + (localPose - spline.nodes.Last().Position)  / 3);
            
            node.Roll = 0;
            node.Scale = Vector2.one;
            node.Up = hit.normal;
            spline.AddNode(node);
            spline.RefreshCurves(); 
        }
    }

    private void SetPositionToKnot(int index, Vector3 globalPosition)
    {
        var node = spline.nodes[index];
        node.Position = transform.InverseTransformPoint(globalPosition);

        // if(index - 1 >= 0)
        // {
        //     node.Direction = (spline.nodes[index - 1].Position) * 0.1f;
        // }

        spline.nodes[index] = node;
    }

}
