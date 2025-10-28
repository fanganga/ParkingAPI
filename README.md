# Parking API readme

## Setup Instructions
1. Set the value of "CarParkSize" in appsettings.json to the desired number of spaces
2. Compile and run the application either through Visual Studio or dotnet CLI.
3. The API is available on https://localhost:7295
4. API documentation is available on https://localhost:7295/swagger
5. An in-memory database is used, which is seeded on startup.

## Assumptions & Questions
- All parking spaces are intercahngable (although the model considers Small, Medium and Large cars, there is not a need for Small, Medium and Large spaces)
- The spaces are numbered from 1 to the maximum number
- There is no need to consider spaces being unavailable for reasons other than being occupied (eg being resurfaced)
- Given the time constraints and the instruction to avoid over-engineering, I chose to have the database represent a real time snapshot of the car park, having the database hold a history of use for eg analytics, sending regular users a monthly bill seems like a likely requirement. How that can be achieved is discussed in Possible scope Increases for Future
- There is only one client in the system, and therefore no need for locking or transactions
- The expected return values for error statuses have not been specified - I have used BadRequest where the supplied data is invalid, including for a registration already used, and Problem where there are no free spaces

## ToDo

### Basic requirements

- [x] Get /parking returns valid JSON
- [x] Get /parking returns data from DB
- [x] Post /parking updates DB, putting specified vehicle in first avaiable space
- [x] Post /parking returns {VehicleReg: string, SpaceNumber: int, TimeIn: DateTime}
- [x] Post /parking returns error for invalid type, reg already recorded
- [x] Post /parking/exit updates DB
- [x] Post /parking/exit returns error if reg does not represent parked vehicle
- [x] Post /parking/exit returns {VehicleReg: string, VehicleCharge: double TimeIn: DateTime, TimeOut: DateTime}
- [x] Vehicle charge calculated appropriately

### Stretch goals

- [x] Integrate swagger
- [x] Fix compiler warnings/notices
- [ ] Extend unit tests to cover behaviour currently in controllers
- [x] Make number of spaces configurable
- [ ] Replace in-memory DB with MSSQL

### Corrections

- [x] Fix duplicate method name
- [ ] Fix use of doubles throughout
- [ ] Fix direct calls of repo from controller
- [ ] Exception handling
- [ ] Defined architectural pattern


### Possible scope increases for future

If a historical record is required in the database, that can be achieved by separating the single table into:
- A table, ParkingSpaces with column Int: Id
- A table, ParkingOccupancies with columns Int: SpaceId, String: OccupierReg, DateTime: OccupierIn, DateTime?: OccupierOut
- A table, Cars with columns String: Reg and Int: Type - this avoids duplication of car type data in the ParkingOccupancies table, moving to a higher normal form

It may be necessary to account for the relationship between registration number and car type changing - eg when someone transfers a personalised number plate to a new car of a different size - by introducing a car ID separate from the registration number

Occupied spaces can be represented by those ParkingOccupancies where OccupierOut is null
Free spaces can be represented by obtaining a Left join of the parking spaces table and the occupied spaces selection, then selecting those records from the join result where OccupierReg has no value.

The Fee Structure could be moved to the database to enable fee changes. A proposed set of tables are:
- BaseFees with columns DateTime: ValidFrom, Int: VehicleType, Double: FeePerIncrement, Int: IncrementMinutes
- LongStayFees with columns DateTime: ValidFrom, Double: FeePerIncrement, Int: IncrementMinutes