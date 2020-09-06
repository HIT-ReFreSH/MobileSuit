docfx ./docs/docfx.json
Copy-Item -Recurse -Force docs/_site/* docs_source
cd docs
Get-ChildItem -Force -Exclude .git | ForEach-Object { Remove-Item -Recurse -Force $_ }
cd ..
Copy-Item -Recurse -Force docs_source/* docs