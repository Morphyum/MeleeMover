﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace MeleeMover {
 public class Helper {

        public static Settings LoadSettings() {
            try {
                using (StreamReader r = new StreamReader($"{MeleeMover.ModDirectory}/settings.json")) {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
                return null;
            }
        }
    }
}