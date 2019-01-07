﻿using System;
using System.Linq;

namespace ContractNet
{
    public class Utils
    {
        public static int defaultMaxWorkload = 10;

        public static int Delay = 100;
        public static int NoOfProcessorAgent = 1;
        public static int NoOfHelperAgents = 1;

        public static int randomMinWorkload = 5;
        public static int randomMaxWorkload = 15;

        public static Random RandNoGen = new Random();

        public static bool EventOccurs(float probability)
        {
            return RandNoGen.NextDouble() <= probability;
        }

        public static void ParseMessage(string content, out string action, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = "";
            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }

        public static void ParseMessageWithActionTaskname(string content, out string action, out string taskName, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];
            taskName = t[1];

            parameters = "";
            if (t.Length > 1)
            {
                for (int i = 2; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }

        public static void ParseMessageTasksManagement(string content, out string result, out string taskName, out string noOfSubtasks, out string parameters)
        {
            string[] t = content.Split();

            result = t[0];
            taskName = t[1];
            noOfSubtasks = t[2];

            parameters = "";
            if (t.Length > 1)
            {
                for (int i = 3; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }

        public static string Str(string p1, double p2)
        {
            return string.Format("{0} {1:F1}", p1, p2);
        }

        public static int[] ParseStringToArray(string inputData)
        {
            int[] inputArray = inputData.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            return inputArray;
        }

        public static string ParseArrayToString(int[] inputData)
        {
            return string.Join(",", inputData);
        }



    }
}
