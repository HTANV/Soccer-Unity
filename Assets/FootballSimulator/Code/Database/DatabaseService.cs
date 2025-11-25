using FStudio.Database;
using System.Linq;
using UnityEngine;

namespace FStudio {
    public static class DatabaseService {
        private static TeamEntry[] loadedTeams;

        public static TeamEntry[] LoadTeams() {
            if (loadedTeams != null) {
                return loadedTeams;
            }
            var database = Resources.LoadAll("Database");
            loadedTeams = database.
                Where(x => x is TeamEntry).
                Select(x => (TeamEntry)x).ToArray();

            return loadedTeams;
        }

        public static Color RandomColor() {
            var r = Random.Range(0, 2);
            var g = Random.Range(0, 2);
            var b = Random.Range(0, 2);

            return new Color(r, g, b, 1);
        }
    }
}

