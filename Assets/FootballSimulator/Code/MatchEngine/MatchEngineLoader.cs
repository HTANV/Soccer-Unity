using FStudio.Events;
using FStudio.Graphics;
using FStudio.Loaders;
using FStudio.Graphics.TimeOfDay;
using FStudio.UI;
using FStudio.UI.Events;
using FStudio.UI.GamepadInput;
using FStudio.UI.MatchThemes;
using FStudio.Utilities;
using Shared.Responses;
using FStudio.Data;
using System.Threading.Tasks;
using UnityEngine;
using FStudio.MatchEngine.Graphics.GraphicsModes;
using FStudio.Graphics.Cameras;
using FStudio.UI.MatchThemes.MatchEvents;
using FStudio.MatchEngine.Enums;

namespace FStudio.MatchEngine {
    public class MatchEngineLoader : SceneObjectSingleton<MatchEngineLoader> {
        [SerializeField] private SingleAddressableLoader loader;

        private bool isLoading;
        private bool isLoaded;

        public static async Task CreateMatch(MatchCreateRequest matchData) {
            // close all UI.
            EventManager.Trigger(new CloseAllPanelsEvent());

            // clear all snap history.
            SnapManager.Clear();

            // load the match UI
            await UILoader.Current.MatchUILoader.Load();

            // unload the general UI
            UILoader.Current.GeneralUILoader.Unload();

            var upcomingMatchEvent = new UpcomingMatchEvent(matchData);

            EventManager.Trigger(upcomingMatchEvent);
        }

        public async Task StartMatchEngine (
            UpcomingMatchEvent matchEvent,
            bool homeKit,
            bool awayKit) {

            if (isLoading) {
                return;
            }

            if (isLoaded) {
                // unload.
                await UnloadMatch();
            }

            // match kits.
            EventManager.Trigger(
                new MatchKitsEvent(
                homeKit ? matchEvent.details.homeTeam.AwayKit : matchEvent.details.homeTeam.HomeKit,
                awayKit ? matchEvent.details.awayTeam.AwayKit : matchEvent.details.awayTeam.HomeKit));
            //

            isLoading = true;

            // close all UI.
            EventManager.Trigger(new CloseAllPanelsEvent());

            // Big loading.
            EventManager.Trigger(new BigLoadingEvent());

            var template = GraphicLoaders.Current;

            // load stadium scene
            StadiumType stadium = StadiumType.SmallStadium;
            await template.stadiumLoader.LoadStadium(stadium);
            // 

            await loader.Load(); // load match prefab.

            if (TimeOfDaySystem.Current != null) {
                // load time of day.
                await TimeOfDaySystem.Current.LoadTemplate(matchEvent.details.dayTime);
            }

            MainCamera.Current.Camera.cullingMask = template.renderLayer;

            // skybox mode on.
            MainCamera.Current.Camera.clearFlags = CameraClearFlags.Skybox;

            Debug.Log("Creating core match...");

            await MatchManager.CreateMatch(
                new MatchManager.MatchDetails(
                    matchEvent,
                    homeKit,
                    awayKit)
                );

            Debug.Log("Loading ball...");

            // load random ball.
            await template.ballLoader.LoadRandomBall();
            
            isLoaded = true;
            isLoading = false;

            Debug.Log("Done...");

            // close loading.
            EventManager.Trigger<BigLoadingEvent>(null);
        }

        public async Task UnloadMatch () {
            if (MatchManager.Current == null) {
                Debug.LogWarning($"Match is not loaded to unload.");
                return;
            }

            // skybox mode off.
            MainCamera.Current.Camera.clearFlags = CameraClearFlags.SolidColor;

            // close all UI.
            EventManager.Trigger(new CloseAllPanelsEvent());

            var template = GraphicLoaders.Current;

            // unload ball & stadium.
            template.ballLoader.UnloadBall();
            template.stadiumLoader.Unload();
            // 

            SnapManager.Clear();

            UILoader.Current.MatchUILoader.Unload();

            MatchManager.Current.ClearMatch(); // clear field.

            loader.Unload(); // clear match manager prefab.

            await UILoader.Current.GeneralUILoader.Load();

            GameInput.SwitchToUI();

            isLoaded = false;
        }
    }
}
