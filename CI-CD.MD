```markdown

\# CI (build) for ywBookStore



What this does

\- Restores NuGet packages for the solution `ywBookStore.sln`.

\- Builds the solution in Release configuration using MSBuild on Windows.

\- Collects any project outputs located in bin\\Release folders, zips them, and uploads a build artifact named `ywbookstore-build`.



Why this shape

\- The solution references projects at:

&nbsp; - assignment 1\\ywBookStoreGUI.csproj

&nbsp; - YWUnitTest\\ywBookStoreLIB.csproj

\- Those project layouts are typical of classic .NET / Visual Studio projects. The workflow uses nuget.exe + msbuild which works for classic .NET Framework projects and also works for many SDK-style projects on the Windows runner with Visual Studio installed.



Where to look if compilation fails

\- Check the Actions run logs (Restore and Build steps).

\- If the project is SDK-style and you'd rather use dotnet build/publish, swap the build step to `dotnet build ywBookStore.sln -c Release`.

\- If the projects output to a different folder, adjust the "Collect bin\\\\Release outputs" step to include that folder.



Notes

\- This workflow focuses on ensuring the repository compiles. It runs on pushes and pull requests to the main branch.

\- If you want artifacts attached to releases or code signing, we can add a release job later.

```

