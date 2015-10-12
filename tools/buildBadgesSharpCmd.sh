mkdir ../build
cd ../src/BadgesSharpCmd/bin/Release
/Library/Frameworks/Mono.framework/Versions/4.0.4/bin/mono ../../../../tools/ILRepack.exe /target:exe /out:../../../../build/BadgesSharpCmd.exe /wildcards BadgesSharpCmd.exe *.dll
cd ../../../../tools