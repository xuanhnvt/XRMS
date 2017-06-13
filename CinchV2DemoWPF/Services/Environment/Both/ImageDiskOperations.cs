using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using System.Xml;

using MEFedMVVM.ViewModelLocator;

namespace CinchV2DemoWPF
{

    public static class CustomXElementExtensions
    {
        public static string SafeValue(this XElement input)
        {
            return (input == null) ? string.Empty : (string)input.Value;
        }
    }


    /// <summary>
    /// Runtime/Deigntime implementation of the 
    /// ImageDiskOperations service used by the <c>ImageLoaderViewModel</c> to save/open data
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Both, typeof(IImageDiskOperations))]
    public class ImageDiskOperations : IImageDiskOperations
    {
        #region IImageDiskOperations Members

        /// <summary>
        /// Saves viewModelsToSave to a XML file, this demonstrates the use of
        /// the <c>SaveFileService</c> from the <c>ImageLoaderViewModel</c>
        /// </summary>
        public bool Save(string fileName, IEnumerable<ImageViewModel> viewModelsToSave)
        {

            CreateInitialFile(fileName, viewModelsToSave.First());
            IQueryable<ImageViewModel> allButFirst = 
                viewModelsToSave.Skip(1).AsQueryable<ImageViewModel>();

            foreach (ImageViewModel imageVM in allButFirst)
	        {
                AppendToFile(fileName, imageVM);
	        }
            return true;
        }


        /// <summary>
        /// retusn a  List<ImageViewModel> from an XML file, this demonstrates the use of
        /// the <c>OpenFileService</c> from the <c>ImageLoaderViewModel</c>
        /// </summary>
        public List<ImageViewModel> Open(string fileName)
        {
            var xmlImageViewModelResults =
                from imageVM in StreamElements(fileName, "ImageVM")
                select new ImageViewModel
                {
                    ImagePath = imageVM.Element("ImagePath").SafeValue(),
                    FileName = imageVM.Element("FileName").SafeValue(),
                    FileDate = DateTime.Parse(imageVM.Element("FileDate").SafeValue()),
                    FileExtension = imageVM.Element("FileExtension").SafeValue(),
                    FileSize = int.Parse(imageVM.Element("FileSize").SafeValue()),
                    Rating = int.Parse(imageVM.Element("Rating").SafeValue())
                };

            return xmlImageViewModelResults.ToList();
        }

        #endregion
        #region Private Methods

        public static IEnumerable<XElement> StreamElements(string uri, string name)
        {
            using (XmlReader reader = XmlReader.Create(uri))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) &&
                      (reader.Name == name))
                    {
                        XElement element = (XElement)XElement.ReadFrom(reader);
                        yield return element;
                    }
                }
                reader.Close();
            }
        }


        private static void AppendToFile(string fullXmlPath, ImageViewModel imageVM)
        {
            XElement imagesVM_XMLDocument = XElement.Load(fullXmlPath);
            imagesVM_XMLDocument.Add(new XElement("ImageVM",
                        new XElement("ImagePath", imageVM.ImagePath),
                        new XElement("FileName", imageVM.FileName),
                        new XElement("FileDate", imageVM.FileDate),
                        new XElement("FileExtension", imageVM.FileExtension),
                        new XElement("FileSize", imageVM.FileSize),
                        new XElement("Rating", imageVM.Rating)));

            imagesVM_XMLDocument.Save(fullXmlPath);
        }

        private static void CreateInitialFile(string fullXmlPath, ImageViewModel imageVM)
        {
            XElement imagesVM_XMLDocument =
                new XElement("AllImageViewModels",
                    new XElement("ImageVM",
                        new XElement("ImagePath", imageVM.ImagePath),
                        new XElement("FileName", imageVM.FileName),
                        new XElement("FileDate", imageVM.FileDate),
                        new XElement("FileExtension", imageVM.FileExtension),
                        new XElement("FileSize", imageVM.FileSize),
                        new XElement("Rating", imageVM.Rating))
                );
            imagesVM_XMLDocument.Save(fullXmlPath);
        }
        #endregion

    }
}
