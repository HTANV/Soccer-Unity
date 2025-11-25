
using FStudio.Loaders;
using FStudio.Utilities;

namespace FStudio.UI {
    public class UILoader : SceneObjectSingleton<UILoader> {
        public SingleAddressableLoader GeneralUILoader;
        public SingleAddressableLoader MatchUILoader;
    }
}
