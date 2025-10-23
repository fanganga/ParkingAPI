# Parking API readme

## Setup Instructions
Compile and run the application either through Visual Studio or dotnet CLI.
The API is available on https://localhost:7295

## Assumptions & Questions
- All parking spaces are intercahngable (although the model considers Small, Medium and Large cars, there is not a need for Small, Medium and Large spaces)
- The spaces are numbered from 1 to the maximum number
- There is no need to consider spaces being unavailable for reasons other than being occupied (eg being resurfaced)
- Given the time constraints and the instruction to avoid over-engineering, I chose to have the database represent a real time snapshot of the car park, having the database hold a history of use for eg analytics, sending regular users a monthly bill seems like a likely requirement. How that can be achieved is discussed in Possible scope Increases for Future

## ToDo

### Basic requirements

- [x] Get /parking returns valid JSON
- [x] Get /parking returns data from DB
- [x] Post /parking updates DB, putting specified vehicle in first avaiable space
- [ ] Post /parking returns {VehicleReg: string, SpaceNumber: int, TimeIn: DateTime}
- [ ] Post /parking returns error for invalid type, reg already recorded
- [ ] Post /parking/exit updates DB
- [ ] Post /parking/exit returns error if reg does not represent parked vehicle
- [ ] Post /parking/exit returns {VehicleReg: string, VehicleCharge: double TimeIn: DateTime, TimeOut: DateTime}

### Stretch goals

- [ ] Integrate swagger
- [ ] Replace in-memory DB with MSSQL
- [ ] Make number of spaces configurable

### Possible scope increases for future

If a historical record is required in the database, that can be achieved by separating the single table into:
- A table, ParkingSpaces with column Int: Id
- A table, ParkingOccupancies with columns Int: SpaceId, String: OccupierReg, DateTime: OccupierIn, DateTime?: OccupierOut
- A table, Cars with columns String: Reg and Int: Type - this avoids duplication of car type data in the ParkingOccupancies table, moving to a higher normal form

It may be necessary to account for the relationship between registration number and car type changing - eg when someone transfers a personalised number plate to a new car of a different size - by introducing a car ID separate from the registration number

Occupied spaces can be represented by those ParkingOccupancies where OccupierOut is null
Free spaces can be represented by obtaining a Left join of the parking spaces table and the occupied spaces selection, then selecting those records from the join result where OccupierReg has no value.