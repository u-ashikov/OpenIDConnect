﻿namespace ImageGallery.Client.Common;

public static class WebConstants
{
    public static class ApiClient
    {
        public const string GetAllImagesEndpoint = "/api/images";

        public const string ImageByIdEndpoint = "/api/images/{0}";
    }
    
    public static class Messages
    {
        public const string NullImage = "The image must not be null.";
    }
}