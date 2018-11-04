using System;
using System.Drawing;

namespace mezzanine.Utility
{
    /// <summary>
    /// Define the orientation of an image.
    /// </summary>
    public enum ImageOrientation
    {
        portrait = 0,
        landscape = 1,
        square = 2
    }
   
    /// <summary>
    /// Class to create an Image from an existing Image with a specific size.
    /// The output image is the specified size centered in both axis relative to its original size.
    /// </summary>
    public sealed class ImageManipulation
    {
        /// <summary>
        /// Change the size of the image so 1 of the sides matches the output size.
        /// </summary>
        /// <param name="inputImage"></param>
        /// <param name="outputSize"></param>
        /// <returns></returns>
        public Image Scale(ref Image inputImage, Size outputSize)
        {
            ImageOrientation inputOrientation;
            ImageOrientation outputOrientation;
            Single inputWHrelation;
            Single outputWHrelation;
            Image returnImg = null;
            Graphics DrawingSurface = null;
            Point ptCrop = new Point(0, 0);
            Single scaleFactor = 1;
            Size scaleSize;
            Boolean scaleToMatchOutputHeight = false;

            this.ValidateParameters(inputImage,  outputSize);

            if (inputImage.Size == outputSize)
            {
                // No need to do any work
                return inputImage;
            }
            else
            {
                // Scale and crop the image
                inputOrientation = this.ImageOrientation(inputImage.Size);
                outputOrientation = this.ImageOrientation(outputSize);
                inputWHrelation = this.WidthHeightRelation(inputImage.Size);
                outputWHrelation = this.WidthHeightRelation(outputSize);

                if (inputOrientation == Utility.ImageOrientation.square && outputOrientation == Utility.ImageOrientation.square)
                {
                    // The same type of orientation so scale only
                    returnImg = new Bitmap(outputSize.Width, outputSize.Height);
                    DrawingSurface = Graphics.FromImage(returnImg);
                    DrawingSurface.DrawImage(inputImage, new Rectangle(ptCrop, outputSize));
                }
                else
                {
                    // First scale the input image to intermediate image
                    scaleToMatchOutputHeight = (inputOrientation == Utility.ImageOrientation.landscape && outputOrientation == Utility.ImageOrientation.square)
                                                || (inputOrientation == Utility.ImageOrientation.square && outputOrientation == Utility.ImageOrientation.portrait)
                                                || (inputOrientation == Utility.ImageOrientation.landscape && outputOrientation == Utility.ImageOrientation.portrait)
                                                || ((inputOrientation == Utility.ImageOrientation.portrait && outputOrientation == Utility.ImageOrientation.portrait) && inputWHrelation >= outputWHrelation)
                                                || ((inputOrientation == Utility.ImageOrientation.landscape && outputOrientation == Utility.ImageOrientation.landscape) && inputWHrelation >= outputWHrelation);

                    if (scaleToMatchOutputHeight == true)
                    {
                        // the hieght of the input image needs to match the output height
                        scaleFactor = (Single)((Single)outputSize.Height / (Single)inputImage.Size.Height);

                        // Create the scale size - this way maintains the width height relation with more precision.
                        scaleSize = new Size((int)(inputImage.Size.Width * scaleFactor), 0);
                        scaleSize.Height = outputSize.Height; // (int)((Single)scaleSize.Width * (Single)(1 / inputWHrelation));
                    }
                    else
                    {
                        // the width of the input image needs to match the output width
                        scaleFactor = (Single)((Single)outputSize.Width / (Single)inputImage.Size.Width);

                        scaleSize = new Size(0, (int)(inputImage.Size.Height * scaleFactor));
                        scaleSize.Width = outputSize.Width; //(int)((Single)scaleSize.Height * inputWHrelation);
                    }

                    returnImg = new Bitmap(scaleSize.Width, scaleSize.Height);
                    DrawingSurface = Graphics.FromImage(returnImg);
                    DrawingSurface.DrawImage(inputImage, new Rectangle(ptCrop, scaleSize), new Rectangle(ptCrop, inputImage.Size), GraphicsUnit.Pixel);
                }
            }

            if (DrawingSurface != null)
            {
                DrawingSurface.Dispose();
            }
            DrawingSurface = null;

            return returnImg;
        }

        public Image Scale(ref Image inputImage, int outputWidth, int outputHeight)
        {
            return this.Scale(ref inputImage, new Size(outputWidth, outputHeight));
        }

        /// <summary>
        /// Change the size and shape of the image to fit the output rectangle. The output is a centered image based on the size. 
        /// The size must be the same size or smaller than the image.
        /// </summary>
        /// <param name="inputImage"></param>
        /// <param name="outputSize"></param>
        /// <returns></returns>
        public Image Crop(ref Image inputImage, Size outputSize)
        {
            Point ptCrop = new Point(0, 0);
            Image returnImg = null;
            Graphics DrawingSurface = null;

            this.ValidateParameters(inputImage, outputSize);

            if (outputSize.Width > inputImage.Width || outputSize.Height > inputImage.Height)
            {
                throw new ArgumentException(@"The output size is greater than the input size.");
            }

            // At least one of the dimensions should be the same as output.
            // Get the crop points
            if (inputImage.Size.Height > outputSize.Height)
            {
                ptCrop.Y = (int)(((inputImage.Size.Height - outputSize.Height) / 2)) * -1;
            }

            if (inputImage.Size.Width > outputSize.Width)
            {
                ptCrop.X = (int)(((inputImage.Size.Width - outputSize.Width) / 2)) * -1;
            }

            // Now create the output image.
            returnImg = new Bitmap(outputSize.Width, outputSize.Height);
            DrawingSurface = Graphics.FromImage(returnImg);
            DrawingSurface.DrawImage(inputImage, ptCrop);

            // clear down
            if (DrawingSurface != null)
            {
                DrawingSurface.Dispose();
            }
            DrawingSurface = null;

            return returnImg;
        }

        public Image Crop(ref Image inputImage, int outputWidth, int outputHeight)
        {
            return this.Crop(ref inputImage, new Size(outputWidth, outputHeight));
        }

        public Image ScaleAndCrop(ref Image inputImage, Size outputSize)
        {
            Image intermediateImg = Scale(ref inputImage, outputSize);
            return this.Crop(ref intermediateImg, outputSize);
        }

        public Image ScaleAndCrop(ref Image inputImage, int outputWidth, int outputHeight)
        {
            return this.ScaleAndCrop(ref inputImage, new Size(outputWidth, outputHeight));
        }

        /// <summary>
        /// Validates the parameters. An exception is thrown when validation fails.
        /// </summary>
        /// <param name="inputImage">The image you want to scale and crop.</param>
        /// <param name="outputSize">The size in pixels of the output image.</param>
        private void ValidateParameters(Image inputImage, Size outputSize)
        {
            if (inputImage == null)
            {
                throw new ArgumentNullException(@"The image is not in the correct format or is null.");
            }

            if (outputSize == null)
            {
                throw new ArgumentNullException(@"The size argument cannot be null.");
            }

            if (outputSize.Height <= 0 || outputSize.Width <= 0)
            {
                throw new ArgumentException(@"The specified size must have a height and width greater than 0.");
            }
        }

        /// <summary>
        /// Return the ImageOrientation (portrait or landscape) of the specified size.
        /// </summary>
        /// <param name="imageSize">The size of the image.</param>
        /// <returns>An ImageOrientation.</returns>
        public ImageOrientation ImageOrientation(Size imageSize)
        {
            ImageOrientation retVal = Utility.ImageOrientation.landscape;
            if (imageSize.Height > imageSize.Width)
            {
                retVal = Utility.ImageOrientation.portrait;
            }
            else if(imageSize.Height == imageSize.Width)
            {
                retVal = Utility.ImageOrientation.square;
            }
            return retVal;
        }

        public ImageOrientation ImageOrientation(int imageWidth, int imageHeight)
        {
            return this.ImageOrientation(new Size(imageWidth, imageHeight));
        }

        /// <summary>
        /// Return the width of an size expressed as a percentage of its height.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Single WidthHeightRelation(Size value)
        {
            return (Single)((Single)value.Width / (Single)value.Height);
        }

        public Single WidthHeightRelation(int imageWidth, int imageHeight)
        {
            return this.WidthHeightRelation(new Size(imageWidth, imageHeight));
        }
    }
}
