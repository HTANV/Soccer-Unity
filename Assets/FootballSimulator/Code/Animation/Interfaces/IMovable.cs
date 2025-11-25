using UnityEngine;

namespace FStudio.Animation
{
    public interface IMovable : IRotatable {
        Vector3 GetPosition();
        void SetPosition(Vector3 targetPosition);
    }
}
