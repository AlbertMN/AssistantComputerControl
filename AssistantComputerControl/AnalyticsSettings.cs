using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AssistantComputerControl {
    class AnalyticsSettings {
        const string sendDataUrl = "https://acc.albe.pw/api/ReceiveAnalyticsData.php";
        const string sendSharingUrl = "https://acc.albe.pw/api/IsSharing.php";
        public const string sentryToken = "https://be790a99ae1f4de0b1af449f8d627455@sentry.io/1287269";
        private static readonly HttpClient client = new HttpClient();

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

        public static void UpdateSharing(bool doShare) {
            Properties.Settings.Default.SendAnonymousAnalytics = doShare;
            Properties.Settings.Default.Save();

            //Notify server whether
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", @"jmF}AXK6NH!#{@/:ZHV#qK6r#YxytM>K/W6#5q/}tEK!&*_Gd\_YNUBNN/$a+$r");

            var parameters = new Dictionary<string, string> {
                ["uid"] = Properties.Settings.Default.UID,
                ["share"] = doShare ? "true" : "false"
            };

            try {
                var response = client.PostAsync(sendSharingUrl, new FormUrlEncodedContent(parameters)).Result;
                var contents = response.Content.ReadAsStringAsync().Result;
                MainProgram.DoDebug("Posted user response");
            } catch (Exception e) {
                MainProgram.DoDebug("Failed to send analytics data - something wrong with the server?");
                MainProgram.DoDebug(e.ToString());
            }

            SetupAnalytics();
        }

        public static void SetupAnalytics() {
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

                DateTime dateTime = DateTime.UtcNow.Date;
                string thisDay = dateTime.ToString("yyyy/MM/dd");
                if (Properties.Settings.Default.AnalyticsUnsentData && Properties.Settings.Default.AnalyticsThisDay == thisDay) {
                    //Resume data for today
                    ScheduleAnalyticsSend();
                } else if (Properties.Settings.Default.AnalyticsUnsentData && Properties.Settings.Default.AnalyticsThisDay != thisDay) {
                    //Didn't send last time - sending now
                    SendAnalyticsData();
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
                if (Properties.Settings.Default.TotalActionsExecuted.Length >= pos) {
                    Properties.Settings.Default.TotalActionsExecuted[pos]++;

                    if (!Properties.Settings.Default.AnalyticsUnsentData) {
                        Properties.Settings.Default.AnalyticsUnsentData = true;
                        ScheduleAnalyticsSend();
                    }
                    Properties.Settings.Default.Save();
                } else {
                    MainProgram.DoDebug("Index " + pos.ToString() + " exceeds 'TotalActionsExecuted' property, which has length; " + Properties.Settings.Default.TotalActionsExecuted.Length.ToString());
                }
            } else {
                MainProgram.DoDebug("Could not find action \"" + action + "\" in action-array (analytics)");
            }
        }
        public static void AddCount(int action, string type) {
            AddTypeCount(type);
            if (actions[action] != null) {
                Properties.Settings.Default.TotalActionsExecuted[action]++;

                if (!Properties.Settings.Default.AnalyticsUnsentData) {
                    Properties.Settings.Default.AnalyticsUnsentData = true;
                    ScheduleAnalyticsSend();
                }
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

        static void ScheduleAnalyticsSend() {
            //Time when method needs to be called
            MainProgram.DoDebug("Analytics scheduled to be sent at 00:00");

            DateTime dateTime = DateTime.UtcNow.Date;
            Properties.Settings.Default.AnalyticsThisDay = dateTime.ToString("yyyy/MM/dd");
            Properties.Settings.Default.Save();

            var DailyTime = "00:00:00";
            var timeParts = DailyTime.Split(new char[1] { ':' });

            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                       int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date > dateNow)
                ts = date - dateNow;
            else {
                date = date.AddDays(1);
                ts = date - dateNow;
            }

            Task.Delay(ts).ContinueWith((x) => SendAnalyticsData());
        }

        public static string SendAnalyticsData() {
            if (Properties.Settings.Default.SendAnonymousAnalytics) {
                if (MainProgram.HasInternet()) {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", @"jmF}AXK6NH!#{@/:ZHV#qK6r#YxytM>K/W6#5q/}tEK!&*_Gd\_YNUBNN/$a+$r");

                    var parameters = new Dictionary<string, string> {
                        ["uid"] = Properties.Settings.Default.UID,
                        ["actions"] = string.Join(",", Properties.Settings.Default.TotalActionsExecuted),
                        ["assistants"] = string.Join(",", Properties.Settings.Default.AssistantType),
                        ["start_with_windows"] = Properties.Settings.Default.StartWithWindows ? "true" : "false",
                        ["version"] = MainProgram.softwareVersion,
                        ["beta_program"] = Properties.Settings.Default.BetaProgram ? "true" : "false",
                        ["date"] = Properties.Settings.Default.AnalyticsThisDay
                    };

                    try {
                        var response = client.PostAsync(sendDataUrl, new FormUrlEncodedContent(parameters)).Result;
                        var contents = response.Content.ReadAsStringAsync().Result;

                        //Success
                        Properties.Settings.Default.AnalyticsUnsentData = false;
                        Properties.Settings.Default.Save();
                        MainProgram.DoDebug("Analytics successfully posted");
                        return contents;
                    } catch (Exception e) {
                        MainProgram.DoDebug("Failed to send analytics data - something wrong with the server?");
                        MainProgram.DoDebug(e.ToString());
                        return "";
                    }
                } else {
                    MainProgram.DoDebug("Failed to send analytics data; no internet connection");
                    return "";
                }
            } else {
                return "";
            }
        }
    }
}