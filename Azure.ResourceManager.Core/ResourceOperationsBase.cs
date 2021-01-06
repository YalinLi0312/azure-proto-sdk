// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;

namespace Azure.ResourceManager.Core
{
    /// <summary>
    /// Base class for all operations over a resource.
    /// </summary>
    public abstract class ResourceOperationsBase : OperationsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class.
        /// </summary>
        /// <param name="options">The client parameters to use in these operations.</param>
        /// <param name="id">The identifier of the resource that is the target of operations.</param>
        public ResourceOperationsBase(AzureResourceManagerClientOptions options, ResourceIdentifier id)
            : base(options, id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase"/> class.
        /// </summary>
        /// <param name="options">The client parameters to use in these operations.</param>
        /// <param name="resource">The resource that is the target of operations.</param>
        public ResourceOperationsBase(AzureResourceManagerClientOptions options, Resource resource)
            : base(options, resource)
        {
        }
    }


    /// <summary>
    /// Base class for all operations over a resource
    /// </summary>
    /// <typeparam name="TOperations">The type implementing operations over the resource</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Types differ by type argument only")]
    public abstract class ResourceOperationsBase<TOperations> : ResourceOperationsBase
        where TOperations : ResourceOperationsBase<TOperations>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase{TOperations}"/> class.
        /// </summary>
        /// <param name="genericOperations">Generic ARMResourceOperations for this resource type</param>
        public ResourceOperationsBase(ArmResourceOperations genericOperations)
            : this(genericOperations.ClientOptions, genericOperations.Id)
        {
        }

        public ResourceOperationsBase(AzureResourceManagerClientOptions options, ResourceIdentifier id)
            : base(options, id)
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase{TOperations}"/> class.
        /// <param name="options">The http client options for these operations</param>
        /// <param name="id">The resource Id of this resource</param>
        {
        }

        public ResourceOperationsBase(AzureResourceManagerClientOptions options, Resource resource)
            : base(options, resource)
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOperationsBase{TOperations}"/> class.
        /// <param name="options">The http client options for these operations</param>
        /// <param name="resource">The object corresponding to this resource</param>
        {
        }

        /// <summary>
        /// Get details for this resource from the service.  This call will block until a response is returne from the service
        /// </summary>
        /// <returns>An Http response with the operations for this resource</returns>
        public abstract ArmResponse<TOperations> Get();

        /// <summary>
        /// Get details for thsi resource from the service.  This call returns a Task, which can  be used to control waiting
        /// for a response from the service.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the caller to cancel the call to the service.</param>
        /// <returns>A Task that is complete when a response is returned from the service.  The task yields the operations
        /// over this resource when complete.</returns>
        public abstract Task<ArmResponse<TOperations>> GetAsync(CancellationToken cancellationToken = default);
    }
}
