using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AssistantComputerControl {
    class AnalyticsSettings {
        //private static string requestKey = null; // < version 1.1
        const string getKeyUrl = "https://acc.albe.pw/functions/ReceiveAnalyticsData.php";
        const string sendDataUrl = "https://acc.albe.pw/functions/ReceiveAnalyticsData.php";
        private static readonly HttpClient client = new HttpClient();

        public static readonly string[] actions = new String[13] { //No changing this order!
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
        };

        public static readonly string[] assistants = new String[4] { //No changing this order!
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
                    MainProgram.DoDebug(actions.Length + "!= " + actionsExecuted.Length);

                    //New action most likely added
                    int[] oldSettings = actionsExecuted
                        , newSettings = new int[actions.Length];

                    //Populate new analytics array with old values
                    int i = 0;
                    foreach(int ac in oldSettings) {
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
                Properties.Settings.Default.TotalActionsExecuted[pos]++;
                Properties.Settings.Default.Save();
            } else {
                MainProgram.DoDebug("Could not find action \"" + action + "\" in action-array (analytics)");
            }
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

        /*public static async System.Threading.Tasks.Task SendDataAsync() { // < for version 1.1
            if (ACC_Updater.RemoteFileExists(sendDataUrl)) {
                using (var wb = new WebClient()) {

                    var values = new Dictionary<string, string> {
                        { "actions_executed", JsonConvert.SerializeObject(Properties.Settings.Default.TotalActionsExecuted) },
                        { "thing2", "world" }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");

                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
        }*/
    }
}