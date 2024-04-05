# Store the current location
$invocationDir = Get-Location

# Set the location to the current script location
Set-Location -Path $PSScriptRoot
Set-Location -Path ".."

# Define the path where the ANTLR executable will be saved
$antlrPath = "Downloads/antlr.jar"

# Define the path of the .g4 file
$g4LexerFilePath = "../ArcSourceCodeLexer.g4"
$g4ParserFilePath = "../ArcSourceCodeParser.g4"

# Compile the .g4 file to a C# target
# Define the output directory
$outputDir = "Generated"

if (-not (Test-Path $outputDir)) {
	New-Item $outputDir -ItemType Directory
}

# Compile the .g4 file to a C# target and output to the specific directory
java -jar $antlrPath -Dlanguage=CSharp $g4LexerFilePath $g4ParserFilePath -o $outputDir

# Return to the directory where the script was invoked
Set-Location -Path $invocationDir
