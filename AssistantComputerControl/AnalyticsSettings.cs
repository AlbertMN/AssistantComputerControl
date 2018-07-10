using System;

namespace AssistantComputerControl {
    class AnalyticsSettings {
        public static readonly string[] actions = new String[12] { //No changing this order!
            "shutdown",         //0
            "restart",          //1
            "open",             //2
            "sleep",            //3
            "lock",             //4
            "logout",           //5
            "set_volume",       //6
            "mute",             //7
            "previous_song",    //8
            "play_pause",       //9
            "next_song",        //10
            "die"               //11
        };

        public static void SetupArray() {
            if (Properties.Settings.Default.TotalActionsExecuted == null) {
                Properties.Settings.Default.TotalActionsExecuted = new int[actions.Length - 1];
                Properties.Settings.Default.Save();
            }
                
            int[] actionsExecuted = Properties.Settings.Default.TotalActionsExecuted;
            if (actionsExecuted.Length != actions.Length) {
                int i = actionsExecuted.Length - 1;
                while (i < actions.Length - 1) {
                    actionsExecuted[i] = 0;
                    i++;
                }
                Properties.Settings.Default.Save();
            }
        }

        public static void PrintAnalytics() {
            int i = 0
                , totalCount = 0;
            if (Properties.Settings.Default.TotalActionsExecuted != null) {
                foreach (int count in Properties.Settings.Default.TotalActionsExecuted) {
                    MainProgram.DoDebug(actions[i] + ": executed " + count + " times");
                    i++;

                    totalCount += count;
                }
            }

            MainProgram.DoDebug("\nTotal executions; " + totalCount);
        }

        public static void AddCount(string action) {
            int pos = Array.IndexOf(actions, action);
            if (pos > -1) {
                Properties.Settings.Default.TotalActionsExecuted[pos]++;
                Properties.Settings.Default.Save();
            } else {
                MainProgram.DoDebug("Could not find action \"" + action + "\" in action-array (analytics)");
            }
        }
        public static void AddCount(int action) {
            if (actions[action] != null) {
                Properties.Settings.Default.TotalActionsExecuted[action]++;
                Properties.Settings.Default.Save();
            } else {
                MainProgram.DoDebug("Could not find action with index \"" + action + "\" in action-array (analytics)");
            }
        }
    }
}