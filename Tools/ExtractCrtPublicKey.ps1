param (
    [string]$certPath = "tls.crt",
    [string]$outputPath = "tls_public.pem"
)

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$cert.Import($certPath)

$publicKey = $cert.GetPublicKeyString()
$publicKeyBytes = [System.Convert]::FromBase64String($publicKey)

$pemHeader = "-----BEGIN PUBLIC KEY-----"
$pemFooter = "-----END PUBLIC KEY-----"

$pemContent = $pemHeader + "`n"
$pemContent += [System.Convert]::ToBase64String($publicKeyBytes, "InsertLineBreaks")
$pemContent += "`n" + $pemFooter

Set-Content -Path $outputPath -Value $pemContent
