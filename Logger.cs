using System;
using System.Collections.Generic;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            string messege = "Error";
            string path = "file.txt";

            DayOfWeek messegeDay = DayOfWeek.Friday;

            Pathfinder pathfinder = new Pathfinder();

            ILogger consoleLogWritter = new ConsoleLogger();
            ILogger fileLogWritter = new FileLogger(path);
            ILogger secureConsoleLogWritter = new SecureLogWritter(new ConsoleLogger(), messegeDay);
            ILogger secureFileLogWritter = new SecureLogWritter(new FileLogger(path), messegeDay);
            Logger compositeLogger = new Logger(CompositeLogger.Create(new ConsoleLogger(),new SecureLogWritter(new FileLogger(path), messegeDay)));

            pathfinder.Find(consoleLogWritter, messege);
            pathfinder.Find(fileLogWritter, messege);
            pathfinder.Find(secureConsoleLogWritter, messege);
            pathfinder.Find(secureFileLogWritter, messege);
            pathfinder.Find(compositeLogger, messege);
        }
    }
    class Logger : ILogger
    {
        private ILogger _iLogger;

        public Logger(ILogger iLogger)
        {
            _iLogger = iLogger;
        }

        public void WriteError(string message)
        {
            if (message == null)
                throw new ArgumentNullException();

            _iLogger.WriteError(message);
        }
    }

    class FileLogger : ILogger
    {
        private string _path;

        public FileLogger(string path)
        {
            _path = path;
        }

        public void WriteError(string message)
        {
            if (message == null)
                throw new ArgumentNullException();

            File.WriteAllText(_path, message);
        }
    }

    class ConsoleLogger : ILogger
    {
        public void WriteError(string message)
        {
            if (message == null)
                throw new ArgumentNullException();

            Console.WriteLine(message);
        }
    }

    class SecureLogWritter : ILogger
    {
        private ILogger _iLogger;
        private DayOfWeek _messegeDay;

        public SecureLogWritter(ILogger iLogger, DayOfWeek messegeDay)
        {
            _iLogger = iLogger;
            _messegeDay = messegeDay;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _messegeDay)
                _iLogger.WriteError(message);
        }
    }

    class CompositeLogger : ILogger
    {
        private IEnumerable<ILogger> _iLoggers;

        public CompositeLogger(IEnumerable<ILogger> iLoggers)
        {
            _iLoggers = iLoggers;
        }

        public void WriteError(string message)
        {
            foreach(ILogger logger in _iLoggers)
                logger.WriteError(message);
        }

        public static CompositeLogger Create(params ILogger[] loggers)
        {
            return new CompositeLogger(loggers);
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder
    {
        public void Find(ILogger logger, string messege)
        {
            if (logger == null)
                throw new ArgumentNullException();

            if (messege == null)
                throw new ArgumentNullException();

            logger.WriteError(messege);
        }
    }
}