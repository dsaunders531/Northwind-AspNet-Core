using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;

namespace tools.Utility
{
    /// <summary>
    /// Create zip type compressed files.
    /// </summary>
    public sealed class QuickZip
    {
        private const int _fileSize = 4096; // bytes in the stream

        /// <summary>
        /// Create a compressed packagePart (zip) file.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="outputPath"></param>
        public void Compress(string[] filePaths, string outputPath)
        {
            // most copied from "Zip Files Easy!" from codeProject
            // www.codeproject.com/articles/28107/Zip-Files-Easy
            if (File.Exists(outputPath) == true)
            {
                File.Delete(outputPath);
            }

            Package zip = ZipPackage.Open(outputPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            string contentType = System.Net.Mime.MediaTypeNames.Application.Zip;
            Uri commitUri;
            PackagePart packageP;
            byte[] fileBytes;

            foreach (string f in filePaths)
            {
                if (f != string.Empty)
                {
                    if (File.Exists(f) == false)
                    {
                        throw new FileNotFoundException(string.Format("The file {0} does not exist.", f));
                    }

                    commitUri = new System.Uri(string.Concat(Path.DirectorySeparatorChar, Path.GetFileName(f)), UriKind.Relative);

                    packageP = zip.CreatePart(commitUri, contentType, CompressionOption.NotCompressed);
                    fileBytes = File.ReadAllBytes(f);
                    packageP.GetStream(FileMode.Append, FileAccess.Write).Write(fileBytes, 0, fileBytes.Length);
                }

                fileBytes = null;
                packageP = null;
                commitUri = null;
            }

            zip.Flush();
            zip.Close();
            zip = null;
        }

        /// <summary>
        /// Extract all the files from a packagepart archive.
        /// </summary>
        /// <param name="inputZipFile"></param>
        /// <param name="extractToDir"></param>
        public void UnCompress(string inputZipFile, string extractToDir)
        {
            if (File.Exists(inputZipFile) == false)
            {
                throw new FileNotFoundException(string.Format("The file {0} does not exist.", inputZipFile));
            }

            Package zip = ZipPackage.Open(inputZipFile, FileMode.Open, FileAccess.Read);
            string partPath = string.Empty;

            foreach (PackagePart p in zip.GetParts())
            {
                partPath = p.Uri.OriginalString.Replace(Convert.ToChar(@"/"), Path.DirectorySeparatorChar);
                GetFileFromZipStream(p.GetStream(), extractToDir + partPath);
            }

            zip.Close();
            zip = null;
        }

        private void GetFileFromZipStream(Stream s, string outFile)
        {
            using (FileStream outF = File.Create(outFile))
            {
                byte[] b = new byte[_fileSize];
                int numread = s.Read(b, 0, _fileSize);
                while (numread != 0)
                {
                    outF.Write(b, 0, numread);
                    numread = s.Read(b, 0, _fileSize);
                }
            }
        }

        /// <summary>
        /// List all the files in a package (zip) file.
        /// </summary>
        /// <param name="inputZipFile"></param>
        /// <returns></returns>
        public List<string> ListZipContent(string inputZipFile)
        {
            if (File.Exists(inputZipFile) == false)
            {
                throw new FileNotFoundException(string.Format("The file {0} does not exist.", inputZipFile));
            }

            List<string> retList = new List<string>();

            Package zip = ZipPackage.Open(inputZipFile, FileMode.Open, FileAccess.Read);
            string partPath = string.Empty;

            foreach (PackagePart p in zip.GetParts())
            {
                partPath = p.Uri.OriginalString.Replace(Convert.ToChar(@"/"), Path.DirectorySeparatorChar);
                retList.Add(partPath);
            }

            zip.Close();
            zip = null;

            return retList;
        }

        /// <summary>
        /// Extract a file as a stream from an archive.
        /// </summary>
        /// <param name="inputZipFile"></param>
        /// <param name="extractPath"></param>
        /// <returns></returns>
        public MemoryStream ExtractStream(string inputZipFile, string extractPath)
        {
            MemoryStream output = null;

            extractPath = extractPath.Replace(Convert.ToChar(@"/"), Path.DirectorySeparatorChar);

            if (ListZipContent(inputZipFile).Contains(extractPath) == false)
            {
                throw new ArgumentException(string.Format("Path {0} was not found in {1}.", extractPath, inputZipFile));
            }

            Package zip = ZipPackage.Open(inputZipFile, FileMode.Open, FileAccess.Read);
            string partPath = string.Empty;

            foreach (PackagePart p in zip.GetParts())
            {
                partPath = p.Uri.OriginalString.Replace(Convert.ToChar(@"/"), Path.DirectorySeparatorChar);

                if (partPath == extractPath)
                {
                    output = (MemoryStream)p.GetStream();
                }
            }

            zip.Close();
            zip = null;

            return output;
        }
    }
}
