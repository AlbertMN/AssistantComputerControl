using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HomeComputerControl {
    class configValidator {
        private enum validateTypes {
            str,
            integer,
            boolean
        };

        public static string config(string get = "") {
            return ConfigurationManager.AppSettings[get];
        }

        //For strings
        public void validate(string configKey, ref string variable) {
            if (doValidate(configKey, validateTypes.str)) {
                variable = config(configKey);
            }
        }
        //For ints
        public void validate(string configKey, ref int variable, Nullable<int> min = null, Nullable<int> max = null) {
            if(doValidate(configKey, validateTypes.integer, min, max)) {
                int n;
                int.TryParse(config(configKey), out n);

                variable = n;
            }
        }
        //For bools
        public void validate(string configKey, ref bool variable) {
            if (doValidate(configKey, validateTypes.boolean)) {
                bool theBool;
                if (bool.TryParse(config(configKey), out theBool)) {
                    variable = theBool;
                } else {
                    MainProgram.doDebug("Config key \"" + configKey + "\" is not a valid boolean");
                }
            }
        }

        private bool doValidate(string configKey, validateTypes type, Nullable<int> min = null, Nullable<int> max = null) {
            string configValue = config(configKey);
            string debugStart = "Config key \"" + configKey + "\"" ;

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
                                    MainProgram.doDebug(debugStart + "value" + n + " is not larger than " + min);
                                }
                            }
                            if(max != null) {
                                if (!(n < max)) {
                                    //n is more than max
                                    proceed = false;
                                    MainProgram.doDebug(debugStart + "value" + n + " is more than " + max);
                                }
                            }

                            if (proceed)
                                return true;
                        } else {
                            MainProgram.doDebug(debugStart + "is not a valid integer (whole number)");
                        }
                        break;
                    default:
                        //requires no additional checks
                        return true;
                }
            } else {
                MainProgram.doDebug("Config key \"" + configKey + "\" is empty/null");
            }

            return false;
        }
    }
}
