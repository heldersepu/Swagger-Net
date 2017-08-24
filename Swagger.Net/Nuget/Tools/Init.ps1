param($installPath, $toolsPath, $package, $project)
$SwaggerConfig = $project.ProjectItems.Item("SwaggerConfig.cs").FileNames(1)

function MoveConfigFile($AppStartDir)
{
    Write-Host $AppStartDir
	$ValidPath = Test-Path $AppStartDir
    If ($ValidPath -eq $True)
    {
        $AppSwaggerConfig = Join-Path $AppStartDir "SwaggerConfig.cs"
		Write-Host $AppSwaggerConfig
        $ValidFile = Test-Path $AppSwaggerConfig
        If ($ValidFile -eq $False)
        {
			$project.ProjectItems.Item("SwaggerConfig.cs").SaveAs($AppSwaggerConfig)
			Write-Host "SwaggerConfig Saved! "
        }
    }
}

try
{
    $AppStartDir = $project.ProjectItems.Item("App_Start").FileNames(1)
    MoveConfigFile $AppStartDir
}
catch
{
    Write-Host "App_Start not found! "
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



