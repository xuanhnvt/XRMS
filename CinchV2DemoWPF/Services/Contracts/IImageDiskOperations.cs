using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// Data service used by the <c>ImageLoaderViewModel</c> to carry out Save/open
    /// operations
    /// </summary>
    public interface IImageDiskOperations
    {
        /// <summary>
        /// Saves viewModelsToSave to a XML file, this demonstrates the use of
        /// the <c>SaveFileService</c> from the <c>ImageLoaderViewModel</c>
        /// </summary>
        bool Save(string fileName, IEnumerable<ImageViewModel> viewModelsToSave);

        /// <summary>
        /// retusn a  List<ImageViewModel> from an XML file, this demonstrates the use of
        /// the <c>OpenFileService</c> from the <c>ImageLoaderViewModel</c>
        /// </summary>
        List<ImageViewModel> Open(string fileName);
    }
}
