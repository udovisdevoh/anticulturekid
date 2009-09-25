using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AntiCulture.Kid
{
    public abstract class AbstractPictureCache
    {
        /// <summary>
        /// Returns the cached image from cache
        /// </summary>
        /// <param name="imageName">image name</param>
        /// <returns>Image from cache</returns>
        public abstract Image GetCachedImage(string imageName);
    }
}
