using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glab.Ui.Teams.Models
{
    public class CreateTeamModel
    {

        [Required(ErrorMessage = "You should choose a laboratory")]
        public string LaboratoryName { get; set; }
        [Required(ErrorMessage = "You should give a name to the team .")]
        public string TeamName { get; set; }
        // [Required(ErrorMessage = "You should give a acronyme to the team .")]
        public string TeamAcronyme { get; set; }



    }
}
