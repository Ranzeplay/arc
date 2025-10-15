# Store the current location
$invocationDir = Get-Location

# Set the location to the current script location
Set-Location -Path $PSScriptRoot
Set-Location -Path ".."

Remove-Item -Recurse .antlr
Remove-Item -Recurse Downloads
Remove-Item -Recurse Generated

# Return to the directory where the script was invoked
Set-Location -Path $invocationDir
