using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRGIN.Core;

namespace AGHVR
{
    public static class MyCursorSet
    {
        private static Harmony _hi;

        public static void Hook()
        {
            if (_hi == null)
                _hi = Harmony.CreateAndPatchAll(typeof(MyCursorSet));
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(CursorSet), nameof(CursorSet.Start))]
        private static IEnumerable<CodeInstruction> StartTpl(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(Cursor), nameof(Cursor.SetCursor), new[] { typeof(Texture2D), typeof(Vector2), typeof(CursorMode) }),
                                        AccessTools.Method(typeof(MyCursorSet), nameof(MyCursorSet.SetCursor)));
        }

        private static void SetCursor(Texture2D texture, Vector3 hotspot, CursorMode mode)
        {
            if (VR.GUI.SoftCursor)
            {
                VR.GUI.SoftCursor.SetCursor(texture ?? GameObject.FindObjectOfType<CursorSet>()?.Cursor00);
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(CursorSet), nameof(CursorSet.Update))]
        private static IEnumerable<CodeInstruction> UpdateTpl(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(Cursor), nameof(Cursor.SetCursor), new[] { typeof(Texture2D), typeof(Vector2), typeof(CursorMode) }),
                                               AccessTools.Method(typeof(MyCursorSet), nameof(MyCursorSet.SetCursor)));
        }
    }
}
