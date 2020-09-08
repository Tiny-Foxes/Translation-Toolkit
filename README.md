# Translation-Toolkit

This project is intended as a small application to automate some tasks for translators.
It is able to parse, analyze and fix the StepMania translation files.

## Features ##

**Duplicates checker**: Parse a translation file, and determine whether there are sections or lines that are duplicates. If so, the user can also for a new file to be generated without the duplicates.

## How to use ##

A pre-release for Windows and Linux has been published on the github.

Releases properties:
* Self-contained mode, so you **do not need to install .NET Core** to run the application
* SingleFileMode, so they generate a single file (+ its associated debug symbols in the .pdb) with everything included in it.

Simply launch TranslationToolKit.exe (on Windows) or TranslationToolKit (on Linux) and it should open the console application. 


## How to compile ##

This project was written in C# and has been made entirely with Visual Studio Community 2019 (free for open-source contributors) and .NET Core 3.7.1. 

You can therefore compile it using only free tools, and it should work on both Windows and Linux (untested as of yet). You will only need the .NET Core 3.7.1 SDK and a C# IDE.

To create a build, you can just run Publish.bat, or look into this files to see how they are done.

> dotnet publish TranslationToolKit.sln -r win-x64 -c Release -o ".\Publish\Win" -p:PublishSingleFile=true