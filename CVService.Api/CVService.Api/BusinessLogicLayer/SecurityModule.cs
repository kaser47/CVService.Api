using System;

namespace CVService.Api
{
    //TODO: Tech Test - In the future this class could connect to a DB and return different access tokens based on
    //permissions and roles of different users.
    public class SecurityModule
    {
        public bool AuthoriseToken(string token)
        {
            var demoToken = "84157CEC-965E-4680-BDD8-AFFD81AD0D2A";
            //DB Lookup operations here to get relevant token from userid
            return Guid.Parse(demoToken).Equals(Guid.Parse(token));
        }
    }
}