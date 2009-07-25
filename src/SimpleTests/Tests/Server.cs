﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using Sample.BusinessServer.Rules;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using Simple.Services.Remoting;
using Simple.ConfigSource;
using Simple.Tests.Service;
using Simple.Services;
using System.Threading;
using Simple.Threading;


namespace Simple.Tests
{
    public class Server
    {
        public const string RemotingTest = "remotingtest";
        public const string RemotingInterceptorTest = "remotinginterceptortest";

        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == RemotingTest)
                {
                    var ev = NamedEvents.OpenOrCreate(RemotingTest, false, EventResetMode.ManualReset);

                    Guid guid = Guid.NewGuid();

                    RemotingSimply.Do.Configure(guid,
                        XmlConfig.LoadXml<RemotingConfig>(Helper.MakeConfig(new Uri(args[1]))));

                    Simply.Get(guid).Host(typeof(SimpleService));

                    ev.Set();
                    Console.ReadLine();
                }
                else if (args[0] == RemotingInterceptorTest)
                {
                    var ev = NamedEvents.OpenOrCreate(RemotingInterceptorTest, false, EventResetMode.ManualReset);

                    Guid guid = Guid.NewGuid();

                    RemotingSimply.Do.Configure(guid,
                        XmlConfig.LoadXml<RemotingConfig>(Helper.MakeConfig(new Uri(args[1]))));

                    Simply.Get(guid).Host(typeof(BaseInterceptorFixture.TestService), 
                        new BaseInterceptorFixture.Interceptor());

                    ev.Set();

                    Console.ReadLine();
                }
            }
            //            NUnit.Gui.AppEntry.Main(new string[] { Assembly.GetExecutingAssembly().Location });

            return 0;
        }
    }
}
