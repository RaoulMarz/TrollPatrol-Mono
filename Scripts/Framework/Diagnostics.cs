using Godot;
using System.Text;
using System.Collections.Generic;

namespace TrollSmasher.Framework
{
    public class Diagnostics
    {
        public static void PrintNullValueMessage(object obj, string objectName)
        {
            if (obj == null)
            {
                GD.Print($"{objectName} is null!");
            }
        }

        public static Dictionary<string, string> GetProperties(object obj)
        {
            var props = new Dictionary<string, string>();
            if (obj == null)
                return props;

            var type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var val = prop.GetValue(obj, new object[] { });
                var valStr = val == null ? "" : val.ToString();
                props.Add(prop.Name, valStr);
            }

            return props;
        }

        public static void PrintStringDictionary(string tag, Dictionary<string, string> stringValuesMap)
        {
            GD.Print($"{tag}, Dictionary = \n");
            StringBuilder sb = new StringBuilder();
            foreach (string xkey in stringValuesMap.Keys)
            {
                sb.Append(xkey);
                sb.Append(stringValuesMap[xkey]);
                sb.Append("\n");
            }
            GD.Print($"{sb.ToString()}");
        }

        public static void PrintChildrenList(string tag, Node parentNode)
        {
            if ( (parentNode != null) && (parentNode.GetChildCount() > 0) )
            {
                GD.Print($"{tag} #children =  {parentNode.GetChildCount()}");
                StringBuilder sb = new StringBuilder();
                foreach (Node childX in parentNode.GetChildren())
                {
                    sb.Append(childX.Name);
                    sb.Append("\n");
                }
                GD.Print($"[ {tag} ] children = {sb.ToString()}");
            }
        }

        public static void PrintObjectProperties(string tag, object obj)
        {
            var props = GetProperties(obj);
            if (props.Count > 0)
            {
                GD.Print($"{tag} #properties =  {props.Count}");

                StringBuilder sb = new StringBuilder();
                foreach (var prop in props)
                {
                    sb.Clear();
                    sb.Append(prop.Key);
                    sb.Append(": ");
                    sb.Append(prop.Value);
                    GD.Print($"[ {tag} ] property = {sb.ToString()}");
                }
            }
        }
    }
}
