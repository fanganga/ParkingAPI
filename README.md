# Parking API readme

## Setup Instructions

## Assumptions & Questions

## ToDo

### Basic requirements

- [] Get /parking returns valid JSON
- [] Get /parking returns data from DB
- [] Post /parking updates DB, putting specified vehicle in first avaiable space
- [] Post /parking returns {VehicleReg: string, SpaceNumber: int, TimeIn: DateTime}
- [] Post /parking returns error for invalid type, reg already recorded
- [] Post /parking/exit updates DB
- [] Post /parking/exit returns error if reg does not represent parked vehicle
- [] Post /parking/exit returns {VehicleReg: string, VehicleCharge: double TimeIn: DateTime, TimeOut: DateTime}

### Stretch goals

- [] Integrate swagger
- [] Replace in-memory DB with MSSQL

### Possible scope increases for future
