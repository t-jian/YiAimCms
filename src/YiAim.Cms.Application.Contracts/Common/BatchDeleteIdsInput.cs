using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YiAim.Cms;
public class BatchDeleteIdsInput
{
    [Required]
    public string Ids { get; set; }
}
