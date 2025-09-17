# Gemini API - UFC Data Service

This project is a .NET API designed to serve UFC (Ultimate Fighting Championship) data.

## Project Overview

The primary goal of this API is to provide structured access to a comprehensive dataset of UFC fights, fighters, and events.

### Data Source

The data is sourced from a collection of CSV files located in the `./data` directory. These files have been scraped from an external source and contain detailed information about:
-   **Events:** `event_data.csv`
-   **Fighters:** `fighter_data.csv`
-   **Fights:** `fight_data.csv`
-   **Rounds:** `round_data.csv`

### Data Models

The project uses Entity Framework Core to map C# objects to the database. It's important to note the distinction between the two model directories:

-   `./CSVObjects`: This directory contains the primary C# classes that are directly mapped to the database tables using EF Core. These classes are designed to align closely with the structure of the source CSV files.
-   `./Objects`: This directory contains older, deprecated models. These classes are scheduled for removal and should be ignored.

### Import Process

The `DB/ImportController.cs` file is responsible for managing the data import process. It contains API endpoints that read the data from the CSV files and populate the corresponding tables in the SQL database. The import is handled in stages, starting with events and fighters, followed by fights and rounds.