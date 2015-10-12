echo Building BadgesSharpCmd...
mkdir ..\build
cd ..\src\BadgesSharpCmd\bin\Release
..\..\..\..\tools\ILRepack.exe /target:exe /targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5" /out:..\..\..\..\build\BadgesSharpCmd.exe /wildcards BadgesSharpCmd.exe *.dll
cd ..\..\..\..\tools

echo done!
pause
