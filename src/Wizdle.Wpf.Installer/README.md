# Wizdle.Wpf.Installer

The Inno Setup scripts for building Windows installers for Wizdle.Wpf

## Files

- **Wizdle.Windows.x64.iss** - Inno Setup script for 64-bit Windows installer
- **Wizdle.Windows.x86.iss** - Inno Setup script for 32-bit Windows installer

## Building Installers

The installers are automatically built during the release workflow (`.github/workflows/release.yaml`).

### Manual Build

1. **Publish the WPF application:**
   ```powershell
   # For x64
   dotnet publish ../Wizdle.Wpf/Wizdle.Wpf.csproj --configuration Release --runtime win-x64 --self-contained true /p:PublishSingleFile=true

   # For x86
   dotnet publish ../Wizdle.Wpf/Wizdle.Wpf.csproj --configuration Release --runtime win-x86 --self-contained true /p:PublishSingleFile=true
   ```

2. **Install Inno Setup:**
   ```powershell
   choco install innosetup -y
   ```

3. **Compile the installer:**
   ```powershell
   # For x64
   & "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "Wizdle.Windows.x64.iss"

   # For x86
   & "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "Wizdle.Windows.x86.iss"
   ```

4. **Output:**
   The compiled installers will be placed in the `Output` folder:
   - `Output/wizdle-setup-x64.exe`
   - `Output/wizdle-setup-x86.exe`

## Download

Pre-built installers are available on the [Wizdle releases page](https://github.com/lyndychivs/Wizdle/releases).
