# tickerObserver
Be sure that you have installed .Net Core SDK 2.1

run next command:

dotnet tool install --global dotnet-ef

to install ef tool globally

restore all packages

then

run on ~/TickerObserver/TickerObserver.DataAccess folder next command:
dotnet ef database update

Set as copy always for file tickerObserver.db in ~/TickerObserver/TickerObserver.DataAccess, f
ile don't must be empty e.g. 0 bytes,
correct file ~20kb 

Run application!
