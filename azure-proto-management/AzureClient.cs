﻿using azure_proto_core;
using System;

namespace azure_proto_management
{
    public class AzureClient : IResource
    {
        public SubscriptionCollection Subscriptions { get; private set; }

        public string Name => "MainClient";

        public string Id => "1";

        public ClientFactory Clients { get; set; }

        public object Data => throw new System.NotImplementedException();

        public AzureClient()
        {
            Subscriptions = new SubscriptionCollection(this);
        }

        public static AzureResourceGroup GetResourceGroup(string subscriptionId, string loc, string rgName, string vmName)
        {
            AzureClient client = new AzureClient();
            var subscription = client.Subscriptions[subscriptionId];
            var location = subscription.Locations[loc]; //intended to be removed
            return location.ResourceGroups[rgName];
        }
    }
}
