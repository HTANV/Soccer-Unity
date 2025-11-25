
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.UI.GamepadInput {
    public interface ISnappable {
        Vector3 position { get; }
        bool blockHorizontalNavigation { get; }
        bool isSnappable { get; }
        GameObject gObject { get; }
        ScrollRect OnSnap();
        void OnSnapLeft();
        void OnClick();
    }
}

