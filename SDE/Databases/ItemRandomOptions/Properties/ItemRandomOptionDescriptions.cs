using System;
using System.Collections.Generic;
using System.IO;
using TokeiLibrary;

namespace SDE.Databases.ItemRandomOptions.Properties
{
    public static class ItemRandomOptionDescriptions
    {
        private static readonly Dictionary<string, string>
            _descriptions =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

        static ItemRandomOptionDescriptions()
        {
            byte[] data =
                ApplicationManager.GetResource(
                    "item_randomopt_description.txt");

            if (data == null)
                return;

            using (StreamReader reader =
                new StreamReader(
                    new MemoryStream(data)))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length == 0)
                        continue;

                    int equalIndex =
                        line.IndexOf('=');

                    if (equalIndex <= 0)
                        continue;

                    string key =
                        line.Substring(
                            0,
                            equalIndex).Trim();

                    string value =
                        line.Substring(
                            equalIndex + 1).Trim();

                    if (value.EndsWith(","))
                    {
                        value =
                            value.Substring(
                                0,
                                value.Length - 1).Trim();
                    }

                    if (value.Length >= 2 &&
                        value[0] == '"' &&
                        value[value.Length - 1] == '"')
                    {
                        value =
                            value.Substring(
                                1,
                                value.Length - 2);
                    }

                    _descriptions[key] = value;
                }
            }
        }

        public static string Get(
            string option)
        {
            if (String.IsNullOrEmpty(option))
                return "";

            if (_descriptions.TryGetValue(
                    option,
                    out string description))
            {
                return description;
            }

            return "";
        }
    }
}