
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class Bandage : MonoBehaviour
{
    [SerializeField]
    private Transform attachedPoint;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float normalsAdditive;

    private List<BandagePoint> points = new();
    private SplineContainer splineContainer;

    private Spline spline => splineContainer.Spline;


#warning IXRHoverFilter

    private Vector3[] AllPositions 
    {
        get { 
            var result = new List<Vector3>();

            result.Add(transform.position);

            result.AddRange(points.Select(a=>a.transform.position));

            result.Add(attachedPoint.position);

            return result.ToArray();
        } 
    }

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
    }
    private void LateUpdate()
    {
        RaysChackers();
        LastPointRayChecker();

        RefreshGraphics();
    }
    private void OnDrawGizmos()
    {
        var list = AllPositions;

        var lastPoint = list[0];

        for(int i = 1; i < list.Length - 1; i++)
        {
            Gizmos.DrawLine(lastPoint, list[i + 1]);
            lastPoint = list[i];
        }    
    }

    private void LastPointRayChecker()
    {
        var list = AllPositions;
        if(points.Any())
        {
            if (!Physics.Linecast(list[list.Count() - 1], list[list.Count() - 2], out var hit, layerMask)) 
            {
                Destroy(points[points.Count() - 1].gameObject);
                points.RemoveAt(points.Count() - 1);
            }
        }
    }

    private void RaysChackers()
    {
        Debug.DrawLine(AllPositions[AllPositions.Count() - 2], AllPositions[AllPositions.Count() - 1]);
        if (Physics.Linecast(AllPositions[AllPositions.Count() - 2], AllPositions[AllPositions.Count() - 1], out var hit, layerMask)) 
        {
            var pointObject = new GameObject("BandagePoint");
            
            pointObject.transform.SetParent(transform);
            pointObject.transform.position = hit.point + hit.normal * normalsAdditive;

            points.Add(pointObject.AddComponent<BandagePoint>());
        }
    }

    private void RefreshGraphics()
    {
    //     while(spline.Count < points.Count + 2)   
    //     {
    //         spline.Insert(0, new());
    //     }

    //     while(spline.Count > points.Count + 2)   
    //     {
    //         spline.RemoveAt(0);
    //     }

    //     SetPositionToKnot(0, Vector3.zero);
    //     SetPositionToKnot(spline.Count - 1, attachedPoint.transform.localPosition);

    //     for(int i = 1; i < spline.Count - 2; i++)
    //     {
    //         SetPositionToKnot(i, points[i].transform.position);
    //     } 
    }

    // private void SetPositionToKnot(int index, Vector3 position)
    // {
    //     var knot = spline[index];
    //     knot.Position = position;
    //     spline[index] = knot;
    // }



}
