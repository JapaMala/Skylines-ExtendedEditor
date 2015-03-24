﻿using System;
using System.Collections.Generic;

using ColossalFramework;

namespace ExtendedEditor
{
    public static class ModDebug
    {
        const string prefix = "ExtendedEditor: ";
        static bool debuggingEnabled = true;

        // Print messages to the in-game console that opens with F7
        public static void Log(object s)
        {
            if (!debuggingEnabled) return;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ModLog.txt", true))
            {
                file.WriteLine(s.ToString());
            }
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, prefix + s.ToString());
        }

        public static void Error(object s)
        {
            if (!debuggingEnabled) return;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ModLog.txt", true))
            {
                file.WriteLine(s.ToString());
            }
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Error, prefix + s.ToString());
        }

        public static void Warning(object s)
        {
            if (!debuggingEnabled) return;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ModLog.txt", true))
            {
                file.WriteLine(s.ToString());
            }
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, prefix + s.ToString());
        }
    }
}
