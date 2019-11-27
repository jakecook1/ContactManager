using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ContactManagerWeb.Models;
using ContactManagerWeb.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerWeb.Helpers
{
    public static class ListExtensions
    {
        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderBy<T>(string sort, string defaultSort)
        {
            // Orderby
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = source => source.OrderBy(e => EF.Property<object>(e, defaultSort));

            if (string.IsNullOrEmpty(sort)) sort = defaultSort;

            bool descending = false;

            if (sort.EndsWith("_desc"))
            {
                sort = sort.Substring(0, sort.Length - 5);
                descending = true;
            }

            orderBy = source => descending
                ? source.OrderByDescending(e => EF.Property<object>(e, sort))
                : source.OrderBy(e => EF.Property<object>(e, sort));

            return orderBy;
        }

        public static List<T> AddNewImages<T>(this string imageUploadDetails)
        {
            var images = new List<T>();

            if (imageUploadDetails != null)
            {
                var imageDetails = Regex.Split(imageUploadDetails, @"\?!\*");

                foreach (var imageDetail in imageDetails)
                    images.Add(GetImage<T>(imageDetail));
            }

            return images;
        }

        public static List<T> DeleteImages<T>(this string deletedImages)
        {
            var images = new List<T>();

            if (deletedImages != null)
            {
                var imageDetails = Regex.Split(deletedImages, @"\?!\*");

                foreach (var imageDetail in imageDetails)
                    images.Add(GetDeletedImage<T>(imageDetail));
            }

            return images;
        }

        private static T GetImage<T>(string imageDetails)
        {
            var details = Regex.Split(imageDetails, @"#\$%");

            var image = (T)Activator.CreateInstance(typeof(T));

            var propertyInfo = image.GetType();
                
            var publicIdColumn = propertyInfo.GetProperty("PublicId");
            publicIdColumn.SetValue(image, details[0], null);

            var versionIdColumn = propertyInfo.GetProperty("Version");
            versionIdColumn.SetValue(image, $"v{details[1]}", null);

            var createdAtColumn = propertyInfo.GetProperty("CreatedAt");
            createdAtColumn.SetValue(image, DateTime.UtcNow, null);

            var updatedAtColumn = propertyInfo.GetProperty("UpdatedAt");
            updatedAtColumn.SetValue(image, DateTime.UtcNow, null);

            return image;
        }

        private static T GetDeletedImage<T>(string image)
        {
            var deletedImage = (T)Activator.CreateInstance(typeof(T));

            var propertyInfo = deletedImage.GetType();
                
            var publicIdColumn = propertyInfo.GetProperty("PublicId");
            publicIdColumn.SetValue(deletedImage, image, null);

            var deleteColumn = propertyInfo.GetProperty("Delete");
            deleteColumn.SetValue(deletedImage, true, null);

            return deletedImage;
        }
    }
}