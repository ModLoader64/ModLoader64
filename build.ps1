cd ./ImGui.NET
cd ./src
./build.ps1 --emulatordll mupen64plus
cd ..
cd ..
cd ModLoader
./build.ps1
cd ..
python ./build.py
dotnet run --project Packaging/Build.csproj