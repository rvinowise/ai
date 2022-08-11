using UnityEngine;

namespace rvinowise.unity.geometry2d {

public static class Distance_from_point_to_line {
    public static float get_distance(
        Vector3 point, Vector3 line_start, Vector3 line_end, out Vector3 closest
    ) {
        float dx = line_end.x - line_start.x;
        float dy = line_end.y - line_start.y;
        if ((dx == 0) && (dy == 0))
        {
            // It's a point not a line segment.
            closest = line_start;
            dx = point.x - line_start.x;
            dy = point.y - line_start.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        // Calculate the t that minimizes the distance.
        float t = ((point.x - line_start.x) * dx + (point.y - line_start.y) * dy) /
            (dx * dx + dy * dy);

        // See if this represents one of the segment's
        // end points or a point in the middle.
        if (t < 0)
        {
            closest = new Vector3(line_start.x, line_start.y);
            dx = point.x - line_start.x;
            dy = point.y - line_start.y;
        }
        else if (t > 1)
        {
            closest = new Vector3(line_end.x, line_end.y);
            dx = point.x - line_end.x;
            dy = point.y - line_end.y;
        }
        else
        {
            closest = new Vector3(line_start.x + t * dx, line_start.y + t * dy);
            dx = point.x - closest.x;
            dy = point.y - closest.y;
        }

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

   


}



}