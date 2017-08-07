param($installPath, $toolsPath, $package, $project)
$SwaggerConfig = $project.ProjectItems.Item("SwaggerConfig.cs").FileNames(1)

try
{
    $AppStartDir = $project.ProjectItems.Item("App_Start").FileNames(1)
    MoveConfigFile $AppStartDir
}
catch
{
    Write-Host "App_Start not found! " + $_.Exception
}


try
{
    $project.ProjectItems.Item("SwaggerConfig.cs").Remove()
    Remove-Item $SwaggerConfig
}
catch
{
    Write-Host "Error deleting SwaggerConfig file: " + $_.Exception
}


function MoveConfigFile($AppStartDir)
{
    $ValidPath = Test-Path $AppStartDir -IsValid
    If ($ValidPath -eq $True)
    {
        $AppSwaggerConfig = Join-Path $AppStartDir "SwaggerConfig.cs"
        $ValidFile = Test-Path $AppSwaggerConfig -IsValid
        If ($ValidFile -eq $False)
        {
            $project.ProjectItems.Item("SwaggerConfig.cs").SaveAs($AppSwaggerConfig)
        }
    }
}
