using System;

namespace AssistantComputerControl {
    class AnalyticsSettings {
        //ACC truly is open source, but for security reasons we will not share this one file with the whole world.
        //This file handles sending analytics to the developers' webserver and contains a few variables that contain -
        //sensitive access-tokens to integrations like Sentry.IO

        //The public version of this file is exactly like the full version, but stripped of -
        //API-keys and sensitive information.
        //Cencored items have placeholders to ensure that everyone can fork and run the project without errors
        public const string sentryToken = "super_secret";

        public static readonly string[] actions = new String[] { //No changing this order!
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
            "die",              //11
            "hibernate",        //12
            "monitors_off",     //13
            "keypress",         //14
            "write_out",        //15

            "key_shortcut",     //16
            "create_file",      //17
            "delete_file",      //18
            "append_text",      //19
            "message_box",      //20
        };

        public static readonly string[] assistants = new String[] { //No changing this order!
            "google",
            "alexa",
            "cortana",
            "unknown",
        };

        class KeyHandler {
            public bool Status { get; set; }
            public string Message { get; set; }
            public string Key { get; set; }
        }

        public static void SetupAnalyticsAsync() {
            //Unique user-ID
            if (Properties.Settings.Default.UID == "" || Properties.Settings.Default.UID == null) {
                string newUID = Guid.NewGuid().ToString("N");
                Properties.Settings.Default.UID = newUID;
                Properties.Settings.Default.Save();

                MainProgram.DoDebug("Set UID to " + newUID);
            } else {
                MainProgram.DoDebug("UID; " + Properties.Settings.Default.UID);
            }

            if (Properties.Settings.Default.SendAnonymousAnalytics) {
                MainProgram.DoDebug("Setting up annonymous analytics...");
                //Execution amounts
                if (Properties.Settings.Default.TotalActionsExecuted == null) {
                    Properties.Settings.Default.TotalActionsExecuted = new int[actions.Length - 1];
                    Properties.Settings.Default.Save();
                }

                int[] actionsExecuted = Properties.Settings.Default.TotalActionsExecuted;
                if (actions.Length != actionsExecuted.Length) {
                    //MainProgram.DoDebug(actions.Length + " != " + actionsExecuted.Length);

                    //New action most likely added
                    int[] oldSettings = actionsExecuted
                        , newSettings = new int[actions.Length];

                    //Populate new analytics array with old values
                    int i = 0;
                    foreach (int ac in oldSettings) {
                        if (i != newSettings.Length)
                            newSettings[i] = ac;
                        i++;
                    }

                    Properties.Settings.Default.TotalActionsExecuted = newSettings;
                    Properties.Settings.Default.Save();
                }

                //Assistant type
                if (Properties.Settings.Default.AssistantType == null) {
                    Properties.Settings.Default.AssistantType = new int[actions.Length - 1];
                    Properties.Settings.Default.Save();
                }

                MainProgram.DoDebug("Annonymous analytics setup done");
            } else {
                MainProgram.DoDebug("Annonymous analytics are not being shared");
            }
        }
        public static void PrintAnalytics() {
            int i = 0
                , totalCount = 0;
            if (Properties.Settings.Default.TotalActionsExecuted != null) {
                foreach (int count in Properties.Settings.Default.TotalActionsExecuted) {
                    MainProgram.DoDebug(i + ". " + actions[i] + ": executed " + count + " times");
                    i++;

                    totalCount += count;
                }
            }

            MainProgram.DoDebug("\nTotal executions; " + totalCount);
        }

        public static void AddCount(string action, string type) {
            AddTypeCount(type);
            int pos = Array.IndexOf(actions, action);
            if (pos > -1) {
                //MainProgram.DoDebug("Added +1 to " + action + " at pos " + pos);
                Properties.Settings.Default.TotalActionsExecuted[pos]++;
                Properties.Settings.Default.Save();
            } else {
                MainProgram.DoDebug("Could not find action \"" + action + "\" in action-array (analytics)");
            }

            SendAnalyticsData();
        }
        public static void AddCount(int action, string type) {
            AddTypeCount(type);
            if (actions[action] != null) {
                Properties.Settings.Default.TotalActionsExecuted[action]++;
                Properties.Settings.Default.Save();
            } else {
                MainProgram.DoDebug("Could not find action with index \"" + action + "\" in action-array (analytics)");
            }
        }

        private static void AddTypeCount(string type) {
            if (type == null) {
                Properties.Settings.Default.AssistantType[3]++;
                Properties.Settings.Default.Save();
            } else {
                int pos = Array.IndexOf(assistants, type);
                if (pos > -1) {
                    Properties.Settings.Default.AssistantType[pos]++;
                    Properties.Settings.Default.Save();
                } else {
                    MainProgram.DoDebug("Assistant type \"" + type + "\" doesn't exist in assistant-array (analytics)");
                }
            }
        }

        public static string SendAnalyticsData() {
            //Sends analytics data to the server
            if (Properties.Settings.Default.SendAnonymousAnalytics) {
                //Do it (sensitive code)
            } else {
                //Don't do it (does nothing here)
            }
            return "";
        }
    }
}