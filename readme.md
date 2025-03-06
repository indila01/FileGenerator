# FileGenerator and FileReader

## Overview
A .NET solution for generating and analyzing data files, consisting of two console applications:
- **FileGenerator**: Creates files with random data with 4 types
- **FileReader**: Analyzes the generated data files and creates a report of the type and the value

## Prerequisites
- .NET 8.0 SDK or later
- Docker
- Visual Studio Code (recommended) or any other .NET IDE

## Project Structure
```plaintext
FileGenerator/
├── data/ 
├── output/ 
├── source/
│   ├── FileGenerator/
│   │   └── Program.cs
│   └── FileReader/
│       └── Program.cs
```

## Building and Running

### Building the Projects
```bash
# Navigate to the solution directory
cd FileGenerator

# Build both projects
dotnet build
```

### Running the File Generator
```bash
# Navigate to the FileGenerator project
cd source/FileGenerator

# Run the generator
dotnet run
```

### Running the File Reader
```bash
# Navigate to the FileReader project
cd ..

# Run the reader
docker-compose run
```

## Output Files
- Generated data files: `source/FileGenerator/data/generated_data_yyyy-MM-dd_HH-mm-ss.txt`
- Analysis reports: `output/analysis_yyyy-MM-dd_HH-mm-ss.txt`

## Features

### FileGenerator
- Generates ~10MB data files
- Creates random objects including:
  - Integers
  - Real numbers
  - Alphabetical strings
  - Alphanumeric values
- Comma-separated format
- Color-coded console output

### FileReader
- Automatically processes most recent data file
- Generates detailed analysis reports
- Shows real-time analysis in console
- Color-coded output for better visibility

## Error Handling
Both applications include comprehensive error handling for:
- Missing directories
- File not found
- Invalid file content
- Access permission issues

Error messages are displayed in red in the console.
