using System;
using System.IO;
using System.Xml.Serialization;
using BepInEx;
using VRGIN.Core;
using VRGIN.Helpers;

namespace ioVR
{

    /// <summary>
    /// This is an example for a VR plugin. At the same time, it also functions as a generic one.
    /// </summary>
    [BepInPlugin(GUID, Name, Version)]
    public class ioVR : BaseUnityPlugin
    {
        public const string GUID = "ioVR";
        public const string Name = "VR mod for Insult Order";
        public const string Version = "0.0.2";

        /// <summary>
        /// Determines when to boot the VR code. In most cases, it makes sense to do the check as described here.
        /// </summary>
        public void Start()
        {
            if (Environment.CommandLine.Contains("--vr"))
            {
                var context = new ioVRContext();
                VRManager.Create<ioInterpreter>(context);
                VR.Manager.SetMode<GenericStandingMode>();
            }
        }

        private IVRManagerContext CreateContext(string path)
        {
            var serializer = new XmlSerializer(typeof(ioVRContext));

            if (File.Exists(path))
            {
                // Attempt to load XML
                using (var file = File.OpenRead(path))
                {
                    try
                    {
                        return serializer.Deserialize(file) as ioVRContext;
                    }
                    catch (Exception e)
                    {
                        VRLog.Error("Failed to deserialize {0} -- using default", path);
                    }
                }
            }

            // Create and save file
            var context = new ioVRContext();
            try
            {
                using (var file = new StreamWriter(path))
                {
                    file.BaseStream.SetLength(0);
                    serializer.Serialize(file, context);
                }
            }
            catch (Exception e)
            {
                VRLog.Error("Failed to write {0}", path);
            }

            return context;
        }
    }
}
