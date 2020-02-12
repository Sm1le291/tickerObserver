# tickerObserver
Be sure that you have installed .Net Core SDK 2.1

run next command:

dotnet tool install --global dotnet-ef

to install ef tool globally

restore all packages

then

run on ~/TickerObserver/TickerObserver.DataAccess folder next command:
dotnet ef database update

Set as copy always for file tickerObserver.db in ~/TickerObserver/TickerObserver.DataAccess, 
file does't must be empty e.g. 0 bytes,
correct file ~20kb 

If something goes wrong with db and you show exception related to db,
copy manually tickerObserver.db to ~/TickerObserver/TickerObserver/bin/Debug

Run application!
