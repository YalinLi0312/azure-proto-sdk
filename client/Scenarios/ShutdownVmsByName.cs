﻿using azure_proto_compute;
using Azure.ResourceManager.Core;
using System;

namespace client
{
    class ShutdownVmsByName: Scenario
    {
        public override void Execute()
        {
            var createMultipleVms = new CreateMultipleVms(Context);
            createMultipleVms.Execute();

            var sub = new AzureResourceManagerClient().GetSubscriptionOperations(Context.SubscriptionId);

            foreach(var armResource in sub.ListVirtualMachinesByName("even"))
            {
                var vmOperations = VirtualMachineOperations.FromGeneric(armResource);
                Console.WriteLine($"Stopping {armResource.Id.Name}");
                vmOperations.PowerOff();
                Console.WriteLine($"Starting {armResource.Id.Name}");
                vmOperations.PowerOn();
            }
        }
    }
}
