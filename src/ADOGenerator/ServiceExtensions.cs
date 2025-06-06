﻿using ADOGenerator.Models;

namespace ADOGenerator
{
    //my first change
    public static class ServiceExtensions
    {
        public static readonly object objLock = new object();

        public static string ReadJsonFile(this Project file, string filePath)
        {
            string fileContents = string.Empty;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    fileContents = sr.ReadToEnd();
                }
            }

            return fileContents;
        }

        public static string ErrorId(this string str)
        {
            str = str + "_Errors";
            return str;
        }
        public static string WarningId(this string str)
        {
            str = str + "_Warning";
            return str;
        }

        public static void AddMessage(this string id, string message)
        {
            lock (objLock)
            {
                // Create Log floder
                if (!Directory.Exists("Log"))
                {
                    Directory.CreateDirectory("Log");
                }
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd")}-{id}.txt";
                if (id.EndsWith("_Errors"))
                {
                    if (!Directory.Exists(Path.Combine(logFilePath, "Errors")))
                    {
                        Directory.CreateDirectory(Path.Combine(logFilePath, "Errors"));
                    }
                    // Create Log file
                    if (!File.Exists(Path.Combine(logFilePath, "Errors", fileName)))
                    {
                        File.Create(Path.Combine(logFilePath, "Errors", fileName)).Dispose();
                    }
                    File.AppendAllLines(Path.Combine(logFilePath, "Errors", fileName), new string[] { message });
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
                else
                {
                    if (!File.Exists(Path.Combine(logFilePath, fileName)))
                    {
                        File.Create(Path.Combine(logFilePath, fileName)).Dispose();
                    }
                    File.AppendAllLines(Path.Combine(logFilePath, fileName), new string[] { message });
                    // Create Log file
                    Console.ForegroundColor = id.EndsWith("_Warning") ? ConsoleColor.Yellow : ConsoleColor.Green;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
            }
        }

    }
}
