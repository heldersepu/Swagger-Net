param($installPath, $toolsPath, $package, $project)

$wshell = New-Object -ComObject Wscript.Shell
$wshell.Popup($installPath,0,"Done",0x1)
$wshell.Popup($toolsPath,0,"Done",0x1)
$wshell.Popup($package,0,"Done",0x1)
$wshell.Popup($project,0,"Done",0x1)