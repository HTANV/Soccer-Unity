using FStudio.Data;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    [CreateAssetMenu(fileName = "NewTeam", menuName = "Database/Team", order = 1)]
    public class TeamEntry : ScriptableObject, ICloneable {
        public string TeamName = "New Team";
        [HideInInspector] public LogoEntry TeamLogo;
        [HideInInspector] public KitEntry HomeKit;
        [HideInInspector] public KitEntry AwayKit;
        [HideInInspector] public PlayerEntry[] Players;
        public Formations Formation;

        [HideInInspector] public bool IsValid;

        public int Overall => 
            Mathf.RoundToInt (Players.Select(x => x.Overall).
            Sum() / 11f);

#if UNITY_EDITOR
        public void Initialize () {
            if (Validate()) {
                IsValid = true;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private bool Validate() {
            var needsValidation = false;

            if (TeamLogo == null) {
                TeamLogo = CreateChildAsset<LogoEntry>("Logo");
                TeamLogo.Initialize();
                needsValidation = true;
            }

            if (HomeKit == null) {
                HomeKit = CreateChildAsset<KitEntry>("Kit - Home");
                HomeKit.Initialize();
                needsValidation = true;
            }

            if (AwayKit == null) {
                AwayKit = CreateChildAsset<KitEntry>("Kit - Away");
                AwayKit.Initialize();
                needsValidation = true;
            }

            if (Players == null || Players.Length != 11) {
                if (Players != null) {
                    var length = Players.Length;
                    for (int i = 0; i < length; i++) {
                        AssetDatabase.RemoveObjectFromAsset(Players[i]);
                    }
                }

                Players = new PlayerEntry[11];

                for (int i = 0; i < 11; i++) {
                    var tIndex = i;

                    var name = $"Player {i + 1}";
                    var player = CreateChildAsset<PlayerEntry>(name);
                    player.team = this;
                    Players[tIndex] = player;
                }

                needsValidation = true;
            }

            return needsValidation;
        }

        private T CreateChildAsset<T>(string name) where T : ScriptableObject {
            var childAsset = CreateInstance<T>();
            childAsset.name = name;
            AssetDatabase.AddObjectToAsset(childAsset, this);
            AssetDatabase.SaveAssets();
            return childAsset;
        }
#endif

        public object Clone() {
            var clone = CreateInstance<TeamEntry>();
            clone.AwayKit = AwayKit;
            clone.Formation = Formation;
            clone.HomeKit = HomeKit;
            clone.TeamLogo = TeamLogo;
            clone.TeamName = TeamName;

            clone.Players = Players.Select(x => x.Clone(clone)).ToArray();

            return clone;
        }
    }
}