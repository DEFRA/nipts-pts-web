# nipts-pts-web

The "nipts-pts-web" web application allows customers to apply online for a Pet Travel Document required for travel from Great Britain to Northern Ireland with a pet.

This repository contains the frontend web application built using ASP.NET Core and related technologies.

## Prerequisites
Before setting up the project, ensure you have the following installed:

.NET SDK 6.0+
Visual Studio 2022 or Visual Studio Code
Access to required environment variables or secrets (e.g., via Azure Key Vault or local settings)

## Setup
1. Clone the repository:
```
git clone https://github.com/DEFRA/nipts-pts-web.git
cd nipts-pts-web
```

2. Restore dependencies:
```
dotnet restore
```

3. Configure local settings: Create a appsettings.development.json file in the root with the following structure:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Cookie": {
    "Name": "PTS",
    "ExpireTimespan": "00:30:00"
  },
  "Puppeteer": {
    "BrowserURL": "http://localhost:3000/"
  }
```

4. PDF setup
To enable PDF downloads locally on dev environment:

- Download and Install Chromium (https://www.googleapis.com/download/storage/v1/b/chromium-browser-snapshots/o/Win%2F884014%2Fchrome-win.zip?generation=1621364626012227&alt=media )

- Run this command in cmd replacing the path based on your local install

```
"C:\Installers\chrome-win/chrome.exe" --headless --remote-debugging-address=0.0.0.0 --remote-debugging-port=3000"
```

5. Configure User Secrets
Configure user secrets to access required endpoints and resources

### Development
To run the project locally:
```
dotnet run
```
Or use Visual Studio to start debugging via F5.

You can access the application at http://localhost:5000 (or the configured port).

### Test
Unit tests are located in the /test directory.

To run tests:
```
dotnet test
```

Ensure all dependencies are restored and the test project builds successfully.

## Running in development
1. Ensure all dependencies are installed.
2. Start the backend using dotnet run.
3. Ensure user secrets are setup

## Running tests
Run all tests using:
```
dotnet test
```

## Contributing to this project

Please read the [contribution guidelines](/CONTRIBUTING.md) before submitting a pull request.

## Licence

THIS INFORMATION IS LICENSED UNDER THE CONDITIONS OF THE OPEN GOVERNMENT LICENCE found at:

<http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3>

The following attribution statement MUST be cited in your products and applications when using this information.

>Contains public sector information licensed under the Open Government licence v3

### About the licence

The Open Government Licence (OGL) was developed by the Controller of Her Majesty's Stationery Office (HMSO) to enable information providers in the public sector to license the use and re-use of their information under a common open licence.

It is designed to encourage use and re-use of information freely and flexibly, with only a few conditions.
