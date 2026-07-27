$ErrorActionPreference = "Stop"

dotnet-CycloneDX ../RolandK.TimeTrack.slnx  `
                 -o ../sbom/cyclonedx_1.6  `
                 --output-format Json  `
                 --spec-version 1.6  `
                 --set-version 1.0.0  `
                 --exclude-test-projects