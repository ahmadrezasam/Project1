using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementSystemCore.Models
{
    public class ResourceLinks
    {
        [Key]
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Custom properties included with this resource link.
        /// </summary>
        public string CustomProperties { get; set; }

        /// <summary>
        /// The link description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The link title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The tool to launch.
        /// </summary>
        //public int CoursesId { get; set; }
        //public int platformsId { get; set; }
        [ForeignKey("ToolsId")]
        public virtual int ToolsId { get; set; }
        public virtual Tools Tools { get; set; }

    }
}