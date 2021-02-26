﻿using Azure.ResourceManager.Core;
using azure_proto_compute;
using azure_proto_network;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace client
{
    class CheckResourceGroupOpsAsync : Scenario
    {
        public CheckResourceGroupOpsAsync() : base() { }

        public CheckResourceGroupOpsAsync(ScenarioContext context) : base(context) { }

        public override void Execute()
        {
            ExecuteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async System.Threading.Tasks.Task ExecuteAsync()
        {
            var client = new AzureResourceManagerClient();
            var subscription = client.GetSubscriptionOperations(Context.SubscriptionId);

            // Create Resource Group
            Console.WriteLine($"--------Start create group {Context.RgName}--------");
            var resourceGroup = subscription.GetResourceGroupContainer().Construct(Context.Loc).CreateOrUpdate(Context.RgName).Value;
            CleanUp.Add(resourceGroup.Id);
            var rgOps = subscription.GetResourceGroupOperations(Context.RgName);

            ShouldThrow<ArgumentException>(() => rgOps.AddTag("", ""), "AddTag with empty string didn't throw");
            //ShouldThrow<ArgumentException>(async () => await rgOps.AddTagAsync(null, null), "AddTagAsync with null string didn't throw");
            ShouldThrow<ArgumentException>(() => rgOps.StartAddTag("", null), "AddTagAsync with empty string didn't throw");
            //ShouldThrow<ArgumentException>(async () => await rgOps.StartAddTagAsync(" ", "test"), "StartAddTagAsync with whitespaces only string didn't throw");

            // Create AvailabilitySet
            Console.WriteLine("--------Create AvailabilitySet async--------");
            var aset = (await (await resourceGroup.GetAvailabilitySetContainer().Construct("Aligned").StartCreateOrUpdateAsync(Context.VmName + "_aSet")).WaitForCompletionAsync()).Value;
            var data = aset.Get().Value.Data;

            ShouldThrow<ArgumentException>(() => rgOps.CreateResource<AvailabilitySetContainer, AvailabilitySet, AvailabilitySetData>("", data), "CreateResource with empty string didn't throw");
            //ShouldThrow<ArgumentException>(async () => await rgOps.CreateResourceAsync<AvailabilitySetContainer, AvailabilitySet, AvailabilitySetData>(" ", data), "CreateResourceAsync with whitespaces string didn't throw");

            ShouldThrow<ArgumentNullException>(() => rgOps.SetTags(null), "SetTags with null didn't throw");
            //ShouldThrow<ArgumentNullException>(async () => await rgOps.SetTagsAsync(null), "SetTagsAsync with null didn't throw");
            ShouldThrow<ArgumentNullException>(() => rgOps.StartSetTags(null), "StartSetTags with null didn't throw");
            //ShouldThrow<ArgumentNullException>(async () => await rgOps.StartSetTagsAsync(null), "StartSetTagsAsync with null didn't throw");

            ShouldThrow<ArgumentException>(() => rgOps.RemoveTag(""), "RemoveTag with empty string didn't throw");
            //ShouldThrow<ArgumentException>(async () => await rgOps.RemoveTagAsync(null), "RemoveTagAsync with null didn't throw");
            ShouldThrow<ArgumentException>(() => rgOps.StartRemoveTag(" "), "StartRemoveTag with whitespace string didn't throw");
            //ShouldThrow<ArgumentException>(async () => await rgOps.StartRemoveTagAsync(null), "StartRemoveTagAsync with null didn't throw");

            ShouldThrow<ArgumentNullException>(() => rgOps.CreateResource<AvailabilitySetContainer, AvailabilitySet, AvailabilitySetData>("tester", null), "CreateResource model exception not thrown");
            ShouldThrow<ArgumentNullException>(async () => await rgOps.CreateResourceAsync<AvailabilitySetContainer, AvailabilitySet, AvailabilitySetData>("tester", null), "CreateResourceAsync model exception not thrown");

            Console.WriteLine("--------Done--------");
        }

        private static void ShouldThrow<T>(Action lambda, string failMessage)
        {
            try
            {
                lambda();
                throw new Exception(failMessage);
            }
            catch (Exception e) when (e.GetType() == typeof(T))
            {
                Console.WriteLine("Exception was thrown as expected.");
            }
        }

    }
}
