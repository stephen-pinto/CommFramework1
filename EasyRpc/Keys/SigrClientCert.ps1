$certificate = New-SelfSignedCertificate `
      -Type Custom `
      -Subject "Ivan Yakimov" `
      -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2") `
      -FriendlyName "SigR Client Authorization" `
      -KeyUsage DigitalSignature `
      -KeyAlgorithm RSA `
      -KeyLength 2048

$pfxPassword = ConvertTo-SecureString `
    -String "p@ssw0rd" `
    -Force `
    -AsPlainText

Export-PfxCertificate `
    -Cert $certificate `
    -FilePath "SigRClientCertificate.pfx" `
    -Password $pfxPassword