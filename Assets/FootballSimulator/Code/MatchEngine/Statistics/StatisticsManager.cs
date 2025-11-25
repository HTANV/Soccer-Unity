
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Players;
using System;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class StatisticsManager : IDisposable {
        private static bool HasInstance;

        // statistics members
        public readonly Dribbling       dribbling;
        public readonly RunningDistance runningDistance;
        public readonly Possesioning    possesioning;
        public readonly Passing         passing;
        public readonly Shooting        shooting;
        public readonly Corners         corners;
        public readonly BallWinning     ballWinning;
        public readonly BallLosing      ballLosing;
        public readonly Saves           saves;

 
        public StatisticsManager (int[] players, Ball ball) {
            if (HasInstance) {
                Debug.LogError("The last one is not disposed.");
            }

            runningDistance = new RunningDistance(in players);
            possesioning = new Possesioning(ball);
            passing = new Passing(in players);
            shooting = new Shooting(in players);
            dribbling = new Dribbling(in players);
            corners = new Corners();
            ballWinning = new BallWinning(in players);
            ballLosing = new BallLosing(in players);
            saves = new Saves(in players);

            HasInstance = true;
        }

        public void Update() {
            dribbling.Update();
            runningDistance.Update();
            possesioning.Update();
        }
        public void ResetPositions() {
            runningDistance.ResetPositions();
            dribbling.ResetPositions();
        }

        public float PlayerRating (int playerId) {
            const float runningDistanceMod = 0.01f;
            const float passCountMod = 0.05f;
            const float successPassCountMod = 0.15f;
            const float failPassCountMod = -0.05f;
            const float shootCountMod = 0.1f;
            const float successShootCountMod = 0.2f;
            const float failedShootCountMod = -0.05f;
            const float goalCountMod = 2f;
            const float dribblingDistanceMod = 0.05f;
            const float ballWinningCountMod = 0.5f;
            const float ballLossCountMod = -0.1f;
            const float savesMod = 2f;


            float rating = 0;

            rating += runningDistance.PlayerDistances[playerId] * runningDistanceMod;
            rating += passing.PlayerPassing[playerId] * passCountMod;
            rating += passing.PlayerSuccesfulPasses[playerId] * successPassCountMod;
            rating += passing.PlayerPassing[playerId] -  passing.PlayerSuccesfulPasses[playerId] * failPassCountMod;
            rating += shooting.PlayerAttempt[playerId] * shootCountMod;
            rating += shooting.PlayerAttemptOnTarget[playerId] * successShootCountMod;
            rating += shooting.PlayerAttempt[playerId] - shooting.PlayerAttemptOnTarget[playerId] * failedShootCountMod;
            rating += shooting.PlayerGoal[playerId] * goalCountMod;
            rating += dribbling.PlayerDribbling[playerId] * dribblingDistanceMod;
            rating += ballWinning.PlayerBallWinning[playerId] * ballWinningCountMod;
            rating += ballLosing.PlayerBallLosing[playerId] * ballLossCountMod;
            rating += saves.PlayerBallSave[playerId] * savesMod;

            rating = (float)Math.Round(rating, 1);

            return rating;
        }

        public void Dispose() {
            dribbling.Dispose();
            runningDistance.Dispose();
            possesioning.Dispose();
            passing.Dispose();
            shooting.Dispose();
            corners.Dispose();
            ballWinning.Dispose();
            ballLosing.Dispose();
            saves.Dispose();

            HasInstance = false;
        }
    }
}
