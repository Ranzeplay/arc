# Store the current location
$invocationDir = Get-Location

# Set the location to the current script location
Set-Location -Path $PSScriptRoot
Set-Location -Path ".."

# Define the URL of the ANTLR executable
$antlrUrl = "https://www.antlr.org/download/antlr-4.8-complete.jar"

$downloadDir = "Downloads"

if (-not (Test-Path $downloadDir)) {
	New-Item $downloadDir -ItemType Directory
}

# Define the path where the ANTLR executable will be saved
$antlrPath = "$downloadDir/antlr.jar"

# Download the ANTLR executable
Invoke-WebRequest -Uri $antlrUrl -OutFile $antlrPath

# Return to the directory where the script was invoked
Set-Location -Path $invocationDir
