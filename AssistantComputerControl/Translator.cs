using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.Control;

namespace AssistantComputerControl {
    class Translator {
        static public string activeLanguage = "English",
            activeLanguageJson = null,
            translationFolder = null,
            fallbackLanguageJson = null;
        static public dynamic dynamicJsonTranslation = null, fallbackDynamicJsonTranslation = null;
        static public string[] languagesArray = null;

        public static string[] GetLanguages() {
            if (!String.IsNullOrEmpty(translationFolder)) {
                List<string> stringList = new List<string>();
                DirectoryInfo d = new DirectoryInfo(translationFolder);
                foreach (var file in d.GetFiles("*.json")) {
                    string fileContent = ReadLanguage(file.FullName);
                    if (fileContent != null) {
                        try {
                            dynamic jsonTest = JsonConvert.DeserializeObject<dynamic>(fileContent);
                            if (jsonTest["translations"] != null) {
                                stringList.Add(Path.GetFileNameWithoutExtension(file.FullName));
                            } else {
                                MainProgram.DoDebug("Invalid translation; " + (jsonTest));
                            }
                        } catch {
                            MainProgram.DoDebug("Could not validate language from file " + file.Name);
                        }
                    }
                }

                string[] theArr = stringList.ToArray<string>();
                return theArr;
            }
            return new string[0];
        }

        public static void SetLanguage(string lang) {
            if (!String.IsNullOrEmpty(translationFolder)) {
                string theFile = Path.Combine(translationFolder, lang + ".json");
                if (File.Exists(theFile)) {
                    activeLanguage = lang;
                    activeLanguageJson = ReadLanguage(theFile);

                    if (lang != "English") {
                        fallbackLanguageJson = ReadLanguage(Path.Combine(translationFolder, "English.json"));
                        fallbackDynamicJsonTranslation = JsonConvert.DeserializeObject<dynamic>(fallbackLanguageJson);
                    } else {
                        fallbackLanguageJson = null;
                    }

                    dynamicJsonTranslation = JsonConvert.DeserializeObject<dynamic>(activeLanguageJson);
                }
            }
        }

        public static string ReadLanguage(string file) {
            try {
                return File.ReadAllText(file);
            } catch {
                return null;
            }
        }

        public static void TranslateWinForms(string domain, ControlCollection ctrl) {
            foreach (Control xx in ctrl) {
                if (xx is Label || xx is Button || xx is CheckBox || xx is LinkLabel) {
                    if (xx.Text[0] != '|') {
                        xx.Text = Translator.__(xx.Text, domain);
                    } else {
                        xx.Text = xx.Text.Remove(0, 1);
                    }

                    //TODO ADD TOOLTIP TEXT IF "_hover_content" EXISTS
                }
            }
        }

        public static string __(string text, string domain, bool doFallback = false) {
            dynamic translation;

            if (doFallback) {
                if (fallbackLanguageJson != null) {
                    //translation = JsonConvert.DeserializeObject<dynamic>(fallbackLanguageJson);
                    translation = fallbackDynamicJsonTranslation;
                } else {
                    return "";
                }
            } else {
                if (activeLanguageJson != null) {
                    //translation = JsonConvert.DeserializeObject<dynamic>(activeLanguageJson);
                    translation = dynamicJsonTranslation;
                } else {
                    Console.WriteLine("Active language JSON is null (trying to translate; " + text + ", " + domain +")");
                    return "";
                }
            }

            if (translation["translations"] != null) {
                if (translation["translations"][domain] != null) {
                    if (translation["translations"][domain][text] != null) {
                        if (doFallback) {
                            MainProgram.DoDebug("Fallback translation success");
                        }
                        return translation["translations"][domain][text];
                    } else {
                        MainProgram.DoDebug("Translation text \"" + text + "\" doesn't exist in domain \"" + domain + "\"");
                    }
                } else {
                    MainProgram.DoDebug("Translation domain \"" + domain + "\" doesn't exist");
                }
            } else {
                MainProgram.DoDebug("Invalid translate json file");
            }
            
            if (fallbackLanguageJson != null && !doFallback) {
                MainProgram.DoDebug("Going to fallback translation");
                return __(text, domain, true);
            } else {
                return "Translation error";
            }
        }
    }
}
