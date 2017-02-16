$projectPath = "C:\WSCAD\WSCAD SUITE\2017\Projects\"
$searchPattern = "PRJDATA6.ldb"
$filesFound = [System.IO.Directory]::EnumerateFiles($projectPath,$searchPattern,"AllDirectories")
$pathToOpen = [System.IO.Path]::GetDirectoryName($filesFound[0])
ii $pathToOpen