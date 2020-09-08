# Translation-Toolkit

This project is intended as a small application to automate some tasks for translators.

It is able to parse, analyze and fix the StepMania translation files.

## Features ##

* **Duplicates checker**: Parse a translation file, and determine whether there are sections or lines that are duplicates. If so, the user can also for a new file to be generated without the duplicates.

## How to use ##

Releases for Windows/Linux/Mac are provided on github.

Releases properties:
* Self-contained mode, so you **do not need to install .NET Core** to run the application
* SingleFileMode, so they generate a single file (+ its associated debug symbols in the .pdb) with everything included in it.

Simply launch TranslationToolKit.exe (on Windows) or TranslationToolKit (on Linux/MacOS) and it should open the console application. 


## How to compile ##

This project was written in C# and has been made entirely with Visual Studio Community 2019 (free for open-source contributors) and .NET Core 3.7.1. You can therefore compile it using only free tools, and it should work on Windows/Linux/MacOS. You will only need the .NET Core 3.7.1 SDK and a C# IDE.

To build, just build the solution (sln) with your favorite IDE, or use the dotnet command line tool.

### Notes for developers ###

All the csproj have the nullable set to Disable. This is a new C# 8 feature that means that, by default, you can't assign null to a reference type. This means you don't have to burden your code with constant null checks. However it only works properly if all projects (test projects excluded) have this feature, because a library with nullable enabled can pass a null to a libray with nullable disabled. So if you have to create a new csproj, **make sure to disable nullable** in the new csproj.

### How to publish a new version ###

To publish a new release, you can just run Publish.bat, which create new Windows/Linux/Mac executables in the Publish directory (Publish directory is deleted before any publish, to ensure you're not using older publish).

You can check the content of the file to see how this is built:

> dotnet publish ".\TranslationToolKit\TranslationToolKit.csproj" -r win-x64 -c Release -o ".\Publish\Win" -p:PublishSingleFile=true

A few notes:
* While, during compiling, we build a solution; when we're publishing, we only publish one csproj. We're only publishing the csproj that will generate the exe, because doing a dotnet publish on the libraries just trigger an error.
* -r defines the target platform
* -c defines the configuration (check the properties of your csproj)
* PublishingSingleFile means all the dlls are packaged into one single executable file (+ its associated pdb), for a cleaner looking install/ease of use.