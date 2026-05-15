# GitHub Copilot Dashboard .NET

A modern ASP.NET Core web application that provides real-time monitoring and analytics for GitHub Copilot usage within your enterprise organization. This dashboard visualizes seat allocation, user activity, and detailed usage metrics across your development team.

## 📋 Table of Contents

- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation & Setup](#-installation--setup)
- [Configuration](#-configuration)
- [Project Structure](#-project-structure)
- [API Endpoints](#-api-endpoints)
- [Architecture](#-architecture)
- [Usage](#-usage)
- [Development](#-development)
- [Troubleshooting](#-troubleshooting)

## ✨ Features

### Core Features
- **Real-time Seat Monitoring**: View current Copilot seat allocation and utilization across your enterprise
- **User Analytics Dashboard**: Interactive charts and statistics showing per-user Copilot usage patterns
- **Multi-Day Usage Metrics**: Track detailed metrics including:
  - User-initiated interactions
  - Code generation activities
  - Code acceptance rates
  - IDE-specific usage breakdown
  - Feature-specific usage patterns
- **Persistent Caching**: Automatic caching of metrics to disk for reliability and performance
- **Smart Data Fetching**: Intelligent daily caching to minimize API calls and improve response times

### Technical Features
- **ASP.NET Core 9**: Built on the latest .NET framework with modern C# features
- **RESTful API**: Clean, well-documented API endpoints for integration
- **Cross-Origin Support**: CORS-enabled for flexible deployment scenarios
- **Responsive Web UI**: Modern HTML5-based dashboard that works across devices
- **Swagger/OpenAPI**: Built-in API documentation available in development mode

## 🔧 Prerequisites

### System Requirements
- **.NET 9.0 Runtime** or later
- **Visual Studio 2022** (or Visual Studio Code with C# Dev Kit) - recommended for development
- **Git** for cloning the repository

### GitHub Enterprise Requirements
- GitHub Enterprise organization with Copilot licensing enabled
- GitHub Personal Access Token (PAT) with appropriate scopes:
  - `admin:enterprise_managed_user` or equivalent enterprise admin permissions
  - `read:enterprise` scope for accessing enterprise data

## 📦 Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/your-organization/CopilotDashboard.NET.git
cd CopilotDashboard.NET
```

### Step 2: Verify .NET Installation

Ensure you have .NET 9.0 installed:

```bash
dotnet --version
```

Expected output should show: `9.0.x`

### Step 3: Restore Dependencies

```bash
cd CopilitDashboard.NET
dotnet restore
```

This downloads all NuGet packages required by the project (specified in `CopilitDashboard.NET.csproj`).

### Step 4: Configure Environment Variables

The application requires the following environment variables to be set:

#### Option A: Using System Environment Variables (Recommended for Production)
Set these in your operating system or deployment platform:

```powershell
# PowerShell (Windows)
[Environment]::SetEnvironmentVariable("NS_GITHUB_ENTERPRISE", "NERVESOLUTIONS", "User")
[Environment]::SetEnvironmentVariable("NS_GITHUB_API_TOKEN", "your_pat_token_here", "User")
```

```bash
# Bash (macOS/Linux)
export GITHUB_ENTERPRISE="NERVESOLUTIONS"
export GITHUB_API_TOKEN="your_pat_token_here"
```

#### Option B: Using appsettings.json (Development Only)

⚠️ **Warning**: Never commit sensitive tokens to version control.

Create or modify `CopilitDashboard.NET/appsettings.Development.json`:

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information"
	}
  },
  "GithubConfig": {
	"Enterprise": "NERVESOLUTIONS",
	"ApiToken": "your_pat_token_here"
  }
}
```

### Step 5: Build the Project

```bash
dotnet build
```

This compiles the C# code and checks for any compilation errors.

### Step 6: Run the Application

#### Development Mode
```bash
dotnet run
```

#### Production Mode
```bash
dotnet run --configuration Release
```

The application will start on `http://localhost:5000` by default. Check the console output for the actual port and HTTPS URL.

### Step 7: Access the Dashboard

Open your web browser and navigate to:
- **Local**: `http://localhost:5000`
- **Dashboard**: The index.html page will load automatically

## ⚙️ Configuration

### Project File Configuration

The project is configured in `CopilitDashboard.NET/CopilitDashboard.NET.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
	<TargetFramework>net9.0</TargetFramework>
	<Nullable>enable</Nullable>
	<ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

**Key Settings**:
- `TargetFramework`: Set to .NET 9.0
- `Nullable`: Enabled for strict null checking
- `ImplicitUsings`: Auto-includes common namespaces

### CORS Configuration

The application allows requests from any origin by default. To restrict this, modify `Program.cs`:

```csharp
// Current configuration (permissive)
policy.AllowAnyOrigin()
	  .AllowAnyMethod()
	  .AllowAnyHeader();

// Restrictive example
policy.WithOrigins("https://yourdomain.com")
	  .AllowAnyMethod()
	  .AllowAnyHeader();
```

### API Token Scope Requirements

Ensure your GitHub PAT has these scopes:
- `admin:org_hook` (for enterprise metrics access)
- Or equivalent enterprise admin permissions
- `read:org` (for organization data access)

## 📁 Project Structure

```
CopilitDashboard.NET/
├── CopilitDashboard.NET/              # Main project directory
│   ├── Controllers/
│   │   └── CopilotController.cs      # API endpoints for Copilot data
│   ├── Models/
│   │   └── Structures.cs              # Data models (MetricsReport, UserMetrics, etc.)
│   ├── Properties/
│   │   └── launchSettings.json        # Launch configuration for debugging
│   ├── wwwroot/                       # Static files (CSS, JS, HTML)
│   │   └── index.html                 # Dashboard UI
│   ├── Program.cs                     # Application startup and configuration
│   ├── appsettings.json               # Default configuration
│   ├── appsettings.Development.json   # Development-specific configuration
│   ├── CopilitDashboard.NET.csproj    # Project file
│   ├── CopilitDashboard.NET.http      # REST client test file (Visual Studio)
│   └── MetricsReports/                # Directory for cached metrics (auto-created)
│       └── YYYY-MM-DD.json            # Daily metrics cache files
└── README.md                          # This file
```

### Key Files Explained

| File | Purpose |
|------|---------|
| `Program.cs` | Configures ASP.NET Core pipeline, CORS, services, and background startup tasks |
| `CopilotController.cs` | Implements API endpoints: `/api/copilot/seats` and `/api/copilot/metrics` |
| `Models/Structures.cs` | Defines data models for deserialization of GitHub API responses |
| `wwwroot/index.html` | Frontend dashboard UI displaying metrics and charts |
| `MetricsReports/` | Directory where daily JSON snapshots of metrics are saved |

## 🔌 API Endpoints

### 1. Get Copilot Seats
Retrieves the current seat allocation for the enterprise.

**Endpoint**: `GET /api/copilot/seats`

**Response**:
```json
{
  "total_seats": 50
}
```

**Status Codes**:
- `200 OK` - Successful retrieval
- `401 Unauthorized` - Invalid GitHub token
- `403 Forbidden` - Insufficient permissions
- `500 Internal Server Error` - API communication error

**Example**:
```bash
curl -X GET "http://localhost:5000/api/copilot/seats"
```

### 2. Get Copilot Metrics
Retrieves detailed usage metrics for all users over the last 28 days.

**Endpoint**: `GET /api/copilot/metrics`

**Response** (Array of UserMetrics objects):
```json
[
  {
	"report_start_day": "2024-01-01",
	"report_end_day": "2024-01-28",
	"day": "2024-01-15",
	"enterprise_id": "MDEyOkVudGVycHJpc2U3ODA=",
	"user_id": 12345,
	"user_login": "john.doe",
	"user_initiated_interaction_count": 45,
	"code_generation_activity_count": 38,
	"code_acceptance_activity_count": 32,
	"totals_by_ide": [
	  {
		"ide": "VS Code",
		"total_engaged_users": 1,
		"total_code_suggestions": 15,
		"total_code_acceptances": 12,
		"total_code_lines_suggested": 245,
		"total_code_lines_accepted": 198
	  }
	],
	"totals_by_feature": [
	  {
		"feature": "Copilot Chat",
		"total_engaged_users": 1,
		"total_chats": 30
	  }
	]
  }
]
```

**Status Codes**:
- `200 OK` - Successful retrieval (returns cached or fresh data)
- `400 Bad Request` - No metrics available
- `401 Unauthorized` - Invalid GitHub token
- `500 Internal Server Error` - API communication error

**Caching Behavior**:
- Fetches fresh data from GitHub only once per day
- Subsequent requests on the same day return cached data
- Cache is persisted to `MetricsReports/{YYYY-MM-DD}.json`

**Example**:
```bash
curl -X GET "http://localhost:5000/api/copilot/metrics"
```

## 🏗️ Architecture

### Data Flow

```
┌─────────────────┐
│  Dashboard UI   │ (index.html - Frontend)
└────────┬────────┘
		 │
		 │ HTTP Requests
		 ▼
┌─────────────────────────────────┐
│   ASP.NET Core Web Server       │
│  (Kestrel on localhost:5000)    │
└────────┬────────────────────────┘
		 │
		 ├─► CopilotController
		 │   ├── GetSeatsAsync()
		 │   └── GetCopilotMetricsAsync()
		 │
		 ▼
┌──────────────────────────────────┐
│  In-Memory Metrics Cache         │
│  (Dictionary<string, UserMetrics>)
└─────────┬────────────────────────┘
		  │
		  ├─► Local File Cache
		  │   (MetricsReports/{date}.json)
		  │
		  └─► GitHub Enterprise API
			  (Copilot Metrics & Billing)
```

### Component Descriptions

#### Program.cs
- Configures the ASP.NET Core pipeline
- Enables CORS for cross-origin requests
- Registers controllers and services
- Initializes background metrics fetch on startup

#### CopilotController
- Implements two API endpoints
- Manages in-memory metrics caching with static Dictionary
- Implements daily metrics fetching to minimize API calls
- Handles authentication with GitHub PATs

#### Models/Structures.cs
- Defines data models for GitHub API responses
- Uses JSON serialization attributes for deserialization
- Includes: MetricsReport, UserMetrics, TotalsByIde, TotalsByFeature

#### Frontend (index.html)
- Responsive HTML/CSS/JavaScript dashboard
- Fetches data from API endpoints
- Displays metrics in charts and tables
- Provides real-time seat allocation visualization

## 🚀 Usage

### Accessing the Dashboard

1. Start the application: `dotnet run`
2. Open browser: `http://localhost:5000`
3. View seat allocation and usage metrics

### Using the API Directly

#### With cURL:
```bash
# Get seats
curl -X GET "http://localhost:5000/api/copilot/seats"

# Get metrics
curl -X GET "http://localhost:5000/api/copilot/metrics"
```

#### With PowerShell:
```powershell
# Get seats
Invoke-RestMethod -Uri "http://localhost:5000/api/copilot/seats" -Method Get

# Get metrics
$metrics = Invoke-RestMethod -Uri "http://localhost:5000/api/copilot/metrics" -Method Get
$metrics | ConvertTo-Json | Out-File "metrics.json"
```

#### With Swagger/OpenAPI (Development Only):

1. Start the application in development mode
2. Navigate to: `http://localhost:5000/openapi/v1.json`
3. Use a Swagger UI tool or Postman to explore endpoints

### Accessing Cached Metrics

Cached metrics are stored in the `MetricsReports` directory:

```powershell
# View available cache files
Get-ChildItem MetricsReports\

# Load cached metrics from a specific date
$cachedMetrics = Get-Content "MetricsReports\2024-01-15.json" | ConvertFrom-Json
```

## 🔧 Development

### Building from Source

```bash
# Clean build
dotnet clean
dotnet build

# Build with specific configuration
dotnet build --configuration Release

# Build and run tests (if included)
dotnet test
```

### Running in Debug Mode

1. Open `CopilitDashboard.NET.sln` in Visual Studio 2022
2. Press `F5` or click "Run" to start debugging
3. Breakpoints and debugging features are available
4. Swagger UI is automatically enabled in debug mode

### Code Style & Standards

- **Language Version**: C# 13 (compatible with .NET 9)
- **Nullable Reference Types**: Enabled
- **Naming Conventions**:
  - Public members: `PascalCase`
  - Private members: `_camelCase` or static prefixes (e.g., `dict_`, `dte_`)
  - Constants: `UPPER_SNAKE_CASE` or `PascalCase`

### Making Changes

1. **Adding New Endpoints**:
   - Add method to `CopilotController.cs`
   - Use `[HttpGet]`, `[HttpPost]`, etc. attributes
   - Implement proper error handling and logging

2. **Modifying Models**:
   - Update `Models/Structures.cs`
   - Ensure JSON serialization attributes match GitHub API response

3. **Updating UI**:
   - Modify `wwwroot/index.html`
   - Changes are reflected on next browser refresh

4. **Configuration Changes**:
   - Update `appsettings.json` or environment variables
   - Restart application for changes to take effect

### Common Development Tasks

#### Run Application
```bash
dotnet run
```

#### Build Release Version
```bash
dotnet publish -c Release -o ./publish
```

#### Clear Cache
```bash
Remove-Item MetricsReports -Recurse
```

## 🐛 Troubleshooting

### Issue: "Failed to fetch metrics report" Error

**Cause**: GitHub Enterprise name or API token is incorrect or expired

**Solution**:
1. Verify `GITHUB_ENTERPRISE` environment variable matches your organization
2. Check that `GITHUB_API_TOKEN` is valid and has not expired
3. Ensure token has appropriate permissions
4. Test with cURL: 
   ```bash
   curl -H "Authorization: Bearer YOUR_TOKEN" \
	 "https://api.github.com/enterprises/YOUR_ENTERPRISE/copilot/metrics/reports/users-28-day/latest"
   ```

### Issue: "No download links found in metrics report" Error

**Cause**: The metrics report exists but contains no downloadable user data

**Solution**:
1. Check that your Copilot licensing is active in GitHub Enterprise
2. Ensure there is actual usage data for the reporting period
3. Try again after 24 hours for updated metrics
4. Verify token has enterprise admin permissions

### Issue: Application Won't Start

**Cause**: Missing or invalid environment variables

**Solution**:
```bash
# Check environment variables are set
# Windows PowerShell:
$env:GITHUB_ENTERPRISE
$env:GITHUB_API_TOKEN

# Linux/macOS:
echo $GITHUB_ENTERPRISE
echo $GITHUB_API_TOKEN
```

### Issue: Port Already in Use

**Cause**: Another application is using port 5000

**Solution**:
1. Run on different port:
   ```bash
   dotnet run --urls "http://localhost:5001"
   ```
2. Or find and kill the process using port 5000:
   ```bash
   # Windows PowerShell:
   Stop-Process -Id (Get-NetTCPConnection -LocalPort 5000).OwningProcess

   # Linux/macOS:
   lsof -ti:5000 | xargs kill -9
   ```

### Issue: CORS Errors in Browser Console

**Cause**: Frontend is making requests from a different origin than expected

**Solution**:
1. Check `Program.cs` CORS configuration
2. If needed, restrict CORS to specific domain:
   ```csharp
   policy.WithOrigins("https://yourdomain.com")
		 .AllowAnyMethod()
		 .AllowAnyHeader();
   ```
3. Clear browser cache and hard refresh (Ctrl+Shift+R)

### Issue: Metrics Cache Not Updating

**Cause**: Cache was created on current date; application only fetches once daily

**Solution**:
1. Delete cache file to force refresh:
   ```bash
   Remove-Item "MetricsReports\$(Get-Date -Format 'yyyy-MM-dd').json"
   ```
2. Restart application
3. Call `/api/copilot/metrics` endpoint

## 📚 References

- [GitHub Copilot Enterprise API Documentation](https://docs.github.com/en/enterprise-cloud@latest/rest/copilot/copilot-usage?apiVersion=2022-11-28)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [GitHub Personal Access Tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token)
- [.NET 9 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)

## 📝 License

This project is provided as-is. Modify as needed for your organization.

## 🤝 Contributing

For issues, feature requests, or contributions, please contact your development team.

## 📧 Support

For support or questions:
- Check the [Troubleshooting](#-troubleshooting) section
- Review GitHub Enterprise Copilot API documentation
- Contact your organization's GitHub administrator

---

**Last Updated**: January 2024  
**Maintained By**: Development Team  
**Version**: 1.13.26.9620
