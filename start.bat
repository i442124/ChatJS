docker run^
 --rm -i^
 -p 8000:80^
 -p 8001:443^
 --name="chatjs-demo"^
 -e ASPNETCORE_URLS="https://+;http://+"^
 -e ASPNETCORE_HTTPS_PORT=8001^
 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password"^
 -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx^
 -v %USERPROFILE%\.aspnet\https:/https/ chatjs-demo