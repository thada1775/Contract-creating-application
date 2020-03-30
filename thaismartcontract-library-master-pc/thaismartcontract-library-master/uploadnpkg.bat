@echo off
dotnet build
dotnet pack  --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.API
dotnet pack  --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.Storage
dotnet pack  --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.CPMO
dotnet pack  --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.WalletService
rem nuget delete -ApiKey rjking -Source http://proget.zarimpun.com/nuget/thaismartcontract/ Thaismartcontract.API %date:~10,4%.%date:~4,2%.%date:~7,2% -Noprompt
nuget push -ApiKey DlAsw7qVDOo-ph-g05y5 -Source https://proget.zarimpun.com/nuget/thaismartcontract/ Thaismartcontract.API\bin\Debug\*.nupkg 
nuget push -ApiKey DlAsw7qVDOo-ph-g05y5 -Source https://proget.zarimpun.com/nuget/thaismartcontract/ Thaismartcontract.Storage\bin\Debug\*.nupkg 
nuget push -ApiKey DlAsw7qVDOo-ph-g05y5 -Source https://proget.zarimpun.com/nuget/thaismartcontract/ Thaismartcontract.CPMO\bin\Debug\*.nupkg 
nuget push -ApiKey DlAsw7qVDOo-ph-g05y5 -Source https://proget.zarimpun.com/nuget/thaismartcontract/ Thaismartcontract.WalletService\bin\Debug\*.nupkg