
using System;
using System.Collections.Generic;
using abstract_ai;
using rvinowise.unity.ai.visuals;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using rvinowise.unity.geometry2d;
using TMPro;
using UnityEngine;

namespace rvinowise.unity.ai.figure {

public class Connection:
MonoBehaviour
{

    #region visualisation

    public ICircle source;
    public ICircle destination;

    public SpriteRenderer tail;
    public SpriteRenderer head;

    private LineRenderer line_renderer;

    void Awake() {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.positionCount = segment_n;
    }
    
    [called_by_prefab]
    public Connection create(
        ICircle source,
        ICircle destination
    ) {
        Connection connection = this.get_from_pool<Connection>();

        connection.source = source;
        connection.destination = destination;
        connection.update();
        
        return connection;
    }

    private const int segment_n = 2;

    void Update() {
        update();
    }
    public void update() {
        Vector3 tail_attachment = source.transform.position.offset_in_direction(
            source.radius,
            source.transform.position.degrees_to(destination.transform.position)
        );
        Vector3 head_attachment = destination.transform.position.offset_in_direction(
            source.radius,
            destination.transform.position.degrees_to(source.transform.position)
        );
            
        line_renderer.SetPosition(0, source.transform.position);
        line_renderer.SetPosition(1, destination.transform.position);
        tail.transform.position = tail_attachment;
        head.transform.position = head_attachment;
        tail.transform.direct_to(destination.transform.position);
        head.transform.direct_to(destination.transform.position);
    }
    
    #endregion
}
}