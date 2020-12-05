using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.pooling;

[RequireComponent(typeof(LineRenderer))]
public class Bezier : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform middle1;
    public Transform middle2;
    public int SEGMENT_COUNT = 10;

    private LineRenderer line_renderer;

    private Pooled_object pooled_object;

    void Awake() {
        line_renderer = GetComponent<LineRenderer>();
        pooled_object = GetComponent<Pooled_object>();
    }


    public Bezier get_between_points(
        Transform start,
        Transform end,
        Vector3 offset_from_start,
        Vector3 offset_from_end    
    ) {
        Bezier bezier = pooled_object.get_from_pool<Bezier>();
        bezier.init_between_points(
             start,
             end,
             offset_from_start,
             offset_from_end
        );
        return bezier;
    }
    
    public void init_between_points(
        Transform start,
        Transform end,
        Vector3 offset_from_start,
        Vector3 offset_from_end
    ) {
        this.start = start;
        this.end = end;
        this.middle1.transform.position = 
            start.position + offset_from_start;
        this.middle2.transform.position = 
            end.position + offset_from_end;
        
        update_curve();
    }

    void Update()
    {
        //update_curve();
    }
    
    public void update_curve()
    {
        line_renderer.positionCount = SEGMENT_COUNT;
        for (int i = 0; i < SEGMENT_COUNT; i++)
        {
            float t = i / (float)(SEGMENT_COUNT-1);
            Vector3 point = CalculateCubicBezierPoint(t);
            
            line_renderer.SetPosition(i, point);
        }
    }
        
    Vector3 CalculateCubicBezierPoint(
        float t
    )
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        
        Vector3 p = uuu * start.position; 
        p += 3 * uu * t * middle1.position; 
        p += 3 * u * tt * middle2.position; 
        p += ttt * end.position; 
        
        return p;
    }
}