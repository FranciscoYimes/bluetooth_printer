using System;
using System.Collections.Generic;
using System.IO;
using Android.Graphics;
using Java.IO;
using ZXing;
using ZXing.Common;
using ZXing.PDF417;
using ZXing.PDF417.Internal;
namespace bluetoothprint.Droid.Services
{
    public static class BarcodeGenerator
    {
        public static byte[] generatePDF147(String ted)
        {

            Bitmap bitmap = null;
            Bitmap barcode = null;

            ted = ted.Replace("\n", "").Replace(" ", "");

            var writer = new PDF417Writer();

            Dictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, Object>();

            Dimensions dimensions = new Dimensions(1, 10, 1, 150);

            hints.Add(EncodeHintType.PDF417_DIMENSIONS, dimensions);
            hints.Add(EncodeHintType.CHARACTER_SET, "ISO-8859-1");
            hints.Add(EncodeHintType.ERROR_CORRECTION, "4");
            hints.Add(EncodeHintType.PDF417_COMPACTION, Compaction.BYTE);

            BitMatrix bm = writer.encode(ted, BarcodeFormat.PDF_417, 700, 390, hints);

            int matrixWidth = bm.Width;
            int matrixHeight = bm.Height;

            bitmap = Bitmap.CreateBitmap(matrixWidth, matrixHeight, Bitmap.Config.Argb8888);

            int[] data = new int[bitmap.Width * bitmap.Height];
            int offset;

            for (int y = 0; y < bitmap.Height; y++)
            {
                offset = y * bitmap.Width;

                for (int x = 0; x < bitmap.Width; x++)
                {
                    data[offset + x] = bm[x, y] ? Color.Black : Color.White;

                }
            }


            bitmap.SetPixels(data, 0, matrixWidth, 0, 0, matrixWidth, matrixHeight);

            Matrix matrix = new Matrix();

            matrix.PostRotate(90);

            matrix.PostScale(1.2f, 0.64f);

            int newWidth = bitmap.Width;

            int newHeight = bitmap.Height;

            barcode = Bitmap.CreateBitmap(bitmap, 0, 0, newWidth, newHeight, matrix, true);


            var stream = new MemoryStream();
            barcode.Compress(Bitmap.CompressFormat.Png, 0, stream);
            byte[] bitmapData = stream.ToArray();

            return bitmapData;

        }
    }
}
