﻿using Azure.Core;
using System;

namespace azure_proto_core
{
    /// <summary>
    /// TODO: split this into a base class for all Operations, and a base class for specific operations
    /// </summary>
    public abstract class OperationsBase
    {
        public OperationsBase(ArmClientContext context, ResourceIdentifier id, Location location = null, ArmClientOptions ClientOptions = null) : this(context, new ArmResource(id, location ?? Location.Default), ClientOptions) { }

        public OperationsBase(ArmClientContext context, Resource resource, ArmClientOptions ClientOptions = null)
        {
            Validate(resource?.Id);

            ClientContext = context;
            Id = resource.Id;
            TrackedResource trackedResource = resource as TrackedResource;
            DefaultLocation = trackedResource?.Location ?? Location.Default;
            Resource = resource;
            this.ClientOptions = ClientOptions;
        }

        public virtual ArmClientContext ClientContext { get; }

        protected virtual Resource Resource { get; set; }

        public virtual ResourceType ResourceType { get; }

        public virtual Location DefaultLocation { get; }

        public virtual ResourceIdentifier Id { get; }

        public virtual ArmClientOptions ClientOptions {get; protected set;}

        public virtual void Validate(ResourceIdentifier identifier)
        {
            if (SubscriptionContainerOperations.AzureResourceType != identifier && identifier?.Type != ResourceType)
            {
                throw new InvalidOperationException($"{identifier} is not a valid resource of type {ResourceType}");
            }
        }

        /// <summary>
        /// Note that this is currently adapting to underlying management clients - once generator changes are in, this would likely be unnecessary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <returns></returns>
        protected T GetClient<T>(Func<Uri, TokenCredential, T> creator)
        {
            return ClientContext.GetClient<T>(creator);
        }
    }
}
