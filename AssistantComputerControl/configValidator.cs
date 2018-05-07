using System;
using System.Configuration;
using System.Collections.Generic;

namespace AssistantComputerControl {
    class ConfigValidator {
        private enum validateTypes {
            str,
            integer,
            boolean
        };
        
        public static string config(string get = "") {
            return ConfigurationManager.AppSettings[get];
        }
        public static Dictionary<string, string> getValues() {
            Dictionary<string, string> config_values = new Dictionary<string, string>();
            foreach (string setting in ConfigurationManager.AppSettings) {
                if (setting != "config_version")
                    config_values.Add(setting, ConfigurationManager.AppSettings[setting]);
            }
            return config_values;
        }
        public static void WriteKey(string key, string value) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
        public static void WriteKey(Dictionary<string, string> config_values) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (string setting in ConfigurationManager.AppSettings) {
                if (config_values.ContainsKey(setting)) {
                    MainProgram.DoDebug("Setting [" + setting + "] to: " + config_values[setting]);
                    config.AppSettings.Settings[setting].Value = config_values[setting];
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
        }
        public static void Save() {
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).Save(ConfigurationSaveMode.Modified);
        }

        //For strings
        public void Validate(string configKey, ref string variable) {
            if (DoValidate(configKey, validateTypes.str)) {
                variable = config(configKey);
            }
        }
        //For ints
        public void Validate(string configKey, ref int variable, Nullable<int> min = null, Nullable<int> max = null) {
            if(DoValidate(configKey, validateTypes.integer, min, max)) {
                int n;
                int.TryParse(config(configKey), out n);

                variable = n;
            }
        }
        //For bools
        public void Validate(string configKey, ref bool variable) {
            if (DoValidate(configKey, validateTypes.boolean)) {
                bool theBool;
                if (bool.TryParse(config(configKey), out theBool)) {
                    variable = theBool;
                } else {
                    MainProgram.DoDebug("Config key \"" + configKey + "\" is not a valid boolean");
                }
            }
        }

        private bool DoValidate(string configKey, validateTypes type, Nullable<int> min = null, Nullable<int> max = null) {
            string configValue = config(configKey);
            string debugStart = "Config key \"" + configKey + "\"" ;

            ConfigurationManager.OpenExeConfiguration("");

            if (!String.IsNullOrEmpty(configValue)) {
                //Value is not empty
                switch (type) {
                    case validateTypes.integer:
                        int n;
                        if (int.TryParse(configValue, out n)) {
                            bool proceed = true;
                            if(min != null) {
                                if(!(n > min)) {
                                    //n is less than min
                                    proceed = false;
                                    MainProgram.DoDebug(debugStart + "value" + n + " is not larger than " + min);
                                }
                            }
                            if(max != null) {
                                if (!(n < max)) {
                                    //n is more than max
                                    proceed = false;
                                    MainProgram.DoDebug(debugStart + "value" + n + " is more than " + max);
                                }
                            }

                            if (proceed)
                                return true;
                        } else {
                            MainProgram.DoDebug(debugStart + "is not a valid integer (whole number)");
                        }
                        break;
                    default:
                        //requires no additional checks
                        return true;
                }
            } else {
                MainProgram.DoDebug("Config key \"" + configKey + "\" is empty/null");
            }

            return false;
        }
    }
}
