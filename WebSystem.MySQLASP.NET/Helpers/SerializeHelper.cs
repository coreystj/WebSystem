using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebSystem.MySQLASP.NET.Helpers
{
    public static class SerializeHelper
    {
        public static void ParseCoords(string rawData, out int[] xValues, out int[] yValues)
        {
            string[] rawCoords = rawData.Split('|');
            var xValuesList = new List<int>();
            var yValuesList = new List<int>();
            string[] segments;

            foreach (string rawCoord in rawCoords)
            {
                segments = rawCoord.Split(':');
                xValuesList.Add(int.Parse(segments[0]));
                yValuesList.Add(int.Parse(segments[1]));
            }

            xValues = xValuesList.ToArray();
            yValues = yValuesList.ToArray();
        }

        public static void SerializeCoords(int[] xValues, int[] yValues)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < xValues.Length; i++)
            {
                result.Append(xValues + ":" + yValues + 
                    ((i < xValues.Length -1)? "|" : string.Empty));
            }
        }
    }
}