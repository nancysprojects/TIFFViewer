                                                       *
//*                                                                                      *
//*  Overview:          Class to open TIFF images inside the browser                     *
//*                     image on the browser.                                            *
//*                                                                                      *
//*  ----------------------------------------------------------------------------------- *
//*Modification History                                                                  *
//*                                                                                      *
//*  Modified    By              Description   
//*                                                                                      *
//****************************************************************************************
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

namespace App.Files
{
    /// <summary>
    /// Summary description for TIF
    /// </summary>
    public class TIF
    {
        // Fields
        private bool m_Disposed = false;
        private Image m_Img = null;
        private int m_PageCount = -1;
        public String m_FilePathName;

        #region CONSTRUCTOR/DESTRUCTOR

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="FilePathName"></param>
        public TIF(String FilePathName)
        {
            if (FilePathName == null) throw new Exception("FilePathName cannot be NULL");
            if (FilePathName == "") throw new Exception("FilePathName cannot be NULL");
            m_FilePathName = FilePathName;
        }

        /// <summary>
        /// Disposes of any resources still available
        /// </summary>
        public void Dispose()
        {
            if (this.m_Img != null)
            {
                this.m_Img.Dispose();
            }
            this.m_Disposed = true;
        }


        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets the Page Count of the TIF
        /// </summary>
        public int PageCount
        {
            get
            {
                if (m_PageCount == -1)
                    this.m_PageCount = GetPageCount();

                return m_PageCount;
            }
        }


        #endregion

        #region METHODS

        /// <summary>
        /// Returns the page count of the TIF
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            int Pgs = -1;
            Image Img = null;
            try
            {
                Img = Image.FromFile(this.m_FilePathName);
                Pgs = Img.GetFrameCount(FrameDimension.Page);
                return Pgs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Img.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Returns an Image of a TIF page
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public Image GetTiffImage(int PageNum)
        {
            if ((PageNum < 1) | (PageNum > this.m_PageCount))
            {
                throw new InvalidOperationException("Page to be retrieved is outside the bounds of the total TIF file pages.  Please choose a page number that exists.");
            }
            MemoryStream ms = null;
            Image SrcImg = null;
            Image returnImage = null;
            try
            {
                SrcImg = Image.FromFile(this.m_FilePathName);
                ms = new MemoryStream();
                FrameDimension FrDim = new FrameDimension(SrcImg.FrameDimensionsList[0]);
                SrcImg.SelectActiveFrame(FrDim, PageNum - 1);
                SrcImg.Save(ms, ImageFormat.Tiff);
                returnImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SrcImg.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return returnImage;
        }

        /// <summary>
        /// Returns an Image of a TIF page, resized
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public Image GetTiffImageThumb(int PageNum, int ImgWidth, int ImgHeight)
        {
            if ((PageNum < 1) | (PageNum > this.PageCount))
            {
                throw new InvalidOperationException("Page to be retrieved is outside the bounds of the total TIF file pages.  Please choose a page number that exists.");
            }
            MemoryStream ms = null;
            Image SrcImg = null;
            Image returnImage = null;
            try
            {
                SrcImg = Image.FromFile(this.m_FilePathName);
                ms = new MemoryStream();
                FrameDimension FrDim = new FrameDimension(SrcImg.FrameDimensionsList[0]);
                SrcImg.SelectActiveFrame(FrDim, PageNum - 1);
                SrcImg.Save(ms, ImageFormat.Tiff);
                // Prevent using images internal thumbnail
                SrcImg.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                SrcImg.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                //Save Aspect Ratio
                if (SrcImg.Width <= ImgWidth) ImgWidth = SrcImg.Width;
                int NewHeight = SrcImg.Height * ImgWidth / SrcImg.Width;
                if (NewHeight > ImgHeight)
                {
                    // Resize with height instead
                    ImgWidth = SrcImg.Width * ImgHeight / SrcImg.Height;
                    NewHeight = ImgHeight;
                }
                //Return Image
                returnImage = Image.FromStream(ms).GetThumbnailImage(ImgWidth, NewHeight, null, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SrcImg.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return returnImage;
        }

        /// <summary>
        /// Returns an images beased on pages specified
        /// </summary>
        /// <param name="StartPageNum"></param>
        /// <param name="EndPageNum"></param>
        /// <returns></returns>
        public TIFPageCollection GetTiffImages(int StartPageNum, int EndPageNum)
        {
            TIFPageCollection Pgs = new TIFPageCollection();
            if (((StartPageNum < 1) | (EndPageNum > this.m_PageCount)) | (EndPageNum > StartPageNum))
            {
                throw new InvalidOperationException("Page being retrieved is outside the bounds of the total TIF file pages.  Please choose a page number that exists.");
            }
            try
            {
                int TotPgs = EndPageNum - StartPageNum;
                for (int i = 0; i <= TotPgs; i++)
                {
                    Pgs.Add(this.GetTiffImage(StartPageNum + i));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return Pgs;
        }

        /// <summary>
        /// Returns an images based on pages specified, resized
        /// </summary>
        /// <param name="StartPageNum"></param>
        /// <param name="EndPageNum"></param>
        /// <returns></returns>
        public System.Drawing.Image[] GetTiffImageThumbs(int StartPageNum, int EndPageNum, int ImgWidth, int ImgHeight)
        {
            TIFPageCollection Pgs = new TIFPageCollection();
            if (((StartPageNum < 1) || (EndPageNum > this.m_PageCount)) || (EndPageNum < StartPageNum))
            {
                throw new InvalidOperationException("Page being retrieved is outside the bounds of the total TIF file pages.  Please choose a page number that exists.");
            }
            Image[] returnImage = new Image[(EndPageNum - StartPageNum) + 1];
            try
            {
                int TotPgs = EndPageNum - StartPageNum;
                for (int i = 0; i <= TotPgs; i++)
                {
                    returnImage[i] = this.GetTiffImageThumb(StartPageNum + i, ImgWidth, ImgHeight);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return returnImage;
        }

        /// <summary>
        /// Returns an images based on pages specified, resized
        /// </summary>
        /// <param name="StartPageNum"></param>
        /// <param name="EndPageNum"></param>
        /// <returns></returns>
        public TIFPageCollection GetTiffImageThumbsCollection(int StartPageNum, int EndPageNum, int ImgWidth, int ImgHeight)
        {
            TIFPageCollection Pgs = new TIFPageCollection();
            if (((StartPageNum < 1) || (EndPageNum > this.m_PageCount)) || (EndPageNum < StartPageNum))
            {
                throw new InvalidOperationException("Page being retrieved is outside the bounds of the total TIF file pages.  Please choose a page number that exists.");
            }
            try
            {
                int TotPgs = EndPageNum - StartPageNum;
                for (int i = 0; i <= TotPgs; i++)
                {
                    Pgs.Add(this.GetTiffImageThumb(StartPageNum + i, ImgWidth, ImgHeight));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return Pgs;
        }

        /// <summary>
        /// Returns a Image of a specific page
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public Image this[int PageNum]
        {
            get
            {
                Image TiffPage;
                try
                {
                    this.m_Img = this.GetTiffImage(PageNum);
                    TiffPage = this.m_Img;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return TiffPage;
            }
        }

        #endregion

    }

    /// <summary>
    /// Collection of objects
    /// </summary>
    [Serializable]
    public class TIFPageCollection : System.Collections.CollectionBase
    {
        private bool m_Disposed = false;

        #region CONSTURCTORS

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TIFPageCollection() { }

        /// <summary>
        /// Disposes of the object by clearing the collection
        /// </summary>
        public void Dispose()
        {   //Make sure each image is disposed of properly
            foreach (System.Drawing.Image Img in this)
            {
                Img.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            this.m_Disposed = true;
            this.Clear();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        /// <param name="Obj"></param>
        public void Add(System.Drawing.Image Obj)
        {
            this.List.Add(Obj);
        }

        /// <summary>
        /// Indicates if the object exists in the collection
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool Contains(System.Drawing.Image Obj)
        {
            return this.List.Contains(Obj);
        }

        /// <summary>
        /// Returns the indexOf the object
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int IndexOf(System.Drawing.Image Obj)
        {
            return this.List.IndexOf(Obj);
        }

        /// <summary>
        /// Inserts an object into the collection at the specifed index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="Obj"></param>
        public void Insert(int index, System.Drawing.Image Obj)
        {
            this.List.Insert(index, Obj);
        }

        /// <summary>
        /// Removes the object from the collection
        /// </summary>
        /// <param name="Obj"></param>
        public void Remove(System.Drawing.Image Obj)
        {
            this.List.Remove(Obj);
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Returns a reference to the object at specified index
        /// </summary>
        /// <param name="index">Index of object</param>
        /// <returns></returns>
        public System.Drawing.Image this[int index]
        {
            get
            {
                return (System.Drawing.Image)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        #endregion

    }
}