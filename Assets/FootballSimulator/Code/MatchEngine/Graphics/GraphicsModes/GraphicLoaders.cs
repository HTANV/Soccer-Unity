

using FStudio.Scriptables;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.GraphicsModes {
    public class GraphicLoaders : SerializedSingletonScriptable<GraphicLoaders> {
        public StadiumLoader stadiumLoader;
        public BallLoader ballLoader;
        public LayerMask renderLayer;
    }
}
