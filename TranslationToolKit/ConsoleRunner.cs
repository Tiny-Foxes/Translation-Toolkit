using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TranslationToolKit.Business;

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

                try
                {
                    switch (command)
                    {
                        case Commands.DuplicateCheck:
                            {
                                DuplicatesCheck();
                                break;
                            }
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error while running command {command.ToString()}: {Environment.NewLine}{e}");
                    Console.ForegroundColor = OriginalColor;
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
            Console.WriteLine("1- Duplicates checker");
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

        /// <summary>
        /// Run the duplicates checker.
        /// </summary>
        private void DuplicatesCheck()
        {
            var checker = new DuplicatesChecker();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("=== Duplicates checker ===");
            Console.WriteLine("");
            string path;
            string error;
            do
            {
                Console.WriteLine("Please enter the path to the file you want to check");
                path = Console.ReadLine();
                if(!DuplicatesChecker.IsFileValid(path, out error))
                {
                    DisplayErrorIfAny(error);
                }
            } while (error != "");

            bool proceed = AskYesNoQuestion($"Do you want to run the duplicates checker on the following file{Environment.NewLine}{path}{Environment.NewLine}");
            if(proceed)
            {
                checker.RunAnalyzer(path);
            }
            return;
        }

        /// <summary>
        /// An helper method for all theses times where you need to get a Yes or No answer from the user.
        /// </summary>
        /// <param name="ask">The question to ask, don't both adding the "?", it will be done for you.</param>
        /// <returns>true or false, depending on answer</returns>
        private bool AskYesNoQuestion(string ask)
        {
            string answer;
            do
            {
                Console.WriteLine("");
                Console.WriteLine($"{ask} ? (Y/N)");
                answer = Console.ReadLine().ToUpper();
            } while (answer != "Y" 
                    && answer != "N"
                    && answer != "NO"
                    && answer != "YES");
            return answer.Equals("Y") || answer.Equals("YES");
        }

        /// <summary>
        /// Display an error, using red font, then go back to original font.
        /// </summary>
        /// <param name="error"></param>
        private void DisplayErrorIfAny(string error)
        {
            if (error != "")
            {
                var currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {error}");
                Console.ForegroundColor = currentColor;
            }
        }
    }
}
