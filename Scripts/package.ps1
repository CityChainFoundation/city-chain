$configuration="release"
$filename="City.Chain.exe"
$output="City.Chain-wrapped.exe"
$runtime="win-x64"
$arch="windows-x64"
$git_commit=(git log --format=%h --abbrev=7 -n 1)
$publish_directory="..\src\City.Chain\bin\$configuration\netcoreapp2.1\$runtime\publish"
$download_directory=$env:temp
$warp=""
$project_path="..\src\City.Chain\City.Chain.csproj"

Write-Host "Download directory is $download_directory" -foregroundcolor "Magenta"
Write-Host "Current directory is $PWD" -foregroundcolor "Magenta"
Write-Host "Git commit to build: $git_commit" -foregroundcolor "Magenta"

Write-Host "Downloading warp..." -foregroundcolor "Magenta"
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Invoke-WebRequest https://github.com/stratisproject/warp/releases/download/v0.2.1/$warp -O $download_directory\$warp

If(Get-ChildItem $warp)
{
    Write-Host "Warp downloaded succesfully." -foregroundcolor "Magenta"

    $size=((Get-Item "$warp").Length)
    Write-Host "Size is $size" -foregroundcolor "Magenta"
}

Write-Host "Building the City Chain daemon..." -foregroundcolor "Magenta"
dotnet --info
dotnet publish $project_path -c $configuration -v m -r $runtime 

Write-Host "List of files to package:" -foregroundcolor "Magenta"
Get-ChildItem -Path $publish_directory

Write-Host "Packaging the daemon..." -foregroundcolor "Magenta"
& $warp --arch $arch --input_dir $publish_directory --exec $filename --output $publish_directory\$output

Write-Host "Done." -foregroundcolor "green"
Read-Host "Press ENTER"
