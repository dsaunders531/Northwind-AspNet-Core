﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tools.Extensions;
using tools.Utility;
using tools;

namespace Northwind.DAL.Models
{
    // TODO add encryption method.

    /// <summary>
    /// The custom IdentityUser model.
    /// </summary>
    public class UserProfileModel : IdentityUser
    {
        // Nothing to add here yet...     
    }
}
