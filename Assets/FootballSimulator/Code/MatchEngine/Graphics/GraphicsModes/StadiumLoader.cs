
using FStudio.Graphics;
using FStudio.Utilities;
using System.Threading.Tasks;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.GraphicsModes {
    public class StadiumLoader : ScriptableObject {
        [SerializeField] private SerializableSceneCollection<StadiumType> stadiums;

        public async Task LoadStadium (StadiumType stadiumType) {
            Debug.Log($"Load Stadium type {stadiumType}");

            await stadiums.Load(stadiumType);
        }

        public void Unload () {
            stadiums.Unload();
        }
    }
}
