@echo off
dotnet build
sleet destroy
sleet init
dotnet pack --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.API
sleet push Thaismartcontract.API\bin\Debug\
dotnet pack --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.Storage
sleet push Thaismartcontract.Storage\bin\Debug\
dotnet pack --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.WalletService
sleet push Thaismartcontract.WalletService\bin\Debug\
dotnet pack --include-symbols -p:Version=%date:~10,4%.%date:~4,2%.%date:~7,2% Thaismartcontract.CPMO
sleet push Thaismartcontract.CPMO\bin\Debug\
copy /y index.html feed
pscp -agent -r -C -i "D:\Google Drives\RMUTSB Computer\RMUTSB Documents\ooh.ppk"  feed\* root@10.8.1.1:/var/www/nuget



