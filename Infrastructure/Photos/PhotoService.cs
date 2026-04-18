using Application.Interfaces;
using Application.Profiles.DTO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Photos
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary( account );
        }

        public async Task<PhotoUploadResult?> UploadPhoto(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),

                    Folder = "Reactivities"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                if (result.Error != null) return null;

                return new PhotoUploadResult
                {
                    PublicId = result.PublicId,
                    Url = result.SecureUrl.AbsoluteUri
                };
            }
            return null;
        }

        public async Task<string> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Error != null) throw new Exception(result.Error.Message);
            return result.Result;
        }
    }
}
