using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// Enumerator for showing the type of relationship the application has with the user.
    /// </summary>
    public enum IdentityRelationship
    {
        Customer = 0,
        Employee = 1
    }

    /// <summary>
    /// Enumerator for showing which part of the application the role applies.
    /// </summary>
    public enum IdentityRoleZone
    {
        ShopFloor = 0,
        BackOffice = 1
    }
}
