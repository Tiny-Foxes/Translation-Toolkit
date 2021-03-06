rmdir /s /q ".\Publish"

if not exist ".\Publish\Linux\NUL" mkdir ".\Publish\Linux"

if not exist ".\Publish\Win\NUL" mkdir ".\Publish\Win"

if not exist ".\Publish\Mac\NUL" mkdir ".\Publish\Mac"

dotnet publish ".\TranslationToolKit\TranslationToolKit.csproj" -r osx-x64 -c Release -o ".\Publish\Mac" -p:PublishSingleFile=true

dotnet publish ".\TranslationToolKit\TranslationToolKit.csproj" -r win-x64 -c Release -o ".\Publish\Win" -p:PublishSingleFile=true

dotnet publish ".\TranslationToolKit\TranslationToolKit.csproj" -r linux-x64 -c Release -o ".\Publish\Linux" -p:PublishSingleFile=true