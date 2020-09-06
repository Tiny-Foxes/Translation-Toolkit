using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TranslationToolKit
{
    /// <summary>
    /// The runner that allows to interact with the application.
    /// </summary>
    public class ConsoleRunner
    {
        public enum Commands
        {
            Quit,
            DuplicateCheck
        };

        /// <summary>
        /// Original color at the start of the application, so we can switch back to it
        /// when we quit, and don't mess up the user's console.
        /// </summary>
        public ConsoleColor OriginalColor;
        internal void Run()
        {
            OriginalColor = Console.ForegroundColor;
            try
            {
                Console.WriteLine("TranslationToolKit v0.1");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Welcome to the translator's toolkit for Outfox translation !");
                Console.ForegroundColor = OriginalColor;
                Console.WriteLine("");

                RunLoop();

                Console.ForegroundColor = OriginalColor;
                return;
            }
            finally
            {
                Console.ForegroundColor = OriginalColor;
            }
        }

        /// <summary>
        /// Run the main loop, that display the menu,
        /// then act on user's choice, until user decides to quit.
        /// </summary>
        private void RunLoop()
        {
            Commands command;
            do
            {
                command = DisplayMainMenu();

                switch (command)
                {
                    case Commands.DuplicateCheck:
                        {
                            break;
                        }
                }
            } while (command != Commands.Quit);
        }

        /// <summary>
        /// Display the main menu that allows the user to pick what they want to do.
        /// </summary>
        /// <returns></returns>
        private Commands DisplayMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine("What do you want to do ?");
            Console.WriteLine("1- Duplicate checker");
            Console.WriteLine("0- Quit");

            var parsed = false;
            var command = Commands.Quit;
            do
            {
                var value = Console.ReadLine();
                parsed = Enum.TryParse<Commands>(value, out command) && Enum.IsDefined(typeof(Commands), command);
            } while(!parsed);

            return command;
        }
    }
}
