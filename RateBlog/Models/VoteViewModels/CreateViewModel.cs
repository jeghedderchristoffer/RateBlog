using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.VoteViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Du skal stille et spørgsmål")]
        public string Question { get; set; }

        [MinLength(2, ErrorMessage = "Du skal vælge mindst 2 svarmuligheder")]
        public string[] FollowerQuestions { get; set; }
    }


}
