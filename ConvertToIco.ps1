Add-Type -AssemblyName System.Drawing

# Load the PNG file
$pngPath = "ozon.png"
$bitmap = New-Object System.Drawing.Bitmap($pngPath)

# Create a memory stream to hold the ICO data
$icoStream = New-Object System.IO.MemoryStream

# Create icon from bitmap
$icon = [System.Drawing.Icon]::FromHandle($bitmap.GetHicon())

# Save as ICO
$icon.Save($icoStream)

# Write to file
$icoPath = "ozon.ico"
$icoData = $icoStream.ToArray()
[System.IO.File]::WriteAllBytes($icoPath, $icoData)

# Clean up
$icon.Dispose()
$bitmap.Dispose()
$icoStream.Dispose()

Write-Host "OZON icon created successfully!"