# CVService.Api
A C# web service to store and allow manipulation of parts of users' CVs in a structured form. 

## Instructions to run:
1. Download or clone the repository from git
1. Open in Visual studio 2019 or similar IDE
1. Rebuild whole solution
1. Set CVService.Api as start-up project
1. Run the project
1. You should now be presented with a Swagger API Documentation page
1. Feel free to use swagger or postman or any other API testing solution you wish
1. To run all the tests in the testing project make sure the API is still running
1. Then open another copy of the solution, right click "CVService.Api.Tests" project and select "Run All Tests".

**PLEASE NOTE: All requests except those to swagger or the maintenance controller need to be authorised with a token.**

Add a header to all requests with the name "PrivateAccessToken" and value "84157CEC-965E-4680-BDD8-AFFD81AD0D2A"

## Tests
There are also unit, integration and acceptance tests included in a separate project.
To test, make sure the API service is running, then right click on the CVService.Api.Tests project and select "Run all tests"

The integration tests have been configured to run serially, it was very important that each integration test start from a clean state with seeded data. When ran in parallel the data would be modified in many different ways and it would be impossible to guarantee the validity of each test run.

**There is a full acceptance test located in a class called "AcceptanceTests.cs", which has one test method written in XUnit. "Demo()".
This will give a good end to end look at most of the API and its Endpoints.**

## Notes
The application uses one view model for all HttpMethods which means that when posting the model to the web API it is possible to include the ID field in the model. This will cause an error as we want to stop this. In a production environment with a longer time frame this would be handled by either creating a separate view model for each Http Method, e.g. one for [GET], one for [POST] (that doesn’t have the Id field), [PUT], [DELETE] OR by going into the swagger config and specifically changing the response for Ids in [POST]s.

I have rolled my own authorisation middleware to show the basic concept of authorisation. In a real world app I would implement something like OAuth2 and use attributes to make sure every endpoint has the required permissions/roles.
If you have a look in the "CompanyHistoryController.cs" line: 108 you will see a sudo coded example of more detailed authorisation.  

if (!_securityModule.DoesUserHaveApiWriteAccess(request.header["token"]))
            //{ throw Forbidden }


There are some classes that extend generic types that do not implement any further functionality e.g. "SkillBusinessLogic.cs" and "SkillRepository.cs", the idea is that in the future when new features come in they can be extended easily.

There is a generic method in the "CvRepository.cs" called DoesEntityExistInCvAsync<T>, this is used to find if a Skill or Company History exists on a specific CV the idea behind this is if in the future more navigation properties are added to a Cv. e.g. EducationDetails or References this method can be used to see if those also exist for a specific CV. Which means the functionality only needs to be tested once.

There are several TODO comments dotted around the solution that help explain certain decisions that were made for the demo. These all start with the following "TODO: Tech Test - "
