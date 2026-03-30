using System;
using System.Drawing;
using System.IO;

namespace CreateIcon
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new bitmap with 256x256 size
            using (Bitmap bitmap = new Bitmap(256, 256))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Fill with blue background (OZON brand color)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(0, 91, 255)), 0, 0, 256, 256);
                    
                    // Draw "OZON" text in white
                    using (Font font = new Font("Arial", 80, FontStyle.Bold))
                    {
                        StringFormat format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        g.DrawString("OZON", font, Brushes.White, new Rectangle(0, 0, 256, 256), format);
                    }
                }
                
                // Save the bitmap as ICO file
                using (FileStream fs = new FileStream("../ozon.ico", FileMode.Create))
                {
                    // Create icon from bitmap
                    using (Icon icon = Icon.FromHandle(bitmap.GetHicon()))
                    {
                        icon.Save(fs);
                    }
                }
            }
            
            Console.WriteLine("OZON icon created successfully!");
            Console.WriteLine("Icon saved to: ../ozon.ico");
        }
    }
}