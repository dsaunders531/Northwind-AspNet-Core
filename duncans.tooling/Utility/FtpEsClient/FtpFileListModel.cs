// <copyright file="FtpFileListModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace duncans.Utility
{
    public class FtpFileListModel
    {
        public FtpFileType FileType { get; set; }

        public string Name { get; set; }

        public List<FtpFileListModel> Contents { get; set; }

        public string RelativePath { get; set; }
    }
}
