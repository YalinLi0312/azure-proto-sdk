﻿using Azure;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Core;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_compute
{
    /// <summary>
    /// A class representing the operations that can be performed over a specific VirtualMachine
    /// </summary>
    public class VirtualMachineOperations : ResourceOperationsBase<VirtualMachine>, ITaggableResource<VirtualMachine>, IDeletableResource
    {
        public VirtualMachineOperations(ArmResourceOperations genericOperations)
            : base(genericOperations)
        {
        }

        internal VirtualMachineOperations(AzureResourceManagerClientOptions options, ResourceIdentifier id)
            : base(options, id)
        {
        }

        public static readonly ResourceType ResourceType = "Microsoft.Compute/virtualMachines";

        public static VirtualMachineOperations FromGeneric(ArmResourceOperations genericOperations)
        {
            return new VirtualMachineOperations(genericOperations);
        }

        public ArmResponse<Response> Delete()
        {
            return new ArmResponse(Operations.StartDelete(Id.ResourceGroup, Id.Name).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }

        public async Task<ArmResponse<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmResponse((await Operations.StartDeleteAsync(Id.ResourceGroup, Id.Name)).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }
        public ArmOperation<Response> StartDelete(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.StartDelete(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        public async Task<ArmOperation<Response>> StartDeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartDeleteAsync(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        #region PowerOn
        public ArmResponse<Response> PowerOn(CancellationToken cancellationToken = default)
        {
            var operation = Operations.StartStart(Id.ResourceGroup, Id.Name, cancellationToken);
            return new ArmResponse(operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult());
        }

        public async Task<ArmResponse<Response>> PowerOnAsync(CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartStartAsync(Id.ResourceGroup, Id.Name, cancellationToken).ConfigureAwait(false);
            return new ArmResponse(await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false));
        }

        public ArmOperation<Response> StartPowerOn(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.StartStart(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        public async Task<ArmOperation<Response>> StartPowerOnAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartStartAsync(Id.ResourceGroup, Id.Name, cancellationToken).ConfigureAwait(false));
        }
        #endregion

        #region PowerOff
        public ArmResponse<Response> PowerOff(bool? skipShutdown = null, CancellationToken cancellationToken = default)
        {
            var operation = Operations.StartPowerOff(Id.ResourceGroup, Id.Name, skipShutdown, cancellationToken);
            return new ArmResponse(operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult());
        }

        public async Task<ArmResponse<Response>> PowerOffAsync(bool? skipShutdown = null, CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartPowerOffAsync(Id.ResourceGroup, Id.Name, skipShutdown, cancellationToken).ConfigureAwait(false);
            return new ArmResponse(await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false));
        }

        public ArmOperation<Response> StartPowerOff(bool? skipShutdown = null, CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.StartPowerOff(Id.ResourceGroup, Id.Name, skipShutdown, cancellationToken));
        }

        public async Task<ArmOperation<Response>> StartPowerOffAsync(bool? skipShutdown = null, CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartPowerOffAsync(Id.ResourceGroup, Id.Name, skipShutdown, cancellationToken).ConfigureAwait(false));
        }
        #endregion

        public override ArmResponse<VirtualMachine> Get()
        {
            return new PhArmResponse<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                Operations.Get(Id.ResourceGroup, Id.Name),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        public async override Task<ArmResponse<VirtualMachine>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                await Operations.GetAsync(Id.ResourceGroup, Id.Name, cancellationToken),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        public ArmOperation<VirtualMachine> Update(VirtualMachineUpdate patchable)
        {
            return new PhArmOperation<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                Operations.StartUpdate(Id.ResourceGroup, Id.Name, patchable),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        public async Task<ArmOperation<VirtualMachine>> UpdateAsync(VirtualMachineUpdate patchable, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                await Operations.StartUpdateAsync(Id.ResourceGroup, Id.Name, patchable, cancellationToken),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        public ArmOperation<VirtualMachine> AddTag(string key, string value)
        {
            var patchable = new VirtualMachineUpdate { Tags = new Dictionary<string, string>() };
            patchable.Tags.Add(key, value);
            return new PhArmOperation<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                Operations.StartUpdate(Id.ResourceGroup, Id.Name, patchable),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        public async Task<ArmOperation<VirtualMachine>> AddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            var patchable = new VirtualMachineUpdate { Tags = new Dictionary<string, string>() };
            patchable.Tags.Add(key, value);
            return new PhArmOperation<VirtualMachine, Azure.ResourceManager.Compute.Models.VirtualMachine>(
                await Operations.StartUpdateAsync(Id.ResourceGroup, Id.Name, patchable, cancellationToken),
                v =>
                {
                    Resource = new VirtualMachineData(v);
                    return new VirtualMachine(ClientOptions, Resource as VirtualMachineData);
                });
        }

        internal VirtualMachinesOperations Operations => GetClient<ComputeManagementClient>((baseUri, creds) =>
            new ComputeManagementClient(baseUri, Id.Subscription, creds, ClientOptions.Convert<ComputeManagementClientOptions>())).VirtualMachines;

        protected override ResourceType GetValidResourceType()
        {
            return ResourceType;
        }
    }
}
