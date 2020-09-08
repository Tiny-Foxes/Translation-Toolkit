# Translation-Toolkit

This project is intended as a small application to automate some tasks for translators.

It is able to parse the StepMania translation files, and then analyze or manipulate them.

## Features ##

**Duplicates checker**: Parse a translation file, and determine whether there are sections or lines that are duplicates. If so, the user can also for a new file to be generated without the duplicates.

## How to use ##

A pre-release for Windows has been published on the github. Simply launch TranslationToolKit.exe and it should open the console application.

## How to compile ##

This project was written in C# and has been made entirely with Visual Studio Community 2019 (free for open-source contributors) and .NET Core 3.7.1. 

You can therefore compile it using only free tools, and it should work on both Windows and Linux (untested as of yet). You will only need the .NET Core 3.7.1 SDK and a C# IDE.

You can create a new release for Windows with

> dotnet publish -r win-x64 -c Release
