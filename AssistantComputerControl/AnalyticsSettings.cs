using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace AssistantComputerControl {
    class AnalyticsSettings {
        //private static string requestKey = null; // < version 1.1
        const string getKeyUrl = "https://acc.albe.pw/functions/ReceiveAnalyticsData.php";
        const string sendDataUrl = "https://acc.albe.pw/functions/ReceiveAnalyticsData.php";

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

        class KeyHandler {
            public bool Status { get; set; }
            public string Message { get; set; }
            public string Key { get; set; }
        }
    
        public static void SetupAnalyticsAsync() {
            if (Properties.Settings.Default.SendAnonymousAnalytics) {
                if (Properties.Settings.Default.TotalActionsExecuted == null) {
                    Properties.Settings.Default.TotalActionsExecuted = new int[actions.Length - 1];
                    Properties.Settings.Default.Save();
                }

                MainProgram.DoDebug("Annonymous analytics setup...");

                //Request analytics-key < For version 1.1
                //RequestKey();

                //Setup array
                int[] actionsExecuted = Properties.Settings.Default.TotalActionsExecuted;
                if (actionsExecuted.Length != actions.Length) {
                    int i = actionsExecuted.Length - 1;
                    while (i < actions.Length - 1) {
                        actionsExecuted[i] = 0;
                        i++;
                    }
                    Properties.Settings.Default.Save();
                }
            } else {
                MainProgram.DoDebug("Annonymous analytics are not being shared");
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

        /*private static void RequestKey() {
            using (var wb = new WebClient()) {
                string actionsExecutedString = string.Join(",", Properties.Settings.Default.TotalActionsExecuted);

                var data = new NameValueCollection {
                    ["key"] = requestKey,
                    ["actions_executed"] = actionsExecutedString
                };

                var response = wb.UploadValues(getKeyUrl, "POST", data);
                KeyHandler jsonResponse = JsonConvert.DeserializeObject<KeyHandler>(Encoding.UTF8.GetString(response));
                if (jsonResponse.Status) {
                    requestKey = jsonResponse.Key;
                    MainProgram.DoDebug("Successfully requested analytics key. Got \"" + requestKey + "\"");
                } else {
                    MainProgram.DoDebug("Failed to get analytics key; " + jsonResponse.Message);
                }
            }
        }*/

        /*public static void SendData() { // < for version 1.1
            if  (requestKey != null) {
                if (ACC_Updater.RemoteFileExists(sendDataUrl)) {
                    using (var wb = new WebClient()) {
                        string actionsExecutedString = string.Join(",", Properties.Settings.Default.TotalActionsExecuted);

                        var data = new NameValueCollection {
                            ["key"] = requestKey,
                            ["actions_executed"] = actionsExecutedString
                        };

                        var response = wb.UploadValues(sendDataUrl, "POST", data);
                        string responseInString = Encoding.UTF8.GetString(response);
                        
                        ... deserialize json
                    }
                }
            } else {
                MainProgram.DoDebug("Attempted to send analytics data, but the key was null, trying to request new");
                RequestKey();

                ...
            }
        }*/
    }
}