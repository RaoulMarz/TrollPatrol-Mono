using Godot;
using System;
using System.Collections.Generic;

namespace TrollPatrolMono.Framework
{
    public static class InputAssistance
    {
        public static Dictionary<string, DateTime> keyBounceMap = new Dictionary<string, DateTime>();
        public static Dictionary<string, DateTime> keyActivateMap = new Dictionary<string, DateTime>();

        public static bool verboseFlag = false;

        public static void SetVerbose(bool verbose)
        {
            verboseFlag = verbose;
        }


        public static bool KeyBounceCheck(string key, float bounceIgnore, float skipSeconds)
        {
            bool res = true;
            if ((key != null) && (keyActivateMap.ContainsKey(key)))
            {
                DateTime keyTimestamp = keyActivateMap[key];
                if (keyTimestamp.Year < 2011)
                    return res;

                var diff = DateTime.Now - keyTimestamp;
                if (verboseFlag)
                {
                    GD.Print($"KeyBounceCheck, keyActivateMap, key = {key} timestamp = {keyTimestamp}");
                    GD.Print($"KeyBounceCheck, keyActivateMap milliseconds diff = {diff.TotalMilliseconds}");
                }
                if (diff.TotalMilliseconds < (skipSeconds * 1000.0f))
                {
                    //keyBounceMap.Remove(key);
                    //keyBounceMap.Add(key, DateTime.Now);
                    res = false;
                    return res;
                }
                else
                {
                    GD.Print($"KeyBounceCheck, skip triggered, key = {key} timestamp = {keyTimestamp}");
                    keyBounceMap.Remove(key);
                    keyActivateMap.Remove(key);
                    keyActivateMap.Add(key, DateTime.Now);
                    return res;
                }
            }
            if ((key != null) && (bounceIgnore >= 0.1))
            {
                if (keyBounceMap.ContainsKey(key))
                {
                    DateTime keyTimestamp = keyBounceMap[key];
                    var diff = DateTime.Now - keyTimestamp;
                    GD.Print($"KeyBounceCheck, milliseconds diff = {diff.TotalMilliseconds}");
                    if (diff.TotalMilliseconds >= (bounceIgnore * 1000.0f))
                    {
                        if (diff.TotalMilliseconds >= (2 * bounceIgnore * 1000.0f))
                            keyBounceMap.Remove(key);
                        res = false;
                    }
                }
                else
                {
                    keyBounceMap.Add(key, DateTime.Now);
                    keyActivateMap.Add(key, DateTime.Now);
                }
            }
            return res;
        }

    }

}
