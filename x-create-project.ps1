# Company name
$oldCompanyName="YourCompanyName"
$newCompanyName="CMC"

# Project name
$oldProjectName="NetApiCleanTemplate" 
$newProjectName="Disertation"

# Rename function
function Rename {
	param (
		$TargetFolder,
		$PlaceHolderCompanyName,
		$PlaceHolderProjectName,
		$NewCompanyName,
		$NewProjectName
	)
	# Config
	$fileType="FileInfo"  
	$dirType="DirectoryInfo"  
	$include=@("*.cs","*.cshtml","*.asax","*.ps1","*.ts","*.csproj","*.sln","*.xaml","*.json","*.js","*.xml","*.config","Dockerfile")

	Write-Host "[$TargetFolder] Renaming folders ..."
	# Rename folders
	Ls $TargetFolder -Recurse | Where { $_.GetType().Name -eq $dirType -and ($_.Name.Contains($PlaceHolderCompanyName) -or $_.Name.Contains($PlaceHolderProjectName)) } | ForEach-Object{
		Write-Host '-(dir. name) ' $_.FullName
		$newDirectoryName=$_.Name.Replace($PlaceHolderCompanyName,$NewCompanyName).Replace($PlaceHolderProjectName,$NewProjectName)
		Rename-Item $_.FullName $newDirectoryName
	} 
	Write-Host '' 

	# Replace file contents and rename file
	Write-Host "[$TargetFolder] Renaming file contents and file names ..."
	Ls $TargetFolder -Include $include -Recurse | Where { $_.GetType().Name -eq $fileType} | ForEach-Object{
		
		if($_.Name.contains('docker')) {
			Write-Host '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'
			Write-Host $_.Name
			Write-Host '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'
		}
		$fileText = Get-Content $_ -Raw -Encoding UTF8
		if($fileText.Length -gt 0 -and ($fileText.contains($PlaceHolderCompanyName) -or $fileText.contains($PlaceHolderProjectName))){
			$fileText.Replace($PlaceHolderCompanyName,$NewCompanyName).Replace($PlaceHolderProjectName,$NewProjectName) | Set-Content $_ -Encoding UTF8
			Write-Host '-(text) ' $_.FullName
		}
		If($_.Name.contains($PlaceHolderCompanyName) -or $_.Name.contains($PlaceHolderProjectName)){
			$newFileName=$_.Name.Replace($PlaceHolderCompanyName,$NewCompanyName).Replace($PlaceHolderProjectName,$NewProjectName)
			Rename-Item $_.FullName $newFileName
			Write-Host '-(name) ' $_.FullName
		}
	}

}

# Start
$elapsed = [System.Diagnostics.Stopwatch]::StartNew()
Write-Host ''

# Copy
$newRoot = $newCompanyName + "." + $newProjectName

Write-Host '----------------------------------------------------------------------------------'
Write-Host " Copying files to $newRoot ..."
Write-Host '----------------------------------------------------------------------------------'

$source = ".\*"
$destination = "..\\" + $newRoot

if (Test-Path -Path $destination) {
	Remove-Item -Recurse $destination
}
mkdir $destination
Copy-Item -Path (Get-Item -Path $source -Exclude ('.git', '.vs', 'x-create-project.ps1')).FullName -Destination $destination -Recurse -Force

Write-Host ''

# Clean
Write-Host '----------------------------------------------------------------------------------'
Write-Host " Cleaning the project ..."
Write-Host '----------------------------------------------------------------------------------'
Write-Host ''

Push-Location $destination
.\x-clean-project.bat
Pop-Location

# Rename
Write-Host '----------------------------------------------------------------------------------'
Write-Host " Renaming the project content ..."
Write-Host '----------------------------------------------------------------------------------'
Write-Host ''

$targetFolder = (Get-Item -Path "..\$newRoot\" -Verbose).FullName
Rename -TargetFolder $targetFolder -PlaceHolderCompanyName $oldCompanyName -PlaceHolderProjectName $oldProjectName -NewCompanyName $newCompanyName -NewProjectName $newProjectName

# Summary
$elapsed.stop()
Write-Host ''
Write-Host '----------------------------------------------------------------------------------'
Write-Host "[$TargetFolder] Total time: $($elapsed.Elapsed.ToString())"
Write-Host '----------------------------------------------------------------------------------'

