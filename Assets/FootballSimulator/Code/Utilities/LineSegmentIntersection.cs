using UnityEngine;

namespace FStudio.Utilities {
    /// <summary>
    /// https://stackoverflow.com/a/37406831
    /// </summary>
    public struct LineSegment2f {
        public Vector3 From { get; }
        public Vector3 To { get; }

        public LineSegment2f(Vector3 @from, Vector3 to) {
            From = @from;
            To = to;
        }

        public Vector3 Delta => new Vector3(To.x - From.x, 0, To.z - From.z);

        /// <summary>
        /// Attempt to intersect two line segments.
        /// </summary>
        /// <remarks>
        /// Even if the line segments do not intersect, <paramref name="t"/> and <paramref name="u"/> will be set.
        /// If the lines are parallel, <paramref name="t"/> and <paramref name="u"/> are set to <see cref="float.NaN"/>.
        /// </remarks>
        /// <param name="other">The line to attempt intersection of this line with.</param>
        /// <param name="intersectionPoint">The point of intersection if within the line segments, or empty..</param>
        /// <param name="t">The distance along this line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <param name="u">The distance along the other line at which intersection would occur, or NaN if lines are collinear/parallel.</param>
        /// <returns><c>true</c> if the line segments intersect, otherwise <c>false</c>.</returns>
        public bool TryIntersect(LineSegment2f other, out Vector3 intersectionPoint, out float t, out float u) {
            var p = From;
            var q = other.From;
            var r = Delta;
            var s = other.Delta;

            // t = (q − p) × s / (r × s)
            // u = (q − p) × r / (r × s)

            var denom = Fake2DCross(r, s);

            if (denom == 0) {
                // lines are collinear or parallel
                t = float.NaN;
                u = float.NaN;
                intersectionPoint = default(Vector3);
                return false;
            }

            var tNumer = Fake2DCross(q - p, s);
            var uNumer = Fake2DCross(q - p, r);

            t = tNumer / denom;
            u = uNumer / denom;

            if (t < 0 || t > 1 || u < 0 || u > 1) {
                // line segments do not intersect within their ranges
                intersectionPoint = default(Vector3);
                return false;
            }

            intersectionPoint = p + r * t;
            return true;
        }

        private static float Fake2DCross(Vector3 a, Vector3 b) {
            return a.x * b.z - a.z * b.x;
        }
    }
}
