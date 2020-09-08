if not exist ".\Publish\Linux\NUL" mkdir ".\Publish\Linux"

if not exist ".\Publish\Win\NUL" mkdir ".\Publish\Win"

dotnet publish TranslationToolKit.sln -r win-x64 -c Release -o ".\Publish\Win" -p:PublishSingleFile=true

dotnet publish TranslationToolKit.sln -r linux-x64 -c Release -o ".\Publish\Linux" -p:PublishSingleFile=true