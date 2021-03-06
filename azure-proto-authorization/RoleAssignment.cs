﻿using System.Threading.Tasks;
using Azure.ResourceManager.Core;

namespace azure_proto_authorization
{
    /// <summary>
    /// A Role Assignment for Role-based access control in ARM
    /// </summary>
    public class RoleAssignment : RoleAssignmentOperations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAssignment"/> class.
        /// </summary>
        /// <param name="operations"> The operations class to copy the http settings from. </param>
        /// <param name="data"> The properties of the resource. </param>
        internal RoleAssignment(OperationsBase operations, RoleAssignmentData data)
            : base(operations, data?.Id)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the properties of the RoleAssignment.
        /// </summary>
        public RoleAssignmentData Data { get; }

        /// <inheritdoc />
        protected override RoleAssignment GetResource()
        {
            return this;
        }

        /// <inheritdoc />
        protected override Task<RoleAssignment> GetResourceAsync()
        {
            return Task.FromResult(this);
        }
    }
}
