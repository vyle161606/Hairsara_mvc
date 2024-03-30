using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Owin;
using Owin;
using System;
using System.IO;

[assembly: OwinStartup(typeof(DoAnCoSo.Startup))]

namespace DoAnCoSo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();

        }
    }
}
