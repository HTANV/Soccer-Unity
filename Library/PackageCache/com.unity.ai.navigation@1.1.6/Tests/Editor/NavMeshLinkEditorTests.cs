using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Unity.AI.Navigation.Editor.Tests
{
    public class NavMeshLinkEditorTests
    {
        List<UnityEngine.Object> m_TestObjects = new();

        GameObject CreateTestObject(string name, params Type[] components)
        {
            var go = new GameObject(name, components);
            m_TestObjects.Add(go);
            return go;
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var obj in m_TestObjects)
            {
                if (obj != null)
                    UnityEngine.Object.DestroyImmediate(obj);
            }
            m_TestObjects.Clear();
        }

        static readonly Vector3[] k_ReverseDirectionPositions =
            { Vector3.zero, new(1f, 2f, 3f), new(1f, -2f, 3f) };
        static readonly Quaternion[] k_ReverseDirectionOrientations =
            { Quaternion.identity, new (0f, 0.7071067812f, 0f, 0.7071067812f) };
        static readonly Vector3[] k_ReverseDirectionScales =
            { Vector3.one, new(0.5f, 1f, 2f), new(0.5f, -1f, 2f) };

        [Test]
        public void ReverseDirection_SwapsStartAndEndPoints(
            [ValueSource(nameof(k_ReverseDirectionPositions))]    Vector3 position,
            [ValueSource(nameof(k_ReverseDirectionOrientations))] Quaternion orientation,
            [ValueSource(nameof(k_ReverseDirectionScales))]       Vector3 scale
        )
        {
            var nml = CreateTestObject("NavMeshLink", typeof(NavMeshLink)).GetComponent<NavMeshLink>();
            nml.transform.position = position;
            nml.transform.rotation = orientation;
            nml.transform.localScale = scale;
            nml.startPoint = new Vector3(2f, 0f, 0f);
            nml.endPoint = new Vector3(0f, 0f, 2f);

            NavMeshLinkEditor.ReverseDirection(nml);

            Assert.That(
                (nml.startPoint, nml.endPoint),
                Is.EqualTo((new Vector3(0f, 0f, 2f), new Vector3(2f, 0f, 0f))),
                "Start and end points did not swap."
            );
        }
    }
}
