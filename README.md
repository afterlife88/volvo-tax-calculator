# volvo-tax-calculator

## Done things
- Fixed calculation by changing logic of calculation of diff minutes, removed ms diff and replaced with built in Subtract method of datetime 
- Changed the logic with switch case around hours calculation as the rules moved into separated json files and desiarilized to DateOnly struct to work with the hours in more efficient way. 
- Covered all basic cases and edge cases provided by assignment (colleagues desk notes) by the unit tests. 
- Covered happy path scenario and basic validation in the integration tests for the API. 
- Added fluent validation 
- API implemented with minimal api
- Implelented error handling using middleware
- Swagger in place

## How to run 
> - `git clone git@github.com:Afterlife88/volvo-tax-calculator.git `
> - `cd volvo-tax-calculator`
> - `dotnet restore MaerskSortingChallenge.sln`
> - `dotnet test` - optional
> - `dotnet run --project src/Volvo.TaxCalculator.WebApi`
> - `Open localhost from the console and explore swagger to request or just curl from bellow example`

## Example request in curl 

```
curl -X 'POST' \
  'https://localhost:7039/calculate-fee' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "city": "GOTHENBURG",
  "vehicle": "Car",
  "passDates": [
    "2022-10-26T06:51:37.064Z"
  ]
}'
```