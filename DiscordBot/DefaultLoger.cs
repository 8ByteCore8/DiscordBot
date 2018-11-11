﻿using DiscordBot.Common;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    /// <summary>
    /// Логер по умолчанию
    /// </summary>
    internal class DefaultLoger : IDisposable, ILoger
    {
        /// <summary>
        /// Создаёт новый обьект класса.
        /// </summary>
        public DefaultLoger() => Init();

        ~DefaultLoger()
        {
            Dispose();
        }

        /// <summary>
        /// Поток файла
        /// </summary>
        private FileStream File { get; set; }

        /// <summary>
        /// Помощник записи в поток.
        /// </summary>
        private StreamWriter LogWriter { get; set; }

        public void Dispose()
        {
            LogWriter.Dispose();
            File.Dispose();
        }

        public void Init()
        {
            //путь к папке с логами
            string logDir = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Logs";

            //проверка наличия папки, если нету создаём
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            //открытие потока для записи логов
            File = new FileStream($"{logDir}\\{DateTime.Now.ToShortDateString()}.log", FileMode.Create);
            //создание помощника для записи в файл
            LogWriter = new StreamWriter(File);
        }

        public void Log(string LogText)
        {
            DateTime time = DateTime.Now;
            string log = $"[{time.ToShortDateString()} | {time.ToShortTimeString()}] --- {LogText};";
            Console.WriteLine(log);
            LogWriter.WriteLine(log);
        }

        public Task LogAsync(string LogText)
        {
            return Task.Factory.StartNew(() =>
            {
                DateTime time = DateTime.Now;
                string log = $"[{time.ToShortDateString()} | {time.ToShortTimeString()}] --- {LogText}";
                Console.WriteLine(log);
                LogWriter.WriteLine(log);
            });
        }
    }
}