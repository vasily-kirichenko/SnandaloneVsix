set MSBuild="%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

%MSBuild% StandaloneVsix\StandaloneVsix.csproj /t:Rebuild /p:Configuration=Release
%MSBuild% install_vsix.build /t:Install /p:VsixPath=%1 /p:VisualStudioVersion=%2 /p:RootSuffix=%3