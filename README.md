# Receipt Processor
 
## This project is to demonstrate proficiency in API development as well as some Docker exposure. This project uses .NET Core 8.0 C#

## This project is based off of a coding challenge by Fetch Rewards as defined [here](https://github.com/fetch-rewards/receipt-processor-challenge).

### For running this project in a Docker container, there is a Dockerfile in `\ReceiptProcessor\ReceiptProcessor\Dockerfile`. There should not be any extra build parameters required for it to run.

### Once the project is running, these are the available endpoints:

1. (HTTP POST) `/receipts/process` // Processes a receipt request. Receipt payload definition can be found [here](https://github.com/fetch-rewards/receipt-processor-challenge/blob/main/api.yml).
2. (HTTP GET) `/receipts/{Receipt GUID}/points` // Returns the amount of points earned by the receipt with the given GUID. Points earned are defined [here](https://github.com/fetch-rewards/receipt-processor-challenge?tab=readme-ov-file#rules).
3. (HTTP DELETE) `/receipts/clear` // For ease of use, I included an endpoint that deletes all created receipts. This would not normally be included in a product API.