﻿using System;
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
                ConsoleWrite("Welcome to the translator's toolkit for Outfox translation !", ConsoleColor.Green);

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
                    ConsoleWrite($"Error while running command {command}: {Environment.NewLine}{e}", ConsoleColor.Red);
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
            bool parsed;
            Commands command;
            do
            {
                var value = Console.ReadLine();
                parsed = Enum.TryParse<Commands>(value, out command) && Enum.IsDefined(typeof(Commands), command);
            } while (!parsed);

            return command;
        }

        /// <summary>
        /// Run the duplicates checker.
        /// </summary>
        private void DuplicatesCheck()
        {
            var checker = new DuplicatesChecker();

            ConsoleWrite($"{Environment.NewLine}=== Duplicates checker ==={Environment.NewLine}", ConsoleColor.Green);

            string path;
            string error;
            do
            {
                ConsoleWrite("Please enter the path to the file you want to check", ConsoleColor.Cyan);
                path = Console.ReadLine();
                if(!DuplicatesChecker.IsFileValid(path, out error))
                {
                    DisplayErrorIfAny(error);
                }
            } while (error != "");

            bool proceed = AskYesNoQuestion($"Do you want to run the duplicates checker on the following file{Environment.NewLine}{path}{Environment.NewLine}", ConsoleColor.Cyan);
            Console.ForegroundColor = OriginalColor;
            if (proceed)
            {
                var report = checker.RunAnalyzer(path);
                Console.WriteLine(report.GetDisplayString());
                proceed = AskYesNoQuestion($"Duplicate found. Do you want to create a new file{Environment.NewLine}({report.FilePath}.generated){Environment.NewLine}without the duplicates", ConsoleColor.Yellow);
                if(proceed)
                {
                    try
                    {
                        var resultPath = checker.RemoveDuplicates();
                        if (resultPath != "")
                        {
                            ConsoleWrite($"File successful written at {resultPath}", ConsoleColor.Green);
                        }
                        else
                        {
                            ConsoleWrite("Unspecified error while trying to write file", ConsoleColor.Red);
                        }
                    }
                    catch(Exception e)
                    {
                        ConsoleWrite($"Error while trying to create new file: {e}", ConsoleColor.Red);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// An helper method for all theses times where you need to get a Yes or No answer from the user.
        /// </summary>
        /// <param name="ask">The question to ask, don't both adding the "?", it will be done for you.</param>
        /// <returns>true or false, depending on answer</returns>
        private bool AskYesNoQuestion(string ask, ConsoleColor color)
        {
            Console.ForegroundColor = color;
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
            Console.ForegroundColor = OriginalColor;
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

        private void ConsoleWrite(string lineToBeWritten, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(lineToBeWritten);
            Console.ForegroundColor = OriginalColor;
        }
    }
}
