using Harmony;
using System.Reflection;

namespace MeleeMover
{
    public class MeleeMover
    {
        internal static string ModDirectory;
        public static void Init(string directory, string settingsJSON) {
            var harmony = HarmonyInstance.Create("de.morphyum.MeleeMover");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            ModDirectory = directory;
        }
    }
}
