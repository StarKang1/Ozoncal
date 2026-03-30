Add-Type -AssemblyName System.Drawing

# Create a new bitmap
$bitmap = New-Object System.Drawing.Bitmap(256, 256)
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)

# Fill with blue background (OZON brand color)
$backgroundColor = [System.Drawing.Color]::FromArgb(0, 91, 255)
$brush = New-Object System.Drawing.SolidBrush($backgroundColor)
$graphics.FillRectangle($brush, 0, 0, 256, 256)

# Draw "OZON" text
$font = New-Object System.Drawing.Font("Arial", 80, [System.Drawing.FontStyle]::Bold)
$textColor = [System.Drawing.Color]::White
$textBrush = New-Object System.Drawing.SolidBrush($textColor)
$format = New-Object System.Drawing.StringFormat
$format.Alignment = [System.Drawing.StringAlignment]::Center
$format.LineAlignment = [System.Drawing.StringAlignment]::Center
$graphics.DrawString("OZON", $font, $textBrush, 128, 128, $format)

# Save as PNG
$bitmap.Save("ozon.png", [System.Drawing.Imaging.ImageFormat]::Png)

# Clean up
$brush.Dispose()
$textBrush.Dispose()
$font.Dispose()
$graphics.Dispose()
$bitmap.Dispose()

Write-Host "OZON logo created successfully!"