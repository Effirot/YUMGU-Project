
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class Bandage : MonoBehaviour
{
    [SerializeField]
    private Transform attachedPoint;

    private List<BandagePoint> points = new();
    private SplineContainer splineContainer;

    private Spline spline => splineContainer.Spline;

    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        points.Add(new GameObject().AddComponent<BandagePoint>());
        points.Add(new GameObject().AddComponent<BandagePoint>());
        points.Add(new GameObject().AddComponent<BandagePoint>());
        points.Add(new GameObject().AddComponent<BandagePoint>());
    }
    private void LateUpdate()
    {
        RefreshGraphics();
    }

    private void RefreshGraphics()
    {
        while(spline.Count < points.Count)   
        {
            spline.Insert(0, new());
        }

        while(spline.Count > points.Count)   
        {
            spline.RemoveAt(0);
        }

        for(int i = 0; i < points.Count; i++)
        {
            var knot = spline[i];
            knot.Position = points[i].transform.localPosition;
            knot.Rotation = points[i].transform.localRotation;
            spline[i] = knot;
        } 
    }


}
