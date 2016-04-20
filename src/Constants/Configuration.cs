using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Constants
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Configuration
    {
        #region Authorization Server
        public const string ASBaseAddress = "http://localhost:23194"; //AS BaseAddress
        //public const string ASBaseAddress = "http://development.chinacloudsites.cn/";

        public const string AuthorizeEndpoint = ASBaseAddress + "/connect/authorize";
        public const string LogoutEndpoint = ASBaseAddress + "/connect/endsession";
        public const string TokenEndpoint = ASBaseAddress + "/connect/token";
        public const string UserInfoEndpoint = ASBaseAddress + "/connect/userinfo";
        public const string IdentityTokenValidationEndpoint = ASBaseAddress + "/connect/identitytokenvalidation";
        public const string TokenRevocationEndpoint = ASBaseAddress + "/connect/revocation";
        public const string IntrospectionEndpoint = ASBaseAddress + "/connect/introspect";
        #endregion

        #region Clients
        
        public const string CodeCallBackUrl = "http://localhost:27898/Home/CallBack";
        //public const string CallBackUrl = "http://mvcclient.chinacloudsites.cn/Home/CallBack";
        #endregion
        #region Resource Server
        public const string RSBaseAddress = "http://localhost:2727/v1/";
        public const string CustomersAPI = "Customers";
        #endregion
    }
}
