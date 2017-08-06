param($installPath, $toolsPath, $package, $project)

$ProjectDir = "C:\Code\SwashbuckleTest\Swagger_Test\"
$SwaggerConfigFile = Join-Path $ProjectDir "SwaggerConfig.cs"
$AppStartDir = Join-Path $ProjectDir "App_Start\"

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

Write-Host $project.ProjectItems.Item("SwaggerConfig.cs").FileNames(1)
