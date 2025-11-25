using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;
using UnityEngine;

using FStudio.MatchEngine.Players.Behaviours;
using FStudio.MatchEngine.Utilities;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FStudio.MatchEngine.Players.PlayerController
{
    public partial class CodeBasedController
    {
#if UNITY_EDITOR
        protected virtual void OnDrawGizmos () {
            if (!debugger) {
                return;
            }
            
            Gizmos.color = Color.yellow;

            Handles.DrawWireDisc(Position + Vector3.up, Vector3.up, 1);

            Gizmos.DrawLine(Position + Vector3.up, Position + Vector3.up + Direction * 5);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere (targetPosition, 1);

            if (BasePlayer.MatchPlayer != null && BasePlayer.MatchPlayer.Player != null) {
                Handles.Label(Position + Vector3.up * 3, $"Name: {BasePlayer.MatchPlayer.Player.Name}");
            }

            if (BasePlayer.IsInOffside) {
                Handles.color = Color.red;
                Handles.Label(Position + Vector3.up*2, $"Offside Position");
            }

            Handles.color = Color.green;
            Handles.Label(Position + Vector3.up, $"Current Act: {BasePlayer.CurrentAct}");

            var markers = "";
            foreach (var marker in BasePlayer.Markers.Members) {
                markers += "," + marker.MatchPlayer.Player.Name;
            }
            Handles.Label(Position + Vector3.up * 3.5f, $"Markers: {markers}");
        }
#endif
    }
}