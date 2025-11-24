using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            string messege = "Error";

            Pathfinder pathfinder = new Pathfinder();

            ConsoleLogWritter consoleLogWritter = new ConsoleLogWritter();
            FileLogWritter fileLogWritter = new FileLogWritter();
            SecureConsoleLogWritter secureConsoleLogWritter = new SecureConsoleLogWritter();
            SecureFileLogWritter secureFileLogWritter = new SecureFileLogWritter();
            SecureFileConsoleLogWritter secureFileConsoleLogWritter = new SecureFileConsoleLogWritter();

            pathfinder.Find(consoleLogWritter, messege);
            pathfinder.Find(fileLogWritter, messege);
            pathfinder.Find(secureConsoleLogWritter, messege);
            pathfinder.Find(secureFileLogWritter, messege);
            pathfinder.Find(secureFileConsoleLogWritter, messege);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            if(message == null)
                throw new ArgumentNullException();

            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            if (message == null)
                throw new ArgumentNullException();

            File.WriteAllText("log.txt", message);
        }
    }

    class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                base.WriteError(message);
        }
    }

    class SecureFileLogWritter : FileLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                base.WriteError(message);
        }
    }

    class SecureFileConsoleLogWritter : SecureFileLogWritter
    {
        public override void WriteError(string message)
        {
            
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
            else
            {
                if (message == null)
                    throw new ArgumentNullException();

                Console.WriteLine(message);
            }
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
                throw ArgumentNullException();

            if (messege == null)
                throw new ArgumentNullException();

            logger.WriteError(messege);
        }
    }
}