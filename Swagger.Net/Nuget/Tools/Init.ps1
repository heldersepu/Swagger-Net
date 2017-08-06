param($installPath, $toolsPath, $package, $project)

try {
    $AppStartDir = $project.ProjectItems.Item("App_Start").FileNames(1)
    MoveConfigFile $AppStartDir
}
catch {
    $project.ProjectItems.Item("SwaggerConfig.cs").Remove()
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
    $project.ProjectItems.Item("SwaggerConfig.cs").Remove()
}
