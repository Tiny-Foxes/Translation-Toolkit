using System;
using System.Reflection;
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
            DuplicateCheck,
            ApplyChanges,
            CountLines
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
                Console.WriteLine($"TranslationToolKit {GetAssemblyVersion()}");
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
        /// Get the assembly version.
        /// </summary>
        /// <returns></returns>
        private static string GetAssemblyVersion()
        {
            var versionString = "0";
            var version = Assembly.GetExecutingAssembly()?.GetName()?.Version;
            if(version != null)
            {
                versionString = $"v{version.Major}.{version.Minor}.{version.Build}";
            }
            
            return versionString;
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
                        case Commands.ApplyChanges:
                        {
                            ApplyChanges();
                            break;
                        }
                        case Commands.CountLines:
                        {
                            CountLines();
                            break;
                        }
                    }
                }
                catch(AbortException)
                {
                    Console.WriteLine("E"); // I don't know why pressing escape eats one character, so I'm adding a dummy one.
                    ConsoleWrite("Command aborted, going back to main menu", ConsoleColor.Yellow);
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
            ConsoleWrite("", ConsoleColor.Cyan);
            ConsoleWrite("What do you want to do ?", ConsoleColor.Cyan);
            ConsoleWrite("1- Duplicates checker", ConsoleColor.Cyan);
            ConsoleWrite("2- Synchronize changes from a reference file to a target translation file", ConsoleColor.Cyan);
            ConsoleWrite("3- Count the number of lines that you have translated versus what remains to be done", ConsoleColor.Cyan);
            ConsoleWrite("0- Quit", ConsoleColor.Cyan);
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

            var path = GetFilePathAndVerify("Please enter the path to the file you want to check");

            bool proceed = AskYesNoQuestion($"Do you want to run the duplicates checker on the following file{Environment.NewLine}{path}{Environment.NewLine}", ConsoleColor.Cyan);
            Console.ForegroundColor = OriginalColor;
            if (proceed)
            {
                var report = checker.RunAnalyzer(path);
                Console.WriteLine(report.GetDisplayString());

                if(report.DuplicatedLines.Count == 0 && report.DuplicatedSections.Count == 0)
                {
                    return;
                }

                proceed = AskYesNoQuestion($"Duplicate(s) found. Do you want to create a new file{Environment.NewLine}({report.FilePath}.generated){Environment.NewLine}without the duplicates", ConsoleColor.Yellow);
                if(proceed)
                {
                    try
                    {
                        var resultPath = checker.RemoveDuplicates();
                        if (resultPath != "")
                        {
                            ConsoleWrite($"File successfully written at {resultPath}", ConsoleColor.Green);
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
        /// Run the changes applier.
        /// </summary>
        private void ApplyChanges()
        {
            var checker = new FileSynchronizer();

            ConsoleWrite($"{Environment.NewLine}=== File Synchronizer ==={Environment.NewLine}", ConsoleColor.Green);
            ConsoleWrite($"This tool will compare a reference file (ENglish translation probably) with a target file.{Environment.NewLine}You MUST have run the Duplicates checker on BOTH files before running this tool, otherwise it won't perform properly{Environment.NewLine}", ConsoleColor.Yellow);

            var referencePath = GetFilePathAndVerify("Please enter the path to the reference file you want to use");
            var targetPath = GetFilePathAndVerify("Please enter the path to the translation file that you want to check");

            bool proceed = AskYesNoQuestion($"Do you want to compare the reference file with the target file{Environment.NewLine}- Reference file: {referencePath}{Environment.NewLine}- Target file: {targetPath}{Environment.NewLine}", ConsoleColor.Cyan);
            Console.ForegroundColor = OriginalColor;
            if (proceed)
            {
                var report = checker.RunAnalyzer(referencePath, targetPath);
                Console.WriteLine(report.GetDisplayString());

                if (!report.IssuesFound)
                {
                    return;
                }

                proceed = AskYesNoQuestion($"Difference(s) found. Do you want to create a new file{Environment.NewLine}({report.TargetPath}.generated){Environment.NewLine}in sync with the reference file", ConsoleColor.Yellow);
                if (proceed)
                {
                    try
                    {
                        var resultPath = checker.SynchronizeFile();
                        if (resultPath != "")
                        {
                            ConsoleWrite($"File successfully written at {resultPath}", ConsoleColor.Green);
                        }
                        else
                        {
                            ConsoleWrite("Unspecified error while trying to write file", ConsoleColor.Red);
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleWrite($"Error while trying to create new file: {e}", ConsoleColor.Red);
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Lines counter.
        /// </summary>
        private void CountLines()
        {
            var checker = new LinesCounter();

            ConsoleWrite($"{Environment.NewLine}=== Lines Counter ==={Environment.NewLine}", ConsoleColor.Green);
            ConsoleWrite($"This tool is here to help you feel better about your translation work:{Environment.NewLine}It counts the number of lines in a reference file, then compares them with a target file, and tells you how many lines you've already translated.{Environment.NewLine}Note that this is not a serious tool as it only can only count which lines are different between the two files, and can't know which lines are left untranslated/identical on purpose.{Environment.NewLine}Still, I find it's good for motivation to see numbers go up as you keep on translating lines.{Environment.NewLine}", ConsoleColor.Magenta);
            ConsoleWrite($"You MUST have run the Duplicates checker on BOTH files before running this tool, otherwise it won't perform properly{Environment.NewLine}", ConsoleColor.Yellow);

            var referencePath = GetFilePathAndVerify("Please enter the path to the reference file you want to use");
            var targetPath = GetFilePathAndVerify("Please enter the path to the translation file that you want to check");

            Console.WriteLine();
            var report = checker.RunAnalyzer(referencePath, targetPath);
            ConsoleWrite(report.GetDisplayString(), ConsoleColor.Cyan);
            ConsoleWrite($">>>> You've already translated {report.Percentage}% of lines! <<<<", ConsoleColor.Green);
            Console.WriteLine();
            ConsoleWrite($"Note: lines whose translation are identical to the reference lines sadly can't be counted as translated.", ConsoleColor.Yellow);
            ConsoleWrite($"But that's good news, because it means your true percentage is likely higher!", ConsoleColor.Yellow);
            ConsoleWrite($"Thanks for helping with the translation, I hope this tool will help you see your progress and keep your motivation :)", ConsoleColor.Yellow);
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
                Console.WriteLine($"{ask} ? Y/N (or press escape to return to main menu)");
                answer = CaptureConsoleInputLine().ToUpper();
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

        /// <summary>
        /// Write a line in the console, in the specified color, but make sure to re-establish the default color afterwards.
        /// </summary>
        /// <param name="lineToBeWritten"></param>
        /// <param name="color"></param>
        private void ConsoleWrite(string lineToBeWritten, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(lineToBeWritten);
            Console.ForegroundColor = OriginalColor;
        }

        /// <summary>
        /// Capture the input from the console, 
        /// Returns the value when Enter is pressed,
        /// Throws an exception that send us back to the main menu if the Escape key is pressed.
        /// </summary>
        /// <returns></returns>
        private string CaptureConsoleInputLine()
        {
            ConsoleKeyInfo key;
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                throw new AbortException();
            }            
            return key.KeyChar + Console.ReadLine();
        }

        /// <summary>
        /// Get the file path from a user, and verify it's a valid path.
        /// </summary>
        /// <returns></returns>
        private string GetFilePathAndVerify(string message, ConsoleColor color = ConsoleColor.Cyan)
        {
            string error;
            string path;
            do
            {
                ConsoleWrite($"{message} (or press escape to return to main menu)", color);
                path = CaptureConsoleInputLine();
                if (!ValidationHelper.IsFileValid(path, out error))
                {
                    DisplayErrorIfAny(error);
                }
            } while (error != "");
            return path;
        }
    }
}
