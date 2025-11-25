using System.Linq;
using System;

namespace Shared {
    public readonly struct Curve {
        private readonly Point[] points;

        public readonly struct Point {
            public readonly bool IsValid;
            public readonly float Time;
            public readonly float Value;

            public Point(float time, float value) {
                this.Time = time;
                this.Value = value;
                IsValid = true;
            }
        }

        public Curve (Point[] points) {
            this.points = points.OrderBy (x=>x.Time).ToArray ();
        }

        public float Evaluate (float time) {
            var firstPoint = points.
                Where(x => x.Time <= time).
                OrderBy(x => Math.Abs (time - x.Time)).
                FirstOrDefault();

            var secondPoint = points.
                Where(x => x.Time >= time).
                OrderBy(x => Math.Abs(x.Time - time)).
                FirstOrDefault();

            if (!firstPoint.IsValid) {
                firstPoint = points.OrderBy(x => x.Time).FirstOrDefault();
            }

            var distanceToFirst = time - firstPoint.Time;

            if (!secondPoint.IsValid) {
                secondPoint = points.Last();
            }

            var size = secondPoint.Time - firstPoint.Time;

            if (distanceToFirst == 0) {
                return firstPoint.Value;
            }

            if (size == 0) {
                return secondPoint.Value;
            }

            var valueDiff = secondPoint.Value - firstPoint.Value;

            var percentage = distanceToFirst / size;

            var result = firstPoint.Value + valueDiff * percentage;

            return result;
        }
    }
}