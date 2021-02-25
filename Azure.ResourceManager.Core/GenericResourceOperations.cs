﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Azure.ResourceManager.Resources;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.ResourceManager.Core
{
    /// <summary>
    /// A class representing the operations that can be performed over a specific ArmResource.
    /// </summary>
    public class GenericResourceOperations : ResourceOperationsBase<GenericResource>, ITaggableResource<GenericResource>, IDeletableResource
    {
        private readonly string _apiVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericResourceOperations"/> class.
        /// </summary>
        /// <param name="operations"> The resource operations to copy the options from. </param>
        /// <param name="id"> The identifier of the resource that is the target of operations. </param>
        internal GenericResourceOperations(ResourceOperationsBase operations, ResourceIdentifier id)
            : base(operations, id)
        {
            _apiVersion = "BAD VALUE";
        }

        /// <inheritdoc/>
        protected override ResourceType ValidResourceType => ResourceGroupOperations.ResourceType;

        private ResourcesOperations Operations => new ResourcesManagementClient(
            BaseUri,
            Id.Subscription,
            Credential,
            ClientOptions.Convert<ResourcesManagementClientOptions>()).Resources;

        /// <summary>
        /// Delete the resource.
        /// </summary>
        /// <returns> The status of the delete operation. </returns>
        public ArmResponse<Response> Delete()
        {
            return new ArmResponse(Operations.StartDeleteById(Id, _apiVersion).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Delete the resource.
        /// </summary>
        /// <param name="cancellationToken"> A token allowing immediate cancellation of any blocking call performed during the deletion. </param>
        /// <returns> A <see cref="Task"/> that on completion returns the status of the delete operation. </returns>
        public async Task<ArmResponse<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartDeleteByIdAsync(Id, _apiVersion, cancellationToken);
            var result = await operation.WaitForCompletionAsync(cancellationToken);
            return new ArmResponse(result);
        }

        /// <summary>
        /// Delete the resource.
        /// </summary>
        /// <param name="cancellationToken"> A token allowing immediate cancellation of any blocking call performed during the deletion. </param>
        /// <returns> A <see cref="ArmOperation{Response}"/> which allows the caller to control polling and waiting for resource deletion.
        /// The operation yields the final http response to the delete request when complete. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning">Details on long running operation object.</see>
        /// </remarks>
        public ArmOperation<Response> StartDelete(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.StartDeleteById(Id, _apiVersion, cancellationToken));
        }

        /// <summary>
        /// Delete the resource.  This call returns a Task that blocks until the delete operation is accepted on the service.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service.
        /// The default value is <see cref="P:System.Threading.CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that on completion returns a <see cref="ArmOperation{Response}"/> which
        /// allows the caller to control polling and waiting for resource deletion.
        /// The operation yields the final http response to the delete request when complete. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning">Details on long running operation object.</see>
        /// </remarks>
        public async Task<ArmOperation<Response>> StartDeleteAsync(CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartDeleteByIdAsync(Id, _apiVersion, cancellationToken);
            return new ArmVoidOperation(operation);
        }

        /// <summary>
        /// Add a tag to the resource.
        /// </summary>
        /// <param name="key"> The tag key. </param>
        /// <param name="value"> The tag value. </param>
        /// <returns>An <see cref="ArmResponse{TOperations}"/> that allows the user to control polling and waiting for Tag completion.</returns>
        public ArmResponse<GenericResource> AddTag(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            UpdateTags(key, value, resource.Data.Tags);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <summary>
        /// Add a tag to the resource.
        /// </summary>
        /// <param name="key"> The tag key. </param>
        /// <param name="value"> The tag value. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service.
        /// The default value is <see cref="P:System.Threading.CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that performs the Tag operation.  The Task yields an an
        /// <see cref="ArmResponse{TOperations}"/> that allows the user to control polling and waiting for
        /// Tag completion. </returns>
        public async Task<ArmResponse<GenericResource>> AddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            UpdateTags(key, value, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public ArmOperation<GenericResource> StartAddTag(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            UpdateTags(key, value, resource.Data.Tags);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public async Task<ArmOperation<GenericResource>> StartAddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            UpdateTags(key, value, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public override ArmResponse<GenericResource> Get()
        {
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.GetById(Id, _apiVersion),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public override async Task<ArmResponse<GenericResource>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await Operations.GetByIdAsync(Id, _apiVersion, cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        protected override void Validate(ResourceIdentifier identifier)
        {
            // TODO: Reenable after Azure.ResourceManager.Resource model has been regenerated
            // Currently test cases uses GenericResourceExpended that does not allow construction
            // with id.
            // if (identifier is null)
            //    throw new ArgumentNullException(nameof(identifier));
        }

        /// <inheritdoc/>
        public ArmResponse<GenericResource> SetTags(IDictionary<string, string> tags)
        {
            if (tags is null)
                throw new ArgumentNullException(nameof(tags));

            GenericResource resource = GetResource();
            ReplaceTags(tags, resource.Data.Tags);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public async Task<ArmResponse<GenericResource>> SetTagsAsync(IDictionary<string, string> tags, CancellationToken cancellationToken = default)
        {
            if (tags is null)
                throw new ArgumentNullException(nameof(tags));

            GenericResource resource = GetResource();
            ReplaceTags(tags, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public ArmOperation<GenericResource> StartSetTags(IDictionary<string, string> tags)
        {
            if (tags is null)
                throw new ArgumentNullException(nameof(tags));

            GenericResource resource = GetResource();
            ReplaceTags(tags, resource.Data.Tags);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public async Task<ArmOperation<GenericResource>> StartSetTagsAsync(IDictionary<string, string> tags, CancellationToken cancellationToken = default)
        {
            if (tags is null)
                throw new ArgumentNullException(nameof(tags));

            GenericResource resource = GetResource();
            ReplaceTags(tags, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public ArmResponse<GenericResource> RemoveTag(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            DeleteTag(key, resource.Data.Tags);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public async Task<ArmResponse<GenericResource>> RemoveTagAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            DeleteTag(key, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmResponse<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public ArmOperation<GenericResource> StartRemoveTag(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            DeleteTag(key, resource.Data.Tags);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                Operations.StartUpdateById(Id, _apiVersion, resource.Data).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                v => new GenericResource(this, new GenericResourceData(v)));
        }

        /// <inheritdoc/>
        public async Task<ArmOperation<GenericResource>> StartRemoveTagAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            GenericResource resource = GetResource();
            DeleteTag(key, resource.Data.Tags);
            var op = await Operations.StartUpdateByIdAsync(Id, _apiVersion, resource.Data, cancellationToken);
            return new PhArmOperation<GenericResource, ResourceManager.Resources.Models.GenericResource>(
                await op.WaitForCompletionAsync(cancellationToken),
                v => new GenericResource(this, new GenericResourceData(v)));
        }
    }
}
