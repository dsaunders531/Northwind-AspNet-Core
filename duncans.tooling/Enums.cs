// <copyright file="Enums.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace duncans
{
    public enum ApiMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        PATCH = 3,
        DELETE = 4
    }

    public enum ApiActionParameterType
    {
        indeterminate = 0,
        jsonBody = 1,
        query = 2,
        route = 3,
        form = 4,
        header = 5
    }

    public enum FtpFileType
    {
        directory = 0,
        file = 1
    }

    public enum EFActionType
    {
        Create = 0,
        Update = 1,
        Delete = 2
    }

    public enum SortDirection
    {
        Ascending = 0,
        Decending = 1
    }

    /// <summary>
    /// Define the orientation of an image.
    /// </summary>
    public enum ImageOrientation
    {
        portrait = 0,
        landscape = 1,
        square = 2
    }
}
