﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace AntiCulture.Kid
{
    public class PictureCache : AbstractPictureCache
    {
        #region Fields
        private static Random random;

        private static readonly string relativePath = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0,System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\')) + "/ImageCache";
        #endregion

        #region Constructor
        static PictureCache()
        {
            random = new Random();
        }
        #endregion

        #region Public Methods
        public override Image GetCachedImage(string imageName)
        {
            Image image;

            image = TryGetImageFromDisk(imageName);

            byte[] fileContent;
            
            if (image == null)
            {
                image = TryGetImageFromWeb(imageName, out fileContent);
                if (fileContent != null)
                {
                    try
                    {
                        WriteJpegImageOnDisk(fileContent, imageName);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return image;
        }

        #endregion

        #region Private Methods
        private Image TryGetImageFromDisk(string imageName)
        {
            if (!File.Exists(relativePath + "/" + imageName + ".jpg"))
                return null;

            Stream imageStreamSource = new FileStream(relativePath + "/" + imageName + ".jpg", FileMode.Open, FileAccess.Read, FileShare.Read);

            return GetJpegImageFromStream(imageStreamSource);
        }

        private Image GetPngImageFromStream(Stream imageStreamSource)
        {
            PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            BitmapSource bitmapSource = decoder.Frames[0];

            Image image = new Image();
            image.Source = bitmapSource;

            return image;
        }

        private Image GetJpegImageFromStream(Stream imageStreamSource)
        {
            try
            {
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                BitmapSource bitmapSource = decoder.Frames[0];

                Image image = new Image();
                image.Source = bitmapSource;
                return image;
            }
            catch (IOException)
            {
                return null;
            }
            catch (FileFormatException)
            {
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }

        private void WritePngImageOnDisk(string imageName, Image image)
        {
            Stream imageStreamSource = new FileStream(relativePath + "/" + imageName + ".png", FileMode.Create, FileAccess.Write, FileShare.Write);

            PngBitmapEncoder encoder = new PngBitmapEncoder();

            BitmapPalette palette = BitmapPalettes.Halftone256;

            int stride = (int)image.Width;
            byte[] pixels = new byte[(int)image.Height * stride];


            BitmapSource bitmapSource = BitmapSource.Create((int)image.Width, (int)image.Height, 72, 72, PixelFormats.Indexed8, palette, pixels,stride);


            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(imageStreamSource);
        }

        private void WriteJpegImageOnDisk(byte[] fileContent, string imageName)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(relativePath + "/" + imageName + ".jpg", FileMode.Create)))
            {
                binaryWriter.Write(fileContent);
            }
        }

        private Image TryGetImageFromWeb(string imageName, out byte[] data)
        {
            string imageUrl = GetImageUrl(imageName);

            if (imageUrl == null)
            {
                data = null;
                return null;
            }

            data = WebExplorer.GetBytesFromUrl(imageUrl);
            MemoryStream stream = new MemoryStream(data);
            

            return GetJpegImageFromStream(stream);
        }

        private string GetImageUrl(string imageName)
        {
            //string pageContent = WebExplorer.GetStringPageContent("http://images.google.com/images?as_q=" + imageName.Replace('_', '+') + "&um=1&hl=fr&btnG=Recherche+Google&as_epq=&as_oq=&as_eq=&imgtype=&imgsz=small|medium|large&as_filetype=&imgc=&as_sitesearch=&safe=active&as_st=y");
            string pageContent = WebExplorer.GetStringPageContent("http://images.google.com/images?as_q=" + imageName.Replace('_', '+') + "&um=1&hl=fr&btnG=Recherche+Google&as_epq=&as_oq=&as_eq=&imgtype=&imgsz=small|medium|large&as_filetype=&imgc=&as_sitesearch=&safe=images&as_st=y");       


            string[] separator = new string[1];
            separator[0] = "imgurl\\x3d";

            List<string> imageUrls = new List<string>(pageContent.Split(separator, 100, 0));


            if (imageUrls.Count < 2)
                return null;

            string url = imageUrls[random.Next(1,imageUrls.Count - 1)];

            if (!url.Contains(".jpg"))
                return null;

            url = url.Substring(0, url.IndexOf(".jpg") + 4);

            return url;
        }
        #endregion
    }
}