using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using VRGIN.Core;

namespace AGHVR
{
    [BepInPlugin(GUID, DisplayName, Version)]
    public class AGHVR : BaseUnityPlugin
    {
        public const string GUID = "AGHVR";
        public const string DisplayName = "VR mode for Houkago Rinkan Chuudoku";
        public const string Version = "0.1";

        public IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            if (Environment.CommandLine.Contains("--vr"))
            {
                var context = new AGHContext();
                VRManager.Create<AGHInterpreter>(context);
                VR.Manager.SetMode<AGHSeatedMode>();
            }
            //VRLog.Info("Layers: " + string.Join(", ", UnityHelper.GetLayerNames(int.MaxValue)));
            //UnityEngine.SceneManagement.SceneManager.LoadScene(7);
        }
    }
}
