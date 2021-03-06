﻿using Microsoft.AspNetCore.Http;
using Bestfluence.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.InfluenterViewModels
{
    public class CreateViewModel : IValidatableObject
    {
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string YoutubeLink { get; set; }
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string TwitchLink { get; set; }

        public IFormFile ProfilePic { get; set; }

        public Influencer Influenter { get; set; }

        public string ProfileText { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProfilePic != null)
            {
                if (ProfilePic.ContentType != "image/png" && ProfilePic.ContentType != "image/jpeg")
                {
                    yield return new ValidationResult("Billedet skal være af typen JPEG eller PGN.");
                }

                if (ProfilePic.Length > 1000000)
                {
                    yield return new ValidationResult("Billedet må ikke overstige 1MB.");
                }
            }

        }
    }
}
