# CVService.Api
A C# web service to store and allow manipulation of parts of users' CVs in a structured form. 

## Instructions to run:
1. Download or clone the repository from git
1. Open in Visual studio 2019 or similar IDE
1. Rebuild whole solution
1. Set CVService.Api as startup project
1. Run the project
1. You should now be presented with a Swagger API Doccumentation page
1. Feel free to use swagger or postman or any other API testing solution you wish

**PLEASE NOTE: All requests except those to swagger or the maintenance controller need to be authorised with a token.**

Add a header to all requests with the name "PrivateAccessToken" and value "84157CEC-965E-4680-BDD8-AFFD81AD0D2A"

## Tests
There are also unit, integration and acceptance tests included in a seperate project.
To test, make sure the API service is running, then right click on the CVService.Api.Tests project and select "Run all tests"

The integration tests have been configured to run serially, it was very important that each integration test start from a clean state with seeded data.
When ran in parallel the data would be modified in many different ways and it would be impossible to gurantee the validaty of each test run. 

**There is a full acceptance test located in a class called "AcceptanceTests.cs", which has one test method written in XUnit. "Demo()".
This will give a good end to end look at most of the API and its Endpoints.**

## Notes
The application uses one view model for all HttpMethods which means that when posting the model to the web api it is possible to include the ID field in the model. This will cause an error as we want to stop this. In a production enviornment with a longer time frame this would be handled by either creating a seperate viewmodel for each Http Method, e.g. one for [GET], one for [POST] (that doesnt have the Id field), [PUT], [DELETE] OR by going into the swagger config and specifically changing the response for Ids in [POST]s.


