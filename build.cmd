set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

%MSBuild% StandaloneVsix\StandaloneVsix.csproj /t:Rebuild /p:Configuration=Release