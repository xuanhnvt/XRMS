using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinchV2DemoWPF
{
    /// <summary>
    /// Simple interface that a View can use
    /// </summary>
    interface IWebBrowserNavigatable
    {
        void NavigateTo(string url);
    }
}
