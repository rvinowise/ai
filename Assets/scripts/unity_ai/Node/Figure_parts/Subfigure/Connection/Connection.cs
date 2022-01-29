
using System;
using System.Collections.Generic;
using rvinowise.ai.general;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using rvinowise.unity.geometry2d;
using TMPro;
using UnityEngine;

namespace rvinowise.ai.unity {

public class Connection:
MonoBehaviour
{

    #region visualisation

    public ICircle source;
    public ICircle destination;

    public Transform tail;
    
    public Transform head;
    public Transform line_end;

    private LineRenderer line_renderer;
    public SpriteRenderer tail_sprite;
    public SpriteRenderer head_sprite;

    void Awake() {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.positionCount = segment_n;
    }
    
    [called_by_prefab]
    public Connection create(
        ICircle source,
        ICircle destination
    ) {
        Connection connection = this.provide_new<Connection>();

        connection.source = source;
        connection.destination = destination;
        connection.update();
        
        return connection;
    }

    private const int segment_n = 2;

    void Update() {
        update();
    }
    Vector3 shift_to_background = new Vector3(0,0,0.5f);
    public void update() {
        Vector3 tail_attachment = source.transform.position.offset_in_direction(
            source.radius,
            source.transform.position.degrees_to(destination.transform.position)
        );//+shift_to_background;
        Vector3 head_attachment = destination.transform.position.offset_in_direction(
            source.radius,
            destination.transform.position.degrees_to(source.transform.position)
        );//+shift_to_background;
            
        tail.position = tail_attachment;
        head.position = head_attachment;
        tail.direct_to(destination.transform.position);
        head.direct_to(destination.transform.position);

        line_renderer.SetPosition(0, tail_attachment);
        line_renderer.SetPosition(1, line_end.position);
    }
    
    public void set_color(Color color) {
        line_renderer.material.color = color;

    }

    #endregion
}
}