%windir%\system32\inetsrv\appcmd stop apppool /apppool.name:DamenDWS_OutboxSender 
dotnet publish --framework netcoreapp1.0 --output c:\inetpub\DamenDWS\Services\OutboxSender\ --configuration Debug
%windir%\system32\inetsrv\appcmd start apppool /apppool.name:DamenDWS_OutboxSender 