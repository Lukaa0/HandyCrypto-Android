using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Graphics;
using Android.Views;
using Android.Widget;

namespace HandyCrypto.Model
{
    public static class PaletteCreator
    {
        public static PaletteModel GetPalette(ImageView image)
        {
            BitmapDrawable bitmapDrawable = (BitmapDrawable)image.Drawable;
            var bitmap = bitmapDrawable.Bitmap;
            var palette = new Palette.Builder(bitmap).Generate();
            var defaultcolor = Color.ParseColor("#00363a");
            PaletteModel paletteModel = new PaletteModel
            {
                VibrantColor = palette.GetVibrantColor(defaultcolor),
                DarkVibrantColor = palette.GetDarkVibrantColor(defaultcolor),
                LightVibrantColor = palette.GetLightVibrantColor(defaultcolor),
                MutedColor = palette.GetMutedColor(defaultcolor),
                MutedDarkColor = palette.GetDarkMutedColor(defaultcolor),
                MutedLightColor = palette.GetLightMutedColor(defaultcolor)
            };
            return paletteModel;
        }
    }
    public class PaletteModel
    {
        public int VibrantColor { get; set; }
        public int MutedColor { get; set; }
        public int DarkVibrantColor { get; set; }
        public int LightVibrantColor { get; set; }
        public int MutedDarkColor { get; set; }
        public int MutedLightColor { get; set; }

    }
}