# Unit Converter App

Small educational pet project from the roadmap.sh project ideas pool:
https://roadmap.sh/projects/unit-converter

The solution is split into two parts:

- Backend exposes the conversion API, validates requests, and performs unit conversion through `UnitsNet`
- Frontend provides a lightweight browser UI for sending conversion requests and showing results

## Overview

The app accepts a conversion request, checks it against a fixed whitelist of supported categories and units, delegates the calculation to `UnitsNet`, and returns the result immediately.

The backend does not store data and does not implement custom conversion formulas.

## Project Structure

```text
roadmapsh-unit-converter/
├── frontend/
│   ├── index.html
│   ├── script.js
│   └── style.css
├── src/
│   └── UnitConverter.Api/
│       ├── Conversion/
│       │   ├── ConversionService.cs
│       │   └── UnitsDictionary.cs
│       ├── Extensions/
│       │   └── ExceptionHandlerExtensions.cs
│       ├── Models/
│       │   ├── ConvertRequest.cs
│       │   └── ConvertResponse.cs
│       ├── Validation/
│       │   ├── AllowedUnits.cs
│       │   └── ConvertRequestValidator.cs
│       ├── Program.cs
│       └── UnitConverter.Api.http
└── tests/
    └── UnitConverter.Tests/
        ├── ConversionServiceTests.cs
        ├── ConvertEndpointTests.cs
        └── UnitConverter.Tests.csproj
```

## Responsibilities

### Backend

- Accept conversion requests from the frontend or any HTTP client
- Validate category, source unit, target unit, and unit compatibility
- Convert values using `UnitsNet`
- Return a stable JSON response with the result and target unit
- Handle expected and unexpected errors consistently

### Frontend

- Render the conversion form in the browser
- Let the user choose category, source unit, target unit, and value
- Send requests to the backend API
- Display the conversion result
- Keep a small local history and support copy-to-clipboard

## How It Works

1. The user enters a category, source unit, target unit, and numeric value in the frontend
2. The frontend sends a `POST` request to the backend `/convert` endpoint
3. The backend validates the request against the allowed whitelist
4. The conversion is delegated to `UnitsNet`
5. The backend returns the numeric result and the selected target unit
6. The frontend renders the result, updates history, and allows copying the value

## Backend

### Features

- .NET 10 backend
- ASP.NET Core Minimal API
- Single conversion endpoint
- Thin wrapper over `UnitsNet`
- Strict request validation with a whitelist
- Centralized error handling
- xUnit coverage for service and endpoint behavior

### Tech Stack

- .NET 10
- ASP.NET Core Minimal API
- UnitsNet
- xUnit

### Supported Categories and Units

The backend accepts only values explicitly allowed by its whitelist.

#### Length

- `millimeter`
- `centimeter`
- `meter`
- `kilometer`
- `inch`
- `foot`
- `yard`
- `mile`

#### Mass

- `milligram`
- `gram`
- `kilogram`
- `ounce`
- `pound`

#### Temperature

- `celsius`
- `fahrenheit`
- `kelvin`

### API Endpoint

#### Convert units

**Method:** `POST`  
**Route:** `/convert`

##### Request body

```json
{
  "category": "length",
  "fromUnit": "meter",
  "toUnit": "kilometer",
  "value": 100
}
```

##### Successful response

```json
{
  "result": 0.1,
  "toUnit": "kilometer"
}
```

### Validation Rules

The API rejects requests when:

- `category` is missing or empty
- `fromUnit` is missing or empty
- `toUnit` is missing or empty
- category is not supported
- source unit is not supported for the selected category
- target unit is not supported for the selected category

Extra units are intentionally not supported. The backend only accepts the values agreed between frontend and backend.

### Error Handling

The API uses centralized exception handling.

Expected invalid input is returned as a client error response. Unexpected conversion or runtime failures are handled without exposing unnecessary implementation details.

### Example Requests

#### Length

```json
{
  "category": "length",
  "fromUnit": "meter",
  "toUnit": "millimeter",
  "value": 1
}
```

#### Mass

```json
{
  "category": "mass",
  "fromUnit": "kilogram",
  "toUnit": "pound",
  "value": 1
}
```

#### Temperature

```json
{
  "category": "temperature",
  "fromUnit": "celsius",
  "toUnit": "fahrenheit",
  "value": 25
}
```

## Frontend

The repository includes a lightweight static frontend in the `frontend/` folder. It is a plain HTML, CSS, and JavaScript client that talks to the backend and provides a simple interactive interface for unit conversion.

### Features

- Category selection for length, mass, and temperature
- Source and target unit selectors
- Swap units action
- Numeric value input with automatic conversion
- Manual convert button as a fallback
- Result display with copy-to-clipboard support
- Small local history of recent conversions
- Client-side caching to avoid duplicate API calls for the same request

### How It Works

1. The page loads the available unit categories and units from the local frontend script
2. The user selects a category, source unit, target unit, and value
3. The frontend sends a `POST` request to `http://localhost:5117/convert`
4. The backend returns the conversion result
5. The frontend renders the result, stores it in history, and allows copying the numeric value

### Run the Frontend

The frontend is static and does not require a build step.

You can open `frontend/index.html` in a browser while the API is running, or serve the `frontend/` folder with any simple static file server.

The frontend expects the backend to be available at `http://localhost:5117`.

## Run the Project

### Prerequisites

- .NET 10 SDK

### Start the Backend

From the repository root:

```bash
dotnet run --project src/UnitConverter.Api
```

Or on Windows PowerShell:

```powershell
dotnet run --project .\src\UnitConverter.Api
```

## Test the Backend

You can use the included HTTP file:

- `src/UnitConverter.Api/UnitConverter.Api.http`

It already contains sample requests for:

- length conversions
- mass conversions
- temperature conversions

You can also use any HTTP client such as Postman or curl.

### Example with curl

```bash
curl -X POST http://localhost:5117/convert \
  -H "Content-Type: application/json" \
  -d '{
    "category": "length",
    "fromUnit": "meter",
    "toUnit": "kilometer",
    "value": 100
  }'
```

## Run Tests

```bash
dotnet test
```

## Test Coverage

The test project covers:

- conversion service behavior
- endpoint behavior
- positive scenarios
- negative scenarios
- validation failures
- edge and boundary cases