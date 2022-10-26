# volvo-tax-calculator

## Done things
- Fixed calculation by changing logic of calculation of diff minutes, removed ms diff and replaced with built in Subtract method of datetime. Sorted passed array, moved the initial pointer of start date to the current from the loop.  
- Rules are stored in json format for specific city in Volvo.TaxCalculator.Domain/CitiesFeeRules and they need to be added into the _**cityFeeRules** dictionary in CongestionTaxCalculator.cs. As the was no requirements to put rules in persistence storage to add them during runtime of application, this way most efficient and easy. 
- Changed the logic with switch case around hours calculation as the rules moved into separated json files and deserialized to DateOnly struct to work with the hours in more efficient way. 
- Covered all basic cases and edge cases provided by assignment (colleagues desk notes) by the unit tests. 
- Covered happy path scenario and basic validation in the integration tests for the API. 
- Added fluent validation 
- API implemented with minimal api
- Implemented error handling using middleware
- Swagger in place

## How to run 
 - `git clone git@github.com:Afterlife88/volvo-tax-calculator.git `
- `cd volvo-tax-calculator`
- `dotnet restore Volvo.TaxCalculator.sln`
- `dotnet test`
- `dotnet run --project src/Volvo.TaxCalculator.WebApi`
- `Open localhost from the console and explore swagger to request make a request or just curl from example bellow`

## Example request in curl 
```
curl -X 'POST' \
  'https://localhost:7039/calculate-fee' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "city": "Gothenburg",
  "vehicle": "Car",
  "passDates": [
    "2022-10-26T08:40:07.036Z"
  ]
}'
```