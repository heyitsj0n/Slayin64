@echo off
set U="C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe"
set P=C:\Users\jonal\Downloads\SlayinPort
set SLAYIN_VARIANT=unlimited
%U% -batchmode -nographics -projectPath %P% -executeMethod SlayinBuild.Android -quit -logFile %P%\unity_build_unlimited.log
echo BUILD exit %errorlevel%
