rm bin/Release/*.nupkg
dotnet pack -c Release
dotnet nuget push -k $(cat ~/.keys/nuget) -s https://api.nuget.org/v3/index.json bin/Release/Ninjax.UdmLib.*.nupkg
