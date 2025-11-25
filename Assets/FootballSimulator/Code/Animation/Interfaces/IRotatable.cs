using UnityEngine;

namespace FStudio.Animation {
    public interface IRotatable {
        Quaternion GetRotation();
        void SetRotation(Quaternion rotation);
    }
}

