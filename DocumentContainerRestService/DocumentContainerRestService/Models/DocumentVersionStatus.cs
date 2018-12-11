using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentContainerRestService.Models
{
    public enum DocumentVersionStatus
    {
        /// <summary>
        /// An old version (not the latest).
        /// </summary>
        Past = 0,

        /// <summary>
        /// A current version.
        /// </summary>
        Current = 1
    }
}