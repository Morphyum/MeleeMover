using BattleTech;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MeleeMover {

    [HarmonyPatch(typeof(Pathing))]
    [HarmonyPatch("GetMeleeDestsForTarget")]
    public static class Pathing_GetMeleeDestsForTarget_Patch {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            List<CodeInstruction> instructionList = instructions.ToList();
            MethodInfo mi = AccessTools.Property(typeof(Vector3), nameof(Vector3.magnitude)).GetGetMethod();
            int index = instructionList.FindIndex(instruction => instruction.operand == mi) - 1;
            Debug.Log("INSTRUCTION before remove: " + instructionList[index]);
            instructionList.RemoveRange(index, 6);
            Debug.Log("INSTRUCTION: " + instructionList[index]);
            instructionList[index].labels.Clear();
            return instructionList;
        }
    }

    [HarmonyPatch(typeof(Pathing))]
    [HarmonyPatch("MeleeGrid", PropertyMethod.Getter)]
    public static class Pathing_MeleeGrid_Getter_Patch {
        static void Postfix(Pathing __instance, ref PathNodeGrid __result) {
            try {
                Settings settings = Helper.LoadSettings();
                if (settings.sprintRangeMelee) {
                    PathNodeGrid SprintingGrid = (PathNodeGrid)ReflectionHelper.InvokePrivateMethode(__instance, "get_SprintingGrid", null);
                    __result = SprintingGrid;
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }
}