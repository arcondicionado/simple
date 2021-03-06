﻿using System;

namespace Simple.Services.Default
{
    public class DefaultHostProvider : DefaultHostBaseProvider, IServiceHostProvider
    {
        #region IServiceHostProvider Members

        public void Host(object server, Type contract)
        {
            ServiceLocationFactory.Do[ConfigCache].Set(server, contract);
        }

        public void Start()
        {
        }

        public void Stop()
        {
            ServiceLocationFactory.Do[ConfigCache].Clear();
        }

        #endregion

        protected override void OnConfig(DefaultHostConfig config)
        {
        }

        protected override void OnClearConfig()
        {
            ServiceLocationFactory.Do[ConfigCache].Clear();
        }
    }
}
