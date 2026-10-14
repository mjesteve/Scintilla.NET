$ErrorActionPreference = "Stop"

$basePath = Split-Path -Parent $MyInvocation.MyCommand.Path

$files = @(
    "EmbeddedNatives\x86\Scintilla.dll",
    "EmbeddedNatives\x86\Lexilla.dll",
    "EmbeddedNatives\x64\Scintilla.dll",
    "EmbeddedNatives\x64\Lexilla.dll"
)

foreach ($relativeFile in $files) {
    $file = Join-Path $basePath $relativeFile
    $gzFile = "$file.gz"

    if (!(Test-Path $file)) {
        throw "No existe el archivo: $file"
    }

    if (Test-Path $gzFile) {
        Remove-Item $gzFile -Force
    }

    $source = [System.IO.File]::OpenRead($file)
    try {
        $target = [System.IO.File]::Create($gzFile)
        try {
            $gzip = New-Object System.IO.Compression.GZipStream($target, [System.IO.Compression.CompressionMode]::Compress)
            try {
                $source.CopyTo($gzip)
            }
            finally {
                $gzip.Dispose()
            }
        }
        finally {
            $target.Dispose()
        }
    }
    finally {
        $source.Dispose()
    }

    Write-Host "Generado: $gzFile"
}