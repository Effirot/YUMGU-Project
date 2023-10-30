
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Splines;

public class Rope : MonoBehaviour
{
    public Transform player;

    public SplineContainer rope;
    public LayerMask collMask;

    public List<Vector3> ropePositions { get; set; } = new List<Vector3>();

    private void Awake() => AddPosToRope(Vector3.zero);

    

    private void Update()
    {
        UpdateRopePositions();
        LastSegmentGoToPlayerPos();

        DetectCollisionEnter();
        if (ropePositions.Count > 2) DetectCollisionExits();

        
    }

    private void DetectCollisionEnter()
    {
        
        RaycastHit hit;
        var knotPosition = rope.Splines.First().Knots.ToArray()[ropePositions.Count - 2].Position;
        
        if (Physics.Linecast(player.position, knotPosition, out hit, collMask))
        {
            ropePositions.RemoveAt(ropePositions.Count - 1);
            AddPosToRope(hit.point);
        }
    }

    private void DetectCollisionExits()
    {
        var knotPosition = rope.Splines.First().Knots.ToArray()[ropePositions.Count - 3].Position;

        RaycastHit hit;
        if (!Physics.Linecast(player.position, knotPosition, out hit, collMask))
        {
            ropePositions.RemoveAt(ropePositions.Count - 2);
        }
    }

    private void AddPosToRope(Vector3 _pos)
    {
        ropePositions.Add(_pos);
        ropePositions.Add(player.position); //Always the last pos must be the player
    }

    private void UpdateRopePositions()
    {
        var spline = rope.Splines.First();
        spline.Clear();

        foreach (var pos in ropePositions)
        {
            spline.Add(new BezierKnot(pos));
        }

        // rope.positionCount = ropePositions.Count;
        // rope.SetPositions(ropePositions.ToArray());
    }

    private void LastSegmentGoToPlayerPos() 
    { 
        var spline = rope.Splines.First();

        var bezier = spline[spline.Count() - 1];
        bezier.Position = player.position;
    }
}
