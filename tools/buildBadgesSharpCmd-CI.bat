echo Building BadgesSharpCmd...

mkdir C:\projects\badgessharp\build\

cd .\src\BadgesSharpCmd\bin\Release

C:\projects\badgessharp\tools\ILRepack.exe /target:exe /targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /out:.C:\projects\badgessharp\build\BadgesSharpCmd.exe /wildcards BadgesSharpCmd.exe *.dll

cd C:\projects\badgessharp\
