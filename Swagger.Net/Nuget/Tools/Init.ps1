param($installPath, $toolsPath, $package, $project)
If ($project -eq $null) { return }
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
			$item = $project.ProjectItems.Item("SwaggerConfig.cs")
			$item.Open('{7651A701-06E5-11D1-8EBD-00A0C90F26EA}')
			$item.SaveAs($AppSwaggerConfig)
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
	#Write-Host $_.Exception
}


try
{
    $project.ProjectItems.Item("SwaggerConfig.cs").Remove()
}
catch
{
    Write-Host "Error removing SwaggerConfig file! "
	#Write-Host $_.Exception
}

try
{
    Remove-Item $SwaggerConfig
}
catch
{
    Write-Host "Error deleting SwaggerConfig file! "
	#Write-Host $_.Exception
}


