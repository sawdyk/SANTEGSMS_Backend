using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ReportCardCommentListReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public Guid UploadedById { get; set; }
    }

    public class CommentList
    {
        [Required]
        public string Comment { get; set; }
    }

    //Inherits from the parent Class
    public class CommentListReqModel : ReportCardCommentListReqModel
    {
        [Required]
        public IList<CommentList> Comments { get; set; }
    }

    //inherits from the Parent Class
    public class UpdateCommentReqModel : ReportCardCommentListReqModel
    {
        [Required]
        public string Comment { get; set; }
    }
}
