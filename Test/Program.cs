// See https://aka.ms/new-console-template for more information
using Ater.Web.Util;

var bytes = ImageHelper.GenerateImageWithCaptcha("12DA");

// bytes to image file 
File.WriteAllBytes("captcha1.png", bytes);