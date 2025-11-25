using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.Animation {
    public interface IScalable {
        Vector3 GetScale();
        void SetScale(Vector3 scale);
    }
}

