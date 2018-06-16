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
            instructionList.RemoveRange(index, 6);
            instructionList[index].labels.Clear();
            
            PropertyInfo prop = AccessTools.Property(typeof(Pathing), "MeleeGrid");
            MethodInfo miold = prop.GetGetMethod(true);
            MethodInfo minew = AccessTools.Property(typeof(Pathing), "SprintingGrid").GetGetMethod(true);

            return instructionList.MethodReplacer(miold, minew);
        }
    }

    [HarmonyPatch(typeof(Pathing))]
    [HarmonyPatch("getGrid")]
    public static class Pathing_getGrid_Patch {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            MethodInfo miold = AccessTools.Property(typeof(Pathing), "MeleeGrid").GetGetMethod(true);
            MethodInfo minew = AccessTools.Property(typeof(Pathing), "SprintingGrid").GetGetMethod(true);
            return instructions.MethodReplacer(miold, minew);
        }
    }

    [HarmonyPatch(typeof(Pathing))]
    [HarmonyPatch("SetMeleeTarget")]
    public static class Pathing_SetMeleeTarget_Patch {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            MethodInfo miold = AccessTools.Property(typeof(Pathing), "MeleeGrid").GetGetMethod(true);
            MethodInfo minew = AccessTools.Property(typeof(Pathing), "SprintingGrid").GetGetMethod(true);
            return instructions.MethodReplacer(miold, minew);
        }
    }
    



    /*[HarmonyPatch(typeof(Mech))]
    [HarmonyPatch("MaxMeleeEngageRangeDistance", PropertyMethod.Getter)]
    public static class Pathing_MaxMeleeEngageRangeDistance_Getter_Patch {
        static void Postfix(Mech __instance, ref float __result) {
            try {
                Settings settings = Helper.LoadSettings();
                if (settings.sprintRangeMelee) {
                    __result = __instance.MaxSprintDistance;
                }
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }*/
}