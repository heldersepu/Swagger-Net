param($installPath, $toolsPath, $package, $project)

Write-Host $installPath
Write-Host $toolsPath
Write-Host $package
Write-Host $project

$ProjectDir = "C:\Code\SwashbuckleTest\Swagger_Test\"
$SwaggerConfigFile = Join-Path $ProjectDir "SwaggerConfig.cs"
$AppStartDir = Join-Path $ProjectDir "App_Start\"
Write-Host $AppStartDir

$ValidPath = Test-Path $AppStartDir -IsValid
If ($ValidPath -eq $True)
{
	$AppSwaggerConfig = Join-Path $AppStartDir "SwaggerConfig.cs"
	$ValidFile = Test-Path $AppSwaggerConfig -IsValid
	If ($ValidFile -eq $True)
	{
		Remove-Item $SwaggerConfigFile
	}
	Else
	{
		Move-Item $SwaggerConfigFile $AppStartDir
	}
}
Else
{
	Remove-Item $SwaggerConfigFile
}

foreach ($item in $project.ProjectItems)
{
    Write-Host $item.Name
}