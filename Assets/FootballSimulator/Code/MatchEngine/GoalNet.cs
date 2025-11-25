using UnityEngine;

using FStudio.MatchEngine.Players;
using FStudio.MatchEngine.Balls;

using System.Linq;
using System.Collections.Generic;

namespace FStudio.MatchEngine {
    public class GoalNet : MonoBehaviour {
        [SerializeField] private Transform middlePoint = default;

        public GameObject GoalColliders, OutColliders;
        public Transform[] goalPoints;

        public Transform leftLimit;
        public Transform rightLimit;

        public Vector3 GetShootingVectorFromPoint (PlayerBase playerBase, 
            Transform point) {

            var playerPosition = playerBase.Position;

            var skill = playerBase.MatchPlayer.GetShooting();

            var dir = point.position - playerPosition;

            var dirErrorApplied = Ball.ApplyDirectionError(dir, skill);

            // restore by distance
            var dist = Vector3.Distance(point.position, playerPosition);
            dirErrorApplied = Vector3.Lerp(dirErrorApplied, dir, 
                EngineSettings.Current.ShootErrorRemoveByDistance.Evaluate(dist));
            // 

            var errorAppliedAngle = Mathf.Abs(Vector3.SignedAngle(dir, dirErrorApplied, Vector3.up));

            //normalize it.
            dirErrorApplied = dirErrorApplied.normalized;

            var dir2D = dirErrorApplied;
            dir2D.y = 0;

            var dirUp = dirErrorApplied;
            dirUp.x = dirUp.z = 0;

            // add multipliers.
            dirErrorApplied += dir2D * EngineSettings.Current.ShootingForwardAxisMultiplier;
            //

            dirErrorApplied *= EngineSettings.Current.ShootPowerByDistanceCurve.Evaluate(dir.magnitude);

            dirErrorApplied *= EngineSettings.Current.ShootPowerBySkillCurve.
                Evaluate(playerBase.MatchPlayer.GetShooting() / 100f);

            Debug.Log($"[Shootpoint found] {dirErrorApplied}");

            dirErrorApplied += Vector3.up * dir.magnitude * EngineSettings.Current.ShootingUpAxisDistanceMultiplier;

            Debug.Log($"[Shooting point y fixed] {dirErrorApplied}");

            Debug.DrawRay(playerPosition, dir, Color.yellow, 1);
            Debug.DrawRay(playerPosition, dirErrorApplied, Color.green, 1);

            if (dirErrorApplied.y < 1) {
                dirErrorApplied.y = 1;
            }

            return dirErrorApplied;
        }

        /// <summary>
        /// Checks all goal points, 
        /// and return shooting velocities with direction error applied.
        /// </summary>
        /// <param name="playerBase">Shooter</param>
        /// <param name="colliders">Possible colliders</param>
        /// <returns>Velocity, and applied error.</returns>
        public (Transform shootPoint, float angleFree)
            GetShootingVector (PlayerBase playerBase, PlayerBase[] colliders) {
            
            if (goalPoints.Length == 0) {
                return default;
            }

            var fieldSizeY = MatchManager.Current.SizeOfField.y;

            var mPosition = playerBase.Position;

            float minAngle (Transform m_point) {
                var pointToPlayer = m_point.position - mPosition;

                float min = colliders.Select(x => Mathf.Min (Mathf.Abs (Vector3.SignedAngle(x.Position - mPosition, pointToPlayer, Vector3.up)), 45)).
                OrderBy (x=>x).FirstOrDefault ();
                return min;
            }

            var shootingVector =
                goalPoints.Select (x=>(x, minAngle (x))).OrderBy(x => 
                 Random.Range (-5, 5) +
                 Mathf.Abs(x.x.position.x - fieldSizeY / 2) + 
                 Random.Range (0, x.x.position.y) + 
                 (45-x.Item2)/2).
                 FirstOrDefault();

            return shootingVector;
        }

        /// <summary>
        /// Middle point of the goal net.
        /// </summary>
        public Vector3 Position => middlePoint.position;

        /// <summary>
        /// Direction of the goal net. System will use this direction for attacking & defending.
        /// </summary>
        public Vector3 Direction => middlePoint.forward;
    }
}
